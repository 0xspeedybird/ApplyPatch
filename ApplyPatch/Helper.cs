using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ApplyPatch
{
    class Helper
    {

        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [System.Runtime.InteropServices.DllImport("Imagehlp.dll")]
        private static extern bool ImageRemoveCertificate(IntPtr handle, int index);

        public static void SistemaPeCks(string file) {
            try
            {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite))
                {
                    ImageRemoveCertificate(fs.SafeFileHandle.DangerousGetHandle(), 0);
                    fs.Close();
                }
                mCheckSum PE = new mCheckSum();
                PE.FixCheckSum(file);
            }
            catch
            {
                //Nothing
            }
            return;
        }

    }
}
