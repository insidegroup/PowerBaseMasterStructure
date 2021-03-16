using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Xml;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
	public class PNROutputRemarkTypeRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get PNR Output RemarkType
		public PNROutputRemarkType GetPNROutputRemarkType(int pnrOutputRemarkTypeCode)
		{
			return db.PNROutputRemarkTypes.SingleOrDefault(x => x.PNROutputRemarkTypeCode == pnrOutputRemarkTypeCode);
		}
		
		//Get PNR Output RemarkTypes
		public List<PNROutputRemarkType> GetPNROutputRemarkTypes()
		{
			return db.PNROutputRemarkTypes.Where(x => x.ShowInDropListFlag == true).OrderBy(x => x.PNROutputRemarkTypeName).ToList();
		}

		//Get Meeting PNR Output RemarkTypes
		public List<PNROutputRemarkType> GetMeetingPNROutputRemarkTypes()
		{
			return db.PNROutputRemarkTypes.Where(x => x.ShowInMeetingDropListFlag == true).OrderBy(x => x.PNROutputRemarkTypeName).ToList();
		}
	}
}