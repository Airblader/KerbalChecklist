using System;
using System.Collections.Generic;
using KSP.IO;
using UnityEngine;
using Tac;

namespace KerbalChecklist {

    [KSPAddon( KSPAddon.Startup.EditorAny, false )]
    public class KerbalChecklist : MonoBehaviour {

        private const string CHECKLISTS_FILE = "checklists.xml";
        public static string configFile;

        private Checklists checklists;
        private ChecklistWindow checklistWindow;

        private ButtonWrapper toolbarButton;

        void Awake() {
            Log.Debug( "Waking up..." );
            configFile = IOUtils.GetFilePathFor( this.GetType(), "KerbalChecklist.cfg" );

            checklists = Checklists.Load( CHECKLISTS_FILE );
            checklistWindow = new ChecklistWindow( checklists );

            SetupToolbar();
        }

        void Start() {
            Log.Debug( "Starting..." );

            Load();
            checklistWindow.SetVisible( true );
        }

        private void SetupToolbar() {
            try {
                toolbarButton = new ButtonWrapper( new Rect( Screen.width * 0.7f, 0, 32, 32 ),
                    "KerbalChecklist/kerbalchecklist", "KC", "Kerbal Checklist", checklistWindow.ToggleVisible );
            } catch( Exception e ) {
                Log.Error( "Failed to instantiate toolbar button", e );
            }
        }

        internal void OnDestroy() {
            Save();
            toolbarButton.Destroy();
        }

        private void Load() {
            Log.Debug( "Loading configuration..." );
            if( File.Exists<KerbalChecklist>( configFile ) ) {
                ConfigNode config = ConfigNode.Load( configFile );

                checklistWindow.Load( config );
                toolbarButton.Load( config );
            }
        }

        private void Save() {
            Log.Debug( "Saving configuration..." );
            ConfigNode config = new ConfigNode();

            checklistWindow.Save( config );
            toolbarButton.Save( config );

            config.Save( configFile );
        }

    }

}
