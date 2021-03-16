using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text;
using System.Xml;
using System.Globalization;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyCountryGroupItemRepository
    {
        private PolicyCountryGroupItemDC db = new PolicyCountryGroupItemDC(Settings.getConnectionString());
        private HierarchyDC dbHierarchyDC = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCountryGroupItems_v1Result> GetPolicyCountryGroupItems(int policyGroupID, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPolicyCountryGroupItems_v1(policyGroupID, adminUserGuid, filter, sortField, Convert.ToBoolean(sortOrder), page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCountryGroupItems_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

        //Get one Item
        public PolicyCountryGroupItem GetPolicyCountryGroupItem(int policyCountryGroupItemId)
        {
            return db.PolicyCountryGroupItems.SingleOrDefault(c => c.PolicyCountryGroupItemId == policyCountryGroupItemId);

        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyCountryGroupItem policyCountryGroupItem)
        {
            //PolicyGroupName
            PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
            PolicyGroup policyGroup = policyGroupRepository.GetGroup(policyCountryGroupItem.PolicyGroupId);
            policyCountryGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;

            //PolicyCountryStatus
            if (policyCountryGroupItem.PolicyCountryStatusId != null)
            {
                int policyCountryStatusId = (int)policyCountryGroupItem.PolicyCountryStatusId;
                PolicyCountryStatusRepository policyCountryStatusRepository = new PolicyCountryStatusRepository();
                PolicyCountryStatus policyCountryStatus = new PolicyCountryStatus();
                policyCountryStatus = policyCountryStatusRepository.GetPolicyCountryStatus(policyCountryStatusId);
                if (policyCountryStatus != null)
                {
                    policyCountryGroupItem.PolicyCountryStatusDescription = policyCountryStatus.PolicyCountryStatusDescription;
                }
            }

            //CountryName    
            CountryRepository countryRepository = new CountryRepository();
            Country country = new Country();
            country = countryRepository.GetCountry(policyCountryGroupItem.CountryCode);
            if (country != null)
            {
                policyCountryGroupItem.CountryName = country.CountryName;
            }


        }

        //Add
        public void Add(PolicyCountryGroupItem policyCountryGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyCountryGroupItem_v1(
                policyCountryGroupItem.PolicyCountryStatusId,
                policyCountryGroupItem.EnabledDate,
                policyCountryGroupItem.ExpiryDate,
                policyCountryGroupItem.TravelDateValidFrom,
                policyCountryGroupItem.TravelDateValidTo,
                policyCountryGroupItem.EnabledFlag,
                policyCountryGroupItem.PolicyGroupId,
                policyCountryGroupItem.CountryCode,
                policyCountryGroupItem.InheritFromParentFlag,
                adminUserGuid
            );

        }

        //Add
        public void Update(PolicyCountryGroupItem policyCountryGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicyCountryGroupItem_v1(
                policyCountryGroupItem.PolicyCountryGroupItemId,
                policyCountryGroupItem.PolicyCountryStatusId,
                policyCountryGroupItem.EnabledDate,
                policyCountryGroupItem.ExpiryDate,
                policyCountryGroupItem.TravelDateValidFrom,
                policyCountryGroupItem.TravelDateValidTo,
                policyCountryGroupItem.EnabledFlag,
                policyCountryGroupItem.PolicyGroupId,
                policyCountryGroupItem.CountryCode,
                policyCountryGroupItem.InheritFromParentFlag,
                adminUserGuid,
                policyCountryGroupItem.VersionNumber
            );

        }
        
        //Delete
        public void Delete(PolicyCountryGroupItem policyCountryGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyCountryGroupItem_v1(
                policyCountryGroupItem.PolicyCountryGroupItemId,
                adminUserGuid,
                policyCountryGroupItem.VersionNumber
                );
        }

		//Export Items to CSV
		public byte[] Export(int id)
		{
			StringBuilder sb = new StringBuilder();

			//Add Headers
			List<string> headers = new List<string>();
			headers.Add("Policy Group Name");
			headers.Add("Country Name");
			headers.Add("Country Code");
            headers.Add("Status Description");
            headers.Add("Enabled Flag");
			headers.Add("Enabled Date");
			headers.Add("Expiry Date");
			headers.Add("Travel Date Valid From");
			headers.Add("Travel Date Valid To");
			headers.Add("Inherit From Parent Flag");
			headers.Add("Creation TimeStamp");
            headers.Add("Last Update Time Stamp");
            headers.Add("Advice");

            sb.AppendLine(String.Join(",", headers.Select(x => x.ToString()).ToArray()));

			//Add Items
			List<PolicyCountryGroupItem> policyCountryGroupItems = db.PolicyCountryGroupItems.Where(x => x.PolicyGroupId == id).ToList();
			foreach (PolicyCountryGroupItem item in policyCountryGroupItems)
			{

				//Edit Item
				EditItemForDisplay(item);

                //Advice Count
                int adviceCount = 0;
                PolicyCountryGroupItemLanguageRepository policyCountryGroupItemLanguageRepository = new PolicyCountryGroupItemLanguageRepository();
                List<PolicyCountryGroupItemLanguage> policyCountryGroupItemLanguages = policyCountryGroupItemLanguageRepository.GetItems(item.PolicyCountryGroupItemId);
                if (policyCountryGroupItemLanguages != null)
                {
                    adviceCount = policyCountryGroupItemLanguages.Count();
                }

                string date_format = "MM/dd/yy HH:mm";

				sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}",
					!string.IsNullOrEmpty(item.PolicyGroupName) ? item.PolicyGroupName : "",
					!string.IsNullOrEmpty(item.CountryName) ? item.CountryName : "",
					!string.IsNullOrEmpty(item.CountryCode) ? item.CountryCode : "",
                    !string.IsNullOrEmpty(item.PolicyCountryStatusDescription) ? item.PolicyCountryStatusDescription : "",
                    item.EnabledFlag == true ? "True" : "False",
					item.EnabledDate.HasValue ? item.EnabledDate.Value.ToString(date_format) : "",
					item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString(date_format) : "",
					item.TravelDateValidFrom.HasValue ? item.TravelDateValidFrom.Value.ToString(date_format) : "",
					item.TravelDateValidTo.HasValue ? item.TravelDateValidTo.Value.ToString(date_format) : "",
					item.InheritFromParentFlag == true ? "True" : "False",
					item.CreationTimestamp.HasValue ? item.CreationTimestamp.Value.ToString(date_format) : "",
					item.LastUpdateTimestamp.HasValue ? item.LastUpdateTimestamp.Value.ToString(date_format) : "",
                    adviceCount > 0 ? "True" : "False"
                );

				sb.Append(Environment.NewLine);
			}

			return Encoding.ASCII.GetBytes(sb.ToString());
		}


        public PolicyCountryImportStep2VM PreImportCheck(HttpPostedFileBase file, int policyGroupId)
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
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("PAVIGs");
            doc.AppendChild(root);

            List<string> returnMessages = new List<string>();
            string returnMessage;
            int i = 0;

            //Split the CSV into lines
            string[] lines = fileToText.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            //loop through CSV lines
            foreach (string line in lines)
            {

                i++;

                if (i > 1) //ignore first line with titles
                {

                    string[] cells = line.Split(';');

                    //extract the data items from the file

                    string CountryCode = cells[0];     //Required: (eg.Preferred)
                    string PolicyCountryStatusDescription = cells[1];			    //Required: (eg.DL)
                    string enabledFlag = cells[2];			    //(True / False)(1, 0)
                    string enabledDate = cells[3];			    //(YYYY / MM / DD)
                    string expiryDate = cells[4];			    //(YYYY / MM / DD)
                    string travelDateValidFrom = cells[5];		//(YYYY / MM / DD)
                    string travelDateValidTo = cells[6];        //(YYYY / MM / DD)

                    //Build the XML Element for items

                    XmlElement xmlPAVItem = doc.CreateElement("PAVIG");

                    XmlElement xmlCountryCode = doc.CreateElement("CountryCode");
                    xmlCountryCode.InnerText = CountryCode;
                    xmlPAVItem.AppendChild(xmlCountryCode);

                    XmlElement xmlPolicyCountryStatusDescription = doc.CreateElement("PolicyCountryStatusDescription");
                    xmlPolicyCountryStatusDescription.InnerText = PolicyCountryStatusDescription;
                    xmlPAVItem.AppendChild(xmlPolicyCountryStatusDescription);

                    XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                    xmlEnabledDate.InnerText = enabledDate;
                    xmlPAVItem.AppendChild(xmlEnabledDate);

                    XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                    xmlExpiryDate.InnerText = expiryDate;
                    xmlPAVItem.AppendChild(xmlExpiryDate);

                    XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                    xmlTravelDateValidFrom.InnerText = travelDateValidFrom;
                    xmlPAVItem.AppendChild(xmlTravelDateValidFrom);

                    XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                    xmlTravelDateValidTo.InnerText = travelDateValidTo;
                    xmlPAVItem.AppendChild(xmlTravelDateValidTo);


                    //Validate data

                    //Validate data country status
                    if (string.IsNullOrEmpty(PolicyCountryStatusDescription) == true)
                    {
                        returnMessage = "Row " + i + ": Policy Country Status Description is missing. Please provide a valid Country Status";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    PolicyCountryStatusRepository policyCountryStatusRepository = new PolicyCountryStatusRepository();
                    PolicyCountryStatus policyCountry = policyCountryStatusRepository.GetPolicyCountryStatusByDescription(PolicyCountryStatusDescription);
                    if (policyCountry == null)
                    {
                        returnMessage = "Row " + i + ": Country Status Description is invalid. Please provide a valid Country Status";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }
                    else
                    {
                        PolicyCountryStatusDescription = policyCountry.PolicyCountryStatusId.ToString();
                    }

                    //Validate data country code
                    if (string.IsNullOrEmpty(CountryCode) == true)
                    {
                        returnMessage = "Row " + i + ": Country Code is missing. Please provide a valid Country Code";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }
                    else
                    {
                        CountryRepository countryRepository = new CountryRepository();
                        Country country = countryRepository.GetCountry(CountryCode);
                        if (country == null)
                        {
                            returnMessage = "Row " + i + ": Country Code is invalid. Please provide a valid Country Code";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    enabledFlag = enabledFlag.ToUpper();
                    if (enabledFlag != "TRUE" && enabledFlag != "FALSE" && enabledFlag != "1" && enabledFlag != "0" && enabledFlag != "")
                    {
                        returnMessage = "Row " + i + ": EnabledFlag value is not valid. Please provide a valid Enabled Flag";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }
                    if (enabledFlag == "TRUE" || string.IsNullOrEmpty(enabledFlag))
                    {
                        enabledFlag = "1";
                    }
                    if (enabledFlag == "FALSE")
                    {
                        enabledFlag = "0";
                    }

                    XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                    xmlEnabledFlag.InnerText = enabledFlag;
                    xmlPAVItem.AppendChild(xmlEnabledFlag);

                    if (enabledDate != "" && enabledDate != "NULL")
                    {
                        DateTime enabledDateDT;
                        if (DateTime.TryParseExact(enabledDate, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out enabledDateDT) == false)
                        {
                            returnMessage = "Row " + i + ": EnabledDate must be in the format YYYY/MM/DD. Please provide a valid date format";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    if (expiryDate != "" && expiryDate != "NULL")
                    {
                        DateTime enabledDateDT;
                        if (DateTime.TryParseExact(expiryDate, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out enabledDateDT) == false)
                        {
                            returnMessage = "Row " + i + ": ExpiryDate must be in the format YYYY/MM/DD. Please provide a valid date format";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    if (travelDateValidFrom != "" && travelDateValidFrom != "NULL")
                    {
                        DateTime enabledDateDT;
                        if (DateTime.TryParseExact(travelDateValidFrom, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out enabledDateDT) == false)
                        {
                            returnMessage = "Row " + i + ": TravelDateFrom must be in the format YYYY/MM/DD. Please provide a valid date format";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    if (travelDateValidTo != "" && travelDateValidTo != "NULL")
                    {
                        DateTime enabledDateDT;
                        if (DateTime.TryParseExact(travelDateValidTo, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out enabledDateDT) == false)
                        {
                            returnMessage = "Row " + i + ": TravelDateTo must be in the format YYYY/MM/DD. Please provide a valid date format";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    //Attach the XML Element for an item to the Document
                    root.AppendChild(xmlPAVItem);

                }
            }
            if (i == 0)
            {
                returnMessage = "There is no data in the file";
                returnMessages.Add(returnMessage);
            }

            PolicyCountryImportStep2VM preImportCheckResult = new PolicyCountryImportStep2VM();
            preImportCheckResult.ReturnMessages = returnMessages;

            if (returnMessages.Count != 0)
            {
                //preImportCheckResult.FileBytes = array;

                preImportCheckResult.IsValidData = false;
            }
            else
            {
                //DB Check
                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

                var output = (
                    from n in dbHierarchyDC.spDesktopDataAdmin_UpdatePolicyCountryGroupItemCount_v1(
                        policyGroupId,
                        System.Xml.Linq.XElement.Parse(doc.OuterXml),
                        adminUserGuid
                    )
                    select n).ToList();

                foreach (spDesktopDataAdmin_UpdatePolicyCountryGroupItemCount_v1Result message in output)
                {
                    returnMessages.Add(message.MessageText.ToString());
                }

                preImportCheckResult.FileBytes = array;

                preImportCheckResult.IsValidData = true;
            }

            return preImportCheckResult;

        }

        public PolicyCountryImportStep3VM Import(byte[] FileBytes, int policyGroupId)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            string fileToText = fileToText = enc.GetString(FileBytes);

            PolicyCountryImportStep3VM cdrPostImportResult = new PolicyCountryImportStep3VM();
            List<string> returnMessages = new List<string>();

            // Create the xml document container, this will be used to store the data after the checks
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("PAVIGs");
            doc.AppendChild(root);

            int i = 0;

            //Split the CSV into lines
            string[] lines = fileToText.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            //loop through CSV lines
            foreach (string line in lines)
            {

                i++;

                if (i > 1) //ignore first line with titles
                {

                    string[] cells = line.Split(';');

                    //extract the data items from the file

                    string CountryCode = cells[0];     //Required: (eg.Preferred)
                    string PolicyCountryStatusId = cells[1];			    //Required: (eg.DL)
                    string enabledFlag = cells[2];			    //(True / False)(1, 0)
                    string enabledDate = cells[3];			    //(YYYY / MM / DD)
                    string expiryDate = cells[4];			    //(YYYY / MM / DD)
                    string travelDateValidFrom = cells[5];		//(YYYY / MM / DD)
                    string travelDateValidTo = cells[6];        //(YYYY / MM / DD)

                    //Build the XML Element for items

                    XmlElement xmlPAVItem = doc.CreateElement("PAVIG");



                    XmlElement xmlCountryCode = doc.CreateElement("CountryCode");
                    xmlCountryCode.InnerText = CountryCode;
                    xmlPAVItem.AppendChild(xmlCountryCode);

                    int val = 3;
                    PolicyCountryStatusId = val.ToString();

                    XmlElement xmlPolicyCountryStatusId = doc.CreateElement("PolicyCountryStatusId");
                    xmlPolicyCountryStatusId.InnerText = PolicyCountryStatusId;
                    xmlPAVItem.AppendChild(xmlPolicyCountryStatusId);

                    XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                    xmlEnabledDate.InnerText = enabledDate;
                    xmlPAVItem.AppendChild(xmlEnabledDate);

                    XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                    xmlExpiryDate.InnerText = expiryDate;
                    xmlPAVItem.AppendChild(xmlExpiryDate);

                    XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                    xmlTravelDateValidFrom.InnerText = travelDateValidFrom;
                    xmlPAVItem.AppendChild(xmlTravelDateValidFrom);

                    XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                    xmlTravelDateValidTo.InnerText = travelDateValidTo;
                    xmlPAVItem.AppendChild(xmlTravelDateValidTo);

                    if (enabledFlag == "TRUE" || string.IsNullOrEmpty(enabledFlag))
                    {
                        enabledFlag = "1";
                    }
                    if (enabledFlag == "FALSE")
                    {
                        enabledFlag = "0";
                    }

                    XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                    xmlEnabledFlag.InnerText = enabledFlag;
                    xmlPAVItem.AppendChild(xmlEnabledFlag);
                    //Attach the XML Element for an item to the Document
                    root.AppendChild(xmlPAVItem);

                }
            }

            //DB Check
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            try
            {
                var output = (from n in dbHierarchyDC.spDesktopDataAdmin_UpdatePolicyCountryGroupItems_v1(
                policyGroupId,
                System.Xml.Linq.XElement.Parse(doc.OuterXml),
                adminUserGuid)
                              select n).ToList();

                int deletedItemCount = 0;
                int addedItemCount = 0;

                foreach (spDesktopDataAdmin_UpdatePolicyCountryGroupItems_v1Result message in output)
                {
                    returnMessages.Add(message.MessageText.ToString());
                }

                cdrPostImportResult.ReturnMessages = returnMessages;
                cdrPostImportResult.AddedItemCount = addedItemCount;
                cdrPostImportResult.DeletedItemCount = deletedItemCount;
            }
            catch (Exception ex)
            {
                returnMessages.Add("<strong class=\"error\">The import of the selected data file has failed Please try again or contact your administrator</strong>");
                cdrPostImportResult.ReturnMessages = returnMessages;
                cdrPostImportResult.AddedItemCount = 0;
                cdrPostImportResult.DeletedItemCount = 0;
            }

            return cdrPostImportResult;
        }
    }
}
