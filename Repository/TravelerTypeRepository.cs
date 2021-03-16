using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Web.Security;

namespace CWTDesktopDatabase.Repository
{
    public class TravelerTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get one Item
        public TravelerType GetTravelerType(string id)
        {
            return db.TravelerTypes.SingleOrDefault(c => c.TravelerTypeGuid == id);
        }


        //Add Data From Linked Tables for Display
        public void EditForDisplay(TravelerType travelerType)
        {
            TravelerBackOfficeTypeRepository travelerBackOfficeTypeRepository = new TravelerBackOfficeTypeRepository();
            TravelerBackOfficeType travelerBackOfficeType = new TravelerBackOfficeType();
            travelerBackOfficeType = travelerBackOfficeTypeRepository.GetTravelerBackOfficeType(travelerType.TravelerBackOfficeTypeCode);
            if (travelerBackOfficeType != null)
            {
                travelerType.TravelerBackOfficeTypeDescription = travelerBackOfficeType.TravelerBackOfficeTypeDescription;
            }

			TravelerTypeSponsorRepository travelerTypeSponsorRepository = new TravelerTypeSponsorRepository();
			TravelerTypeSponsor travelerTypeSponsor = travelerTypeSponsorRepository.GetTravelerTypeSponsor(travelerType.TravelerTypeGuid);
			if (travelerTypeSponsor != null)
			{
				travelerType.TravelerTypeSponsor = travelerTypeSponsor;
			}

		}
    }
}
