using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
    public class PartnerRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List Partners
        public CWTPaginatedList<spDesktopDataAdmin_SelectPartner_v1Result> GetPartners(string sortField, int sortOrder, int page)
        {
			var result = db.spDesktopDataAdmin_SelectPartner_v1(sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPartner_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

		//Get Partners
		public List<Partner> GetAllPartners()
		{
			return db.Partners.OrderBy(x => x.PartnerName).ToList();
		}
		
		//Get one Partner
		public Partner GetPartner(int partnerId)
		{
			return db.Partners.SingleOrDefault(c => c.PartnerId == partnerId); ;
		}

		//Add Partner
        public void Add(PartnerVM partnerVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPartner_v1(
				partnerVM.Partner.PartnerName,
				partnerVM.Partner.CountryCode,
				adminUserGuid
			);
        }

		//Edit Partner
		public void Update(PartnerVM partnerVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePartner_v1(
				partnerVM.Partner.PartnerId,
				partnerVM.Partner.PartnerName,
				partnerVM.Partner.CountryCode,
				adminUserGuid
			);
		}

		//Delete Partner
		public void Delete(PartnerVM partnerVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePartner_v1(
				partnerVM.Partner.PartnerId,
				adminUserGuid,
				partnerVM.Partner.VersionNumber
			);
		}

		public List<PartnerReference> GetPartnerReferences(int partnerId)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = from n in db.spDesktopDataAdmin_SelectPartnerReferences_v1(partnerId, adminUserGuid)
						 select
							 new PartnerReference
							 {
								 TableName = n.TableName.ToString()
							 };

			return result.ToList();
		}
    }
}