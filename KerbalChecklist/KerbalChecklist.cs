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
            
            // TODO add Tooltip button
        }

        void Start() {
            checklistWindow.SetVisible( true );
        }

    }

}
