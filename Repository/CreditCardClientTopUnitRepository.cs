using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Transactions;
using System.Configuration;

namespace CWTDesktopDatabase.Repository
{
    public class CreditCardClientTopUnitRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //List of ClientAccounts in a ClientTopUnit - Now Sortable
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitCreditCards_v1Result> GetCreditCardsByTopUnit(string clientTopUnitGuid, string filter, int page, string sortField, int sortOrder)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            int totalRecords = 0;

            var result = db.spDesktopDataAdmin_SelectClientTopUnitCreditCards_v1(clientTopUnitGuid, filter, sortField, sortOrder, page).ToList();
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitCreditCards_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

        public CreditCard GetCreditCardClientTopUnit(int creditCardId)
        {
            //CreditCardClientTopUnit differes from other CreditCards as it has no link table, the Id is stored in the CreditCard table
            HierarchyDC db2 = new HierarchyDC(Settings.getConnectionString());
            return db2.CreditCards.SingleOrDefault(c => (c.CreditCardId == creditCardId));
        }

        //Add Data From Linked Tables for Display
        /*public void EditForDisplay(CreditCardClientTopUnit creditCardClientTopUnit)
        {
            CreditCardRepository creditCardRepository = new CreditCardRepository();
            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(creditCardClientTopUnit.CreditCardId);
            if (creditCard != null)
            {
                creditCardClientTopUnit.CreditCardHolderName = creditCard.CreditCardHolderName;
            }

            ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(creditCardClientTopUnit.ClientTopUnitGuid);
            if (clientTopUnit != null)
            {
                creditCardClientTopUnit.ClientTopUnitName = clientTopUnit.ClientTopUnitName;
            }

        }
         * */

        //Add CreditCard for ClientTopUnit
        public void Add(CreditCard creditCard, string clientTopUnitGuid)
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
                    desktopDatabase.spDesktopDataAdmin_InsertClientTopUnitCreditCard_v1(
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
                        null,
                        null,
                        null
                        );
                }
                ts.Complete();
            }
        }

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
                    desktopDatabase.spDesktopDataAdmin_DeleteClientTopUnitCreditCard_v1(
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

    }
}
