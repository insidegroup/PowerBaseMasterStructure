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
    public class PolicyCityGroupItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
        private HierarchyDC dbHierarchyDC = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCityGroupItems_v1Result> GetPolicyCityGroupItems(int policyGroupID, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPolicyCityGroupItems_v1(policyGroupID, adminUserGuid, filter, sortField, Convert.ToBoolean(sortOrder), page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCityGroupItems_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

        //Get one Item
        public PolicyCityGroupItem GetPolicyCityGroupItem(int policyCityGroupItemId)
        {
            return db.PolicyCityGroupItems.SingleOrDefault(c => c.PolicyCityGroupItemId == policyCityGroupItemId);

        }

		//Get PolicyCityGroupItemsByCityCode
		public List<PolicyCityGroupItem> PolicyCityGroupItemsByCityCode(string cityCode)
		{
			return db.PolicyCityGroupItems.Where(c => c.CityCode == cityCode).ToList();
		}

        //Add
        public void Add(PolicyCityGroupItem policyCityGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyCityGroupItem_v1(
                policyCityGroupItem.PolicyGroupId,
                policyCityGroupItem.PolicyCityStatusId,
                policyCityGroupItem.EnabledFlag, 
                policyCityGroupItem.EnabledDate,
                policyCityGroupItem.ExpiryDate,
                policyCityGroupItem.TravelDateValidFrom,
                policyCityGroupItem.TravelDateValidTo,
                policyCityGroupItem.CityCode,
                policyCityGroupItem.InheritFromParentFlag,
                adminUserGuid
            );

        }

        //Add
        public void Update(PolicyCityGroupItem policyCityGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicyCityGroupItem_v1(
                policyCityGroupItem.PolicyCityGroupItemId,
                policyCityGroupItem.PolicyCityStatusId,
                policyCityGroupItem.EnabledFlag, 
                policyCityGroupItem.EnabledDate,
                policyCityGroupItem.ExpiryDate,
                policyCityGroupItem.TravelDateValidFrom,
                policyCityGroupItem.TravelDateValidTo,
                policyCityGroupItem.CityCode,
                policyCityGroupItem.InheritFromParentFlag,
                adminUserGuid,
                policyCityGroupItem.VersionNumber
            );

        }
        
        //Delete
        public void Delete(PolicyCityGroupItem policyCityGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyCityGroupItem_v1(
                policyCityGroupItem.PolicyCityGroupItemId,
                adminUserGuid,
                policyCityGroupItem.VersionNumber
                );
        }

        
        //Export Items to CSV
        public byte[] Export(int id)
        {
            StringBuilder sb = new StringBuilder();

            //Add Headers
            List<string> headers = new List<string>();
            headers.Add("Policy Group Name");
            headers.Add("City Name");
            headers.Add("City Code");
            headers.Add("PolicyCityStatusDescription");
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
            List<PolicyCityGroupItem> policyCityGroupItems = db.PolicyCityGroupItems.Where(x => x.PolicyGroupId == id).ToList();

            foreach (PolicyCityGroupItem item in policyCityGroupItems)
            {

                //PolicyGroupName    
                PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
                PolicyGroup policyGroup = new PolicyGroup();
                policyGroup = policyGroupRepository.GetGroup(item.PolicyGroupId);
                string PolicyGroupName = policyGroup.PolicyGroupName;

                int adviceCount = 0;

                string date_format = "MM/dd/yy HH:mm";

                string short_date_format = "yyyy/MM/dd";

                sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}",
                    !string.IsNullOrEmpty(PolicyGroupName) ? PolicyGroupName : "",
                    !string.IsNullOrEmpty(item.City.Name) ? item.City.Name : "",
                    !string.IsNullOrEmpty(item.CityCode) ? item.CityCode : "",
                    !string.IsNullOrEmpty(item.PolicyCityStatus.PolicyCityStatusDescription) ? item.PolicyCityStatus.PolicyCityStatusDescription: "",
                    item.EnabledFlag == true ? "True" : "False",
                    item.EnabledDate.HasValue ? item.EnabledDate.Value.ToString(short_date_format) : "",
                    item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString(short_date_format) : "",
                    item.TravelDateValidFrom.HasValue ? item.TravelDateValidFrom.Value.ToString(short_date_format) : "",
                    item.TravelDateValidTo.HasValue ? item.TravelDateValidTo.Value.ToString(short_date_format) : "",
                    item.InheritFromParentFlag == true ? "True" : "False",
                    item.CreationTimestamp.HasValue ? item.CreationTimestamp.Value.ToString(date_format) : "",
                    item.LastUpdateTimestamp.HasValue ? item.LastUpdateTimestamp.Value.ToString(date_format) : "",
                    adviceCount > 0 ? "True" : "False"
                    );

                sb.Append(Environment.NewLine);
            }
            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        public PolicyCityImportStep2VM PreImportCheck(HttpPostedFileBase file, int policyGroupId)
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

                    string CityCode = cells[0];     //Required
                    string PolicyCityStatusDescription = cells[1];			    //Required
                    string enabledFlag = cells[2];			    //(True / False)(1, 0)
                    string enabledDate = cells[3];			    //(YYYY / MM / DD)
                    string expiryDate = cells[4];			    //(YYYY / MM / DD)
                    string travelDateValidFrom = cells[5];		//(YYYY / MM / DD)
                    string travelDateValidTo = cells[6];        //(YYYY / MM / DD)

                    //Build the XML Element for items

                    XmlElement xmlPAVItem = doc.CreateElement("PAVIG");

                    XmlElement xmlCityCode = doc.CreateElement("CityCode");
                    xmlCityCode.InnerText = CityCode;
                    xmlPAVItem.AppendChild(xmlCityCode);

                    XmlElement xmlPolicyCityStatusDescription = doc.CreateElement("PolicyCityStatusDescription");
                    xmlPolicyCityStatusDescription.InnerText = PolicyCityStatusDescription;
                    xmlPAVItem.AppendChild(xmlPolicyCityStatusDescription);

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


                    //Validate data city status
                    if (string.IsNullOrEmpty(PolicyCityStatusDescription) == true)
                    {
                        returnMessage = "Row " + i + ": Policy City Status Description is missing. Please provide a valid City Status";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    PolicyCityStatusRepository policyCityStatusRepository = new PolicyCityStatusRepository();
                    PolicyCityStatus policyCity = policyCityStatusRepository.GetPolicyCityStatusByDescrition(PolicyCityStatusDescription);
                    if (policyCity == null)
                    {
                        returnMessage = "Row " + i + ": City Status Description is invalid. Please provide a valid City Status";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }
                    else
                    {
                        PolicyCityStatusDescription = policyCity.PolicyCityStatusId.ToString();
                    }

                    //Validate data city code
                    if (string.IsNullOrEmpty(CityCode) == true)
                    {
                        returnMessage = "Row " + i + ": City Code is missing. Please provide a valid City Code";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }
                    else
                    {
                        CityRepository cityRepository = new CityRepository();
                        City city = cityRepository.GetCity(CityCode);
                        if (city == null)
                        {
                            returnMessage = "Row " + i + ": City Code is invalid. Please provide a valid City Code";
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

            PolicyCityImportStep2VM preImportCheckResult = new PolicyCityImportStep2VM();
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
                    from n in dbHierarchyDC.spDesktopDataAdmin_UpdatePolicyCityGroupItemCount_v1(
                        policyGroupId,
                        System.Xml.Linq.XElement.Parse(doc.OuterXml),
                        adminUserGuid
                    )
                    select n).ToList();

                foreach (spDesktopDataAdmin_UpdatePolicyCityGroupItemCount_v1Result message in output)
                {
                    returnMessages.Add(message.MessageText.ToString());
                }

                preImportCheckResult.FileBytes = array;

                preImportCheckResult.IsValidData = true;
            }

            return preImportCheckResult;

        }

        public PolicyCityImportStep3VM Import(byte[] FileBytes, int policyGroupId)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            string fileToText = fileToText = enc.GetString(FileBytes);

            PolicyCityImportStep3VM cdrPostImportResult = new PolicyCityImportStep3VM();
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

                    string CityCode = cells[0];     //Required: (eg.Preferred)
                    string PolicyCityStatusId = cells[1];			    //Required: (eg.DL)
                    string enabledFlag = cells[2];			    //(True / False)(1, 0)
                    string enabledDate = cells[3];			    //(YYYY / MM / DD)
                    string expiryDate = cells[4];			    //(YYYY / MM / DD)
                    string travelDateValidFrom = cells[5];		//(YYYY / MM / DD)
                    string travelDateValidTo = cells[6];        //(YYYY / MM / DD)

                    //Build the XML Element for items

                    XmlElement xmlPAVItem = doc.CreateElement("PAVIG");



                    XmlElement xmlCityCode = doc.CreateElement("CityCode");
                    xmlCityCode.InnerText = CityCode;
                    xmlPAVItem.AppendChild(xmlCityCode);

                    PolicyCityStatusRepository policyCityStatusRepository = new PolicyCityStatusRepository();
                    PolicyCityStatus policyCity = policyCityStatusRepository.GetPolicyCityStatusByDescrition(PolicyCityStatusId);
                    int PolicyCityStatusDescriptionId = policyCity.PolicyCityStatusId;
        

                    XmlElement xmlPolicyCityStatusId = doc.CreateElement("PolicyCityStatusId");
                    xmlPolicyCityStatusId.InnerText = PolicyCityStatusDescriptionId.ToString();
                    xmlPAVItem.AppendChild(xmlPolicyCityStatusId);

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
                var output = (from n in dbHierarchyDC.spDesktopDataAdmin_UpdatePolicyCityGroupItems_v1(
                policyGroupId,
                System.Xml.Linq.XElement.Parse(doc.OuterXml),
                adminUserGuid)
                              select n).ToList();

                int deletedItemCount = 0;
                int addedItemCount = 0;

                foreach (spDesktopDataAdmin_UpdatePolicyCityGroupItems_v1Result message in output)
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
