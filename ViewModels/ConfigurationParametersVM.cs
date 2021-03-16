using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ConfigurationParametersVM : CWTBaseViewModel
   {
        public CWTPaginatedList<spDesktopDataAdmin_SelectConfigurationParameters_v1Result> ConfigurationParameters { get; set; }
        public bool HasWriteAccess { get; set; }
 
        public ConfigurationParametersVM()
        {
            HasWriteAccess = false;
        }
        public ConfigurationParametersVM(CWTPaginatedList<spDesktopDataAdmin_SelectConfigurationParameters_v1Result> configurationParameters, bool hasWriteAccess)
        {
            ConfigurationParameters = configurationParameters;
            HasWriteAccess = hasWriteAccess;
        }
    }
}