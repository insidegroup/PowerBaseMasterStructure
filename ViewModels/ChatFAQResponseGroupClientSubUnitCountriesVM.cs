using System.Collections.Generic;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ChatFAQResponseGroupClientSubUnitCountriesVM : CWTBaseViewModel
    {
        public int ChatFAQResponseGroupId { get; set; }
        public string ChatFAQResponseGroupName { get; set; }
        public int VersionNumber { get; set; }
        public List<ChatFAQResponseGroupClientSubUnitCountryVM> ClientSubUnitsAvailable { get; set; }
        public List<ChatFAQResponseGroupClientSubUnitCountryVM> ClientSubUnitsUnAvailable { get; set; }

        public ChatFAQResponseGroupClientSubUnitCountriesVM()
        {
        }
        public ChatFAQResponseGroupClientSubUnitCountriesVM(
            int chatFAQResponseGroupId, 
            string chatFAQResponseGroupName, 
            int versionNumber, 
            List<ChatFAQResponseGroupClientSubUnitCountryVM> clientSubUnitsAvailable, 
            List<ChatFAQResponseGroupClientSubUnitCountryVM> clientSubUnitsUnAvailable)
        {
            ChatFAQResponseGroupName = chatFAQResponseGroupName;
            ChatFAQResponseGroupId = chatFAQResponseGroupId;
            VersionNumber = versionNumber;
            ClientSubUnitsAvailable = clientSubUnitsAvailable;
            ClientSubUnitsUnAvailable = clientSubUnitsUnAvailable;
        }
    }
}