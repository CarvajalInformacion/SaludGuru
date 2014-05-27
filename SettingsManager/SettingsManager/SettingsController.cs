using SettingsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsManager
{
    public class SettingsController
    {
        #region public properties
        public Dictionary<string, ModuleModel> ModulesParams { get; private set; }
        #endregion

        #region constructors
        //standar read all modules
        public SettingsController()
        {
            SettingsReader sr = new SettingsReader(System.Configuration.ConfigurationManager.AppSettings["SettingsConfig"]);
            ModulesParams = sr.LoadAll();
        }
        //personalized read all modules
        public SettingsController(string oSettingsConfigLocation)
        {

            SettingsReader sr = new SettingsReader(oSettingsConfigLocation);
            ModulesParams = sr.LoadAll();
        }
        //personalized read one module
        public SettingsController(string oSettingsConfigLocation, string ModuleName)
        {
            string SettingLoc = !string.IsNullOrEmpty(oSettingsConfigLocation) ? oSettingsConfigLocation : System.Configuration.ConfigurationManager.AppSettings["SettingsConfig"];
            SettingsReader sr = new SettingsReader(SettingLoc);
            ModuleModel CurrentModule = sr.LoadModule(ModuleName);
            ModulesParams = new Dictionary<string, ModuleModel>();
            ModulesParams.Add(CurrentModule.Name, CurrentModule);
        }
        #endregion
    }
}
