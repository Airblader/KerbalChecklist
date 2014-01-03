using System.Collections.Generic;
using UnityEngine;
using Tac;

namespace KerbalChecklist {

    class ChecklistWindow : Window<KerbalChecklist> {

        private List<Checklist> allChecklists;
        private List<Checklist> selectedChecklists;

        private Vector2 checklistItemsScrollPosition = Vector2.zero;

        private const int DEFAULT_WINDOW_WIDTH = 400;
        private const int DEFAULT_WINDOW_HEIGHT = 500;

        public ChecklistWindow( ref List<Checklist> allChecklists )
            : base( "KerbalChecklist", DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT ) {

            this.allChecklists = allChecklists;
            this.selectedChecklists = allChecklists;
        }

        public static void SetupGuiSkin() {
            GUI.skin = null;

            GUISkin skin = (GUISkin) GameObject.Instantiate( GUI.skin );
            skin.window.padding = new RectOffset( 5, 5, 20, 5 );

            // TODO scrollview background

            GUI.skin = skin;
        }


        protected override void DrawWindowContents( int windowID ) {
            checklistItemsScrollPosition = GUILayout.BeginScrollView( checklistItemsScrollPosition );

            foreach( Checklist checklist in selectedChecklists ) {
                GUILayout.BeginHorizontal();
                // TODO display ("(5/10)")
                GUILayout.Label( checklist.name );
                GUILayout.EndHorizontal();

                // TODO sort items by state
                foreach( Item item in checklist.items ) {
                    GUILayout.BeginHorizontal();
                    item.isChecked = GUILayout.Toggle( item.isChecked, new GUIContent( item.name, item.description ) );
                    GUILayout.EndHorizontal();
                }

                // TODO display mixin lists
            }
            GUILayout.EndScrollView();

            if( GUILayout.Button( "Select Checklists" ) ) {
                // TODO open window to select current checklists
                // (consider visible attribute)
            }
        }

        override protected void ConfigureStyles() {
            SetupGuiSkin();
            base.ConfigureStyles();
        }

    }

}
