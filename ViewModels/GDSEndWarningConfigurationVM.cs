using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	public class GDSEndWarningConfigurationVM : CWTBaseViewModel
	{
		public GDSEndWarningConfiguration GDSEndWarningConfiguration { get; set; }
		public IEnumerable<SelectListItem> GDSs { get; set; }
		public IEnumerable<SelectListItem> GDSEndWarningBehaviorTypes { get; set; }
		public List<AutomatedCommand> AutomatedCommands { get; set; }

        public GDSEndWarningConfigurationVM()
        {
          
        }

		public GDSEndWarningConfigurationVM(
			IEnumerable<SelectListItem> gDSs, 
			IEnumerable<SelectListItem> gdsEndWarningBehaviorTypes,
			List<AutomatedCommand> automatedCommands)
        {
			GDSs = gDSs;
			GDSEndWarningBehaviorTypes = gdsEndWarningBehaviorTypes;
			AutomatedCommands = automatedCommands;
        }
    }
}