﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Tac;

namespace KerbalChecklist {

    class ChecklistWindow : Window<KerbalChecklist> {

        private Checklists checklists;
        private SelectionWindow selectionWindow;

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
        }

        private int getNumberOfCheckedItems( Checklist checklist ) {
            int numberOfCheckedItems = 0;
            foreach( Item item in checklist.GetItemsRecursively( checklists ) ) {
                if( item.isChecked ) {
                    numberOfCheckedItems++;
                }
            }

            return numberOfCheckedItems;
        }

        // TODO refactor this
        protected override void DrawWindowContents( int windowID ) {
            checklistItemsScrollPosition = GUILayout.BeginScrollView( checklistItemsScrollPosition );

            foreach( Checklist checklist in checklists.checklists ) {
                if( !checklist.isSelected ) {
                    continue;
                }

                int numberOfCheckedItems = getNumberOfCheckedItems( checklist );
                int numberOfItems = checklist.GetItemsRecursively( checklists ).Count;

                GUILayout.BeginHorizontal( numberOfCheckedItems == numberOfItems
                    ? checklistSectionDoneHeaderBackgroundStyle : checklistSectionHeaderBackgroundStyle );
                string label = checklist.isCollapsed ? "▶ " : "▼ ";
                label += checklist.name;
                label += " (" + numberOfCheckedItems + "/" + numberOfItems + ")";
                if( GUILayout.Button( label, checklistSectionHeaderLabelStyle, GUILayout.ExpandWidth( true ) ) ) {
                    checklist.isCollapsed = !checklist.isCollapsed;
                }
                if( GUILayout.Button( new GUIContent( "X", "Remove this checklist" ), GUILayout.ExpandWidth( false ) ) ) {
                    checklist.isSelected = false;
                }
                GUILayout.EndHorizontal();

                // TODO have this wrap the foreach loop
                if( checklist.isCollapsed ) {
                    continue;
                }
                foreach( Item item in checklist.GetItemsRecursively( checklists ) ) {
                    if( item.isChecked && !displayCheckedItems ) {
                        continue;
                    }

                    GUILayout.BeginHorizontal();
                    item.isChecked = GUILayout.Toggle( item.isChecked, new GUIContent( item.name, item.description ),
                        checklistToggleStyle );
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            GUILayout.Label( "Description: " + ( String.IsNullOrEmpty( GUI.tooltip ) ? "Hover over an item" : GUI.tooltip ) );
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            displayCheckedItems = GUILayout.Toggle( displayCheckedItems, "Display checked items" );
            GUILayout.EndHorizontal();

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

    }

}
