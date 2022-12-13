using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ApplyPatch
{

    static class Program
    {
   
        static internal bool runSilently = false;

        
        /// <summary>
        /// Entry point.
        /// </summary>
        [STAThread]
        static void Main(string[] parameters)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            foreach (var parameter in parameters)
            {
                switch (parameter.ToLower())
                {
                    case "/auto":
                        runSilently = true;
                        break;;
                }
            }
            MainForm mainForm = new MainForm();

            if (runSilently)
            {
                PatchHelper.automate(mainForm);
            }
            else 
            { 
                Application.Run(mainForm);
            }

        }
    }
}
