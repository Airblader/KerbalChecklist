using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using KerbalChecklist.Extensions;

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

        public static Checklists LoadMaster( string filename ) {
            // TODO do this with ConfigNode?
            string fullFilename = KSP.IO.IOUtils.GetFilePathFor( typeof( KerbalChecklist ), filename );

            XmlSerializer serializer = new XmlSerializer( typeof( Checklists ) );
            FileStream fileStream = new FileStream( fullFilename, FileMode.Open );

            try {
                return serializer.Deserialize( fileStream ) as Checklists;
            } finally {
                fileStream.Close();
            }
        }

        public bool LoadState() {
            string craftName = EditorLogic.fetch.shipNameField.Text;
            Log.Debug( "Loading state for craft = " + craftName );

            // TODO return full namespace reference
            if( !KSP.IO.File.Exists<KerbalChecklist>( KerbalChecklist.craftStatesFile ) ) {
                Log.Debug( "No file with saved craft information found, cannot load saved state" );
                return false;
            }

            ConfigNode config = ConfigNode.Load( KerbalChecklist.craftStatesFile );
            ConfigNode craftNode = null;
            foreach( ConfigNode node in config.GetNodes( StateKeys.CRAFT ) ) {
                if( node.GetValue( StateKeys.CRAFT_NAME ) == craftName ) {
                    craftNode = node;
                    break;
                }
            }

            if( craftNode == null ) {
                Log.Debug( "Found no saved state for this craft" );
                return false;
            }

            foreach( ConfigNode listNode in craftNode.GetNodes( StateKeys.LIST ) ) {
                Checklist list = GetChecklistByName( listNode.GetValue( StateKeys.LIST_NAME ) );
                if( list == null ) {
                    continue;
                }

                list.isSelected = listNode.GetBooleanValue( StateKeys.IS_SELECTED );
                list.isCollapsed = listNode.GetBooleanValue( StateKeys.IS_COLLAPSED );

                foreach( ConfigNode itemNode in listNode.GetNodes( StateKeys.ITEM ) ) {
                    Item item = list.GetItemByName( itemNode.GetValue( StateKeys.ITEM_NAME ) );
                    if( item == null ) {
                        continue;
                    }

                    item.isChecked = itemNode.GetBooleanValue( StateKeys.IS_CHECKED );
                }
            }

            Log.Debug( "Successfully loaded saved state" );
            return true;
        }

        public void SaveState() {
            string craftName = EditorLogic.fetch.shipNameField.Text;
            Log.Debug( "Saving state for craft = " + craftName );

            if( craftName == "Untitled Space Craft" ) {
                Log.Debug( "Will not save state for untitled space craft" );
                return;
            }

            if( !HasSelectedChecklists() ) {
                Log.Debug( "Nothing to save as no checklists were selected" );
                return;
            }

            ConfigNode craftNode = new ConfigNode( StateKeys.CRAFT );
            craftNode.AddValue( StateKeys.CRAFT_NAME, craftName );
            craftNode.AddValue( StateKeys.LAST_UPDATE, DateTime.Today.ToString( "d" ) );

            foreach( Checklist list in checklists ) {
                ConfigNode listNode = craftNode.AddNode( new ConfigNode( StateKeys.LIST ) );
                listNode.AddValue( StateKeys.LIST_NAME, list.name );

                listNode.AddValue( StateKeys.IS_SELECTED, list.isSelected );
                listNode.AddValue( StateKeys.IS_COLLAPSED, list.isCollapsed );

                foreach( Item item in list.items ) {
                    ConfigNode itemNode = listNode.AddNode( new ConfigNode( StateKeys.ITEM ) );
                    itemNode.AddValue( StateKeys.ITEM_NAME, item.name );
                    itemNode.AddValue( StateKeys.IS_CHECKED, item.isChecked );
                }
            }

            ConfigNode config = null;
            // TODO get rid of entire namespace when XML stuff is gone
            if( KSP.IO.File.Exists<KerbalChecklist>( KerbalChecklist.craftStatesFile ) ) {
                config = ConfigNode.Load( KerbalChecklist.craftStatesFile );

                // remove any saved states that already exist
                config.RemoveNodesWithValue( StateKeys.CRAFT, StateKeys.CRAFT_NAME, craftName );
            } else {
                config = new ConfigNode();
            }

            config.AddNode( craftNode );
            config.Save( KerbalChecklist.craftStatesFile );
            Log.Debug( "Successfully saved state for this craft" );
        }

        public bool Validate() {
            List<string> listNames = new List<string>();
            foreach( Checklist list in checklists ) {
                if( listNames.Contains( list.name ) ) {
                    Log.Error( "Duplicate list with name " + list.name + "." );
                    return false;
                }

                listNames.Add( list.name );

                if( list.checklists.Contains( list.name ) ) {
                    Log.Error( "List with name " + list.name + " contains itself as a mix-in" );
                    return false;
                }
            }

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

        public List<Item> GetItemsRecursively( Checklist checklist ) {
            List<Item> allItems = new List<Item>( checklist.items );
            foreach( string listName in checklist.checklists ) {
                allItems.AddRange( GetItemsRecursively( GetChecklistByName( listName ) ) );
            }

            return allItems;
        }

        public bool HasSelectedChecklists() {
            foreach( Checklist list in checklists ) {
                if( list.isSelected ) {
                    return true;
                }
            }

            return false;
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

    internal class StateKeys {

        public const string CRAFT = "CRAFT";
        public const string LIST = "LIST";
        public const string ITEM = "ITEM";

        public const string CRAFT_NAME = "vesselName";
        public const string LIST_NAME = "listName";
        public const string ITEM_NAME = "itemName";

        public const string LAST_UPDATE = "lastUpdate";
        public const string IS_SELECTED = "isSelected";
        public const string IS_COLLAPSED = "isCollapsed";
        public const string IS_CHECKED = "isChecked";

        private StateKeys() {
            // prevent instantiation
        }

    }

}