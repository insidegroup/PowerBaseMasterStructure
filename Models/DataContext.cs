using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Data.Linq;

namespace CWTDesktopDatabase.Models
{

    /*
     * Override the ConnectionString of any DataContexts by taking ConnectionString from Cookie
     * Every DataContext MUST be listed here, otherwise it will use the default CWTDesktopDatabase ConnectionString 
     * CWTDesktopDatabase is used for login, then the user can choose another DB, db connection name is stored in Cookie
     * 
     */

	//public partial class ccdbDC
	//{
	//	partial void OnCreated()
	//	{
	//		this.Connection.ConnectionString = Settings.getConnectionString();
	//		this.CommandTimeout = Settings.GetCommandTimeout();
	//	}
	//}

	public partial class ClientDefinedRuleDC
	{
		partial void OnCreated()
		{
			this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
		}
	}

    public partial class ClientDetailDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }

    public partial class ExternalSystemLoginSystemUserDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class ExternalSystemParameterDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
   
    public partial class GDSAdditionalEntryDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }

    public partial class HierarchyDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
	public partial class MeetingDC
	{
		partial void OnCreated()
		{
			this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
		}
	}

    public partial class PointOfSaleFeeLoadDataContext
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
            this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }

    public partial class PolicyAdviceDataContext
	{
		partial void OnCreated()
		{
			this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
		}
	}
   
    public partial class PolicyAirCabinGroupItemDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    //public partial class PolicyAirCabinGroupItemLanguageDC
    //{
    //    partial void OnCreated()
    //    {
    //        this.Connection.ConnectionString = Settings.getConnectionString();
	//			this.CommandTimeout = Settings.GetCommandTimeout();
    //    }
    //}
   
	public partial class PolicyAirParameterGroupItemLanguageDC
    {
        partial void OnCreated()
        {
			this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
	
    public partial class PolicyAirVendorGroupItemDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    public partial class PolicyAirVendorGroupItemLanguageDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
   // public partial class PolicyCarStatusDC
   // {
   //     partial void OnCreated()
    //    {
    //        CWTConnectionString cwtConnectionString = new CWTConnectionString();
    //        this.Connection.ConnectionString = cwtConnectionString.GetConnectionString();
    //    }
    //}
    public partial class PolicyCarTypeGroupItemDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    public partial class PolicyCarTypeGroupItemLanguageDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class PolicyCarVendorGroupItemDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    public partial class PolicyCarVendorGroupItemLanguageDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    public partial class PolicyCountryGroupItemDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    public partial class PolicyCountryGroupItemLanguageDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    public partial class PolicyCountryStatusDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class PolicyGroupDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
   
    public partial class PolicyHotelCapRateGroupItemDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    public partial class PolicyHotelCapRateGroupItemLanguageDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class PolicyHotelPropertyGroupItemDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class PolicyHotelPropertyGroupItemLanguageDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
   
    public partial class PolicyHotelVendorGroupItemDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    public partial class PolicyHotelVendorGroupItemLanguageDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    public partial class PolicyLocationDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
   
    public partial class PolicySupplierDealCodeDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    public partial class PolicySupplierDealCodeTypeDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    public partial class PolicySupplierServiceInformationDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class PolicySupplierServiceInformationTypeDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
	public partial class PriceTrackingDC
	{
		partial void OnCreated()
		{
			this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
		}
	}
   
    public partial class PublicHolidayGroupDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    public partial class PublicHolidayDateDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
   
    public partial class ReasonCodeAlternativeDescriptionDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    public partial class ReasonCodeGroupDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class ReasonCodeItemDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class ReasonCodeProductTypeDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
	public partial class ReasonCodeProductTypeDescriptionDC
	{
		partial void OnCreated()
		{
			this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
		}
	}
	
	public partial class ReasonCodeProductTypeTravelerDescriptionDC
	{
		partial void OnCreated()
		{
			this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
		}
	}
    public partial class ReasonCodeProductTypeTranslationDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class ReasonCodeTypeDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class RolesDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    public partial class ServicingOptionDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class ServicingOptionGroupDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class ServicingOptionItemDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class ServicingOptionItemValueDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
   
    public partial class SupplierDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class SystemUserDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class SystemUserGDSDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class SystemUserLocationDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class SystemUserTeamDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }

    public partial class TicketQueueGroupDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    public partial class TicketQueueItemDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class TicketTypeDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class TravelerBackOfficeTypeDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
   
    public partial class TravelPortLanguageDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    public partial class TravelPortTypeDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
   
    public partial class TripTypeGroupDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
    
    public partial class TripTypeItemDC
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = Settings.getConnectionString();
			this.CommandTimeout = Settings.GetCommandTimeout();
        }
    }
}