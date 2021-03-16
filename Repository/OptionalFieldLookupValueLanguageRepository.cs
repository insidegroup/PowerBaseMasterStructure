using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class OptionalFieldLookupValueLanguageRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
		
		//Get one Item
		public OptionalFieldLookupValueLanguage GetItem(int optionalFieldLookupValueId)
		{
			return db.OptionalFieldLookupValueLanguages.SingleOrDefault(
				c => (
					c.OptionalFieldLookupValueId == optionalFieldLookupValueId)
				);
		}
	}
}