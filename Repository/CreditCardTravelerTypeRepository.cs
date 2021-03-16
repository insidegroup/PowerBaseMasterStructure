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
    public class CreditCardTravelerTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //List of ClientAccounts in a TravelerType - Not Sortable
        public CWTPaginatedList<spDesktopDataAdmin_SelectTravelerTypeCreditCards_v1Result> GetCreditCards(string travelerTypeGuid, int page, string filter, string sortField, int sortOrder)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            int totalRecords = 0;

            var result = db.spDesktopDataAdmin_SelectTravelerTypeCreditCards_v1(travelerTypeGuid, filter, sortField, sortOrder, page).ToList();
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectTravelerTypeCreditCards_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

        public CreditCardTravelerType GetCreditCardTravelerType(int creditCardId)
        {
            return db.CreditCardTravelerTypes.SingleOrDefault(c => (c.CreditCardId == creditCardId));
        }

        //Add Data From Linked Tables for Display
        /*public void EditForDisplay(CreditCardTravelerType creditCardTravelerType)
        {
            CreditCardRepository creditCardRepository = new CreditCardRepository();
            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(creditCardTravelerType.CreditCardId);
            if (creditCard != null)
            {
                creditCardTravelerType.CreditCardHolderName = creditCard.CreditCardHolderName;
            }

            TravelerTypeRepository clientSubUnitRepository = new TravelerTypeRepository();
            TravelerType clientSubUnit = new TravelerType();
            clientSubUnit = clientSubUnitRepository.GetTravelerType(creditCardTravelerType.TravelerTypeGuid);
            if (clientSubUnit != null)
            {
                creditCardTravelerType.TravelerTypeName = clientSubUnit.TravelerTypeName;
            }

        }
        */
        //Add CreditCard for TravelerType
        public void Add(CreditCard creditCard, string travelertypeGuid)
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
                    desktopDatabase.spDesktopDataAdmin_InsertTravelerTypeCreditCard_v1(
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
                        creditCard.ClientSubUnitGuid,
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
                    desktopDatabase.spDesktopDataAdmin_DeleteTravelerTypeCreditCard_v1(
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
                        creditCard.ClientSubUnitGuid,
                        null
                    );

                }
                ts.Complete();
            }
        }

    }
}

