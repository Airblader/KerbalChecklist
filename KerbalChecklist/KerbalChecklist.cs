using System.Collections.Generic;
using UnityEngine;
using Tac;

namespace KerbalChecklist {

    [KSPAddon( KSPAddon.Startup.EditorAny, false )]
    public class KerbalChecklist : MonoBehaviour {

        public const string CONFIG_FILENAME = "checklists.xml";

        private Checklists checklists; // TODO rename this
        private ChecklistWindow checklistWindow;

        void Awake() {
            checklists = Checklists.Load( CONFIG_FILENAME );
            checklistWindow = new ChecklistWindow( ref checklists.checklists );

            // TODO redo this to not use TacLib
            ButtonWrapper button = new ButtonWrapper(
                new Rect( Screen.width * 0.7f, 0, 32, 32 ), "", "KC", 
                "KerbalChecklist", OnIconClicked );
            button.Visible = true;
        }

        // TODO rename
        private void OnIconClicked() {
            checklistWindow.ToggleVisible();
        }

        void Start() {
            checklistWindow.SetVisible( true );
        }

    }

}
