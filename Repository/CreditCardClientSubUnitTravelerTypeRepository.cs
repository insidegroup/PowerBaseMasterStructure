using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Configuration;
using System.Transactions;

namespace CWTDesktopDatabase.Repository
{
    public class CreditCardClientSubUnitTravelerTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //List of ClientAccounts in a ClientSubUnitTravelerType - Not Sortable
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitTravelerTypeCreditCards_v1Result> GetCreditCardsBySubUnit(string clientSubUnitGuid, string travelerTypeGuid, int page, string sortField, int sortOrder)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            int totalRecords = 0;

            var result = db.spDesktopDataAdmin_SelectClientSubUnitTravelerTypeCreditCards_v1(clientSubUnitGuid, travelerTypeGuid, sortField, sortOrder, page).ToList();
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitTravelerTypeCreditCards_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

        public CreditCardClientSubUnitTravelerType GetCreditCardClientSubUnitTravelerType(int creditCardId)
        {
            return db.CreditCardClientSubUnitTravelerTypes.SingleOrDefault(c => (c.CreditCardId == creditCardId));
        }

        //Add Data From Linked Tables for Display
        /*public void EditForDisplay(CreditCardClientSubUnitTravelerType creditCardClientSubUnitTravelerType)
        {
            CreditCardRepository creditCardRepository = new CreditCardRepository();
            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(creditCardClientSubUnitTravelerType.CreditCardId);
            if (creditCard != null)
            {
                creditCardClientSubUnitTravelerType.CreditCardHolderName = creditCard.CreditCardHolderName;
            }

            ClientSubUnitTravelerTypeRepository clientSubUnitRepository = new ClientSubUnitTravelerTypeRepository();
            ClientSubUnitTravelerType clientSubUnit = new ClientSubUnitTravelerType();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnitTravelerType(creditCardClientSubUnitTravelerType.ClientSubUnitTravelerTypeGuid);
            if (clientSubUnit != null)
            {
                creditCardClientSubUnitTravelerType.ClientSubUnitTravelerTypeName = clientSubUnit.ClientSubUnitTravelerTypeName;
            }

        }
        */
        //Add CreditCard for ClientSubUnitTravelerType
        public void Add(CreditCard creditCard, string clientSubUnitGuid, string travelertypeGuid)
        {
            LogRepository logRepository = new LogRepository();
            string computerName = logRepository.GetComputerName();

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            string maskedCreditCardNumber = creditCard.CreditCardNumber.Substring(creditCard.CreditCardNumber.Length - 4).PadLeft(creditCard.CreditCardNumber.Length, '*');
			string maskedCVVNumber = (!string.IsNullOrEmpty(creditCard.CVV)) ? new string('*', creditCard.CVV.Length) : string.Empty;
			string creditCardToken = "";
            
			//Valid To Date Should Be End Of Month
            DateTime creditCardValidTo = new DateTime(creditCard.CreditCardValidTo.Year, creditCard.CreditCardValidTo.Month, 1).AddMonths(1).AddDays(-1);

            using (TransactionScope ts = TransactionUtils.CreateTransactionScope())
            {
                using (ccdbDC creditCardDatabase = new ccdbDC(ConfigurationManager.ConnectionStrings["CreditCardDatabase"].ConnectionString))
                {
                    creditCardDatabase.spDesktopDataAdmin_InsertCreditCard_v1(creditCard.CreditCardNumber, creditCard.CVV, ref creditCardToken);
                }
               
                using (HierarchyDC desktopDatabase = new HierarchyDC(Settings.getConnectionString()))
                {
                    desktopDatabase.spDesktopDataAdmin_InsertClientSubUnitTravelerTypeCreditCard_v1(
                        clientSubUnitGuid,
                        travelertypeGuid,
                        creditCardToken,
                        maskedCreditCardNumber,
						maskedCVVNumber,
						creditCard.CreditCardValidFrom,
                        creditCardValidTo,
                        creditCard.CreditCardIssueNumber,
                        creditCard.CreditCardHolderName,
                        creditCard.CreditCardVendorCode,
                        creditCard.ClientTopUnitGuid,
                        creditCard.CreditCardTypeId,
						creditCard.ProductId,
						creditCard.SupplierCode,
                        adminUserGuid,
                        Settings.ApplicationName(),
                        Settings.ApplicationVersion(),
                        null,
                        computerName,
                        null,
                        null,
                        null
                     );
                }
                ts.Complete();
            }
        }
        /*
        //Edit CreditCard for ClientAccount
        public void Edit(CreditCard creditCard, string OriginalCreditCardNumber)
        {

            //Valid To Date Should Be End Of Month
            DateTime creditCardValidTo = new DateTime(creditCard.CreditCardValidTo.Year, creditCard.CreditCardValidTo.Month, 1).AddMonths(1).AddDays(-1);

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientSubUnitTravelerTypeCreditCard_v1(
                creditCard.CreditCardId,
                creditCard.CreditCardNumber,
                OriginalCreditCardNumber,
                creditCard.CreditCardValidFrom,
                creditCardValidTo,
                creditCard.CreditCardIssueNumber,
                creditCard.CreditCardHolderName,
                creditCard.CreditCardVendorCode,
                creditCard.ClientTopUnitGuid,
                creditCard.CreditCardTypeId,
                adminUserGuid,
                creditCard.VersionNumber
            );
        }
        */
        //Delete Item
        public void Delete(CreditCard creditCard)
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
                    desktopDatabase.spDesktopDataAdmin_DeleteClientSubUnitTravelerTypeCreditCard_v1(
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
                        null
                    );

                }
                ts.Complete();
            }
        }

    }
}
