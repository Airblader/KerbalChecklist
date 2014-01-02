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
            string fullFilename = FileUtils.getFullPathForFilename( filename );

            XmlSerializer serializer = new XmlSerializer( typeof( Checklists ) );
            TextWriter streamWriter = new StreamWriter( fullFilename );

            try {
                serializer.Serialize( streamWriter, this );
            } finally {
                streamWriter.Close();
            }
        }

        public static Checklists Load( string filename ) {
            string fullFilename = FileUtils.getFullPathForFilename( filename );

            XmlSerializer serializer = new XmlSerializer( typeof( Checklists ) );
            FileStream fileStream = new FileStream( fullFilename, FileMode.Open );

            try {
                return serializer.Deserialize( fileStream ) as Checklists;
            } finally {
                fileStream.Close();
            }
        }

        public bool Validate() {
            // TODO implement
            /*
             * - checklist names are unique
             */

            return true;
        }

        public Checklist getChecklistByName( String name ) {
            foreach( Checklist checklist in checklists ) {
                if( checklist.name == name ) {
                    return checklist;
                }
            }

            return null;
        }

    }

    public class Checklist {

        [XmlAttribute()]
        public String name;
        [XmlAttribute()]
        public bool visible;

        public List<string> checklists = new List<string>();
        public List<Item> items = new List<Item>();

        public Checklist() {
            // for XML serialization        
        }

        public Checklist( String name, bool visible = true ) {
            this.name = name;
            this.visible = visible;
        }

        public Checklist AddItem( Item item ) {
            items.Add( item );
            return this;
        }

        public Checklist AddChecklist( Checklist checklist ) {
            checklists.Add( checklist.name );
            return this;
        }

    }

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
