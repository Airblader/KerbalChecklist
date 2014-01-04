using System;
using System.Collections.Generic;
using Tac;
using UnityEngine;

namespace KerbalChecklist {

    class SelectionWindow : Window<KerbalChecklist> {

        private List<Checklist> checklists;
        private Action<List<Checklist>> onSelected;

        private GUIStyle selectedStyle;
        private GUIStyle unselectedStyle;

        public SelectionWindow( List<Checklist> checklists, Action<List<Checklist>> onSelected )
            : base( "Select Checklists", 200, 200 ) {

            this.checklists = new List<Checklist>();
            this.onSelected = onSelected;
            foreach( Checklist checklist in checklists ) {
                if( !checklist.visible ) {
                    continue;
                }

                this.checklists.Add( checklist );
            }
        }

        private void SetAllChecklists( bool selected ) {
            foreach( Checklist list in checklists ) {
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
                GUILayout.BeginHorizontal( checklist.isSelected ? selectedStyle : unselectedStyle );
                if( GUILayout.Button( checklist.name, GUI.skin.label ) ) {
                    checklist.isSelected = !checklist.isSelected;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if( GUILayout.Button( "Select" ) ) {
                SetVisible( false );
                // TODO focus other window

                List<Checklist> selectedLists = new List<Checklist>();
                foreach( Checklist list in checklists ) {
                    if( list.isSelected ) {
                        selectedLists.Add( list );
                    }
                }
                onSelected( selectedLists );
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
