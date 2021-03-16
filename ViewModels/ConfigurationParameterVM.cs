using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class ConfigurationParameterVM : CWTBaseViewModel
    {
        public ConfigurationParameter ConfigurationParameter { get; set; }
 
        public ConfigurationParameterVM()
        {
        }
        public ConfigurationParameterVM(ConfigurationParameter configurationParameter)
        {
            ConfigurationParameter = configurationParameter;
        }
    }
}
