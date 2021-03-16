using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System.Configuration;
using System.Data.Common;
using System.Transactions;
 
namespace CWTDesktopDatabase.Repository
{
    public class CreditCardRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //List of CWT Credit Cards - Not Sortable
        public CWTPaginatedList<spDesktopDataAdmin_SelectCWTCreditCards_v1Result> GetCWTCreditCards( int page)
        {
            int totalRecords = 0;

            var result = db.spDesktopDataAdmin_SelectCWTCreditCards_v1(page).ToList();
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectCWTCreditCards_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

        //Get a Card
        //LastRetrievedTimestamp is updated when we retrieve a card to Edit, but not when we retrieve to Delete or View
        public CreditCard GetCreditCard(int creditCardId, bool updateLastRetrievedTimestamp)
        {
           // return db.spDesktopDataAdmin_SelectCreditCard_v1(creditCardId).FirstOrDefault();
            var result = (from n in db.spDesktopDataAdmin_SelectCreditCard_v1(creditCardId)
                select new CreditCard
                {
                    CreditCardId = n.CreditCardId,
                    MaskedCreditCardNumber = n.MaskedCreditCardNumber,
                    CreditCardValidFrom = n.CreditCardValidFrom,
                    CreditCardValidTo = n.CreditCardValidTo,
                    CreditCardIssueNumber = n.CreditCardIssueNumber,
                    CreditCardHolderName = n.CreditCardHolderName,
                    CreditCardVendorCode = n.CreditCardVendorCode,
                    ClientTopUnitGuid = n.ClientTopUnitGuid,
                    CreditCardTypeId = n.CreditCardTypeId,
                    CreditCardToken = n.CreditCardToken,
					MaskedCVVNumber = n.MaskedCVVNumber,
					ProductId = n.ProductId,
					ProductName = n.ProductName,
					SupplierCode = n.SupplierCode,
					SupplierName = n.SupplierName,
                    VersionNumber = n.VersionNumber
                }).FirstOrDefault();

			if (result != null && result.CreditCardToken != null)
            {
                if ((bool)updateLastRetrievedTimestamp)
                {
                    using (ccdbDC creditCardDatabase = new ccdbDC(ConfigurationManager.ConnectionStrings["CreditCardDatabase"].ConnectionString))
                    {
                        creditCardDatabase.spDesktopDataAdmin_UpdateCreditCard_v1(result.CreditCardToken);
                    }

                }
            }

            return result;
        }
  
        //Get a Card
        //LastRetrievedTimestamp is updated when we retrieve a card to Edit, but not when we retrieve to Delete or View
        public CreditCardEditable GetCreditCardEditable(int creditCardId, bool updateLastRetrievedTimestamp)
        {
           // return db.spDesktopDataAdmin_SelectCreditCard_v1(creditCardId).FirstOrDefault();
            var result = (from n in db.spDesktopDataAdmin_SelectCreditCard_v1(creditCardId)
                select new CreditCardEditable
                {
                    CreditCardId = n.CreditCardId,
                    MaskedCreditCardNumber = n.MaskedCreditCardNumber,
                    CreditCardValidFrom = n.CreditCardValidFrom,
                    CreditCardValidTo = n.CreditCardValidTo,
                    CreditCardIssueNumber = n.CreditCardIssueNumber,
                    CreditCardHolderName = n.CreditCardHolderName,
                    CreditCardVendorCode = n.CreditCardVendorCode,
                    ClientTopUnitGuid = n.ClientTopUnitGuid,
                    CreditCardTypeId = n.CreditCardTypeId,
                    CreditCardToken = n.CreditCardToken,
                    VersionNumber = n.VersionNumber
                }).FirstOrDefault();
            if (result.CreditCardToken != null)
            {
                if ((bool)updateLastRetrievedTimestamp)
                {
                    using (ccdbDC creditCardDatabase = new ccdbDC(ConfigurationManager.ConnectionStrings["CreditCardDatabase"].ConnectionString))
                    {
                        creditCardDatabase.spDesktopDataAdmin_UpdateCreditCard_v1(result.CreditCardToken);
                    }

                }
            }
            return result;
        }

        //Add Data From Linked Tables for Display
        //type is optional to letus know if it is a TopUnit card, which has no links to other tables
        public void EditForDisplay(CreditCard creditCard)
        {
            ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(creditCard.ClientTopUnitGuid);
            if (clientTopUnit != null)
            {
                creditCard.ClientTopUnitName = clientTopUnit.ClientTopUnitName;
            }

            CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
            CreditCardVendor creditCardVendor = new CreditCardVendor();
            creditCardVendor = creditCardVendorRepository.GetCreditCardVendor(creditCard.CreditCardVendorCode);
            if (creditCardVendor != null)
            {
                creditCard.CreditCardVendorName = creditCardVendor.CreditCardVendorName;
            }

            if (creditCard.CreditCardTypeId != null)
            {
                int creditCardTypeId = (int)creditCard.CreditCardTypeId;
                CreditCardTypeRepository creditCardTypeRepository = new CreditCardTypeRepository();
                CreditCardType creditCardType = new CreditCardType();
                creditCardType = creditCardTypeRepository.GetCreditCardType(creditCardTypeId);
                if (creditCardType != null)
                {
                    creditCard.CreditCardTypeDescription = creditCardType.CreditCardTypeDescription;
                }
            }

            //If Linked to ClientSubUnit
            CreditCardClientSubUnit creditCardClientSubUnit = new CreditCardClientSubUnit();
            CreditCardClientSubUnitRepository creditCardClientSubUnitRepository = new CreditCardClientSubUnitRepository();
            creditCardClientSubUnit = creditCardClientSubUnitRepository.GetCreditCardClientSubUnit(creditCard.CreditCardId);

            if (creditCardClientSubUnit != null)
            {
                ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
                ClientSubUnit clientSubUnit = new ClientSubUnit();
                clientSubUnit = clientSubUnitRepository.GetClientSubUnit(creditCardClientSubUnit.ClientSubUnitGuid);
                if (clientSubUnit != null)
                {
                    creditCard.ClientSubUnitGuid = clientSubUnit.ClientSubUnitGuid;
                    creditCard.ClientSubUnitName = clientSubUnit.ClientSubUnitName;
                    creditCard.ClientTopUnitGuid = clientSubUnit.ClientTopUnit.ClientTopUnitGuid;
                    creditCard.ClientTopUnitName = clientSubUnit.ClientTopUnit.ClientTopUnitName;
                }
            }

            //If Linked to ClientAccount
            CreditCardClientAccount creditCardClientAccount = new CreditCardClientAccount();
            CreditCardClientAccountRepository creditCardClientAccountRepository = new CreditCardClientAccountRepository();
            creditCardClientAccount = creditCardClientAccountRepository.GetCreditCardClientAccount(creditCard.CreditCardId);

            if (creditCardClientAccount != null)
            {
                ClientAccountRepository clientAccountRepository = new ClientAccountRepository();
                ClientAccount clientAccount = new ClientAccount();
                clientAccount = clientAccountRepository.GetClientAccount(creditCardClientAccount.ClientAccountNumber, creditCardClientAccount.SourceSystemCode);
                if (clientAccount != null)
                {
                    creditCard.ClientAccountNumber = clientAccount.ClientAccountNumber;
                    creditCard.ClientAccountName = clientAccount.ClientAccountName;
                    creditCard.SourceSystemCode = clientAccount.SourceSystemCode;
                }
            }

           
                HierarchyRepository hierarchyRepository = new HierarchyRepository();

                fnDesktopDataAdmin_SelectCreditCardHierarchy_v1Result hierarchy = new fnDesktopDataAdmin_SelectCreditCardHierarchy_v1Result();
                hierarchy = GetGroupHierarchy(creditCard.CreditCardId);

                if (hierarchy == null)
                {
                }
                else
                {
                    creditCard.HierarchyType = hierarchy.HierarchyType;
                    creditCard.HierarchyCode = hierarchy.HierarchyCode.ToString();
                    creditCard.HierarchyItem = hierarchy.HierarchyName.Trim();

                    if (hierarchy.HierarchyType == "ClientSubUnitTravelerType")
                    {
                        creditCard.ClientSubUnitGuid = hierarchy.HierarchyCode.ToString();
                        creditCard.ClientSubUnitName = hierarchy.HierarchyName.Trim();
                        creditCard.TravelerTypeGuid = hierarchy.TravelerTypeGuid;
                        creditCard.TravelerTypeName = hierarchy.TravelerTypeName.Trim();
                    }
                }
            }

        //Add Data From Linked Tables for Display
        //type is optional to letus know if it is a TopUnit card, which has no links to other tables
        public void EditForDisplay(CreditCardEditable creditCard)
        {
            ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(creditCard.ClientTopUnitGuid);
            if (clientTopUnit != null)
            {
                creditCard.ClientTopUnitName = clientTopUnit.ClientTopUnitName;
            }

            CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
            CreditCardVendor creditCardVendor = new CreditCardVendor();
            creditCardVendor = creditCardVendorRepository.GetCreditCardVendor(creditCard.CreditCardVendorCode);
            if (creditCardVendor != null)
            {
                creditCard.CreditCardVendorName = creditCardVendor.CreditCardVendorName;
            }

            if (creditCard.CreditCardTypeId != null)
            {
                int creditCardTypeId = (int)creditCard.CreditCardTypeId;
                CreditCardTypeRepository creditCardTypeRepository = new CreditCardTypeRepository();
                CreditCardType creditCardType = new CreditCardType();
                creditCardType = creditCardTypeRepository.GetCreditCardType(creditCardTypeId);
                if (creditCardType != null)
                {
                    creditCard.CreditCardTypeDescription = creditCardType.CreditCardTypeDescription;
                }
            }

            //If Linked to ClientSubUnit
            CreditCardClientSubUnit creditCardClientSubUnit = new CreditCardClientSubUnit();
            CreditCardClientSubUnitRepository creditCardClientSubUnitRepository = new CreditCardClientSubUnitRepository();
            creditCardClientSubUnit = creditCardClientSubUnitRepository.GetCreditCardClientSubUnit(creditCard.CreditCardId);

            if (creditCardClientSubUnit != null)
            {
                ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
                ClientSubUnit clientSubUnit = new ClientSubUnit();
                clientSubUnit = clientSubUnitRepository.GetClientSubUnit(creditCardClientSubUnit.ClientSubUnitGuid);
                if (clientSubUnit != null)
                {
                    creditCard.ClientSubUnitGuid = clientSubUnit.ClientSubUnitGuid;
                    creditCard.ClientSubUnitName = clientSubUnit.ClientSubUnitName;
                    creditCard.ClientTopUnitGuid = clientSubUnit.ClientTopUnit.ClientTopUnitGuid;
                    creditCard.ClientTopUnitName = clientSubUnit.ClientTopUnit.ClientTopUnitName;
                }
            }

            //If Linked to ClientAccount
            CreditCardClientAccount creditCardClientAccount = new CreditCardClientAccount();
            CreditCardClientAccountRepository creditCardClientAccountRepository = new CreditCardClientAccountRepository();
            creditCardClientAccount = creditCardClientAccountRepository.GetCreditCardClientAccount(creditCard.CreditCardId);

            if (creditCardClientAccount != null)
            {
                ClientAccountRepository clientAccountRepository = new ClientAccountRepository();
                ClientAccount clientAccount = new ClientAccount();
                clientAccount = clientAccountRepository.GetClientAccount(creditCardClientAccount.ClientAccountNumber, creditCardClientAccount.SourceSystemCode);
                if (clientAccount != null)
                {
                    creditCard.ClientAccountNumber = clientAccount.ClientAccountNumber;
                    creditCard.ClientAccountName = clientAccount.ClientAccountName;
                    creditCard.SourceSystemCode = clientAccount.SourceSystemCode;
                }
            }

           
                HierarchyRepository hierarchyRepository = new HierarchyRepository();

                fnDesktopDataAdmin_SelectCreditCardHierarchy_v1Result hierarchy = new fnDesktopDataAdmin_SelectCreditCardHierarchy_v1Result();
                hierarchy = GetGroupHierarchy(creditCard.CreditCardId);

                if (hierarchy == null)
                {
                }
                else
                {
                    creditCard.HierarchyType = hierarchy.HierarchyType;
                    creditCard.HierarchyCode = hierarchy.HierarchyCode.ToString();
                    creditCard.HierarchyItem = hierarchy.HierarchyName.Trim();

                    if (hierarchy.HierarchyType == "ClientSubUnitTravelerType")
                    {
                        creditCard.ClientSubUnitGuid = hierarchy.HierarchyCode.ToString();
                        creditCard.ClientSubUnitName = hierarchy.HierarchyName.Trim();
                        creditCard.TravelerTypeGuid = hierarchy.TravelerTypeGuid;
                        creditCard.TravelerTypeName = hierarchy.TravelerTypeName.Trim();
                    }
                }
            }

        //Get Hierarchy Details
        public fnDesktopDataAdmin_SelectCreditCardHierarchy_v1Result GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectCreditCardHierarchy_v1(id).FirstOrDefault();
            return result;
        }

        //Get CreditCard Behavior (Editable and Are Real Numbers Allowed
        public CreditCardBehavior GetCreditCardBehavior()
        {
            string databaseConnectionName = Settings.getConnectionString();

            DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
            builder.ConnectionString = databaseConnectionName;
            string databaseName = builder["Initial Catalog"] as string;

            var result = (from n in db.spDesktopDataAdmin_SelectCreditCardBehaviour_v1(databaseName)
                         select new CreditCardBehavior
                              {
                                  CanHaveRealCreditCardsFlag = n.CanHaveRealCreditCardsFlag,
                                  CanChangeCreditCardsFlag = n.CanChangeCreditCardsFlag
                              }).ToList();


            if (result != null)
            {
                return result.FirstOrDefault();
            }
            else
            {
                CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
                creditCardBehavior.CanChangeCreditCardsFlag = false;
                creditCardBehavior.CanHaveRealCreditCardsFlag = false;
                return creditCardBehavior;
            }
            
        }
        
        //Edit CreditCard
		public void Edit(CreditCardEditable creditCard)
		{
			LogRepository logRepository = new LogRepository();
			string computerName = logRepository.GetComputerName();
			string maskedCVVNumber = creditCard.MaskedCVVNumber;
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			//Valid To Date Should Be End Of Month (Removed v19.0.1 for MNG Cards that expire mid month)
			//DateTime creditCardValidTo = new DateTime(creditCard.CreditCardValidTo.Year, creditCard.CreditCardValidTo.Month, 1).AddMonths(1).AddDays(-1);

			if(creditCard.MaskedCVVNumber == null && creditCard.CVV != null)
			{
				maskedCVVNumber = new string('*', creditCard.CVV.Length);
				using (ccdbDC creditCardDatabase = new ccdbDC(ConfigurationManager.ConnectionStrings["CreditCardDatabase"].ConnectionString))
				{
					creditCardDatabase.spDesktopDataAdmin_UpdateCreditCardCVVNumber_v1(creditCard.CreditCardToken, creditCard.CVV);
				}
			}

			using (HierarchyDC desktopDatabase = new HierarchyDC(Settings.getConnectionString()))
			{
				desktopDatabase.spDesktopDataAdmin_UpdateCreditCard_v1(
				  creditCard.CreditCardId,
				  creditCard.CreditCardValidFrom,
                  creditCard.CreditCardValidTo,
				  creditCard.CreditCardIssueNumber,
				  creditCard.CreditCardHolderName,
				  creditCard.CreditCardTypeId,
				  creditCard.ProductId,
				  creditCard.SupplierCode,
				  maskedCVVNumber,
				  adminUserGuid,
				  creditCard.VersionNumber,
				  Settings.ApplicationName(),
				  Settings.ApplicationVersion(),
				  null,
				  computerName,
				  null,
				  null,
				  creditCard.ClientSubUnitGuid,
				  creditCard.TravelerTypeGuid,
				  null
			  );
			}
		}

        //Edit CreditCard
        public void EditCWTCreditCard(CreditCardEditable creditCard, string OriginalCreditCardNumber)
        {
            LogRepository logRepository = new LogRepository();
            string computerName = logRepository.GetComputerName(); 

            //Valid To Date Should Be End Of Month
            DateTime creditCardValidTo = new DateTime(creditCard.CreditCardValidTo.Year, creditCard.CreditCardValidTo.Month, 1).AddMonths(1).AddDays(-1);

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateCWTCreditCard_v1(
                creditCard.CreditCardId,
                creditCard.CreditCardValidFrom,
                creditCardValidTo,
                creditCard.CreditCardIssueNumber,
                creditCard.CreditCardHolderName,
                creditCard.CreditCardVendorCode,
                creditCard.ClientTopUnitGuid,
                creditCard.CreditCardTypeId,
                creditCard.HierarchyType,
                creditCard.HierarchyCode,
                creditCard.TravelerTypeGuid,
                creditCard.ClientSubUnitGuid,
                creditCard.SourceSystemCode,
                adminUserGuid,
                creditCard.VersionNumber,
                Settings.ApplicationName(),
                Settings.ApplicationVersion(),
                null,
                computerName,
                null,
                null,
                12,
                null
            );
        }

        //Add Group
        public void AddCWTCreditCard(CreditCard creditCard)
        {
            LogRepository logRepository = new LogRepository();
            string computerName = logRepository.GetComputerName();

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            string maskedCreditCardNumber = creditCard.CreditCardNumber.Substring(creditCard.CreditCardNumber.Length - 4).PadLeft(creditCard.CreditCardNumber.Length, '*');
			string maskedCVVNumber = (!string.IsNullOrEmpty(creditCard.CVV)) ? new string('*', creditCard.CVV.Length) : string.Empty;
			string creditCardToken = "";
            
			//Valid To Date Should Be End Of Month
            DateTime creditCardValidTo = new DateTime(creditCard.CreditCardValidTo.Year, creditCard.CreditCardValidTo.Month, 1).AddMonths(1).AddDays(-1);
                                
            using(TransactionScope ts = TransactionUtils.CreateTransactionScope())
            {
                using (ccdbDC creditCardDatabase = new ccdbDC(ConfigurationManager.ConnectionStrings["CreditCardDatabase"].ConnectionString))
                {
                    creditCardDatabase.spDesktopDataAdmin_InsertCreditCard_v1(creditCard.CreditCardNumber, creditCard.CVV, ref creditCardToken);
                }
                using (HierarchyDC desktopDatabase = new HierarchyDC(Settings.getConnectionString()))
                {
                    desktopDatabase.spDesktopDataAdmin_InsertCWTCreditCard_v1(
                        creditCardToken,
                        maskedCreditCardNumber,
                        creditCard.CreditCardValidFrom,
                        creditCardValidTo,
                        creditCard.CreditCardIssueNumber,
                        creditCard.CreditCardHolderName,
                        creditCard.CreditCardVendorCode,
                        creditCard.ClientTopUnitGuid,
                        creditCard.CreditCardTypeId,
                        creditCard.HierarchyType,
                        creditCard.HierarchyCode,
                        creditCard.TravelerTypeGuid,
                        creditCard.ClientSubUnitGuid,
                        creditCard.SourceSystemCode,
                        adminUserGuid,
                        Settings.ApplicationName(),
                        Settings.ApplicationVersion(),
                        null,
                        computerName,
                        null,
                        null,
                        11,
                        null
                    );
                }
                ts.Complete();
            }
        }

        //Delete Item
        public void DeleteCWTCreditCard(CreditCard creditCard)
        {
            LogRepository logRepository = new LogRepository();
            string computerName = logRepository.GetComputerName();
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            using (TransactionScope ts = TransactionUtils.CreateTransactionScope())
            {
                using (ccdbDC creditCardDatabase = new ccdbDC(ConfigurationManager.ConnectionStrings["CreditCardDatabase"].ConnectionString))
                {
                    creditCardDatabase.spDesktopDataAdmin_DeleteCreditCard_v1(creditCard.CreditCardToken);
                }
                using (HierarchyDC desktopDatabase = new HierarchyDC(Settings.getConnectionString()))
                {
                    desktopDatabase.spDesktopDataAdmin_DeleteCWTCreditCard_v1(
                        creditCard.CreditCardId,
                        adminUserGuid,
                        creditCard.VersionNumber,
                        Settings.ApplicationName(),
                        Settings.ApplicationVersion(),
                        null,
                        computerName,
                        null,
                        null,
                        13,
                        null,
                        null,
                        null
                     );

                }
                ts.Complete();
            }
        }
        //Inserts a CreditCard to Credit Card Database, return Identifier String
        /*public string AddCreditCard_UNUSED(string creditCardNumber)
        {

            string creditCardToken = "";

            try
            {
                string ccdbConnectionString = ConfigurationManager.AppSettings["CreditCardDatabase"];
                ccdbDC creditCardDatabase = new ccdbDC(ccdbConnectionString);
                creditCardDatabase.spDesktopDataAdmin_InsertCreditCard_v1(creditCardNumber, ref creditCardToken);
            }
            catch
            {

            }
            return creditCardToken;
        }
        */
        //Add Group
        /*public void AddCWTCreditCard_ORIG_BACKUP(CreditCard creditCard)
        {
            LogRepository logRepository = new LogRepository();
            string computerName = logRepository.GetComputerName();

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            //Valid To Date Should Be End Of Month
            DateTime creditCardValidTo = new DateTime(creditCard.CreditCardValidTo.Year, creditCard.CreditCardValidTo.Month, 1).AddMonths(1).AddDays(-1);

            db.spDesktopDataAdmin_InsertCWTCreditCard_v1(
                creditCard.CreditCardNumber,
                creditCard.CreditCardValidFrom,
                creditCardValidTo,
                creditCard.CreditCardIssueNumber,
                creditCard.CreditCardHolderName,
                creditCard.CreditCardVendorCode,
                creditCard.ClientTopUnitGuid,
                creditCard.CreditCardTypeId,
                creditCard.HierarchyType,
                creditCard.HierarchyCode,
                creditCard.TravelerTypeGuid,
                creditCard.ClientSubUnitGuid,
                creditCard.SourceSystemCode,
                adminUserGuid,
                Settings.ApplicationName(),
                Settings.ApplicationVersion(),
                null,
                computerName,
                null,
                null,
                11,
                null
            );
        }*/
    
    }

}
