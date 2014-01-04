using System;
using System.Collections.Generic;
using Tac;
using UnityEngine;

namespace KerbalChecklist {

    class SelectionWindow : Window<KerbalChecklist> {

        private List<SelectableChecklist> checklists;
        private Action<List<Checklist>> onSelected;

        private GUIStyle selectedStyle;
        private GUIStyle unselectedStyle;

        public SelectionWindow( List<Checklist> checklists, Action<List<Checklist>> onSelected )
            : base( "Select Checklists", 200, 200 ) {

            this.checklists = new List<SelectableChecklist>();
            this.onSelected = onSelected;
            foreach( Checklist checklist in checklists ) {
                if( !checklist.visible ) {
                    continue;
                }

                this.checklists.Add( new SelectableChecklist( checklist ) );
            }
        }

        private void SetAllChecklists( bool selected ) {
            foreach( SelectableChecklist list in checklists ) {
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
                // TODO focus other window

                List<Checklist> selectedLists = new List<Checklist>();
                foreach( SelectableChecklist list in checklists ) {
                    if( list.isSelected ) {
                        selectedLists.Add( list.checklist );
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

    // TODO use DTO property instead
    internal class SelectableChecklist {

        public Checklist checklist;
        public bool isSelected;

        public SelectableChecklist( Checklist checklist, bool isSelected = false ) {
            this.checklist = checklist;
            this.isSelected = isSelected;
        }

    }

}
