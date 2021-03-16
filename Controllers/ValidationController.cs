using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Globalization;
using System.Web.UI;

using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Controllers
{
     [AjaxTimeOutCheck]
    public class ValidationController : Controller
    {
        ValidationRepository validationRepository = new ValidationRepository();

        //IsValidCityCode
        public JsonResult IsValidCityCodeUNUSEDOLD(string cityCode)
        {
            var result =  Json(validationRepository.IsValidCityCode(cityCode));
            return result;
        }
        
		//IsValidCityCode
		[HttpPost]
		public JsonResult IsValidCityCode(string cityCode)
        {
            City c = new City();
            c = validationRepository.IsValidCityCode(cityCode);

            if (c == null)
            {
				return Json(null);
            }
            else
            {
                return Json(new{city = new[] {new Dictionary<string, object> { { "CityCode", c.CityCode }}}});
            }
            //{"city":[{"CityCode":"AES"}]}
        }
        
         //IsValidClientSubUnitCDR combination for a ClientSubUnitCDR
        public JsonResult IsValidClientSubUnitCDR(int? cdrId, string csuGuid, string cdrValue, string canssc, int? ccId)
        {
            return Json(validationRepository.IsValidClientSubUnitCDR(cdrId, csuGuid, cdrValue, canssc, ccId));
        }

        //IsValidSupplierProduct combination for a ClientDetail item
        public JsonResult IsValidClientDetailSupplierProduct(int clientDetailId, int productId, string supplierCode)
        {
            return Json(validationRepository.IsValidClientDetailSupplierProduct(clientDetailId, productId, supplierCode));
        }

		//IsValidSupplierCode
		public JsonResult IsValidSupplierCode(string supplierCode)
		{
			return Json(validationRepository.IsValidSupplierCode(supplierCode));
		}
		 //IsValidSupplierName
        public JsonResult IsValidSupplierName(string supplierName)
        {
            return Json(validationRepository.IsValidSupplierName(supplierName));
        }

		//IsValidSupplierProduct combination
		public JsonResult IsValidSupplierProduct(int productId, string supplierCode)
		{
			return Json(validationRepository.IsValidSupplierProduct(productId, supplierCode));
		}

		//IsValidSupplierProduct combination
		public JsonResult IsValidSupplierProductName(int productId, string supplierName)
		{
			return Json(validationRepository.IsValidSupplierProductName(productId, supplierName));
		}

        //is the country valid (and user has access) - no longer used
        public JsonResult IsValidCountryWithLocations(string countryName)
        {
            return Json(validationRepository.IsValidCountryWithLocations(countryName));
        }

        //is the country valid (and user has access)
		[HttpPost]
		public JsonResult IsValidAdminUserCountry(string countryName)
        {
            return Json(validationRepository.IsValidAdminUserCountry(countryName));
        }

        //is the location valid (and user has access)
        public JsonResult IsValidLocation(string locationName)
        {
            return Json(validationRepository.IsValidLocation(locationName));
        }

        //Validation of CountryCodes for APISCountries
        [HttpPost]
        public JsonResult IsValidAPISCountries(string occ, string dcc)
        {

            var result = validationRepository.IsValidAPISCountries(occ, dcc);
            return Json(result);
        }

        // POST: FOr Validation of PolicyRouting From/To
        [HttpPost]
        public JsonResult IsValidPolicyRoutingFromTo(string fromTo, string codeType)
        {
            var result = validationRepository.IsValidPolicyRoutingFromTo(fromTo, codeType);
            return Json(result);
        }

        // POST: FOr Validation of MerchantFee
        [HttpPost]
        public JsonResult IsMerchantFee(int merchantFeeId)
        {
            var result = validationRepository.IsMerchantFee(merchantFeeId);
            return Json(result);
        }

        // POST: FOr Validation of TransactionFeeAir
        [HttpPost]
        public JsonResult IsTransactionFeeAir(int transactionFeeId)
        {
            var result = validationRepository.IsTransactionFeeAir(transactionFeeId);
            return Json(result);
        }

		// POST: FOr Validation of TransactionFeeCarHotel
		[HttpPost]
		public JsonResult IsTransactionFeeCarHotel(int transactionFeeId)
		{
			var result = validationRepository.IsTransactionFeeCarHotel(transactionFeeId);
			return Json(result);
		}

		// POST: For SystemUserDefineRoles
		[HttpPost]
		public JsonResult IsValidSystemUser(string id)
		{
			var result = validationRepository.IsValidSystemUser(id);
			return Json(result);
		}

		// POST: For ClientProfileRowItem
		[HttpPost]
		public JsonResult IsValidClientProfileRowItem(ClientProfileItemRow clientProfileItemRow)
		{
			var result = validationRepository.IsValidClientProfileRowItem(clientProfileItemRow);
			return Json(result);
		}

		// POST: For GDSCommandFormat
		[HttpPost]
		public JsonResult IsValidGDSCommandFormat(string GDSCommandFormat, string GDSCode)
		{
			var result = validationRepository.IsValidGDSCommandFormat(GDSCommandFormat, GDSCode);
			return Json(result);
		}

		//IsValidTravelPortCode
		[HttpPost]
		public JsonResult IsValidTravelPortCode(string travelPortCode)
		{
			TravelPort travelport = new TravelPort();
			travelport = validationRepository.IsValidTravelPortCode(travelPortCode);

			if (travelport == null)
			{
				return Json(null);
			}
			else
			{
				return Json(new { travelport = new[] { new Dictionary<string, object> { { "TravelPortCode", travelport.TravelPortCode } } } });
			}
		}

        // POST: For Partner
        [HttpPost]
        public JsonResult IsValidPartner(int partnerId, string partnerName)
        {
            var result = validationRepository.IsValidPartner(partnerId, partnerName);
            return Json(result);
        }
    }
}
