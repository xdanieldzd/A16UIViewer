using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace A16UIViewer
{
    static class Program
    {
        public static string BasePath { get; set; }

        public static string DataPath { get { return Path.Combine(BasePath, "Data\\PS3"); } }

        [STAThread]
        static void Main()
        {
            BasePath = @"D:\Games\PlayStation 3\Atelier Shallie PAL\BLES02143\PS3_GAME\USRDIR";

            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
