using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class FeeLoadDescriptionTypeCodeRepository
    {
        private PointOfSaleFeeLoadDataContext db = new PointOfSaleFeeLoadDataContext(Settings.getConnectionString());

        //Get FeeLoadDescriptionTypes
        public List<FeeLoadDescriptionType> GetAllFeeLoadDescriptionTypeCodes()
		{
			return db.FeeLoadDescriptionTypes.OrderBy(x => x.FeeLoadDescriptionTypeCode).ToList();
		}

        //Get one FeeLoadDescriptionType
        public FeeLoadDescriptionType GetFeeLoadDescriptionTypeCode(string feeLoadDescriptionTypeCode)
		{
			return db.FeeLoadDescriptionTypes.SingleOrDefault(c => c.FeeLoadDescriptionTypeCode == feeLoadDescriptionTypeCode);
		}

	}
}