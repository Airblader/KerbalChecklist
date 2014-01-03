using System;
using System.Collections.Generic;
using UnityEngine;
using Tac;

namespace KerbalChecklist {

    class ChecklistWindow : Window<KerbalChecklist> {

        private List<Checklist> allChecklists;
        private List<Checklist> selectedChecklists;

        private Vector2 checklistItemsScrollPosition = Vector2.zero;
        private GUIStyle checklistSectionHeaderBackgroundStyle;
        private GUIStyle checklistSectionHeaderLabelStyle;
        private GUIStyle checklistToggleStyle;

        private const int DEFAULT_WINDOW_WIDTH = 300;
        private const int DEFAULT_WINDOW_HEIGHT = 300;

        public ChecklistWindow( ref List<Checklist> allChecklists )
            : base( "KerbalChecklist", DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT ) {

            this.allChecklists = allChecklists;
            this.selectedChecklists = allChecklists;
        }

        private int getNumberOfCheckedItems( Checklist checklist ) {
            int numberOfCheckedItems = 0;
            foreach( Item item in checklist.items ) {
                if( item.isChecked ) {
                    numberOfCheckedItems++;
                }
            }

            // TODO check mix-in lists

            return numberOfCheckedItems;
        }

        protected override void DrawWindowContents( int windowID ) {
            checklistItemsScrollPosition = GUILayout.BeginScrollView( checklistItemsScrollPosition );

            foreach( Checklist checklist in selectedChecklists ) {
                GUILayout.BeginHorizontal( checklistSectionHeaderBackgroundStyle );
                // TODO display ("(5/10)")
                GUILayout.Label( checklist.name, checklistSectionHeaderLabelStyle );
                GUILayout.EndHorizontal();

                foreach( Item item in checklist.items ) {
                    GUILayout.BeginHorizontal();
                    item.isChecked = GUILayout.Toggle( item.isChecked, new GUIContent( item.name, item.description ),
                        checklistToggleStyle );
                    GUILayout.EndHorizontal();
                }

                // TODO display mixin lists
            }
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            GUILayout.Label( "Description: " + ( String.IsNullOrEmpty( GUI.tooltip ) ? "Hover over an item" : GUI.tooltip ) );
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Toggle( true, "Display checked items" );
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if( GUILayout.Button( "Select Checklists" ) ) {
                // TODO open window to select current checklists
                // (consider visible attribute)
            }
            GUILayout.EndHorizontal();
        }

        override protected void ConfigureStyles() {
            SetupGuiSkin();
            base.ConfigureStyles();

            if( checklistSectionHeaderBackgroundStyle == null ) {
                checklistSectionHeaderBackgroundStyle = new GUIStyle( GUI.skin.label );
                checklistSectionHeaderBackgroundStyle.normal.background =
                    createSolidColorTexture( new Color( 1f, 1f, 1f, 0.2f ) );
            }

            if( checklistSectionHeaderLabelStyle == null ) {
                checklistSectionHeaderLabelStyle = new GUIStyle( GUI.skin.label );
                checklistSectionHeaderLabelStyle.fontStyle = FontStyle.Bold;
                checklistSectionHeaderLabelStyle.margin = new RectOffset( 10, 0, 3, 3 );
            }

            if( checklistToggleStyle == null ) {
                checklistToggleStyle = new GUIStyle( GUI.skin.toggle );
                checklistToggleStyle.margin = new RectOffset( 15, 0, 2, 2 );
            }
        }

        private void SetupGuiSkin() {
            GUI.skin = null;

            GUISkin skin = (GUISkin) GameObject.Instantiate( GUI.skin );
            skin.window.padding = new RectOffset( 5, 5, 20, 5 );

            // TODO scrollview background

            GUI.skin = skin;
        }

        private Texture2D createSolidColorTexture( Color color ) {
            Texture2D texture = new Texture2D( 1, 1 );
            texture.SetPixel( 0, 0, color );
            texture.Apply();

            return texture;
        }

    }

}
