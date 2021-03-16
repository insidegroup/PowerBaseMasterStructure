using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text;
using System.Xml;
using CWTDesktopDatabase.ViewModels;
using System.Globalization;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyHotelCapRateGroupItemRepository
    {
        private PolicyHotelCapRateGroupItemDC db = new PolicyHotelCapRateGroupItemDC(Settings.getConnectionString());

        private HierarchyDC dbHierarchyDC = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyHotelCapRateGroupItems_v1Result> GetPolicyHotelCapRateGroupItems(int policyGroupID, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyHotelCapRateGroupItems_v1(policyGroupID, adminUserGuid, filter, sortField, Convert.ToBoolean(sortOrder), page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyHotelCapRateGroupItems_v1Result>(result, page, totalRecords);
            return paginatedView;

        }
 
        //Add
        public void Add(PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyHotelCapRateGroupItem_v1(
                policyHotelCapRateGroupItem.PolicyGroupId,
                policyHotelCapRateGroupItem.PolicyLocationId,
                policyHotelCapRateGroupItem.EnabledFlag,
                policyHotelCapRateGroupItem.CurrencyCode,
                policyHotelCapRateGroupItem.CapRate,
                policyHotelCapRateGroupItem.EnabledDate,
                policyHotelCapRateGroupItem.ExpiryDate,
                policyHotelCapRateGroupItem.TravelDateValidFrom,
                policyHotelCapRateGroupItem.TravelDateValidTo,
				policyHotelCapRateGroupItem.TaxInclusiveFlag,
                adminUserGuid
            );

        }

        //Edit
        public void Update(PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicyHotelCapRateGroupItem_v1(
                policyHotelCapRateGroupItem.PolicyHotelCapRateItemId,
                policyHotelCapRateGroupItem.PolicyGroupId,
                policyHotelCapRateGroupItem.PolicyLocationId,
                policyHotelCapRateGroupItem.EnabledFlag,
                policyHotelCapRateGroupItem.CurrencyCode,
                policyHotelCapRateGroupItem.CapRate,
                policyHotelCapRateGroupItem.EnabledDate,
                policyHotelCapRateGroupItem.ExpiryDate,
                policyHotelCapRateGroupItem.TravelDateValidFrom,
                policyHotelCapRateGroupItem.TravelDateValidTo,
                policyHotelCapRateGroupItem.TaxInclusiveFlag,
                adminUserGuid,
                policyHotelCapRateGroupItem.VersionNumber
            );

        }

        //Delete
        public void Delete(PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyHotelCapRateGroupItem_v1(
                policyHotelCapRateGroupItem.PolicyHotelCapRateItemId,
                adminUserGuid,
                policyHotelCapRateGroupItem.VersionNumber
                );
        }

        //Get one Item
        public PolicyHotelCapRateGroupItem GetPolicyHotelCapRateGroupItem(int policyHotelCapRateGroupItemId)
        {
            return db.PolicyHotelCapRateGroupItems.SingleOrDefault(c => c.PolicyHotelCapRateItemId == policyHotelCapRateGroupItemId);
        }


        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem)
        {
            //PolicyGroupName    
            PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyHotelCapRateGroupItem.PolicyGroupId);
            policyHotelCapRateGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;

            //Currency
            if (policyHotelCapRateGroupItem.CurrencyCode != null)
            {
                string currencyCode = policyHotelCapRateGroupItem.CurrencyCode;
                CurrencyRepository currencyRepository = new CurrencyRepository();
                Currency currency = new Currency();
                currency = currencyRepository.GetCurrency(currencyCode);
                policyHotelCapRateGroupItem.CurrencyName = currency.Name;
            }

            //PolicyLocation
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            PolicyLocation policyLocation = new PolicyLocation();
            policyLocation = policyLocationRepository.GetPolicyLocation(policyHotelCapRateGroupItem.PolicyLocationId);
            if (policyLocation != null)
            {
                policyHotelCapRateGroupItem.PolicyLocation = policyLocation.PolicyLocationName;
            }

        }

		//Export Items to CSV
		public byte[] Export(int id)
		{
			StringBuilder sb = new StringBuilder();

			//Add Headers
			List<string> headers = new List<string>();
			headers.Add("Policy Group Name");
			headers.Add("Location Name");
			headers.Add("Location Code");
			headers.Add("Currency Name");
			headers.Add("Currency Code");
			headers.Add("Hotel Cap Rate Amount");
			headers.Add("Enabled Flag");
			headers.Add("Enabled Date");
			headers.Add("Expiry Date");
			headers.Add("Travel Date Valid From");
			headers.Add("Travel Date Valid To");
			headers.Add("Tax Inclusive Flag");
			headers.Add("Creation TimeStamp");
            headers.Add("Last Update Time Stamp");
            headers.Add("Advice");

            sb.AppendLine(String.Join(",", headers.Select(x => x.ToString()).ToArray()));

			//Add Items
			List<PolicyHotelCapRateGroupItem> policyHotelCapRateGroupItems = db.PolicyHotelCapRateGroupItems.Where(x => x.PolicyGroupId == id).ToList();
			foreach (PolicyHotelCapRateGroupItem item in policyHotelCapRateGroupItems)
			{

				//Edit Item
				EditItemForDisplay(item);

				//Location
				PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
				PolicyLocation policyLocation = new PolicyLocation();
				policyLocation = policyLocationRepository.GetPolicyLocation(item.PolicyLocationId);
				if(policyLocation != null){
					policyLocationRepository.EditForDisplay(policyLocation);
				}

                //Advice Count
                int adviceCount = 0;
                PolicyHotelCapRateGroupItemLanguageRepository policyHotelCapRateGroupItemLanguageRepository = new PolicyHotelCapRateGroupItemLanguageRepository();
                List<PolicyHotelCapRateGroupItemLanguage> policyHotelCapRateGroupItemLanguages = policyHotelCapRateGroupItemLanguageRepository.GetItems(item.PolicyHotelCapRateItemId);
                if(policyHotelCapRateGroupItemLanguages != null)
                {
                    adviceCount = policyHotelCapRateGroupItemLanguages.Count();
                }

                string date_format = "MM/dd/yy HH:mm";

                string short_date_format = "yyyy/MM/dd";

                sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}",
					!string.IsNullOrEmpty(item.PolicyGroupName) ? item.PolicyGroupName : "",
					!string.IsNullOrEmpty(policyLocation.LocationName) ? policyLocation.LocationName : "",
					!string.IsNullOrEmpty(policyLocation.LocationCode) ? policyLocation.LocationCode : "",
					!string.IsNullOrEmpty(item.CurrencyName) ? item.CurrencyName : "",
					!string.IsNullOrEmpty(item.CurrencyCode) ? item.CurrencyCode : "",
					item.CapRate.HasValue ? item.CapRate.Value.ToString() : "",
					item.EnabledFlag == true ? "True" : "False",
					item.EnabledDate.HasValue ? item.EnabledDate.Value.ToString(short_date_format) : "",
					item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString(short_date_format) : "",
					item.TravelDateValidFrom.HasValue ? item.TravelDateValidFrom.Value.ToString(short_date_format) : "",
					item.TravelDateValidTo.HasValue ? item.TravelDateValidTo.Value.ToString(short_date_format) : "",
					item.TaxInclusiveFlag == true ? "True" : "False",
					item.CreationTimestamp.HasValue ? item.CreationTimestamp.Value.ToString(date_format) : "",
					item.LastUpdateTimestamp.HasValue ? item.LastUpdateTimestamp.Value.ToString(date_format) : "",
                    adviceCount > 0 ? "True" : "False"
                );

				sb.Append(Environment.NewLine);
			}

			return Encoding.ASCII.GetBytes(sb.ToString());
		}

        public PolicyHotelCapRateImportStep2VM PreImportCheck(HttpPostedFileBase file, int policyGroupId)
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
            XmlElement root = doc.CreateElement("PHCRIGs");
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

                    string[] cells = line.Split(',');

                    //extract the data items from the file
                    string locationCode = cells[0];			//Required
                    string currencyCode = cells[1];         //Required
					string capRate = cells[2];              //Required
					string enabledFlag = cells[3] ?? "";
                    string enabledDate = cells[4] ?? "";
                    string expiryDate = cells[5] ?? "";
                    string travelDateValidFrom = cells[6] ?? "";
                    string travelDateValidTo = cells[7] ?? "";
                    string taxInclusiveFlag = cells[8];		//Required

					//Build the XML Element for items

					XmlElement xmlCDRItem = doc.CreateElement("PHCRIG");

                    XmlElement xmlLocationCode = doc.CreateElement("LocationCode");
                    xmlLocationCode.InnerText = locationCode;
                    xmlCDRItem.AppendChild(xmlLocationCode);

                    XmlElement xmlCurrencyCode = doc.CreateElement("CurrencyCode");
                    xmlCurrencyCode.InnerText = currencyCode;
                    xmlCDRItem.AppendChild(xmlCurrencyCode);

                    XmlElement xmlCapRate = doc.CreateElement("CapRate");
                    xmlCapRate.InnerText = capRate;
                    xmlCDRItem.AppendChild(xmlCapRate);

					XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                    xmlEnabledDate.InnerText = enabledDate;
                    xmlCDRItem.AppendChild(xmlEnabledDate);

                    XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                    xmlExpiryDate.InnerText = expiryDate;
                    xmlCDRItem.AppendChild(xmlExpiryDate);

                    XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                    xmlTravelDateValidFrom.InnerText = travelDateValidFrom;
                    xmlCDRItem.AppendChild(xmlTravelDateValidFrom);

                    XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                    xmlTravelDateValidTo.InnerText = travelDateValidTo;
                    xmlCDRItem.AppendChild(xmlTravelDateValidTo);

                    //Validate data
                    if (string.IsNullOrEmpty(locationCode) == true)
                    {
                        returnMessage = "Row " + i + ": LocationCode is missing. Please provide a valid Location Code";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    if (string.IsNullOrEmpty(currencyCode) == true)
                    {
                        returnMessage = "Row " + i + ": CurrencyCode is missing. Please provide a valid Currency Code";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    if (string.IsNullOrEmpty(capRate) == true)
                    {
                        returnMessage = "Row " + i + ": CapRate is missing. Please provide a valid Cap Rate";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    if (string.IsNullOrEmpty(taxInclusiveFlag) == true)
                    {
                        returnMessage = "Row " + i + ": TaxInclusiveFlag is missing. Please provide a valid TaxInclusiveFlag";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    string locationId = dbHierarchyDC.fnDesktopDataAdmin_GetPolicyLocationIdBasedOnCode_v1(locationCode).ToString();
                    if (locationId == "-1")
                    {
                        returnMessage = "Row " + i + ": There is no Policy Location available for the Location Code value " + locationCode + ". Please contact your Administrator";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    XmlElement xmlLocationCodeId = doc.CreateElement("LocationCodeId");
                    xmlLocationCodeId.InnerText = locationId;
                    xmlCDRItem.AppendChild(xmlLocationCodeId);

                    var currencyRepository = new CurrencyRepository();
                    if (currencyRepository.GetCurrency(currencyCode) == null)
                    {
                        returnMessage = "Row " + i + ": Currency Code " + currencyCode + " is invalid. Please provide a valid Currency Code";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    float capRateNumeric;
                    if (float.TryParse(capRate, out capRateNumeric) == false)
                    {
                        returnMessage = "Row " + i + ": CapRate must be numerical. Please provide a valid Cap Rate";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    if ((capRate.IndexOf('.') == -1 && capRate.Length > 12) || (capRate.IndexOf('.') != -1 && (capRate.Length - (capRate.IndexOf('.') + 1) > 5 || capRate.IndexOf('.') > 12)))
                    {
                        returnMessage = "Row " + i + ": CapRate exceeds the allowable size. Please provide a valid Cap Rate";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    taxInclusiveFlag = taxInclusiveFlag.ToUpper();
                    if (taxInclusiveFlag != "TRUE" && taxInclusiveFlag != "FALSE" && taxInclusiveFlag != "1" && taxInclusiveFlag != "0")
                    {
                        returnMessage = "Row " + i + ": TaxInclusiveFlag value is not valid. Please provide a valid Tax Inclusive Flag";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }
                    if (taxInclusiveFlag == "TRUE")
                    {
                        taxInclusiveFlag = "1";
                    }
                    if (taxInclusiveFlag == "FALSE")
                    {
                        taxInclusiveFlag = "0";
                    }

                    XmlElement xmlTaxInclusiveFlag = doc.CreateElement("TaxInclusiveFlag");
                    xmlTaxInclusiveFlag.InnerText = taxInclusiveFlag;
                    xmlCDRItem.AppendChild(xmlTaxInclusiveFlag);


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
                    xmlCDRItem.AppendChild(xmlEnabledFlag);

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
                    root.AppendChild(xmlCDRItem);



                }
            }
            if (i == 0)
            {
                returnMessage = "There is no data in the file";
                returnMessages.Add(returnMessage);
            }

            PolicyHotelCapRateImportStep2VM preImportCheckResult = new PolicyHotelCapRateImportStep2VM();
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
                    from n in dbHierarchyDC.spDesktopDataAdmin_UpdatePolicyHotelCapDefinedReferencesCount_v1(
                        policyGroupId,
                        System.Xml.Linq.XElement.Parse(doc.OuterXml),
                        adminUserGuid
                    )
                    select n).ToList();

                foreach (spDesktopDataAdmin_UpdatePolicyHotelCapDefinedReferencesCount_v1Result message in output)
                {
                    returnMessages.Add(message.MessageText.ToString());
                }

                preImportCheckResult.FileBytes = array;

                preImportCheckResult.IsValidData = true;
            }

            return preImportCheckResult;

        }

        public PolicyHotelCapRateImportStep3VM Import(byte[] FileBytes, int policyGroupId)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            string fileToText = fileToText = enc.GetString(FileBytes);

            PolicyHotelCapRateImportStep3VM cdrPostImportResult = new PolicyHotelCapRateImportStep3VM();
            List<string> returnMessages = new List<string>();

            // Create the xml document container, this will be used to store the data after the checks
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("PHCRIGs");
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

                    string[] cells = line.Split(',');

                    //extract the data items from the file
                    string locationCode = cells[0];		//Required
					string currencyCode = cells[1];		//Required
					string capRate = cells[2];			//Required
					string enabledFlag = cells[3] ?? "";
                    string enabledDate = cells[4] ?? "";
                    string expiryDate = cells[5] ?? "";
                    string travelDateValidFrom = cells[6] ?? "";
                    string travelDateValidTo = cells[7] ?? "";
                    string taxInclusiveFlag = cells[8];	//Required

					//Build the XML Element for items

					XmlElement xmlCDRItem = doc.CreateElement("PHCRIG");

                    XmlElement xmlLocationCode = doc.CreateElement("LocationCode");
                    xmlLocationCode.InnerText = locationCode;
                    xmlCDRItem.AppendChild(xmlLocationCode);

                    XmlElement xmlCurrencyCode = doc.CreateElement("CurrencyCode");
                    xmlCurrencyCode.InnerText = currencyCode;
                    xmlCDRItem.AppendChild(xmlCurrencyCode);

                    XmlElement xmlCapRate = doc.CreateElement("CapRate");
                    xmlCapRate.InnerText = capRate;
                    xmlCDRItem.AppendChild(xmlCapRate);

                    XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                    xmlEnabledDate.InnerText = enabledDate;
                    xmlCDRItem.AppendChild(xmlEnabledDate);

                    XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                    xmlExpiryDate.InnerText = expiryDate;
                    xmlCDRItem.AppendChild(xmlExpiryDate);

                    XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                    xmlTravelDateValidFrom.InnerText = travelDateValidFrom;
                    xmlCDRItem.AppendChild(xmlTravelDateValidFrom);

                    XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                    xmlTravelDateValidTo.InnerText = travelDateValidTo;
                    xmlCDRItem.AppendChild(xmlTravelDateValidTo);

                    string locationId = dbHierarchyDC.fnDesktopDataAdmin_GetPolicyLocationIdBasedOnCode_v1(locationCode).ToString();
                    XmlElement xmlLocationCodeId = doc.CreateElement("LocationCodeId");
                    xmlLocationCodeId.InnerText = locationId;
                    xmlCDRItem.AppendChild(xmlLocationCodeId);

                    if (taxInclusiveFlag == "TRUE")
                    {
                        taxInclusiveFlag = "1";
                    }
                    if (taxInclusiveFlag == "FALSE")
                    {
                        taxInclusiveFlag = "0";
                    }

                    XmlElement xmlTaxInclusiveFlag = doc.CreateElement("TaxInclusiveFlag");
                    xmlTaxInclusiveFlag.InnerText = taxInclusiveFlag;
                    xmlCDRItem.AppendChild(xmlTaxInclusiveFlag);

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
                    xmlCDRItem.AppendChild(xmlEnabledFlag);

                    XmlElement xmlSequenceNumber = doc.CreateElement("SequenceNumber");
                    xmlSequenceNumber.InnerText = i.ToString();
                    xmlCDRItem.AppendChild(xmlSequenceNumber);

                    //Attach the XML Element for an item to the Document
                    root.AppendChild(xmlCDRItem);

                }
            }

            //DB Check
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            try
            {
                var output = (from n in dbHierarchyDC.spDesktopDataAdmin_UpdatePolicyHotelCapDefinedReferences_v1(
                policyGroupId,
                System.Xml.Linq.XElement.Parse(doc.OuterXml),
                adminUserGuid)
                              select n).ToList();

                int deletedItemCount = 0;
                int addedItemCount = 0;

                foreach (spDesktopDataAdmin_UpdatePolicyHotelCapDefinedReferences_v1Result message in output)
                {
                    returnMessages.Add(message.MessageText.ToString());
                }

                cdrPostImportResult.ReturnMessages = returnMessages;
                cdrPostImportResult.AddedItemCount = addedItemCount;
                cdrPostImportResult.DeletedItemCount = deletedItemCount;
            }
            catch(Exception ex)
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
