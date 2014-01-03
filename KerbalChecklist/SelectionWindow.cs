using System;
using System.Collections.Generic;
using Tac;
using UnityEngine;

namespace KerbalChecklist {

    class SelectionWindow : Window<KerbalChecklist> {

        private Checklists checklists;

        public SelectionWindow( Checklists checklists )
            : base( "KerbalChecklist", 200, 200 ) {

            this.checklists = checklists;
        }

        protected override void DrawWindowContents( int windowID ) {
            // TODO
            GUILayout.BeginHorizontal();
            GUILayout.Button( "Foo" );
            GUILayout.EndHorizontal();
        }

        override protected void ConfigureStyles() {
            GuiUtils.SetupGuiSkin();
            base.ConfigureStyles();
        }

    }

}
