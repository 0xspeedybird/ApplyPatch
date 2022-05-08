using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ApplyPatch
{
    class PostProcessFallout2 : PostProcess
    {

        private const string SFALL_DRAW_INI_FILE = "ddraw.ini";
        private const string SFALL_DRAW_SECTION_DEBUG = "Debugging";
        private const string SFALL_DRAW_KEY_EXTRACRC = "ExtraCRC";

        private static string addCrcToStoredValue(string crc, string extraCrc)
        {
            if (extraCrc.Length > 0) {
                bool found = false;
                string[] crcs = extraCrc.Split(',');
                foreach (var c in crcs) {
                    if (crc.CompareTo(c.ToLower()) == 0) {
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    extraCrc += "," + crc;
                }
            } else {
                extraCrc = crc;
            }
            return extraCrc;
        }

        public override string run(string patchedFile)
        {
            StringBuilder value = new StringBuilder(255);
            UInt32 crc = sfallCrc(patchedFile);
            string crcStr = "0x" + crc.ToString("X").ToLower();
            string sfallDrawIni = Path.GetDirectoryName(patchedFile) + "\\" + SFALL_DRAW_INI_FILE;

            if (!File.Exists(sfallDrawIni)) {
                MessageBox.Show(
                   $"sfall {SFALL_DRAW_INI_FILE} not found!\n" +
                    $"Write the new hash {crcStr} into [{SFALL_DRAW_SECTION_DEBUG}] \\ {SFALL_DRAW_KEY_EXTRACRC}\n" +
                    "for playing without init warn-message.",
                    "Write new CRC code to sfall config",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return "Write new CRC code to config failed";
            }

            Helper.GetPrivateProfileString(SFALL_DRAW_SECTION_DEBUG, SFALL_DRAW_KEY_EXTRACRC, "", value, 255, sfallDrawIni);
            crcStr = addCrcToStoredValue(crcStr, value.ToString());
            Helper.WritePrivateProfileString(SFALL_DRAW_SECTION_DEBUG, SFALL_DRAW_KEY_EXTRACRC, crcStr, sfallDrawIni);

            return null;
        }

        private static UInt32 sfallCrc(string file) {
            UInt32[] table = new UInt32[256];
            const UInt32 kPoly = 0x1EDC6F41;
            UInt32 crc = 0xffffffff;
            byte[] fBytes = new byte[0];

            for (UInt32 i = 0; i < 256; i++)
            {
                UInt32 r = i;
                for (int j = 0; j < 8; j++)
                {
                    if ((r & 1) != 0) r = (r >> 1) ^ kPoly;
                    else r >>= 1;
                }
                table[i] = r;
            }

            try
            {
                BinaryReader bReader = new BinaryReader(new FileStream(file, FileMode.Open, FileAccess.Read));
                fBytes = bReader.ReadBytes((int)bReader.BaseStream.Length);
                bReader.Close();
            } catch {
                return 0;
            }

            foreach (UInt32 d in fBytes)
            {
                crc = table[((byte)crc) ^ d] ^ (crc >> 8);
            }
            return crc ^ 0xffffffff;
        }

    }
}
