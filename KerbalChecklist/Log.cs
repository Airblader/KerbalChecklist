using System;
using Tac;

namespace KerbalChecklist {

    public static class Log {

        public static bool ACTIVE = true;
        private const string KERBAL_CONTEXT = "KerbalChecklist";

        public static void Debug( string message ) {
            if( !ACTIVE ) {
                return;
            }

            Logging.Log( KERBAL_CONTEXT, message );
        }

        public static void Error( string message, Exception e = null ) {
            if( !ACTIVE ) {
                return;
            }

            string compositeMessage = message;
            if( e != null ) {
                compositeMessage += " [" + e.Message + "]: " + e.StackTrace;
            }

            Logging.LogError( KERBAL_CONTEXT, compositeMessage );
        }

    }

}
