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
    public class CreditCardClientSubUnitRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //List of CreditCards in a ClientSubUnit - Now Sortable
        public CWTPaginatedList<spDesktopDataAdmin_SelectPageClientSubUnitCreditCards_v1Result> GetCreditCardsBySubUnit(string clientSubUnitGuid, string filter, int page, string  sortField, int sortOrder)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            int totalRecords = 0;

			var result = db.spDesktopDataAdmin_SelectPageClientSubUnitCreditCards_v1(clientSubUnitGuid, sortField, sortOrder, page, filter).ToList();
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPageClientSubUnitCreditCards_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

        //All  CreditCards for a ClientSubUnit
        public List<CreditCard> GetAllCreditCardsBySubUnit(string clientSubUnitGuid)
        {

            return (from n in db.spDesktopDataAdmin_SelectClientSubUnitCreditCards_v1(clientSubUnitGuid, string.Empty, null)
                    select new CreditCard
                         {
                             CreditCardValidTo = n.CreditCardValidTo,
                             MaskedCreditCardNumber = n.MaskedCreditCardNumber,
                             CreditCardId = n.CreditCardId,
							 CreditCardHolderName = n.CreditCardHolderName
                         }).ToList();
                  
        }


        public CreditCardClientSubUnit GetCreditCardClientSubUnit(int creditCardId)
        {
            return db.CreditCardClientSubUnits.SingleOrDefault(c => (c.CreditCardId == creditCardId));
        }



        //Add Data From Linked Tables for Display
        /*public void EditForDisplay(CreditCardClientSubUnit creditCardClientSubUnit)
        {
            CreditCardRepository creditCardRepository = new CreditCardRepository();
            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(creditCardClientSubUnit.CreditCardId);
            if (creditCard != null)
            {
                creditCardClientSubUnit.CreditCardHolderName = creditCard.CreditCardHolderName;
            }

            ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(creditCardClientSubUnit.ClientSubUnitGuid);
            if (clientSubUnit != null)
            {
                creditCardClientSubUnit.ClientSubUnitName = clientSubUnit.ClientSubUnitName;
            }

        }
         * */

        //Add CreditCard for ClientSubUnit
        public void Add(CreditCard creditCard, string clientSubUnitGuid)
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
                    desktopDatabase.spDesktopDataAdmin_InsertClientSubUnitCreditCard_v1(
                        clientSubUnitGuid,
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
                        null
                    );
                }
                ts.Complete();
            }
        }

        //Delete Item
        public void Delete(CreditCard creditCard, CreditCardClientSubUnit creditCardClientSubUnit)
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
                    desktopDatabase.spDesktopDataAdmin_DeleteClientSubUnitCreditCard_v1(
                    creditCardClientSubUnit.CreditCardId,
                    adminUserGuid,
                    creditCardClientSubUnit.VersionNumber,
                    Settings.ApplicationName(),
                    Settings.ApplicationVersion(),
                    null,
                    computerName,
                    null,
                    null,
                    13,
                    null,
                    null
                    );
                }
                ts.Complete();
            }

        }
    }
}
