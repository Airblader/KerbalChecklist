using System;
using System.IO;
using System.Reflection;

namespace KerbalChecklist {

    class FileUtils {

        public static string getFullPathForFilename( string filename ) {
            return Path.GetDirectoryName( typeof( KerbalChecklist ).Assembly.Location )
                + "/" + filename;
        }

    }

}
