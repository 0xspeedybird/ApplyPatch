using System;
using System.IO;
using System.Text;

namespace ApplyPatch
{
    class Patch
    {

        public const int NO_ERR_OK = 0;

        public const int ERR_PATCH_INVALID_DEFFILE = -1;
        public const int ERR_PATCH_INVALID_CONTENT = -2;

        public const int ERR_CHECK_INVALID = -3;
        public const int ERR_CHECK_ALREADY = -4;

        private string lastError;

        private string defaultFileName;

        public string DefaultFileName { get => defaultFileName; set => defaultFileName = value; }
        public string GetLastError { get => lastError; set => lastError = value; }

        public virtual int parseFile(string file)
        {
            return parse(File.ReadAllBytes(file));
        }

        public virtual int parse(string content)
        {
            throw new NotImplementedException();
        }

        public virtual int parse(byte[] content)
        {
            return parse(Encoding.ASCII.GetString(content));
        }

        public virtual int apply(string file, bool isBackup = false)
        {
            throw new NotImplementedException();
        }

        public virtual int check(string file)
        {
            throw new NotImplementedException();
        }

    }
}
