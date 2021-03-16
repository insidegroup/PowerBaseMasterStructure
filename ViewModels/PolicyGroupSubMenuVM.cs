using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
    public class PolicyGroupSubMenuVM : CWTBaseViewModel
    {
        public PolicyGroup PolicyGroup { get; set; }
        public int PolicyAirCabinGroupItemCount { get; set; }
        public int PolicyAirMissedSavingsThresholdGroupItemCount { get; set; }
        public int PolicyAirVendorGroupItemCount { get; set; }
        public int PolicyCarTypeGroupItemCount { get; set; }
        public int PolicyCarVendorGroupItemCount { get; set; }
        public int PolicyCityGroupItemCount { get; set; }
        public int PolicyCountryGroupItemCount { get; set; }
        public int PolicyHotelCapRateGroupItemCount { get; set; }
        public int PolicyHotelPropertyItemCount { get; set; }
        public int PolicyHotelVendorGroupItemCount { get; set; }
        public int PolicyMessageGroupItemCount { get; set; }
        public int PolicySupplierDealCodeCount { get; set; }
        public int PolicySupplierServiceInformationCount { get; set; }

		//Other Items
		public int Policy24HSCOtherGroupItemCount { get; set; }
		public int PolicyAirOtherGroupItemCount { get; set; }
		public int PolicyAllOtherGroupItemCount { get; set; }
		public int PolicyCarOtherGroupItemCount { get; set; }
		public int PolicyHotelOtherGroupItemCount { get; set; }
		public int PolicyOtherGroupItemCount { get; set; }

		public List<PolicyType> PolicyTypes { get; set; }


        public PolicyGroupSubMenuVM()
        {
        }
        public PolicyGroupSubMenuVM(PolicyGroup policyGroup, int policyAirCabinGroupItemCount, int policyAirMissedSavingsThresholdGroupItemCount, int policyAirVendorGroupItemCount, int policyCarTypeGroupItemCount,
                        int policyCarVendorGroupItemCount, int policyCityGroupItemCount, int policyCountryGroupItemCount, int policyHotelCapRateGroupItemCount,
                        int policyHotelPropertyItemCount, int policyHotelVendorGroupItemCount, int policyMessageGroupItemCount, int policySupplierDealCodeCount,
                        int policySupplierServiceInformationCount, 
			
						int policy24HSCOtherGroupItemCount, int policyAirOtherGroupItemCount, int policyAllOtherGroupItemCount, 
						int policyCarOtherGroupItemCount, int policyHotelOtherGroupItemCount, int policyOtherGroupItemCount
						)
        {
            PolicyGroup = policyGroup;
            PolicyAirCabinGroupItemCount = policyAirCabinGroupItemCount;
            PolicyAirMissedSavingsThresholdGroupItemCount = policyAirMissedSavingsThresholdGroupItemCount;
            PolicyAirVendorGroupItemCount = policyAirVendorGroupItemCount;
            PolicyCarTypeGroupItemCount = policyCarTypeGroupItemCount;
            PolicyCarVendorGroupItemCount = policyCarVendorGroupItemCount;
            PolicyCityGroupItemCount = policyCityGroupItemCount;
            PolicyCountryGroupItemCount = policyCountryGroupItemCount;
            PolicyHotelCapRateGroupItemCount = policyHotelCapRateGroupItemCount;
            PolicyHotelPropertyItemCount = policyHotelPropertyItemCount;
            PolicyHotelVendorGroupItemCount = policyHotelVendorGroupItemCount;
            PolicyMessageGroupItemCount = policyMessageGroupItemCount;
            PolicySupplierDealCodeCount = policySupplierDealCodeCount;
            PolicySupplierServiceInformationCount = policySupplierServiceInformationCount;

			Policy24HSCOtherGroupItemCount = policy24HSCOtherGroupItemCount; 
			PolicyAirOtherGroupItemCount = policyAirOtherGroupItemCount;
			PolicyAllOtherGroupItemCount = policyAllOtherGroupItemCount;
			PolicyCarOtherGroupItemCount = policyCarOtherGroupItemCount;
			PolicyHotelOtherGroupItemCount = policyHotelOtherGroupItemCount;
			PolicyOtherGroupItemCount = policyOtherGroupItemCount;
        }
    }
}