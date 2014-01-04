using System.Collections.Generic;
using UnityEngine;
using Tac;

namespace KerbalChecklist {

    [KSPAddon( KSPAddon.Startup.EditorAny, false )]
    public class KerbalChecklist : MonoBehaviour {

        public const string CONFIG_FILENAME = "checklists.xml";

        private Checklists checklists; // TODO rename this
        private ChecklistWindow checklistWindow;

        private ButtonWrapper toolbarButton;

        void Awake() {
            checklists = Checklists.Load( CONFIG_FILENAME );
            checklistWindow = new ChecklistWindow( checklists );

            SetupToolbar();
        }

        void Start() {
            checklistWindow.SetVisible( true );
        }

        private void SetupToolbar() {
            toolbarButton = new ButtonWrapper( new Rect( Screen.width * 0.7f, 0, 32, 32 ),
                "KerbalChecklist/kerbalchecklist", "KC", "Kerbal Checklist", checklistWindow.ToggleVisible );
        }

        internal void OnDestroy() {
            toolbarButton.Destroy();
        }

    }

}
