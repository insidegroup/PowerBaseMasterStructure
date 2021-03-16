using ClientProfileServiceBusiness;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Helpers
{
	public class GDSIntegration
	{
		
		public static string FormatPccString(string pccString)
		{
			switch (pccString.Length)
			{
				case 1:
					pccString = string.Format("000{0}", pccString);
					break;
				case 2:
					pccString = string.Format("00{0}", pccString);
					break;
				case 3:
					pccString = string.Format("0{0}", pccString);
					break;
			}

			return pccString;
		}

		public static List<ClientProfileItemVM> GetProfileLineItems(int id, bool showMandatoryItems = false)
		{
			List<ClientProfileItemVM> clientProfileItemsAllList = new List<ClientProfileItemVM>();
			List<ClientProfileItemVM> clientProfileItemsList = new List<ClientProfileItemVM>();
			ClientProfileGroupRepository clientProfileGroupRepository = new ClientProfileGroupRepository();
			ClientProfileGroup clientProfileGroup = clientProfileGroupRepository.GetGroup(id);

			if (clientProfileGroup != null)
			{
				ClientProfileItemRepository clientProfileItemRepository = new ClientProfileItemRepository();

				//General (Removed)
				//foreach (ClientProfileItemVM item in clientProfileItemRepository.GetClientProfilePanelClientProfileDataElements(id, 1))
				//{
				//	clientProfileItemsAllList.Add(item);
				//}

				//Land Policy (removed)
				//foreach (ClientProfileItemVM item in clientProfileItemRepository.GetClientProfilePanelClientProfileDataElements(id, 8))
				//{
				//	clientProfileItemsAllList.Add(item);
				//}
				
				//1. Client Detail
				foreach (ClientProfileItemVM item in clientProfileItemRepository.GetClientProfilePanelClientProfileDataElements(id, 6))
				{
					clientProfileItemsAllList.Add(item);	
				}
				
				//2. Mid Office
				foreach (ClientProfileItemVM item in clientProfileItemRepository.GetClientProfilePanelClientProfileDataElements(id, 3))
				{
					clientProfileItemsAllList.Add(item);
				}

				//3. Back Office
				foreach (ClientProfileItemVM item in clientProfileItemRepository.GetClientProfilePanelClientProfileDataElements(id, 5))
				{
					clientProfileItemsAllList.Add(item);
				}
				
				//4. Air/Rail/Land
				foreach (ClientProfileItemVM item in clientProfileItemRepository.GetClientProfilePanelClientProfileDataElements(id, 7))
				{
					clientProfileItemsAllList.Add(item);
				}

				//5. Itinerary
				foreach (ClientProfileItemVM item in clientProfileItemRepository.GetClientProfilePanelClientProfileDataElements(id, 9))
				{
					clientProfileItemsAllList.Add(item);
				}
				
				//6. 24 Hour
				foreach (ClientProfileItemVM item in clientProfileItemRepository.GetClientProfilePanelClientProfileDataElements(id, 10))
				{
					clientProfileItemsAllList.Add(item);
				}

				//7. Amadeus TPM
				foreach (ClientProfileItemVM item in clientProfileItemRepository.GetClientProfilePanelClientProfileDataElements(id, 11))
				{
					clientProfileItemsAllList.Add(item);
				}				

				foreach (ClientProfileItemVM item in clientProfileItemsAllList)
				{
					if ((showMandatoryItems && item.ClientProfileItem.MandatoryFlag) || (item.ClientProfileItem.ClientProfileMoveStatusId != null && item.ClientProfileItem.GDSCommandFormat != null && item.ClientProfileItem.Remark != null))
					{
						clientProfileItemsList.Add(item);
					}
				}
				
			}

			return clientProfileItemsList;
		}

		public static string WriteGDSProfile(int id)
		{

			string clientProfileBuilder = string.Empty;

			List<ClientProfileItemVM> clientProfileItemList = GetProfileLineItems(id);

			if (clientProfileItemList.Count > 0)
			{
				foreach (ClientProfileItemVM clientProfileItem in clientProfileItemList)
				{
                    if (!string.IsNullOrEmpty(clientProfileItem.ClientProfileItem.GDSCommandFormat))
                    {
                        clientProfileBuilder += clientProfileItem.ClientProfileItem.GDSCommandFormat;
                    }
                    if (!string.IsNullOrEmpty(clientProfileItem.ClientProfileItem.Remark))
                    {
                        clientProfileBuilder += clientProfileItem.ClientProfileItem.Remark;
                        clientProfileBuilder += Environment.NewLine;
                    }
                }
			}
			
			return clientProfileBuilder;

		}

		public static List<ClientProfileLine> WriteProfileLines(List<ClientProfileItemVM> clientProfileItemsList, string gdsCode)
		{
			HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
			
			List<ClientProfileLine> lines = new List<ClientProfileLine>();

			int counter = 1;

			if (clientProfileItemsList.Count > 0)
			{
				foreach (ClientProfileItemVM item in clientProfileItemsList)
				{
					if (item.ClientProfileItem.ClientProfileMoveStatusId != null &&
						item.ClientProfileItem.GDSCommandFormat != null &&
						item.ClientProfileItem.Remark != null)
					{
						ClientProfileLine clientProfileLine = new ClientProfileLine();

						ClientProfileMoveStatus clientProfileMoveStatus = new ClientProfileMoveStatus();
						ClientProfileMoveStatusRepository clientProfileMoveStatusRepository = new ClientProfileMoveStatusRepository();
						clientProfileMoveStatus = clientProfileMoveStatusRepository.GetMoveStatus(item.ClientProfileItem.ClientProfileMoveStatusId);

						//Map GDSCommandFormat to the line type enum
						var TID = (from n in db.ClientProfileTravelportGDSFormats
								   where n.DefaultGDSCommandFormat == item.ClientProfileItem.GDSCommandFormat && n.GDSCode == gdsCode
								   select n.TravelIndustryDesignatorValue).SingleOrDefault();

						//Set a default LineType to GeneralRemark
						//Common.LineType.Address (default as first in enum) caused too many validation errors
						if (TID == null)
						{
							clientProfileLine.LineType = Common.LineType.GeneralRemark;
						}
						else
						{
							switch (TID)
							{
								case "0003":
									clientProfileLine.LineType = Common.LineType.Address;
									break;

								case "0004":
									clientProfileLine.LineType = Common.LineType.AddressDelivery;
									break;

								case "0045":
									clientProfileLine.LineType = Common.LineType.Blank;
									break;

								case "0025":
									clientProfileLine.LineType = Common.LineType.CustomCheck;
									break;

								case "0024":
									clientProfileLine.LineType = Common.LineType.CustomizerID;
									break;

								case "0002":
									clientProfileLine.LineType = Common.LineType.Email;
									break;

								case "0015":
									if (gdsCode == "1A") { clientProfileLine.LineType = Common.LineType.Eticket; }
									break;

								case "0021":
									clientProfileLine.LineType = Common.LineType.FOP;
									break;

								case "0005":
									clientProfileLine.LineType = Common.LineType.FrequentFlyer;
									break;

								case "0010":
									clientProfileLine.LineType = Common.LineType.GeneralRemark;
									break;

								case "0027":
									if (gdsCode == "1G") { clientProfileLine.LineType = Common.LineType.InvoiceRemark; }
									break;

								case "0012":
									clientProfileLine.LineType = Common.LineType.ItineraryRemarkAssociated;
									break;

								case "0023":
									clientProfileLine.LineType = Common.LineType.ItineraryRemarkUnassociated;
									break;

								case "0001":
									clientProfileLine.LineType = Common.LineType.Name;
									break;

								case "0006":
									clientProfileLine.LineType = Common.LineType.OSI;
									break;

								case "0013":
									clientProfileLine.LineType = Common.LineType.PassengerInfo;
									break;

								case "0018":
									if (gdsCode == "1G") { clientProfileLine.LineType = Common.LineType.PassengerReporting; }
									break;

								case "0030":
									if (gdsCode == "1A") { clientProfileLine.LineType = Common.LineType.PostScript; }
									break;

								case "0026":
									if (gdsCode == "1A")
									{ clientProfileLine.LineType = Common.LineType.QueueMinder; }
									break;

								case "0014":
									if (gdsCode == "1A")
									{ clientProfileLine.LineType = Common.LineType.QueueMinder; }
									break;

								case "0022":
									clientProfileLine.LineType = Common.LineType.ReceivedField;
									break;

								case "0029":
									if (gdsCode == "1G")
									{
										clientProfileLine.LineType = Common.LineType.ReportingAddress;
									}
									break;

								case "0009":
									clientProfileLine.LineType = Common.LineType.SeatData;
									break;

								case "0007":
									clientProfileLine.LineType = Common.LineType.SSRManual;
									break;

								case "0008":
									clientProfileLine.LineType = Common.LineType.SSRProgrammatic;
									break;

								case "0031":
									clientProfileLine.LineType = Common.LineType.TextOnly;
									break;

								case "0019":
									clientProfileLine.LineType = Common.LineType.TicketingArrangement;
									break;

								case "0020":
									if (gdsCode == "1A") { clientProfileLine.LineType = Common.LineType.TicketingRemark; }
									break;

								case "0011":
									clientProfileLine.LineType = Common.LineType.VendorRemark;
									break;
							}
						}

						//Remark
						clientProfileLine.LineText = item.ClientProfileItem.Remark;

						//Move Status
						char lineIndicator = new char();

						switch (clientProfileMoveStatus.ClientProfileMoveStatusCode)
						{
							case "Always Move":
								lineIndicator = 'Y';
								break;
							case "Optional Move":
								lineIndicator = 'O';
								break;
							case "Never Move":
								lineIndicator = 'N';
								break;
							case "Priority":
								lineIndicator = 'P';
								break;
						}

						clientProfileLine.LineIndicator = lineIndicator;

						//Line Number
						clientProfileLine.LineNumber = counter;

						lines.Add(clientProfileLine);

						counter++;
					}
				}
			}

			return lines;
		}
	}
}