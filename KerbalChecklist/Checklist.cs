using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace KerbalChecklist {

    [XmlRoot( "KerbalChecklist" )]
    public class Checklists {

        public const int CURRENT_VERSION = 1;

        public int version = CURRENT_VERSION;
        public List<Checklist> checklists = new List<Checklist>();

        public Checklists AddChecklist( Checklist item ) {
            checklists.Add( item );
            return this;
        }

        public void Save( string filename ) {
            string fullFilename = KSP.IO.IOUtils.GetFilePathFor( typeof( KerbalChecklist ), filename );

            XmlSerializer serializer = new XmlSerializer( typeof( Checklists ) );
            TextWriter streamWriter = new StreamWriter( fullFilename );

            try {
                serializer.Serialize( streamWriter, this );
            } finally {
                streamWriter.Close();
            }
        }

        public static Checklists Load( string filename ) {
            string fullFilename = KSP.IO.IOUtils.GetFilePathFor( typeof( KerbalChecklist ), filename );

            XmlSerializer serializer = new XmlSerializer( typeof( Checklists ) );
            FileStream fileStream = new FileStream( fullFilename, FileMode.Open );

            try {
                return serializer.Deserialize( fileStream ) as Checklists;
            } finally {
                fileStream.Close();
            }
        }

        public void LoadState() {
            string craftName = EditorLogic.fetch.shipNameField.Text;
            ConfigNode config = ConfigNode.Load( KerbalChecklist.configFile );
            if( config == null ) {
                return;
            }

            ConfigNode craftNode = null;
            foreach( ConfigNode node in config.GetNodes( "CRAFT" ) ) {
                if( node.GetValue( "name" ) == craftName ) {
                    craftNode = node;
                    break;
                }
            }

            if( craftNode == null ) {
                // no saved state found
                return;
            }

            foreach( ConfigNode listNode in craftNode.GetNodes( "LIST" ) ) {
                Checklist list = GetChecklistByName( listNode.GetValue( "listName" ) );
                if( list == null ) {
                    continue;
                }

                list.isSelected = listNode.GetValue( "isSelected" ) == "true";
                list.isCollapsed = listNode.GetValue( "isCollapsed" ) == "true";

                foreach( ConfigNode itemNode in listNode.GetNodes( "ITEM" ) ) {
                    Item item = list.GetItemByName( itemNode.GetValue( "name" ) );
                    if( item == null ) {
                        continue;
                    }

                    item.isChecked = itemNode.GetValue( "isChecked" ) == "true";
                }
            }
        }

        public void SaveState() {
            string craftName = EditorLogic.fetch.shipNameField.Text;

            ConfigNode craftNode = new ConfigNode( "CRAFT" );
            craftNode.AddValue( "vesselName", craftName );
            craftNode.AddValue( "lastUpdate", DateTime.Today.ToString( "d" ) );

            foreach( Checklist list in checklists ) {
                ConfigNode listNode = craftNode.AddNode( new ConfigNode( "LIST" ) );
                listNode.AddValue( "listName", list.name );

                listNode.AddValue( "isSelected", list.isSelected );
                listNode.AddValue( "isCollapsed", list.isCollapsed );

                foreach( Item item in list.items ) {
                    ConfigNode itemNode = listNode.AddNode( new ConfigNode( "ITEM" ) );
                    itemNode.AddValue( "name", item.name );
                    itemNode.AddValue( "isChecked", item.isChecked );
                }
            }

            ConfigNode config = ConfigNode.Load( KerbalChecklist.configFile );

            // TODO do this in an extension method
            foreach( ConfigNode node in config.GetNodes( "CRAFT" ) ) {
                if( node.GetValue( "name" ) == craftName ) {
                    config.nodes.Remove( node );
                }
            }

            config.AddNode( craftNode );
            config.Save( KerbalChecklist.configFile );
        }

        public bool Validate() {
            List<string> listNames = new List<string>();
            foreach( Checklist list in checklists ) {
                if( listNames.Contains( list.name ) ) {
                    Log.Error( "Duplicate list with name " + list.name + "." );
                    return false;
                }

                listNames.Add( list.name );
            }

            // TODO
            // 1. List does not contain itself

            Log.Debug( "Successfully validated loaded checklists." );
            return true;
        }

        public Checklist GetChecklistByName( String name ) {
            foreach( Checklist checklist in checklists ) {
                if( checklist.name == name ) {
                    return checklist;
                }
            }

            return null;
        }

    }

    [XmlType( "checklist" )]
    public class Checklist {

        [XmlAttribute()]
        public String name;
        [XmlAttribute()]
        public bool visible;
        [XmlIgnore()]
        public bool isSelected = false;
        [XmlIgnore()]
        public bool isCollapsed = false;

        [XmlArray()]
        [XmlArrayItem( "checklist" )]
        public List<string> checklists = new List<string>();
        public List<Item> items = new List<Item>();

        public Checklist() {
            // for XML serialization        
        }

        public Checklist( String name, bool visible = true, bool isSelected = false ) {
            this.name = name;
            this.visible = visible;
            this.isSelected = isSelected;
        }

        public Checklist AddItem( Item item ) {
            items.Add( item );
            return this;
        }

        public Checklist AddChecklist( Checklist checklist ) {
            checklists.Add( checklist.name );
            return this;
        }

        // TODO this should be a method of Checklists
        public List<Item> GetItemsRecursively( Checklists parent ) {
            List<Item> allItems = new List<Item>( items );
            foreach( string listName in checklists ) {
                Checklist list = parent.GetChecklistByName( listName );
                allItems.AddRange( list.GetItemsRecursively( parent ) );
            }

            return allItems;
        }

        public Item GetItemByName( string name ) {
            foreach( Item item in items ) {
                if( item.name == name ) {
                    return item;
                }
            }

            return null;
        }

    }

    [XmlType( "item" )]
    public class Item {

        public String name;
        public String description;
        [XmlIgnore]
        public bool isChecked = false;

        public Item() {
            // for XML serialization
        }

        public Item( String name, String description ) {
            this.name = name;
            this.description = description;
        }

    }

}