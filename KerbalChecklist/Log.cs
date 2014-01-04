using System;
using Tac;

namespace KerbalChecklist {

    static class Log {

        private const string KERBAL_CONTEXT = "KerbalChecklist";

        public static void Debug( string message ) {
            Logging.Log( KERBAL_CONTEXT, message );
        }

        public static void Error( string message, Exception e = null ) {
            string compositeMessage = message;
            if( e != null ) {
                compositeMessage += " [" + e.Message + "]: " + e.StackTrace;
            }

            Logging.LogError( KERBAL_CONTEXT, compositeMessage );
        }

    }

}
