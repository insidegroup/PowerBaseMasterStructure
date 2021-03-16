using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
	public class GDSEndWarningConfigurationLanguagesVM : CWTBaseViewModel
	{
		public GDSEndWarningConfiguration GDSEndWarningConfiguration { get; set; }
		public CWTPaginatedList<spDesktopDataAdmin_SelectGDSEndWarningConfigurationLanguages_v1Result> GDSEndWarningConfigurationLanguages { get; set; }
        public bool HasWriteAccess { get; set; }
 
        public GDSEndWarningConfigurationLanguagesVM()
        {
            HasWriteAccess = false;
        }
		
		public GDSEndWarningConfigurationLanguagesVM(
			GDSEndWarningConfiguration gdsEndWarningConfiguration,
			CWTPaginatedList<spDesktopDataAdmin_SelectGDSEndWarningConfigurationLanguages_v1Result> gdsEndWarningConfigurationLanguages, 
			bool hasWriteAccess)
        {
			GDSEndWarningConfiguration = gdsEndWarningConfiguration;
			GDSEndWarningConfigurationLanguages = gdsEndWarningConfigurationLanguages;
			HasWriteAccess = hasWriteAccess;
        }
    }
}