using System;
using UnityEngine;
using Tac;
using KerbalChecklist.Extensions;
using KSP.IO;

namespace KerbalChecklist {

    class ChecklistWindow : Window<KerbalChecklist> {

        private Checklists checklists;
        private SelectionWindow selectionWindow;
        private OptionsWindow optionsWindow;

        private bool displayCheckedItems = true;

        private Vector2 checklistItemsScrollPosition = Vector2.zero;
        private GUIStyle checklistSectionHeaderBackgroundStyle;
        private GUIStyle checklistSectionDoneHeaderBackgroundStyle;
        private GUIStyle checklistSectionHeaderLabelStyle;
        private GUIStyle checklistToggleStyle;


        private const int DEFAULT_WINDOW_WIDTH = 300;
        private const int DEFAULT_WINDOW_HEIGHT = 300;

        public ChecklistWindow( Checklists checklists )
            : base( "Kerbal Checklist", DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT ) {

            this.checklists = checklists;
            this.selectionWindow = new SelectionWindow( ref checklists.checklists );
            this.optionsWindow = new OptionsWindow( SetDisplayCheckedItems, DeleteSavedState );
        }

        private void SetDisplayCheckedItems( bool newValue ) {
            displayCheckedItems = newValue;
        }

        // TODO maybe put this into Checklists to the other related methods
        private void DeleteSavedState() {
            if( !File.Exists<KerbalChecklist>( KerbalChecklist.craftStatesFile ) ) {
                return;
            }

            // remove saved state
            ConfigNode config = ConfigNode.Load( KerbalChecklist.craftStatesFile );
            config.RemoveNodesWithValue( StateKeys.CRAFT, StateKeys.CRAFT_NAME, EditorLogic.fetch.shipNameField.Text );
            config.Save( KerbalChecklist.craftStatesFile );

            // reload state for current display
            checklists.LoadState();
        }

        private int GetNumberOfCheckedItems( Checklist checklist ) {
            int numberOfCheckedItems = 0;
            foreach( Item item in checklists.GetItemsRecursively( checklist ) ) {
                if( item.isChecked ) {
                    numberOfCheckedItems++;
                }
            }

            return numberOfCheckedItems;
        }

        override protected void DrawWindowContents( int windowID ) {
            DrawChecklists();
            DrawTooltipDisplay();
            DrawOptionsButton();
            DrawSelectChecklistsButton();
        }

        private void DrawChecklists() {
            checklistItemsScrollPosition = GUILayout.BeginScrollView( checklistItemsScrollPosition );

            foreach( Checklist checklist in checklists.checklists ) {
                if( !checklist.isSelected ) {
                    continue;
                }

                DrawChecklistHeader( checklist );
                DrawItems( checklist );
            }

            GUILayout.EndScrollView();
        }

        private void DrawChecklistHeader( Checklist checklist ) {
            int numberOfCheckedItems = GetNumberOfCheckedItems( checklist );
            int numberOfItems = checklists.GetItemsRecursively( checklist ).Count;

            GUILayout.BeginHorizontal( numberOfCheckedItems == numberOfItems
                ? checklistSectionDoneHeaderBackgroundStyle : checklistSectionHeaderBackgroundStyle );

            string label = checklist.isCollapsed ? "▶" : "▼";
            label += " " + checklist.name;
            label += " (" + numberOfCheckedItems + "/" + numberOfItems + ")";

            if( GUILayout.Button( label, checklistSectionHeaderLabelStyle, GUILayout.ExpandWidth( true ) ) ) {
                checklist.isCollapsed = !checklist.isCollapsed;
            }

            if( GUILayout.Button( new GUIContent( "X", "Remove this checklist" ),
                GUILayout.ExpandWidth( false ) ) ) {

                checklist.isSelected = false;
            }

            GUILayout.EndHorizontal();
        }

        private void DrawItems( Checklist checklist ) {
            if( checklist.isCollapsed ) {
                return;
            }

            foreach( Item item in checklists.GetItemsRecursively( checklist ) ) {
                if( item.isChecked && !displayCheckedItems ) {
                    continue;
                }

                DrawItem( item );
            }
        }

        private void DrawItem( Item item ) {
            GUILayout.BeginHorizontal();

            item.isChecked = GUILayout.Toggle( item.isChecked, new GUIContent( item.name, item.description ),
                checklistToggleStyle );

            GUILayout.EndHorizontal();
        }

        private void DrawTooltipDisplay() {
            GUILayout.BeginHorizontal();
            GUILayout.Label( "Description: " + ( String.IsNullOrEmpty( GUI.tooltip ) ? "Hover over an item" : GUI.tooltip ) );
            GUILayout.EndHorizontal();
        }

        private void DrawOptionsButton() {
            GUILayout.BeginHorizontal();

            if( GUILayout.Button( "Options" ) ) {
                optionsWindow.SetVisible( true );
                GUI.FocusWindow( optionsWindow.windowId );
            }

            GUILayout.EndHorizontal();
        }

        private void DrawSelectChecklistsButton() {
            GUILayout.BeginHorizontal();

            if( GUILayout.Button( "Select Checklists" ) ) {
                selectionWindow.SetVisible( true );
                GUI.FocusWindow( selectionWindow.windowId );
            }

            GUILayout.EndHorizontal();
        }

        override protected void ConfigureStyles() {
            GuiUtils.SetupGuiSkin();
            base.ConfigureStyles();

            if( checklistSectionHeaderBackgroundStyle == null ) {
                checklistSectionHeaderBackgroundStyle = new GUIStyle( GUI.skin.label );
                checklistSectionHeaderBackgroundStyle.normal.background =
                    GuiUtils.createSolidColorTexture( new Color( 1f, 1f, 1f, 0.2f ) );
            }

            if( checklistSectionDoneHeaderBackgroundStyle == null ) {
                checklistSectionDoneHeaderBackgroundStyle = new GUIStyle( checklistSectionHeaderBackgroundStyle );
                checklistSectionDoneHeaderBackgroundStyle.normal.background =
                    GuiUtils.createSolidColorTexture( new Color( 0.7f, 1f, 0.7f, 0.2f ) );
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

        public override void SetVisible( bool newValue ) {
            if( !newValue ) {
                selectionWindow.SetVisible( false );
                optionsWindow.SetVisible( false );
            } else if( !checklists.HasSelectedChecklists() ) {
                selectionWindow.SetVisible( true );
            }

            base.SetVisible( newValue );
        }

        override public ConfigNode Save( ConfigNode config ) {
            selectionWindow.Save( config );
            optionsWindow.Save( config );
            return base.Save( config );
        }

        public override ConfigNode Load( ConfigNode config ) {
            selectionWindow.Load( config );
            optionsWindow.Load( config );
            return base.Load( config );
        }

    }

}
