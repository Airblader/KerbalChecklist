using UnityEngine;

namespace KerbalChecklist.Extensions {

    public static class ConfigNodeExtensions {

        /// <summary>
        /// Returns true if the value of the property with the given name is "true" or "True".
        /// </summary>
        public static bool GetBooleanValue( this ConfigNode node, string name ) {
            return node.GetValue( name ).ToLower() == "true";
        }

        /// <summary>
        /// Removes all subnodes which have the given name and contain a property of given name and value.
        /// </summary>
        /// <param name="nodeName">Only subnodes with this name will be searched</param>
        /// <param name="valueName">Name of the property that needs to be contained</param>
        /// <param name="value">Value which the given property has to have</param>
        public static void RemoveNodesWithValue( this ConfigNode node, string nodeName, string valueName, string value ) {
            foreach( ConfigNode subNode in node.GetNodes( nodeName ) ) {
                if( subNode.GetValue( valueName ) == value ) {
                    node.nodes.Remove( subNode );
                }
            }
        }

        /// <summary>
        /// Returns true if and only if there exists a subnode with given name that has a property with the given value.
        /// </summary>
        public static bool HasNodeWithValue( this ConfigNode node, string nodeName, string valueName, string value ) {
            foreach( ConfigNode subNode in node.GetNodes( nodeName ) ) {
                if( subNode.GetValue( valueName ) == value ) {
                    return true;
                }
            }

            return false;
        }

    }

}
