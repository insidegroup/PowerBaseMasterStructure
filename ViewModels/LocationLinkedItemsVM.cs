using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class LocationLinkedItemsVM : CWTBaseViewModel
    {
        public Location Location { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Contact> Contacts { get; set; }
        public List<CreditCard> CreditCards { get; set; }
        public List<ExternalSystemParameter> ExternalSystemParameters { get; set; }
        public List<GDSAdditionalEntry> GDSAdditionalEntries { get; set; }
        public List<LocalOperatingHoursGroup> LocalOperatingHoursGroups { get; set; }
        public List<PNROutputGroup> PNROutputGroups { get; set; }
        public List<PolicyGroup> PolicyGroups { get; set; }
        public List<PublicHolidayGroup> PublicHolidayGroups { get; set; }
        public List<QueueMinderGroup> QueueMinderGroups { get; set; }
        public List<ServicingOptionGroup> ServicingOptionGroups { get; set; }
        public List<TicketQueueGroup> TicketQueueGroups { get; set; }
        public List<TripTypeGroup> TripTypeGroups { get; set; }
        public List<ValidPseudoCityOrOfficeId> ValidPseudoCityOrOfficeIds { get; set; }
        public List<WorkFlowGroup> WorkFlowGroups { get; set; }
        public List<Team> Teams { get; set; }
        
        
        
        public LocationLinkedItemsVM()
        {
        }

        public LocationLinkedItemsVM(Location location,
            List<Address> addresses,
            List<Contact> contacts,
            List<ExternalSystemParameter> externalSystemParameters,
            List<GDSAdditionalEntry> gdsAdditionalEntries,
            List<CreditCard> creditCards,
            List<QueueMinderGroup> queueMinderGroups,
            List<ValidPseudoCityOrOfficeId> validPseudoCityOrOfficeIds,
            List<ServicingOptionGroup> servicingOptionGroups,
            List<TicketQueueGroup> ticketQueueGroups,
            List<TripTypeGroup> tripTypeGroups,
            List<PNROutputGroup> pnrOutputGroups,
            List<PublicHolidayGroup> publicHolidayGroups,
            List<WorkFlowGroup> workFlowGroups,
            List<LocalOperatingHoursGroup> localOperatingHoursGroups,
            List<PolicyGroup> policyGroups,
            List<Team> teams
            )
        {
            Teams = teams;
            Location = location;
            Addresses = addresses;
            Contacts = Contacts;
            ExternalSystemParameters = ExternalSystemParameters;
            GDSAdditionalEntries = gdsAdditionalEntries;
            CreditCards = CreditCards;
            QueueMinderGroups = QueueMinderGroups;
            ValidPseudoCityOrOfficeIds = validPseudoCityOrOfficeIds;
            ServicingOptionGroups =servicingOptionGroups;
            TicketQueueGroups = ticketQueueGroups;
            TripTypeGroups = tripTypeGroups;
            PNROutputGroups = pnrOutputGroups;
            PublicHolidayGroups = publicHolidayGroups;
            WorkFlowGroups = workFlowGroups;
            LocalOperatingHoursGroups = localOperatingHoursGroups;
            PolicyGroups = policyGroups;
        }
    }
}
