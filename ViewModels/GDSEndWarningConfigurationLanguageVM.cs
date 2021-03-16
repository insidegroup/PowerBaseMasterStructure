using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	public class GDSEndWarningConfigurationLanguageVM : CWTBaseViewModel
	{
		public GDSEndWarningConfigurationLanguage GDSEndWarningConfigurationLanguage { get; set; }
		public GDSEndWarningConfiguration GDSEndWarningConfiguration { get; set; }
		public IEnumerable<SelectListItem> Languages { get; set; }

        public GDSEndWarningConfigurationLanguageVM()
        {
          
        }

		public GDSEndWarningConfigurationLanguageVM(
			GDSEndWarningConfigurationLanguage gdsEndWarningConfigurationLanguage,
			GDSEndWarningConfiguration gdsEndWarningConfiguration,
			IEnumerable<SelectListItem> languages)
        {
			GDSEndWarningConfigurationLanguage = gdsEndWarningConfigurationLanguage;
			GDSEndWarningConfiguration = gdsEndWarningConfiguration;
			Languages = languages;
        }
    }
}