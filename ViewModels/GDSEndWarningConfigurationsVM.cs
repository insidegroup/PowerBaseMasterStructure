using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
	public class GDSEndWarningConfigurationsVM : CWTBaseViewModel
	{
		public GDSEndWarningConfiguration GDSEndWarningConfiguration { get; set; }
		public CWTPaginatedList<spDesktopDataAdmin_SelectGDSEndWarningConfigurations_v1Result> GDSEndWarningConfigurations { get; set; }
        public bool HasWriteAccess { get; set; }
 
        public GDSEndWarningConfigurationsVM()
        {
            HasWriteAccess = false;
        }
		public GDSEndWarningConfigurationsVM(
			GDSEndWarningConfiguration gdsEndWarningConfiguration,
			CWTPaginatedList<spDesktopDataAdmin_SelectGDSEndWarningConfigurations_v1Result> gdsEndWarningConfigurations, 
			bool hasWriteAccess)
        {
			GDSEndWarningConfiguration = gdsEndWarningConfiguration;
			GDSEndWarningConfigurations = gdsEndWarningConfigurations;
			HasWriteAccess = hasWriteAccess;
        }
    }
}