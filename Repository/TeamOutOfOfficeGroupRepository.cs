using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
	public class TeamOutOfOfficeGroupRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		private TeamOutOfOfficeItemRepository teamOutOfOfficeItemRepository = new TeamOutOfOfficeItemRepository();
		
		//Get a Page of Team Out of Office Groups - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectTeamOutOfOfficeGroups_v1Result> PageTeamOutOfOfficeGroups(bool deleted, int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectTeamOutOfOfficeGroups_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectTeamOutOfOfficeGroups_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get one Team Out of Office Group
		public TeamOutOfOfficeGroup GetGroup(int id)
		{
			return db.TeamOutOfOfficeGroups.SingleOrDefault(c => c.TeamOutOfOfficeGroupId == id);
		}

		//Change the deleted status on an item
		public void UpdateGroupDeletedStatus(TeamOutOfOfficeGroup group)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateTeamOutOfOfficeGroupDeletedStatus_v1(
					group.TeamOutOfOfficeGroupId,
					group.DeletedFlag,
					adminUserGuid,
					group.VersionNumber
					);

		}

		public Dictionary<string, string> GetHierarchyTypes()
		{
			Dictionary<string, string> hierarchyTypes = new Dictionary<string, string>();
			hierarchyTypes.Add("ClientSubUnit", "ClientSubUnit");
			hierarchyTypes.Add("ClientSubUnitGUID", "ClientSubUnitGUID");
			return hierarchyTypes;
		}

		//Add Data From Linked Tables for Display
		public void EditGroupForDisplay(TeamOutOfOfficeGroup group)
		{
			//group.TeamOutOfOfficeGroupName = Regex.Replace(group.TeamOutOfOfficeGroupName, @"[^\w\-()*]", "-");

            List<fnDesktopDataAdmin_SelectTeamOutOfOfficeGroupHierarchy_v1Result> hierarchy = new List<fnDesktopDataAdmin_SelectTeamOutOfOfficeGroupHierarchy_v1Result>();
			hierarchy = GetGroupHierarchy(group.TeamOutOfOfficeGroupId);

			if (hierarchy.Count > 0)
			{
                HierarchyRepository hierarchyRepository = new HierarchyRepository();
                HierarchyGroup hierarchyGroup = hierarchyRepository.GetHierarchyGroup(
                    hierarchy[0].HierarchyType ?? "",
					hierarchy[0].HierarchyCode ?? "",
                    hierarchy[0].HierarchyName ?? "",
                    hierarchy[0].TravelerTypeGuid ?? "",
                    hierarchy[0].TravelerTypeName ?? "",
                    hierarchy[0].SourceSystemCode ?? ""
                );

                if (hierarchyGroup != null)
                {
                    group.HierarchyType = hierarchyGroup.HierarchyType;
                    group.HierarchyCode = hierarchyGroup.HierarchyCode;
                    group.HierarchyItem = hierarchyGroup.HierarchyItem;
                    group.ClientTopUnitName = hierarchyGroup.ClientTopUnitName;
                }
			}
		}

		//Get Hierarchy Details
		public List<fnDesktopDataAdmin_SelectTeamOutOfOfficeGroupHierarchy_v1Result> GetGroupHierarchy(int id)
		{
			var result = db.fnDesktopDataAdmin_SelectTeamOutOfOfficeGroupHierarchy_v1(id);
			return result.ToList();
		}

		//Edit Group
		public void Edit(TeamOutOfOfficeGroup group)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateTeamOutOfOfficeGroup_v1(
				adminUserGuid,
				group.TeamOutOfOfficeGroupId,
				group.TeamOutOfOfficeGroupName,
				group.EnabledFlag,
				group.EnabledDate,
				group.ExpiryDate,
				group.HierarchyType,
				group.HierarchyCode,
				adminUserGuid,
				group.VersionNumber
			);
		}

		//Add Group
		public void Add(TeamOutOfOfficeGroup group)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertTeamOutOfOfficeGroup_v1(
				adminUserGuid,
				group.TeamOutOfOfficeGroupName,
				group.EnabledFlag,
				group.EnabledDate,
				group.ExpiryDate,
				group.HierarchyType,
				group.HierarchyCode,
				adminUserGuid
			);
		}

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        //There can only be one active group created for each SubUnit (whether in UnDeleted or Deleted).
        //When considering an active group, the Enabled flag, Enabled Date and Expiry date of the existing group should be evaluated
        public bool CanTeamOutOfOfficeGroupBeActive(int? groupId, string clientSubUnitGuid)
        {
            int count = 0;

            if (groupId.HasValue && groupId.Value > 0)
            {
                var result = from n in db.TeamOutOfOfficeGroups
                             join TeamOutOfOfficeGroupClientSubUnit in db.TeamOutOfOfficeGroupClientSubUnits    
                                on n.TeamOutOfOfficeGroupId equals TeamOutOfOfficeGroupClientSubUnit.TeamOutOfOfficeGroupId
                             where
                                TeamOutOfOfficeGroupClientSubUnit.ClientSubUnitGuid == clientSubUnitGuid &&
                                (n.EnabledFlag.Equals(true) && ((n.EnabledDate.HasValue && n.EnabledDate < DateTime.Now) || !n.EnabledDate.HasValue)) &&
                                ((n.ExpiryDate.HasValue && n.ExpiryDate > DateTime.Now) || !n.ExpiryDate.HasValue) && 
                                n.TeamOutOfOfficeGroupId != groupId
                             select n.TeamOutOfOfficeGroupName;
                count = result.Count();
            }
            else
            {
                var result = from n in db.TeamOutOfOfficeGroups
                             join TeamOutOfOfficeGroupClientSubUnit in db.TeamOutOfOfficeGroupClientSubUnits
                                on n.TeamOutOfOfficeGroupId equals TeamOutOfOfficeGroupClientSubUnit.TeamOutOfOfficeGroupId
                             where
                                TeamOutOfOfficeGroupClientSubUnit.ClientSubUnitGuid == clientSubUnitGuid &&
                                (n.EnabledFlag.Equals(true) && ((n.EnabledDate.HasValue && n.EnabledDate < DateTime.Now) || !n.EnabledDate.HasValue)) &&
                                ((n.ExpiryDate.HasValue && n.ExpiryDate > DateTime.Now) || !n.ExpiryDate.HasValue)
                             select n.TeamOutOfOfficeGroupName;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		public List<TeamOutOfOfficeItemExport> GetTeamOutOfOfficeItemsExport()
		{
			return db.spDesktopDataAdmin_SelectTeamOutOfOfficeItemsExport_v1()
				.Select(n => new TeamOutOfOfficeItemExport
				{
					TeamOutOfOfficeGroupName = n.TeamOutOfOfficeGroupName,
					ClientSubUnitGuid = n.ClientSubUnitGuid,
					ClientSubUnitName = n.ClientSubUnitName,
					TeamOutOfOfficeGroup_CreationTimestamp = n.TeamOutOfOfficeGroup_CreationTimestamp,
					TeamOutOfOfficeGroup_CreationUserIdentifier = n.TeamOutOfOfficeGroup_CreationUserIdentifier,
					TeamOutOfOfficeGroup_DeletedFlag = n.TeamOutOfOfficeGroup_DeletedFlag,
					TeamOutOfOfficeGroup_DeletedDateTime = n.TeamOutOfOfficeGroup_DeletedDateTime,
					TeamOutOfOfficeGroup_EnabledFlag = n.TeamOutOfOfficeGroup_EnabledFlag,
					TeamOutOfOfficeGroup_EnabledDate = n.TeamOutOfOfficeGroup_EnabledDate,
					PrimaryTeamId = n.PrimaryTeamId,
					PrimaryTeamName = n.PrimaryTeamName,
					PrimaryBackupTeamId = n.PrimaryBackupTeamId,
					PrimaryBackupTeamName = n.PrimaryBackupTeamName,
					SecondaryBackupTeamId = n.SecondaryBackupTeamId,
					SecondaryBackupTeamName = n.SecondaryBackupTeamName,
					TertiaryBackupTeamId = n.TertiaryBackupTeamId,
					TertiaryBackupTeamName = n.TertiaryBackupTeamName,
					TeamOutOfOfficeItem_CreationTimestamp = n.TeamOutOfOfficeGroupItem_CreationTimestamp,
					TeamOutOfOfficeItem_CreationUserIdentifier = n.TeamOutOfOfficeGroupItem_CreationUserIdentifier
				})
			  .ToList();
		}

		//Export Items to CSV
		public byte[] Export()
		{
			StringBuilder sb = new StringBuilder();

			//Add Headers
			List<string> headers = new List<string>
            {
                "TeamOutOfOfficeGroupName",
				"ClientSubUnitGuid",
				"ClientSubUnitName",				
				"GroupCreationTimestamp",
				"GroupCreationUserIdentifier",
				"GroupDeletedFlag",
				"GroupDeletedDateTime",
				"GroupEnabledFlag",
				"GroupEnabledDate",
				"PrimaryTeamId",
				"PrimaryTeamName",
				"PrimaryBackupTeamId",
				"PrimaryBackupTeamName",
				"SecondaryBackupTeamId",
				"SecondaryBackupTeamName",
				"TertiaryBackupTeamId",
				"TertiaryBackupTeamName",
				"CreationTimestamp",
				"CreationUserIdentifier"
            };

			sb.AppendLine(String.Join(",", headers.Select(x => x.ToString()).ToArray()));

			//Add Items
			List<TeamOutOfOfficeItemExport> teamOutOfOfficeItems = GetTeamOutOfOfficeItemsExport().ToList();

			foreach (TeamOutOfOfficeItemExport item in teamOutOfOfficeItems)
			{
				string date_format = "MM/dd/yy HH:mm";

				sb.AppendFormat(
					"{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18}",

					item.TeamOutOfOfficeGroupName,
					item.ClientSubUnitGuid,
					item.ClientSubUnitName,
					item.TeamOutOfOfficeGroup_CreationTimestamp != null && item.TeamOutOfOfficeGroup_CreationTimestamp.HasValue ? item.TeamOutOfOfficeGroup_CreationTimestamp.Value.ToString(date_format) : "NULL",
					item.TeamOutOfOfficeGroup_CreationUserIdentifier != null ? item.TeamOutOfOfficeGroup_CreationUserIdentifier : "NULL",
					item.TeamOutOfOfficeGroup_DeletedFlag.HasValue ? item.TeamOutOfOfficeGroup_DeletedFlag.Value.ToString().ToUpper() : "NULL",
					item.TeamOutOfOfficeGroup_DeletedDateTime != null && item.TeamOutOfOfficeGroup_DeletedDateTime.HasValue ? item.TeamOutOfOfficeGroup_DeletedDateTime.Value.ToString(date_format) : "NULL",
					item.TeamOutOfOfficeGroup_EnabledFlag.HasValue ? item.TeamOutOfOfficeGroup_EnabledFlag.Value.ToString().ToUpper() : "NULL",
					item.TeamOutOfOfficeGroup_EnabledDate != null && item.TeamOutOfOfficeGroup_EnabledDate.HasValue ? item.TeamOutOfOfficeGroup_EnabledDate.Value.ToString(date_format) : "NULL",
					item.PrimaryTeamId > 0 ? item.PrimaryTeamId.ToString() : "NULL",
					item.PrimaryTeamName != null ? item.PrimaryTeamName : "NULL",
					item.PrimaryBackupTeamId != null && item.PrimaryBackupTeamId > 0 ? item.PrimaryBackupTeamId.ToString() : "NULL",
					item.PrimaryBackupTeamName != null ? item.PrimaryBackupTeamName : "NULL",
					item.SecondaryBackupTeamId != null && item.SecondaryBackupTeamId > 0 ? item.SecondaryBackupTeamId.ToString() : "NULL",
					item.SecondaryBackupTeamName != null ? item.SecondaryBackupTeamName : "NULL",
					item.TertiaryBackupTeamId != null && item.TertiaryBackupTeamId > 0 ? item.TertiaryBackupTeamId.ToString() : "NULL",
					item.TertiaryBackupTeamName != null ? item.TertiaryBackupTeamName : "NULL",
					item.TeamOutOfOfficeItem_CreationTimestamp != null && item.TeamOutOfOfficeItem_CreationTimestamp.HasValue ? item.TeamOutOfOfficeItem_CreationTimestamp.Value.ToString(date_format) : "NULL",
					item.TeamOutOfOfficeItem_CreationUserIdentifier != null ? item.TeamOutOfOfficeItem_CreationUserIdentifier : "NULL"
				);

				sb.Append(Environment.NewLine);
			}

			return Encoding.ASCII.GetBytes(sb.ToString());
		}

		private void ValidateLines(ref XmlDocument doc, string[] lines, ref List<string> returnMessages)
		{
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("TeamOutOfOfficeGroups");
			doc.AppendChild(root);

			string returnMessage;

			int i = 0;

			//Store Valid ClientSubUnits
			List<string> validClientSubUnitGuids = new List<string>();
			List<string> invalidClientSubUnitGuids = new List<string>();

			//loop through CSV lines
			foreach (string line in lines)
			{

				i++;

				if (i > 1) //ignore first line with titles
				{

					Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
					String[] cells = csvParser.Split(line);

					//extract the data items from the file
					string clientSubUnitGuid = CWTStringHelpers.NullToEmpty(CWTStringHelpers.UnescapeQuotes(cells[0]));			//Required
					string primaryBackupTeamValue = CWTStringHelpers.NullToEmpty(CWTStringHelpers.UnescapeQuotes(cells[1]));	//Required
					string secondaryBackupTeamValue = CWTStringHelpers.NullToEmpty(CWTStringHelpers.UnescapeQuotes(cells[2]));
					string tertiaryBackupTeamValue = CWTStringHelpers.NullToEmpty(CWTStringHelpers.UnescapeQuotes(cells[3]));

					//Build the XML Element for items

					XmlElement xmlTeamOutOfOfficeGroupItem = doc.CreateElement("TeamOutOfOfficeGroupItem");

					//Validate data

					/* Client SubUnit Guid */

					//Required
					if (string.IsNullOrEmpty(clientSubUnitGuid) == true)
					{
						returnMessage = "Row " + i + ": Client SubUnit Guid is missing. Please provide a Client SubUnit Guid";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}
					else
					{
						bool existingValidClient = validClientSubUnitGuids.Contains(clientSubUnitGuid);
						bool existingInvalidClient = invalidClientSubUnitGuids.Contains(clientSubUnitGuid);

						//Check ClientSubUnit is valid
						if (existingInvalidClient)
						{
							//Error: ClientSubUnit is invalid
							returnMessage = string.Format("Row " + i + ": ClientSubUnitGuid {0} is invalid. Please provide a valid Client SubUnit Guid", clientSubUnitGuid);
							if (!returnMessages.Contains(returnMessage))
							{
								returnMessages.Add(returnMessage);
							}
						}
						else if (existingValidClient)
						{
							//No processing required as already marked as valid
						}
						else
						{
							ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
							ClientSubUnit clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);
							if (clientSubUnit == null)
							{
								//Error: ClientSubUnit is invalid
								returnMessage = string.Format("Row " + i + ": ClientSubUnitGuid {0} is invalid. Please provide a valid Client SubUnit Guid", clientSubUnitGuid);
								if (!returnMessages.Contains(returnMessage))
								{
									returnMessages.Add(returnMessage);
								}

								if (!invalidClientSubUnitGuids.Contains(clientSubUnitGuid))
								{
									invalidClientSubUnitGuids.Add(clientSubUnitGuid);
								}
							}
							else
							{
								//We have a valid Client SubUnit, now check TeamOutOfOfficeGroupClientSubUnit
								List<TeamOutOfOfficeGroupClientSubUnit> teamOutOfOfficeGroupClientSubUnits = db.TeamOutOfOfficeGroupClientSubUnits.Where(x => x.ClientSubUnitGuid == clientSubUnitGuid).ToList();
								if (teamOutOfOfficeGroupClientSubUnits.Count > 0)
								{
									//Error: ClientSubUnit relationship already exists
									returnMessage = string.Format("Row " + i + ": ClientSubUnitGuid {0} is not unique. Please update existing Team Out of Office Group", clientSubUnitGuid);
									if (!returnMessages.Contains(returnMessage))
									{
										returnMessages.Add(returnMessage);
									}

									if (!invalidClientSubUnitGuids.Contains(clientSubUnitGuid))
									{
										invalidClientSubUnitGuids.Add(clientSubUnitGuid);
									}
								}
								else
								{
									//ClientSubUnitGuid must have an entry in the IsPrimaryTeamForSub table
									List<IsPrimaryTeamForSub> primaryTeamForClientSubUnits = db.IsPrimaryTeamForSubs.Where(x => x.ClientSubUnitGuid == clientSubUnitGuid).ToList();
									if (primaryTeamForClientSubUnits.Count == 0)
									{
										//Error: ClientSubUnit relationship already exists
										returnMessage = string.Format("Row " + i + ": ClientSubUnitGuid {0} does not have a Primary Team setup. Please complete in Client Wizard for this ClientSubUnitGuid", clientSubUnitGuid);
										if (!returnMessages.Contains(returnMessage))
										{
											returnMessages.Add(returnMessage);
										}

										if (!invalidClientSubUnitGuids.Contains(clientSubUnitGuid))
										{
											invalidClientSubUnitGuids.Add(clientSubUnitGuid);
										}
									}
									else
									{
										if (!validClientSubUnitGuids.Contains(clientSubUnitGuid))
										{
											validClientSubUnitGuids.Add(clientSubUnitGuid);
										}
									}
								}
							}
						}
					}

					XmlElement xmlClientSubUnitGuid = doc.CreateElement("ClientSubUnitGuid");
					xmlClientSubUnitGuid.InnerText = clientSubUnitGuid;
					xmlTeamOutOfOfficeGroupItem.AppendChild(xmlClientSubUnitGuid);

					//Primary Backup Team
					int primaryBackupTeamId = 0;
					if (string.IsNullOrEmpty(primaryBackupTeamValue) || !Int32.TryParse(primaryBackupTeamValue, out primaryBackupTeamId))
					{
						//Error: PrimaryBackupTeamId is invalid
						returnMessage = string.Format("Row " + i + ": PrimaryBackupTeamId is missing. Please provide a Team Id", clientSubUnitGuid);
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}
					else
					{
						TeamRepository teamRepository = new TeamRepository();
						Team primaryBackupTeam = teamRepository.GetTeam(primaryBackupTeamId);
						if (primaryBackupTeam == null)
						{
							//Error: PrimaryBackupTeamId is invalid
							returnMessage = string.Format("Row " + i + ": PrimaryBackupTeamId {0} is invalid. Please provide a valid Team Id", primaryBackupTeamValue);
							if (!returnMessages.Contains(returnMessage))
							{
								returnMessages.Add(returnMessage);
							}
						}
					}

					XmlElement xmlPrimaryBackupTeamId = doc.CreateElement("PrimaryBackupTeamId");
					xmlPrimaryBackupTeamId.InnerText = primaryBackupTeamValue;
					xmlTeamOutOfOfficeGroupItem.AppendChild(xmlPrimaryBackupTeamId);

					//Secondary Backup Team
					if (!string.IsNullOrEmpty(secondaryBackupTeamValue))
					{
						int secondaryBackupTeamId = 0;
						if (!Int32.TryParse(secondaryBackupTeamValue, out secondaryBackupTeamId))
						{
							//Error: SecondaryBackupTeamId is invalid
							returnMessage = string.Format("Row " + i + ": SecondaryBackupTeamId {0} is invalid. Please provide a valid Team Id", secondaryBackupTeamValue);
							if (!returnMessages.Contains(returnMessage))
							{
								returnMessages.Add(returnMessage);
							}
						}
						else
						{
							TeamRepository teamRepository = new TeamRepository();
							Team secondaryBackupTeam = teamRepository.GetTeam(secondaryBackupTeamId);
							if (secondaryBackupTeam == null)
							{
								//Error: SecondaryBackupTeamId is invalid
								returnMessage = string.Format("Row " + i + ": SecondaryBackupTeamId {0} is invalid. Please provide a valid Team Id", secondaryBackupTeamValue);
								if (!returnMessages.Contains(returnMessage))
								{
									returnMessages.Add(returnMessage);
								}
							}
						}
					}

					XmlElement xmlSecondaryBackupTeamId = doc.CreateElement("SecondaryBackupTeamId");
					xmlSecondaryBackupTeamId.InnerText = secondaryBackupTeamValue;
					xmlTeamOutOfOfficeGroupItem.AppendChild(xmlSecondaryBackupTeamId);

					//Tertiary Backup Team
					if (!string.IsNullOrEmpty(tertiaryBackupTeamValue))
					{
						int tertiaryBackupTeamId = 0;
						if (!Int32.TryParse(tertiaryBackupTeamValue, out tertiaryBackupTeamId))
						{
							//Error: TertiaryBackupTeamId is invalid
							returnMessage = string.Format("Row " + i + ": TertiaryBackupTeamId {0} is invalid. Please provide a valid Team Id", tertiaryBackupTeamValue);
							if (!returnMessages.Contains(returnMessage))
							{
								returnMessages.Add(returnMessage);
							}
						}
						else
						{
							TeamRepository teamRepository = new TeamRepository();
							Team tertiaryBackupTeam = teamRepository.GetTeam(tertiaryBackupTeamId);
							if (tertiaryBackupTeam == null)
							{
								//Error: TertiaryBackupTeamId is invalid
								returnMessage = string.Format("Row " + i + ": TertiaryBackupTeamId {0} is invalid. Please provide a valid Team Id", tertiaryBackupTeamValue);
								if (!returnMessages.Contains(returnMessage))
								{
									returnMessages.Add(returnMessage);
								}
							}
						}
					}

					XmlElement xmlTertiaryBackupTeamId = doc.CreateElement("TertiaryBackupTeamId");
					xmlTertiaryBackupTeamId.InnerText = tertiaryBackupTeamValue;
					xmlTeamOutOfOfficeGroupItem.AppendChild(xmlTertiaryBackupTeamId);

					//Attach the XML Element for an item to the Document
					root.AppendChild(xmlTeamOutOfOfficeGroupItem);
				}
			}

			if (i == 0)
			{
				returnMessage = "There is no data in the file";
				returnMessages.Add(returnMessage);
			}
		}

		public TeamOutOfOfficeGroupImportStep2VM PreImportCheck(HttpPostedFileBase file, string clientSubUnitGuid)
		{
			//convert file to string so that we can parse
			int length = file.ContentLength;
			byte[] tempFile = new byte[length];
			file.InputStream.Read(tempFile, 0, length);
			byte[] array = tempFile.ToArray();
			System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
			string fileToText = fileToText = enc.GetString(array);

			// Create the xml document container, this will be used to store the data after the checks
			XmlDocument doc = new XmlDocument();

			//Prepare a list of error messages
			List<string> returnMessages = new List<string>();

			//Split the CSV into lines
			string[] lines = fileToText.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			//Check Headers
			bool validHeaders = true;
			if (lines[0] != null)
			{
				Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
				string[] headers = csvParser.Split(lines[0]);

				if (headers.Length < 2)
				{
					string returnMessage = "File contains incorrect number of columns. File must contain columns for ClientSubUnitGuid, PrimaryBackupTeamId and optionally SecondaryBackupTeamId and TertiaryBackupTeamId";
					returnMessages.Add(returnMessage);
					validHeaders = false;
				}
			}

			//Validate CSV Lines
			if (validHeaders)
			{
				ValidateLines(ref doc, lines, ref returnMessages);
			}

			TeamOutOfOfficeGroupImportStep2VM preImportCheckResult = new TeamOutOfOfficeGroupImportStep2VM();
			preImportCheckResult.ReturnMessages = returnMessages;

			if (returnMessages.Count != 0)
			{
				preImportCheckResult.IsValidData = false;
			}
			else
			{
				//DB Check
				string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

				var output = (
					from n in db.spDesktopDataAdmin_UpdateTeamOutOfOfficeGroupCount_v1(
						System.Xml.Linq.XElement.Parse(doc.OuterXml),
						adminUserGuid
					)
					select n).ToList();

				foreach (spDesktopDataAdmin_UpdateTeamOutOfOfficeGroupCount_v1Result message in output)
				{
					returnMessages.Add(message.MessageText.ToString());
				}

				preImportCheckResult.FileBytes = array;

				preImportCheckResult.IsValidData = true;
			}

			return preImportCheckResult;

		}

		public TeamOutOfOfficeGroupImportStep3VM Import(byte[] FileBytes)
		{
			System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
			string fileToText = fileToText = enc.GetString(FileBytes);

			TeamOutOfOfficeGroupImportStep3VM cdrPostImportResult = new TeamOutOfOfficeGroupImportStep3VM();

			// Create the xml document container, this will be used to store the data after the checks
			XmlDocument doc = new XmlDocument();

			//Prepare a list of error messages
			List<string> returnMessages = new List<string>();

			//Split the CSV into lines
			string[] lines = fileToText.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			//Validate CSV Lines
			ValidateLines(ref doc, lines, ref returnMessages);

			//DB Check
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			var output = (from n in db.spDesktopDataAdmin_UpdateTeamOutOfOfficeGroups_v1(
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid)
				select n).ToList();

			int addedItemCount = 0;

			foreach (spDesktopDataAdmin_UpdateTeamOutOfOfficeGroups_v1Result message in output)
			{
				returnMessages.Add(message.MessageText.ToString());
			}

			cdrPostImportResult.ReturnMessages = returnMessages;
			cdrPostImportResult.AddedItemCount = addedItemCount;

			return cdrPostImportResult;
		}
	}
}