using System;
using Tac;
using UnityEngine;

namespace KerbalChecklist {

    public class OptionsWindow : Window<KerbalChecklist> {

        private Action<bool> onDisplayCheckedItemsChange;
        private Action onDeleteSavedState;

        private bool displayCheckedItems = true;

        public OptionsWindow( Action<bool> onDisplayCheckedItemsChange, Action onDeleteSavedState )
            : base( "Options", 200, 100 ) {

            this.onDisplayCheckedItemsChange = onDisplayCheckedItemsChange;
            this.onDeleteSavedState = onDeleteSavedState;
        }

        override protected void DrawWindowContents( int windowID ) {
            DrawDisplayCheckedItemsButton();
            DrawDeleteCurrentStateButton();
            DrawHideButton();
        }

        private void DrawDisplayCheckedItemsButton() {
            GUILayout.BeginHorizontal();

            if( displayCheckedItems != GUILayout.Toggle( displayCheckedItems, "Display checked items" ) ) {
                displayCheckedItems = !displayCheckedItems;
                onDisplayCheckedItemsChange.Invoke( displayCheckedItems );
            }

            GUILayout.EndHorizontal();
        }

        private void DrawDeleteCurrentStateButton() {
            GUILayout.BeginHorizontal();

            if( GUILayout.Button( "Reset for current craft" ) ) {
                onDeleteSavedState.Invoke();
            }

            GUILayout.EndHorizontal();
        }

        private void DrawHideButton() {
            GUILayout.BeginHorizontal();

            if( GUILayout.Button( "Hide" ) ) {
                SetVisible( false );
            }

            GUILayout.EndHorizontal();
        }

        override protected void ConfigureStyles() {
            GuiUtils.SetupGuiSkin();
            base.ConfigureStyles();
        }

    }

}
