using System;
using UnityEngine;

namespace KerbalChecklist {

    public static class GuiUtils {

        public static void SetupGuiSkin() {
            GUI.skin = null;

            GUISkin skin = (GUISkin) GameObject.Instantiate( GUI.skin );
            skin.window.padding = new RectOffset( 5, 5, 20, 5 );

            skin.label.alignment = TextAnchor.MiddleLeft;

            GUI.skin = skin;
        }

        public static Texture2D createSolidColorTexture( Color color ) {
            Texture2D texture = new Texture2D( 1, 1 );
            texture.SetPixel( 0, 0, color );
            texture.Apply();

            return texture;
        }

    }

}
