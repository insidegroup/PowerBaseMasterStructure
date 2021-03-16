using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace CWTDesktopDatabase.Repository
{
	public class ServiceFundRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of GDS Responses
		public CWTPaginatedList<spDesktopDataAdmin_SelectServiceFunds_v1Result> PageServiceFunds(int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectServiceFunds_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectServiceFunds_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get one ServiceFund
		public ServiceFund GetServiceFund(int serviceFundID)
		{
			return db.ServiceFunds.Where(c => c.ServiceFundId == serviceFundID).FirstOrDefault();
		}

		//Add Data From Linked Tables for Display
		public void EditGroupForDisplay(ServiceFund serviceFund)
		{
			//ClientTopUnit
			ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
			ClientTopUnit clientTopUnit = clientTopUnitRepository.GetClientTopUnit(serviceFund.ClientTopUnitGuid);
			if (clientTopUnit != null)
			{
				serviceFund.ClientTopUnitName = clientTopUnit.ClientTopUnitName;
			}

			//ClientAccount
			ClientAccountRepository clientAccountRepository = new ClientAccountRepository();
			ClientAccount clientAccount = clientAccountRepository.GetClientAccount(serviceFund.ClientAccountNumber, serviceFund.SourceSystemCode);
			if (clientAccount != null)
			{
				serviceFund.ClientAccountName = clientAccount.ClientAccountName;
			}

			//Supplier
			SupplierRepository supplierRepository = new SupplierRepository();
			Supplier supplier = supplierRepository.GetSupplier(serviceFund.SupplierCode, serviceFund.ProductId);
			if (supplier != null)
			{
				serviceFund.SupplierName = supplier.SupplierName;
			}

			//Product
			ProductRepository productRepository = new ProductRepository();
			Product product = productRepository.GetProduct(serviceFund.ProductId);
			if (product != null)
			{
				serviceFund.ProductName = product.ProductName;
			}

			//Times

			string serviceFundStartTime;

			if (serviceFund.ServiceFundStartTime.Hour < 10)
			{
				serviceFundStartTime = String.Concat("0", serviceFund.ServiceFundStartTime.Hour, ":");
			}
			else
			{
				serviceFundStartTime = String.Concat(serviceFund.ServiceFundStartTime.Hour, ":");
			}

			if (serviceFund.ServiceFundStartTime.Minute < 10)
			{
				serviceFundStartTime = String.Concat(serviceFundStartTime, "0", serviceFund.ServiceFundStartTime.Minute);
			}
			else
			{
				serviceFundStartTime = String.Concat(serviceFundStartTime, serviceFund.ServiceFundStartTime.Minute);
			}

			if (serviceFund.ServiceFundStartTime.Second != 0)
			{
				if (serviceFund.ServiceFundStartTime.Second < 10)
				{
					serviceFundStartTime = String.Concat(serviceFundStartTime, ":0", serviceFund.ServiceFundStartTime.Second);
				}
				else
				{
					serviceFundStartTime = String.Concat(serviceFundStartTime, ":", serviceFund.ServiceFundStartTime.Second);
				}
			}

			serviceFund.ServiceFundStartTimeString = serviceFundStartTime;

			string serviceFundEndTime;

			if (serviceFund.ServiceFundEndTime.Hour < 10)
			{
				serviceFundEndTime = String.Concat("0", serviceFund.ServiceFundEndTime.Hour, ":");
			}
			else
			{
				serviceFundEndTime = String.Concat(serviceFund.ServiceFundEndTime.Hour, ":");
			}

			if (serviceFund.ServiceFundEndTime.Minute < 10)
			{
				serviceFundEndTime = String.Concat(serviceFundEndTime, "0", serviceFund.ServiceFundEndTime.Minute);
			}
			else
			{
				serviceFundEndTime = String.Concat(serviceFundEndTime, serviceFund.ServiceFundEndTime.Minute);
			}

			if (serviceFund.ServiceFundEndTime.Second != 0)
			{
				if (serviceFund.ServiceFundEndTime.Second < 10)
				{
					serviceFundEndTime = String.Concat(serviceFundEndTime, ":0", serviceFund.ServiceFundEndTime.Second);
				}
				else
				{
					serviceFundEndTime = String.Concat(serviceFundEndTime, ":", serviceFund.ServiceFundEndTime.Second);
				}
			}

			serviceFund.ServiceFundEndTimeString = serviceFundEndTime;
		}

		//Add ServiceFund
		public void Add(ServiceFund serviceFund)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertServiceFund_v1(
				serviceFund.ClientTopUnitGuid,
				serviceFund.ServiceFundPseudoCityOrOfficeId,
				serviceFund.PCCCountryCode,
				serviceFund.FundUseStatus,
				serviceFund.ServiceFundQueue,
				serviceFund.ServiceFundStartTime,
				serviceFund.ServiceFundEndTime,
				serviceFund.TimeZoneRuleCode,
				serviceFund.ClientAccountNumber,
				serviceFund.SourceSystemCode,
				serviceFund.ProductId,
				serviceFund.SupplierCode,
				serviceFund.ServiceFundSavingsType,
				serviceFund.ServiceFundMinimumValue,
				serviceFund.ServiceFundCurrencyCode,
				serviceFund.ServiceFundRouting,
				serviceFund.ServiceFundClass,
				serviceFund.ServiceFundChannelTypeId,
				serviceFund.GDSCode,
                adminUserGuid
			);
		}

		//Edit ServiceFund
		public void Edit(ServiceFund serviceFund)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateServiceFund_v1(
				serviceFund.ServiceFundId,
				serviceFund.ClientTopUnitGuid,
				serviceFund.ServiceFundPseudoCityOrOfficeId,
				serviceFund.PCCCountryCode,
				serviceFund.FundUseStatus,
				serviceFund.ServiceFundQueue,
				serviceFund.ServiceFundStartTime,
				serviceFund.ServiceFundEndTime,
				serviceFund.TimeZoneRuleCode,
				serviceFund.ClientAccountNumber,
				serviceFund.SourceSystemCode,
				serviceFund.ProductId,
				serviceFund.SupplierCode,
				serviceFund.ServiceFundSavingsType,
				serviceFund.ServiceFundMinimumValue,
				serviceFund.ServiceFundCurrencyCode,
				serviceFund.ServiceFundRouting,
				serviceFund.ServiceFundClass,
				serviceFund.ServiceFundChannelTypeId,
				serviceFund.GDSCode,
                adminUserGuid,
				serviceFund.VersionNumber
			);
		}

		public void Delete(ServiceFund serviceFund)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteServiceFund_v1(
				serviceFund.ServiceFundId,
				adminUserGuid,
				serviceFund.VersionNumber
			);
		}

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailableServiceFund(string clientTopUnitGUID, string clientAccountNumber, int productId, string supplierCode, string serviceFundChannelTypeId, int? serviceFundId)
		{
			List<ServiceFund> serviceFunds = new List<ServiceFund>();

			if (serviceFundId.HasValue)
			{
				//if serviceFundId has value, we are editing and therefore can include the current value
				var result = from n in db.ServiceFunds
							 where n.ClientTopUnitGuid.Trim().Equals(clientTopUnitGUID)
							 && n.ClientAccountNumber.Equals(clientAccountNumber)
							 && n.ProductId.Equals(productId)
							 && n.SupplierCode.Equals(supplierCode)
							 && n.ServiceFundChannelTypeId.Equals(serviceFundChannelTypeId)
							 && !n.ServiceFundId.Equals(serviceFundId)
							 select n;
				serviceFunds = result.ToList();
			}
			else
			{
				var result = from n in db.ServiceFunds
							 where n.ClientTopUnitGuid.Trim().Equals(clientTopUnitGUID)
							 && n.ClientAccountNumber.Equals(clientAccountNumber)
							 && n.ProductId.Equals(productId)
							 && n.SupplierCode.Equals(supplierCode)
							 && n.ServiceFundChannelTypeId.Equals(serviceFundChannelTypeId)
							 select n;
				serviceFunds = result.ToList();
			}

			if (serviceFunds.Count > 0)
			{
				return false;   //already in use
			}
			else
			{
				return true;
			}
		}
	}
}