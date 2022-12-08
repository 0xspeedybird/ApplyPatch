using System.Collections.Generic;
using System.IO;

namespace ApplyPatch
{
    class PatchBytes : Patch
    {
        public class PatchElement
        {
            public int lineId;
            public int offset;
            public byte original;
            public byte edited;
        }

        private List<PatchElement> patch;
        private bool hasOriginalBytes = true;

        public bool HasOriginalBytes { get => hasOriginalBytes; set => hasOriginalBytes = value; }

        public void addPatchElement(PatchElement e) {
            patch.Add(e);
        }

        public PatchBytes()
        {
            patch = new List<PatchElement>();
        }

        public override int apply(string file, bool isBackup = false) {
            byte[] target;
            string backupFile = file + ".BAK";

            if (patch.Count == 0) return NO_ERR_OK;

            target = File.ReadAllBytes(file);

            foreach (PatchElement e in patch)
                target[e.offset] = e.edited;

            if (isBackup) {
                if (File.Exists(backupFile))
                    File.Delete(backupFile);
                File.Copy(file, backupFile);
            }

            if (File.Exists(file))
                File.Delete(file);

            File.WriteAllBytes(file, target);
            Helper.SistemaPeCks(file);

            return NO_ERR_OK;
        }

        public override int check(string file)
        {
            byte[] target;
            bool ok = true;
            string err = "Error!\nTarget file byte mismatch in:\n\n" +
                "PatchLine | Offset   | File      | PatchOrig.\n";

            if (patch.Count == 0) return NO_ERR_OK;

            target = File.ReadAllBytes(file);

            //check already patched
            foreach (PatchElement e in patch)
                if (target[e.offset] == e.edited)
                {
                    ok = false;
                    break;
                }
            
            if (!ok) {
                GetLastError = "File was already patched!";
                return ERR_CHECK_ALREADY;
            }

            //check original elements
            if (hasOriginalBytes) {
                foreach (PatchElement e in patch)
                    if (target[e.offset] != e.original)
                    {
                        err += $"{e.lineId}\t | 0x{e.offset.ToString("X").ToLower()} | 0x{target[e.offset].ToString("X").ToLower()}\t| 0x{e.original.ToString("X").ToLower()}\n";
                        ok = false;
                    }

                if (!ok) {
                    GetLastError = err;
                    return ERR_CHECK_INVALID;
                }

            }

            return NO_ERR_OK;
        }

    }
}
