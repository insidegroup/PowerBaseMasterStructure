using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    /*
    Items Linked to a Team(not including Systems or ClientSubUnits)
    */
    public class TeamLinkedItemsVM : CWTBaseViewModel
    {
        public Team Team { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Contact> Contacts { get; set; }
        public List<CreditCard> CreditCards { get; set; }
        public List<ExternalSystemParameter> ExternalSystemParameters { get; set; }
        public List<ExternalSystemLogin> ExternalSystemLogins { get; set; }
        public List<GDSAdditionalEntry> GDSAdditionalEntries { get; set; }
        public List<LocalOperatingHoursGroup> LocalOperatingHoursGroups { get; set; }
        public List<PNROutputGroup> PNROutputGroups { get; set; }
        public List<PolicyGroup> PolicyGroups { get; set; }
        public List<PublicHolidayGroup> PublicHolidayGroups { get; set; }
        public List<QueueMinderGroup> QueueMinderGroups { get; set; }
        public List<ServicingOptionGroup> ServicingOptionGroups { get; set; }
        public List<TicketQueueGroup> TicketQueueGroups { get; set; }
        public List<ValidPseudoCityOrOfficeId> ValidPseudoCityOrOfficeIds { get; set; }
        //public List<GlobalRegion> GlobalRegions { get; set; }
        //public List<GlobalSubRegion> GlobalSubRegions { get; set; }
        //public List<Country> Countries { get; set; }
        //public List<CountryRegion> CountryRegions { get; set; }
        //public List<Location> Locations { get; set; }
        
        
        
        
        public TeamLinkedItemsVM()
        {
        }

        public TeamLinkedItemsVM(Team team,
            List<Address> addresses,
            List<Contact> contacts,
            List<ValidPseudoCityOrOfficeId> validPseudoCityOrOfficeIds,
            List<CreditCard> creditCards, 
            List<ExternalSystemParameter> externalSystemParameters,
            List<ExternalSystemLogin> externalSystemLogins,
            List<GDSAdditionalEntry> gdsAdditionalEntries,
            List<LocalOperatingHoursGroup> localOperatingHoursGroups,
            List<PNROutputGroup> pnrOutputGroups,
            List<PolicyGroup> policyGroups,
            List<PublicHolidayGroup> publicHolidayGroups,
            List<QueueMinderGroup> queueMinderGroups,
            List<ServicingOptionGroup> servicingOptionGroups,
            List<TicketQueueGroup> ticketQueueGroups        
)
        {
            Team = team;
            Addresses = addresses;
            Contacts = Contacts;
            ValidPseudoCityOrOfficeIds = validPseudoCityOrOfficeIds;
            CreditCards = CreditCards;
            ExternalSystemParameters = ExternalSystemParameters;
            ExternalSystemLogins = externalSystemLogins;
            GDSAdditionalEntries = gdsAdditionalEntries;
            LocalOperatingHoursGroups = localOperatingHoursGroups; 
            QueueMinderGroups = QueueMinderGroups;
            PNROutputGroups = pnrOutputGroups;
            PolicyGroups = policyGroups;
            PublicHolidayGroups = publicHolidayGroups;
            QueueMinderGroups = queueMinderGroups;
            ServicingOptionGroups = servicingOptionGroups;
            TicketQueueGroups = ticketQueueGroups;
        }
    }
}
