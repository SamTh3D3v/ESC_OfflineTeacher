using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomAction
{
    [RunInstaller(true)]
    public partial class CustomInstaller : System.Configuration.Install.Installer
    {
        public CustomInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {


            base.Install(stateSaver);

            //ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap(Path.Combine(this.Context.Parameters["targetDir"].ToString(), "ESC_OfflineTeacher.exe.config"));

            Configuration config = ConfigurationManager.OpenExeConfiguration(this.Context.Parameters["targetDir"].ToString()+ "ESC_OfflineTeacher.exe");
            if (config != null)
            {
                ConnectionStringsSection connectionStringSection = config.ConnectionStrings;
                if (!connectionStringSection.SectionInformation.IsProtected)
                    connectionStringSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");

                config.Save();
            }

            

        }
    }
}
