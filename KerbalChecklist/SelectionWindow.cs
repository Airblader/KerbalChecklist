using System;
using System.Collections.Generic;
using Tac;
using UnityEngine;

namespace KerbalChecklist {

    class SelectionWindow : Window<KerbalChecklist> {

        private List<SelectableChecklist> checklists;

        private GUIStyle selectedStyle;
        private GUIStyle unselectedStyle;

        public SelectionWindow( Checklists checklists )
            : base( "Select Checklists", 200, 200 ) {

            this.checklists = new List<SelectableChecklist>();
            foreach( Checklist checklist in checklists.checklists ) {
                if( !checklist.visible ) {
                    continue;
                }

                this.checklists.Add( new SelectableChecklist( checklist ) );
            }
        }

        protected override void DrawWindowContents( int windowID ) {
            GUILayout.BeginScrollView( Vector2.zero );
            foreach( SelectableChecklist checklist in checklists ) {
                GUILayout.BeginHorizontal( checklist.isSelected ? selectedStyle : unselectedStyle );
                if( GUILayout.Button( checklist.checklist.name, GUI.skin.label ) ) {
                    checklist.isSelected = !checklist.isSelected;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if( GUILayout.Button( "Select" ) ) {
                SetVisible( false );
                // TODO transfer to other window and focus it
            }
            GUILayout.EndHorizontal();
        }

        override protected void ConfigureStyles() {
            GuiUtils.SetupGuiSkin();
            base.ConfigureStyles();

            if( selectedStyle == null ) {
                selectedStyle = new GUIStyle( GUI.skin.label );
                selectedStyle.normal.background =
                    GuiUtils.createSolidColorTexture( new Color( 1f, 1f, 1f, 0.2f ) );
            }

            if( unselectedStyle == null ) {
                unselectedStyle = new GUIStyle( GUI.skin.label );
            }
        }

    }

    internal class SelectableChecklist {

        public Checklist checklist;
        public bool isSelected;

        public SelectableChecklist( Checklist checklist, bool isSelected = false ) {
            this.checklist = checklist;
            this.isSelected = isSelected;
        }

    }

}
