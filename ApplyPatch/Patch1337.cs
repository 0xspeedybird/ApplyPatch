using System;

namespace ApplyPatch
{
    class Patch1337 : PatchBytes
    {

        private const int FILEOFFSET = 0xC00;

        public override int parse(string content)
        {
            try { 
                string[] patchLines = content.Split(
                    new string[] { Environment.NewLine },
                    StringSplitOptions.None
                );

                if (!patchLines[0].StartsWith(">")) {
                    GetLastError = $"Invalid default-file. 1st row not start with '>'.\n({patchLines[0]})\n";
                    return ERR_PATCH_INVALID_DEFFILE;
                }

                DefaultFileName = patchLines[0].Substring(1).ToLower().Trim();

                for (var i = 1; i < patchLines.Length; i += 1)
                {
                    if (patchLines[i].Trim() != "")
                    {
                        PatchElement pe = new PatchElement();

                        pe.lineId = i;

                        string[] tmp = patchLines[i].Split(':');
                        pe.offset = int.Parse(tmp[0], System.Globalization.NumberStyles.HexNumber) - FILEOFFSET;
                        string[] tmp2 = tmp[1].Replace("->", ":").Split(':');

                        pe.original = byte.Parse(tmp2[0], System.Globalization.NumberStyles.HexNumber);
                        pe.edited = byte.Parse(tmp2[1], System.Globalization.NumberStyles.HexNumber);

                        addPatchElement(pe);
                    }
                }

                return NO_ERR_OK;
            }catch(Exception ex){

                GetLastError = $"An error occured loading the patch.  Check that the URL is valid..\n({ex.Message})\n";
                return ERR_PATCH_INVALID_DEFFILE;
            }
        }

    }
}
