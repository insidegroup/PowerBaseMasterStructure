using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;


namespace CWTDesktopDatabase.Repository
{
    public class ClientSubUnitClientAccountRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
        //private string groupName = "Client Detail";

        //List of ClientAccounts in a ClientSubUnit - Paged
        public CWTPaginatedList<spDesktopDataAdmin_SelectPageClientSubUnitClientAccounts_v1Result> PageClientAccounts(string id, int page)
        {   
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPageClientSubUnitClientAccounts_v1(id, adminUserGuid, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPageClientSubUnitClientAccounts_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;

        }

		//SelectList of ClientAccounts in a ClientSubUnit - All
		public SelectList GetClientAccountsBySubUnit(string clientSubUnitGuid, string selected)
		{
			var accs = db.spDesktopDataAdmin_SelectClientSubUnitClientAccounts_v1(clientSubUnitGuid)
					  .Select(n => new
					  {
						  ClientAccountNumber = n.ClientAccountNumber + "|" + n.SourceSystemCode,
						  ClientAccountName = string.Format("{0}\xA0\xA0\xA0\xA0\xA0\xA0\xA0\xA0\xA0\xA0\xA0\xA0{1}", n.ClientAccountName, n.ClientAccountNumber)
					  })
					  .ToList();

			return new SelectList(accs, "ClientAccountNumber", "ClientAccountName", selected);
		}

		//List of ClientAccounts for a ClientSubUnit
		public List<ClientAccount> GetClientAccountsBySubUnit(string clientSubUnitGuid)
		{
			var result = from n in db.spDesktopDataAdmin_SelectClientSubUnitClientAccounts_v1(clientSubUnitGuid)
						 select new ClientAccount
						 {
							 ClientAccountNumber = n.ClientAccountNumber.Trim(),
							 ClientAccountName = n.ClientAccountName.Trim(),
							 SourceSystemCode = n.SourceSystemCode.Trim(),
							 ClientMasterCode = n.ClientMasterCode.Trim()
						 };

			return result.ToList(); 
		}

        public ClientSubUnitClientAccount GetClientSubUnitClientAccount(string clientAccountNumber, string sourceSystemCode, string clientSubUnitGuid)
        {
            return db.ClientSubUnitClientAccounts.SingleOrDefault(c => (c.ClientAccountNumber.Equals(clientAccountNumber) && c.SourceSystemCode.Equals(sourceSystemCode) && c.ClientSubUnitGuid.Equals(clientSubUnitGuid)));
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(ClientSubUnitClientAccount clientSubUnitClientAccount)
        {
            ClientAccountRepository clientAccountRepository = new ClientAccountRepository();
            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(clientSubUnitClientAccount.ClientAccountNumber, clientSubUnitClientAccount.SourceSystemCode);
            if (clientAccount != null)
            {
                clientSubUnitClientAccount.ClientAccountName = clientAccount.ClientAccountName;
            }

            ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitClientAccount.ClientSubUnitGuid);
            if (clientSubUnit != null)
            {
                clientSubUnitClientAccount.ClientSubUnitName = clientSubUnit.ClientSubUnitName;
            }

            ConfidenceLevelForLoadRepository confidenceLevelForLoadRepository = new ConfidenceLevelForLoadRepository();
            ConfidenceLevelForLoad confidenceLevelForLoad = new ConfidenceLevelForLoad();
            if (clientSubUnitClientAccount.ConfidenceLevelForLoadId != null)
            {
                int confidenceLevelForLoadId = (int)clientSubUnitClientAccount.ConfidenceLevelForLoadId;
                confidenceLevelForLoad = confidenceLevelForLoadRepository.GetConfidenceLevelForLoad(confidenceLevelForLoadId);
                if (confidenceLevelForLoad != null)
                {
                    clientSubUnitClientAccount.ConfidenceLevelForLoadDescription = confidenceLevelForLoad.ConfidenceLevelForLoadDescription;
                }
            }

            LineOfBusinessRepository lineOfBusinessRepository = new LineOfBusinessRepository();
            LineOfBusiness lineOfBusiness = new LineOfBusiness();
            if (clientSubUnitClientAccount.ClientAccountLineOfBusinessId != null)
            {
                int lineOfBusinessId = (int)clientSubUnitClientAccount.ClientAccountLineOfBusinessId;
                lineOfBusiness = lineOfBusinessRepository.GetLineOfBusiness(lineOfBusinessId);
                if (lineOfBusiness != null)
                {
                    clientSubUnitClientAccount.LineOfBusiness = lineOfBusiness;
                }
            }
        }

        //Add Item
        public void Add(ClientSubUnitClientAccount clientSubUnitClientAccount)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientSubUnitClientAccount_v1(
                clientSubUnitClientAccount.ClientSubUnitGuid,
                clientSubUnitClientAccount.SourceSystemCode,
                clientSubUnitClientAccount.ClientAccountNumber,
                clientSubUnitClientAccount.ConfidenceLevelForLoadId,
                clientSubUnitClientAccount.DefaultFlag,
                adminUserGuid
            );
        }

        //Delete Item
        public void Delete(ClientSubUnitClientAccount clientSubUnitClientAccount)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteClientSubUnitClientAccount_v1(
                clientSubUnitClientAccount.ClientSubUnitGuid,
                clientSubUnitClientAccount.SourceSystemCode,
                clientSubUnitClientAccount.ClientAccountNumber,
                adminUserGuid,
                clientSubUnitClientAccount.VersionNumber
            );

        }

        //Update item
        public void Update(ClientSubUnitClientAccount clientSubUnitClientAccount)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientSubUnitClientAccount_v1(
                clientSubUnitClientAccount.ClientSubUnitGuid,
                clientSubUnitClientAccount.SourceSystemCode,
                clientSubUnitClientAccount.ClientAccountNumber,
                clientSubUnitClientAccount.ClientAccountLineOfBusinessId,
                clientSubUnitClientAccount.DefaultFlag,
                adminUserGuid,
                clientSubUnitClientAccount.VersionNumber
             );


        }
    }
}
