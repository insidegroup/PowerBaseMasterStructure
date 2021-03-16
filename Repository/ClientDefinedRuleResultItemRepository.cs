using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Repository
{
	public class ClientDefinedRuleResultItemRepository
	{
		private ClientDefinedRuleDC db = new ClientDefinedRuleDC(Settings.getConnectionString());

		public ClientDefinedRuleResultItem GetItem(int clientDefinedRuleResultItemId)
		{
			return db.ClientDefinedRuleResultItems.Where(x => x.ClientDefinedRuleResultItemId == clientDefinedRuleResultItemId).SingleOrDefault();
		}

	}
}