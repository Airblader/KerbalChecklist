using System;
using System.Collections.Generic;
using Tac;
using UnityEngine;

namespace KerbalChecklist {

    class SelectionWindow : Window<KerbalChecklist> {

        private List<Checklist> checklists;

        private GUIStyle selectedStyle;
        private GUIStyle unselectedStyle;

        public SelectionWindow( ref List<Checklist> checklists )
            : base( "Select Checklists", 200, 200 ) {

            this.checklists = checklists;
        }

        private void SetAllChecklists( bool selected ) {
            foreach( Checklist list in checklists ) {
                if( !list.visible ) {
                    continue;
                }

                list.isSelected = selected;
            }
        }

        // TODO refactor this
        protected override void DrawWindowContents( int windowID ) {
            GUILayout.BeginHorizontal();
            if( GUILayout.Button( "All" ) ) {
                SetAllChecklists( true );
            }
            if( GUILayout.Button( "None" ) ) {
                SetAllChecklists( false );
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginScrollView( Vector2.zero );
            foreach( Checklist checklist in checklists ) {
                if( !checklist.visible ) {
                    continue;
                }

                GUILayout.BeginHorizontal( checklist.isSelected ? selectedStyle : unselectedStyle );
                if( GUILayout.Button( checklist.name, GUI.skin.label ) ) {
                    checklist.isSelected = !checklist.isSelected;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if( GUILayout.Button( "Hide" ) ) {
                SetVisible( false );
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

}
