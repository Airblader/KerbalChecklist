using System;
using System.Collections.Generic;
using KSP.IO;
using UnityEngine;
using Tac;

namespace KerbalChecklist {

    [KSPAddon( KSPAddon.Startup.EditorAny, false )]
    public class KerbalChecklist : MonoBehaviour {

        // master file containing the checklist definitions
        private const string CHECKLISTS_FILE = "checklists.xml";

        // contains settings for window position etc.
        public static string windowSettingsFile;

        // contains the saved states of each craft
        public static string craftStatesFile;

        private Checklists checklists;
        private ChecklistWindow checklistWindow;
        private ButtonWrapper toolbarButton;

        // keep track of the root part so we know when to reload the current saved state
        private Part rootPart;

        void Awake() {
            Log.Debug( "Waking up..." );

            windowSettingsFile = IOUtils.GetFilePathFor( this.GetType(), "WindowSettings.cfg" );
            craftStatesFile = IOUtils.GetFilePathFor( this.GetType(), "CraftSettings.cfg" );

            checklists = Checklists.LoadMaster( CHECKLISTS_FILE );
            checklistWindow = new ChecklistWindow( checklists );

            SetupToolbar();
        }

        void Start() {
            Log.Debug( "Starting..." );

            Load();
            EditorController.Activate();
        }

        void FixedUpdate() {
            // when the root part changes, we reload the saved state
            if( rootPart != EditorController.fetch.rootPart ) {
                rootPart = EditorController.fetch.rootPart;

                if( rootPart != null ) {
                    bool loadedSavedState = checklists.LoadState();
                    checklistWindow.SetVisible( loadedSavedState );
                }
            }
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

            if( File.Exists<KerbalChecklist>( windowSettingsFile ) ) {
                ConfigNode config = ConfigNode.Load( windowSettingsFile );

                checklistWindow.Load( config );
                toolbarButton.Load( config );
            }
        }

        private void Save() {
            Log.Debug( "Saving configuration..." );

            checklists.SaveState();

            ConfigNode config = new ConfigNode();
            checklistWindow.Save( config );
            toolbarButton.Save( config );
            config.Save( windowSettingsFile );
        }

    }

}
