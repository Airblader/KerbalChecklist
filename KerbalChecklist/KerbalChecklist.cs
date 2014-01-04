using System.Collections.Generic;
using KSP.IO;
using UnityEngine;
using Tac;

namespace KerbalChecklist {

    [KSPAddon( KSPAddon.Startup.EditorAny, false )]
    public class KerbalChecklist : MonoBehaviour {

        private const string CHECKLISTS_FILE = "checklists.xml";
        private const string CONFIG_FILE = "KerbalChecklist.cfg";

        private Checklists checklists; // TODO rename this
        private ChecklistWindow checklistWindow;

        private ButtonWrapper toolbarButton;

        void Awake() {
            checklists = Checklists.Load( CHECKLISTS_FILE );
            checklistWindow = new ChecklistWindow( checklists );

            SetupToolbar();
        }

        void Start() {
            Load();
            checklistWindow.SetVisible( true );
        }

        private void SetupToolbar() {
            toolbarButton = new ButtonWrapper( new Rect( Screen.width * 0.7f, 0, 32, 32 ),
                "KerbalChecklist/kerbalchecklist", "KC", "Kerbal Checklist", checklistWindow.ToggleVisible );
        }

        internal void OnDestroy() {
            toolbarButton.Destroy();
        }

        private void Load() {
            if( File.Exists<KerbalChecklist>( CONFIG_FILE ) ) {
                ConfigNode config = ConfigNode.Load( CONFIG_FILE );

                checklistWindow.Load( config );
                toolbarButton.Load( config );
            }
        }

        private void Save() {
            ConfigNode config = new ConfigNode();

            checklistWindow.Save( config );
            toolbarButton.Save( config );

            config.Save( CONFIG_FILE );
        }

    }

}
