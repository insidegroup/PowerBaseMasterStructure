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
	public class CreditCardClientAccountRepository
	{

		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List of ClientAccounts in a ClientSubUnit - Now Sortable
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientAccountCreditCards_v1Result> GetCreditCardsByClientAccount(string clientAccountNumber, string sourceSystemCode, int page, string sortField, int sortOrder)
		{
			int totalRecords = 0;

			var result = db.spDesktopDataAdmin_SelectClientAccountCreditCards_v1(sourceSystemCode, clientAccountNumber, sortField, sortOrder, page).ToList();
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientAccountCreditCards_v1Result>(result, page, totalRecords);
			return paginatedView;

		}

		public CreditCardClientAccount GetCreditCardClientAccount(int creditCardId)
		{
			return db.CreditCardClientAccounts.SingleOrDefault(c => (c.CreditCardId == creditCardId));
		}

		//Add Data From Linked Tables for Display
		/*public void EditForDisplay(CreditCardClientAccount creditCardClientAccount)
		{
			CreditCardRepository creditCardRepository = new CreditCardRepository();
			CreditCard creditCard = new CreditCard();
			creditCard = creditCardRepository.GetCreditCard(creditCardClientAccount.CreditCardId);
			if (creditCard != null)
			{
				creditCardClientAccount.CreditCardHolderName = creditCard.CreditCardHolderName;
			}

			ClientAccountRepository clientAccountRepository = new ClientAccountRepository();
			ClientAccount clientAccount = new ClientAccount();
			clientAccount = clientAccountRepository.GetClientAccount(creditCardClientAccount.ClientAccountNumber, creditCardClientAccount.SourceSystemCode);
			if (clientAccount != null)
			{
				creditCardClientAccount.ClientAccountName = clientAccount.ClientAccountName;
			}

		}
		*/
		//Add CreditCard for ClientAccount
		public void Add(CreditCard creditCard, string can, string ssc)
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
					desktopDatabase.spDesktopDataAdmin_InsertClientAccountCreditCard_v1(
						ssc,
						can,
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
						null,
						null
					);
				}
				ts.Complete();
			}
		}




		//Delete CreditCard for ClientAccount
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
					desktopDatabase.spDesktopDataAdmin_DeleteClientAccountCreditCard_v1(
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
						null,
						null
					);

				}
				ts.Complete();
			}
		}

	}
}
