using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Repository
{
	public class ClientDefinedRuleBusinessEntityCategoryRepository
	{
		private ClientDefinedRuleDC db = new ClientDefinedRuleDC(Settings.getConnectionString());

		//Get Client Defined Rule Business Entity Categories
		public List<ClientDefinedRuleBusinessEntityCategory> GetClientDefinedRuleBusinessEntityCategories()
		{
			return db.ClientDefinedRuleBusinessEntityCategories.OrderBy(x => x.ClientDefinedRuleBusinessEntityCategoryDescription).ToList();
		}
	}
}


