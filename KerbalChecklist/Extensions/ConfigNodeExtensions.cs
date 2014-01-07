using UnityEngine;

namespace KerbalChecklist.Extensions {

    public static class ConfigNodeExtensions {

        public static bool GetBooleanValue( this ConfigNode node, string name ) {
            return node.GetValue( name ).ToLower() == "true";
        }

    }

}
