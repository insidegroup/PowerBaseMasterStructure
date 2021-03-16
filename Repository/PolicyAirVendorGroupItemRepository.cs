using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Text;
using System.Xml;
using CWTDesktopDatabase.ViewModels;
using System.Globalization;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyAirVendorGroupItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
        private HierarchyDC dbHierarchyDC = new HierarchyDC(Settings.getConnectionString());
        private PolicyAirVendorGroupItemDC db_policy = new PolicyAirVendorGroupItemDC(Settings.getConnectionString());

        //Sortable List
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirVendorGroupItems_v1Result> GetPolicyAirVendorGroupItems(int policyGroupID, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPolicyAirVendorGroupItems_v1(policyGroupID, adminUserGuid, filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirVendorGroupItems_v1Result>(result,page,totalRecords);
            return paginatedView;

        }

        public List<SelectListItem> AirVendorRankings()
        {
            var numbers = (from p in Enumerable.Range(1, 9)
                           select new SelectListItem
                           {
                               Text = p.ToString(),
                               Value = p.ToString()
                           });
            return numbers.ToList();
        }

        //Add
        public void Add(PolicyAirVendorGroupItem policyAirVendorGroupItem, PolicyRouting policyRouting)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            int? policyAirVendorGroupItemId = new Int32();

            db.spDesktopDataAdmin_InsertPolicyAirVendorGroupItem_v1(
                ref policyAirVendorGroupItemId,
                policyAirVendorGroupItem.PolicyAirStatusId,
                policyAirVendorGroupItem.EnabledFlag,
                policyAirVendorGroupItem.EnabledDate,
                policyAirVendorGroupItem.ExpiryDate,
                policyAirVendorGroupItem.TravelDateValidFrom,
                policyAirVendorGroupItem.TravelDateValidTo,
                policyAirVendorGroupItem.PolicyGroupId,
                policyAirVendorGroupItem.SupplierCode,
                policyAirVendorGroupItem.ProductId,
                policyAirVendorGroupItem.AirVendorRanking,
                policyRouting.Name,
                policyRouting.FromGlobalFlag,
                policyRouting.FromGlobalRegionCode,
                policyRouting.FromGlobalSubRegionCode,
                policyRouting.FromCountryCode,
                policyRouting.FromCityCode,
                policyRouting.FromTravelPortCode,
                policyRouting.FromTraverlPortTypeId,
                policyRouting.ToGlobalFlag,
                policyRouting.ToGlobalRegionCode,
                policyRouting.ToGlobalSubRegionCode,
                policyRouting.ToCountryCode,
                policyRouting.ToCityCode,
                policyRouting.ToTravelPortCode,
                policyRouting.ToTravelPortTypeId,
                policyRouting.RoutingViceVersaFlag,
                adminUserGuid
            );

            policyAirVendorGroupItem.PolicyAirVendorGroupItemId = (int)policyAirVendorGroupItemId;
        }

        //Edit
        public void Update(PolicyAirVendorGroupItem policyAirVendorGroupItem, PolicyRouting policyRouting)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicyAirVendorGroupItem_v1(
                policyAirVendorGroupItem.PolicyAirVendorGroupItemId,
                policyAirVendorGroupItem.PolicyAirStatusId,
                policyAirVendorGroupItem.EnabledFlag,
                policyAirVendorGroupItem.EnabledDate,
                policyAirVendorGroupItem.ExpiryDate,
                policyAirVendorGroupItem.TravelDateValidFrom,
                policyAirVendorGroupItem.TravelDateValidTo,
                policyAirVendorGroupItem.PolicyGroupId,
                policyAirVendorGroupItem.SupplierCode,
                policyAirVendorGroupItem.ProductId,
                policyAirVendorGroupItem.AirVendorRanking,
                policyRouting.Name,
                policyRouting.FromGlobalFlag,
                policyRouting.FromGlobalRegionCode,
                policyRouting.FromGlobalSubRegionCode,
                policyRouting.FromCountryCode,
                policyRouting.FromCityCode,
                policyRouting.FromTravelPortCode,
                policyRouting.FromTraverlPortTypeId,
                policyRouting.ToGlobalFlag,
                policyRouting.ToGlobalRegionCode,
                policyRouting.ToGlobalSubRegionCode,
                policyRouting.ToCountryCode,
                policyRouting.ToCityCode,
                policyRouting.ToTravelPortCode,
                policyRouting.ToTravelPortTypeId,
                policyRouting.RoutingViceVersaFlag,
                adminUserGuid,
                policyAirVendorGroupItem.VersionNumber
            );

        }

        //Delete
        public void Delete(PolicyAirVendorGroupItem policyAirVendorGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyAirVendorGroupItem_v1(
                policyAirVendorGroupItem.PolicyAirVendorGroupItemId,
                adminUserGuid,
                policyAirVendorGroupItem.VersionNumber
                );
        }

        //Get one Item
        public PolicyAirVendorGroupItem GetPolicyAirVendorGroupItem(int policyAirVendorGroupItemId)
        {
            PolicyAirVendorGroupItemDC db = new PolicyAirVendorGroupItemDC(Settings.getConnectionString());
            return db.PolicyAirVendorGroupItems.SingleOrDefault(c => c.PolicyAirVendorGroupItemId == policyAirVendorGroupItemId);
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyAirVendorGroupItem policyAirVendorGroupItem)
        {
            if (policyAirVendorGroupItem.PolicyAirStatusId != null)
            {
                int policyAirStatusId = (int)policyAirVendorGroupItem.PolicyAirStatusId;
                PolicyAirStatusRepository policyAirStatusRepository = new PolicyAirStatusRepository();
                PolicyAirStatus policyAirStatus = new PolicyAirStatus();
                policyAirStatus = policyAirStatusRepository.GetPolicyAirStatus(policyAirStatusId);
                policyAirVendorGroupItem.PolicyAirStatus = policyAirStatus.PolicyAirStatusDescription;
            }
            else
            {
                policyAirVendorGroupItem.PolicyAirStatus = "None";
            }

            //populate new PolicyAirVendorGroupItem with PolicyGroupName    
            if (policyAirVendorGroupItem.PolicyGroupId != 0)
            {
                PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
                PolicyGroup policyGroup = new PolicyGroup();
                policyGroup = policyGroupRepository.GetGroup(policyAirVendorGroupItem.PolicyGroupId);
                policyAirVendorGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;
            }

            //Supplier
            SupplierRepository supplierRepository = new SupplierRepository();
            Supplier supplier = new Supplier();
            supplier = supplierRepository.GetSupplier(policyAirVendorGroupItem.SupplierCode, policyAirVendorGroupItem.ProductId);
            if (supplier != null)
            {
                policyAirVendorGroupItem.SupplierName = supplier.SupplierName;
            }

            //Product
            ProductRepository productRepository = new ProductRepository();
            Product product = new Product();
            product = productRepository.GetProduct(policyAirVendorGroupItem.ProductId);
            if (product != null)
            {
                policyAirVendorGroupItem.ProductName = product.ProductName;
            }

        }

		//Export Items to CSV
		public byte[] Export(int id)
		{
			StringBuilder sb = new StringBuilder();
			
			//Add Headers
			List<string> headers = new List<string>();
			headers.Add("Air Status Description");
			headers.Add("Supplier Code");
			headers.Add("Ranking");
			headers.Add("Sequence Number");
			headers.Add("Enabled Flag");
			headers.Add("Enabled Date");
			headers.Add("Expiry Date");
			headers.Add("Travel Date Valid From");
			headers.Add("Travel Date Valid To");
            headers.Add("FromGlobalFlag");
            headers.Add("FromGlobalRegionCode");
            headers.Add("FromGlobalSubRegionCode");
            headers.Add("FromCountryCode");
            headers.Add("FromCityCode");
            headers.Add("FromTravelPortCode");
            headers.Add("ToGlobalFlag");
            headers.Add("ToGlobalRegionCode");
            headers.Add("ToGlobalSubRegionCode");
            headers.Add("ToCountryCode");
            headers.Add("ToCityCode");
            headers.Add("ToTravelPortCode");
            headers.Add("Routing Vice Versa Flag");
			headers.Add("Creation TimeStamp");
			headers.Add("Last Update Time Stamp");
            headers.Add("Advice");

            sb.AppendLine(String.Join(",", headers.Select(x => x.ToString()).ToArray()));

			//Add Items
			List<PolicyAirVendorGroupItem> policyAirVendorGroupItems = db_policy.PolicyAirVendorGroupItems.Where(x => x.PolicyGroupId == id).ToList();
			foreach (PolicyAirVendorGroupItem item in policyAirVendorGroupItems)
			{

				//Edit Item
				EditItemForDisplay(item);

				//Get Policy Routing
				PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();
				PolicyRouting policyRouting = policyRoutingRepository.GetPolicyRouting(item.PolicyRoutingId);
				policyRoutingRepository.EditForDisplay(policyRouting);

                //Advice Count
                int adviceCount = 0;
                PolicyAirVendorGroupItemLanguageRepository policyAirVendorGroupItemLanguageRepository = new PolicyAirVendorGroupItemLanguageRepository();
                List<PolicyAirVendorGroupItemLanguage> policyAirVendorGroupItemLanguages = policyAirVendorGroupItemLanguageRepository.GetItems(item.PolicyAirVendorGroupItemId);
                if (policyAirVendorGroupItemLanguages != null)
                {
                    adviceCount = policyAirVendorGroupItemLanguages.Count();
                }

                string date_format = "MM/dd/yy HH:mm";
                string short_date_format = "yyyy/MM/dd";

				sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24}",
					!string.IsNullOrEmpty(item.PolicyAirStatus) ? item.PolicyAirStatus : "",
					!string.IsNullOrEmpty(item.SupplierCode) ? item.SupplierCode : "",
					!string.IsNullOrEmpty(item.PolicyAirStatus) && item.PolicyAirStatus == "Preferred" ? item.AirVendorRanking.ToString() : "NULL",
					item.SequenceNumber != 0 ? item.SequenceNumber.ToString() : "",
					item.EnabledFlag == true ? "True" : "False",
					item.EnabledDate.HasValue ? item.EnabledDate.Value.ToString(short_date_format) : "",
					item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString(short_date_format) : "",
					item.TravelDateValidFrom.HasValue ? item.TravelDateValidFrom.Value.ToString(short_date_format) : "",
					item.TravelDateValidTo.HasValue ? item.TravelDateValidTo.Value.ToString(short_date_format) : "",

                    //Policy Routing
                    policyRouting.FromGlobalFlag ? "True" : "False",
                    !string.IsNullOrEmpty(policyRouting.FromGlobalRegionCode) ? policyRouting.FromGlobalRegionCode : "",
                    !string.IsNullOrEmpty(policyRouting.FromGlobalSubRegionCode) ? policyRouting.FromGlobalSubRegionCode : "",
                    !string.IsNullOrEmpty(policyRouting.FromCountryCode) ? policyRouting.FromCountryCode : "",
                    !string.IsNullOrEmpty(policyRouting.FromCityCode) ? policyRouting.FromCityCode : "",
                    !string.IsNullOrEmpty(policyRouting.FromTravelPortCode) ? policyRouting.FromTravelPortCode : "",
                    policyRouting.ToGlobalFlag ? "True" : "False",
                    !string.IsNullOrEmpty(policyRouting.ToGlobalRegionCode) ? policyRouting.ToGlobalRegionCode : "",
                    !string.IsNullOrEmpty(policyRouting.ToGlobalSubRegionCode) ? policyRouting.ToGlobalSubRegionCode : "",
                    !string.IsNullOrEmpty(policyRouting.ToCountryCode) ? policyRouting.ToCountryCode : "",
                    !string.IsNullOrEmpty(policyRouting.ToCityCode) ? policyRouting.ToCityCode : "",
                    !string.IsNullOrEmpty(policyRouting.ToTravelPortCode) ? policyRouting.ToTravelPortCode : "",
                    policyRouting.RoutingViceVersaFlag ? "True" : "False",
					
                    item.CreationTimestamp.HasValue ? item.CreationTimestamp.Value.ToString(date_format) : "",
					item.LastUpdateTimestamp.HasValue ? item.LastUpdateTimestamp.Value.ToString(date_format) : "",
                    adviceCount > 0 ? "True" : "False"
                );

				sb.Append(Environment.NewLine);
			} 
			
			return Encoding.ASCII.GetBytes(sb.ToString());
		}

        public PolicyAirVendorImportStep2VM PreImportCheck(HttpPostedFileBase file, int policyGroupId)
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

                    string[] cells = line.Split(',');

                    //extract the data items from the file

                    string airStatusDescription = cells[0];     //Required: (eg.Preferred)
                    string supplierCode = cells[1];			    //Required: (eg.DL)
                    string ranking = cells[2];			        //(eg. 1 or NULL if status is other than Preferred)
                    string order = cells[3];			        //(from SequenceNumber)
                    string enabledFlag = cells[4];			    //(True / False)(1, 0)
                    string enabledDate = cells[5];			    //(YYYY / MM / DD)
                    string expiryDate = cells[6];			    //(YYYY / MM / DD)
                    string travelDateValidFrom = cells[7];		//(YYYY / MM / DD)
                    string travelDateValidTo = cells[8];        //(YYYY / MM / DD)

                    string fromGlobalFlag = cells[9];			//(True / False)(1, 0)
                    string fromGlobalRegionCode = cells[10];	//(eg.APAC)
                    string fromGlobalSubRegionCode = cells[11]; //(eg.AFRIC)
                    string fromCountryCode = cells[12];			//(eg.US)
                    string fromCityCode = cells[13];			//(eg.LON)
                    string fromTravelPortCode = cells[14];      //(eg.LHR)

                    string toGlobalFlag = cells[15];			//(True / False)(1, 0)
                    string toGlobalRegionCode = cells[16];		//(eg.APAC)
                    string toGlobalSubRegionCode = cells[17];	//(eg.AFRIC)
                    string toCountryCode = cells[18];			//(eg.US)
                    string toCityCode = cells[19];			    //(eg.LON)
                    string toTravelPortCode = cells[20];        //(eg.LHR)

                    string routingViceVersaFlag = cells[21];	//Required: (True / False)(1, 0)

                    //Build the XML Element for items

                    XmlElement xmlPAVItem = doc.CreateElement("PAVIG");

                    XmlElement xmlSupplierCode = doc.CreateElement("SupplierCode");
                    xmlSupplierCode.InnerText = supplierCode;
                    xmlPAVItem.AppendChild(xmlSupplierCode);

                    XmlElement xmlRanking = doc.CreateElement("Ranking");
                    xmlRanking.InnerText = ranking;
                    xmlPAVItem.AppendChild(xmlRanking);

                    XmlElement xmlOrder = doc.CreateElement("SequenceNumber");
                    xmlOrder.InnerText = order;
                    xmlPAVItem.AppendChild(xmlOrder);

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

                    XmlElement xmlFromGlobalFlag = doc.CreateElement("FromGlobalFlag");
                    xmlFromGlobalFlag.InnerText = fromGlobalFlag;
                    xmlPAVItem.AppendChild(xmlFromGlobalFlag);

                    XmlElement xmlFromGlobalRegionCode = doc.CreateElement("FromGlobalRegionCode");
                    xmlFromGlobalRegionCode.InnerText = fromGlobalRegionCode;
                    xmlPAVItem.AppendChild(xmlFromGlobalRegionCode);

                    XmlElement xmlFromGlobalSubRegionCode = doc.CreateElement("FromGlobalSubRegionCode");
                    xmlFromGlobalSubRegionCode.InnerText = fromGlobalSubRegionCode;
                    xmlPAVItem.AppendChild(xmlFromGlobalSubRegionCode);

                    XmlElement xmlFromCountryCode = doc.CreateElement("FromCountryCode");
                    xmlFromCountryCode.InnerText = fromCountryCode;
                    xmlPAVItem.AppendChild(xmlFromCountryCode);

                    XmlElement xmlFromCityCode = doc.CreateElement("FromCityCode");
                    xmlFromCityCode.InnerText = fromCityCode;
                    xmlPAVItem.AppendChild(xmlFromCityCode);

                    XmlElement xmlFromTravelPortCode = doc.CreateElement("FromTravelPortCode");
                    xmlFromTravelPortCode.InnerText = fromTravelPortCode;
                    xmlPAVItem.AppendChild(xmlFromTravelPortCode);

                    XmlElement xmlToGlobalFlag = doc.CreateElement("ToGlobalFlag");
                    xmlToGlobalFlag.InnerText = toGlobalFlag;
                    xmlPAVItem.AppendChild(xmlToGlobalFlag);

                    XmlElement xmlToGlobalRegionCode = doc.CreateElement("ToGlobalRegionCode");
                    xmlToGlobalRegionCode.InnerText = toGlobalRegionCode;
                    xmlPAVItem.AppendChild(xmlToGlobalRegionCode);

                    XmlElement xmlToGlobalSubRegionCode = doc.CreateElement("ToGlobalSubRegionCode");
                    xmlToGlobalSubRegionCode.InnerText = toGlobalSubRegionCode;
                    xmlPAVItem.AppendChild(xmlToGlobalSubRegionCode);

                    XmlElement xmlToCountryCode = doc.CreateElement("ToCountryCode");
                    xmlToCountryCode.InnerText = toCountryCode;
                    xmlPAVItem.AppendChild(xmlToCountryCode);

                    XmlElement xmlToCityCode = doc.CreateElement("ToCityCode");
                    xmlToCityCode.InnerText = toCityCode;
                    xmlPAVItem.AppendChild(xmlToCityCode);

                    XmlElement xmlToTravelPortCode = doc.CreateElement("ToTravelPortCode");
                    xmlToTravelPortCode.InnerText = toTravelPortCode;
                    xmlPAVItem.AppendChild(xmlToTravelPortCode);

                    XmlElement xmlRoutingViceVersaFlag = doc.CreateElement("RoutingViceVersaFlag");
                    xmlRoutingViceVersaFlag.InnerText = routingViceVersaFlag;
                    xmlPAVItem.AppendChild(xmlRoutingViceVersaFlag);

                    //Validate data

                    if (string.IsNullOrEmpty(airStatusDescription) == true)
                    {
                        returnMessage = "Row " + i + ": Air Status Description is missing. Please provide a valid Air Status";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    int policyAirStatusId = 0;
                    PolicyAirStatusRepository policyAirStatusRepository = new PolicyAirStatusRepository();
                    PolicyAirStatus policyAirStatus = policyAirStatusRepository.GetPolicyAirStatusByDescription(airStatusDescription);
                    if (policyAirStatus == null)
                    {
                        returnMessage = "Row " + i + ": Air Status Description is invalid. Please provide a valid Air Status";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    } else
                    {
                        policyAirStatusId = policyAirStatus.PolicyAirStatusId;
                    }

                    XmlElement xmlPolicyAirStatusId = doc.CreateElement("PolicyAirStatusId");
                    xmlPolicyAirStatusId.InnerText = policyAirStatusId.ToString();
                    xmlPAVItem.AppendChild(xmlPolicyAirStatusId);

                    if (airStatusDescription == "Preferred" && string.IsNullOrEmpty(ranking) == true)
                    {
                        returnMessage = "Row " + i + ": Ranking is missing. Please provide a valid Ranking";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    if (string.IsNullOrEmpty(supplierCode) == true)
                    {
                        returnMessage = "Row " + i + ": Supplier Code is missing. Please provide a valid Supplier Code";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }
                    else
                    {
                        //Validate supplier code is in table for product Air
                        SupplierRepository supplierRepository = new SupplierRepository();
                        Supplier supplier = supplierRepository.GetSupplier(supplierCode, 1);
                        if(supplier == null)
                        {
                            returnMessage = "Row " + i + ": Supplier Code is invalid. Please provide a valid Supplier Code";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(fromGlobalFlag) == true &&
                        string.IsNullOrEmpty(fromGlobalRegionCode) == true &&
                        string.IsNullOrEmpty(fromGlobalSubRegionCode) == true &&
                        string.IsNullOrEmpty(fromCountryCode) == true &&
                        string.IsNullOrEmpty(fromCityCode) == true &&
                        string.IsNullOrEmpty(fromTravelPortCode) == true)
                    {
                        returnMessage = "Row " + i + ": From routing is missing. Please provide a valid origination";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    HierarchyRepository hierarchyRepository = new HierarchyRepository();

                    //Validate Routings

                    if (!string.IsNullOrEmpty(fromGlobalRegionCode))
                    {
                        GlobalRegion globalRegion = new GlobalRegion();
                        globalRegion = hierarchyRepository.GetGlobalRegion(fromGlobalRegionCode);
                        if (globalRegion == null)
                        {
                            returnMessage = "Row " + i + ": From Global Region is invalid. Please provide a valid From Global Region";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(fromGlobalSubRegionCode))
                    {
                        GlobalSubRegion globalSubRegion = new GlobalSubRegion();
                        globalSubRegion = hierarchyRepository.GetGlobalSubRegion(fromGlobalSubRegionCode);
                        if (globalSubRegion == null)
                        {
                            returnMessage = "Row " + i + ": From Global Sub Region is invalid. Please provide a valid From Global Sub Region";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(fromCountryCode))
                    {
                        Country country = new Country();
                        country = hierarchyRepository.GetCountry(fromCountryCode);
                        if (country == null)
                        {
                            returnMessage = "Row " + i + ": From Country is invalid. Please provide a valid From Country";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(fromCityCode))
                    {
                        CityRepository cityRepository = new CityRepository();
                        City city = cityRepository.GetCity(fromCityCode);
                        if (city == null)
                        {
                            returnMessage = "Row " + i + ": From City is invalid. Please provide a valid From City";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(fromTravelPortCode))
                    {
                        TravelPortRepository travelPortRepository = new TravelPortRepository();
                        TravelPort travelPort = travelPortRepository.GetTravelPort(fromTravelPortCode);
                        if (travelPort == null)
                        {
                            returnMessage = "Row " + i + ": From Travel Port is invalid. Please provide a valid From Travel Port";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }
                    
                    //Sanitise From Global Flag
                    if (!string.IsNullOrEmpty(fromGlobalFlag) && fromGlobalFlag != "0" && fromGlobalFlag.ToUpper() != "FALSE" && fromGlobalFlag.ToUpper() != "NULL" && fromGlobalFlag.ToUpper() != "TRUE" && fromGlobalFlag.ToUpper() != "1")
                    {
                        returnMessage = "Row " + i + ":From Global routing is invalid. Please provide a valid origination";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    } else {

                        if (
                            (
                                string.IsNullOrEmpty(fromGlobalFlag) == true ||
                                fromGlobalFlag == "0" ||
                                fromGlobalFlag.ToUpper() == "FALSE" ||
                                fromGlobalFlag.ToUpper() == "NULL"
                            ) &&
                            string.IsNullOrEmpty(fromGlobalRegionCode) == true &&
                            string.IsNullOrEmpty(fromGlobalSubRegionCode) == true &&
                            string.IsNullOrEmpty(fromCountryCode) == true &&
                            string.IsNullOrEmpty(fromCityCode) == true &&
                            string.IsNullOrEmpty(fromTravelPortCode) == true)
                        {
                            returnMessage = "Row " + i + ": From routing is missing. Please provide a valid origination";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }

                        if (
                            (fromGlobalFlag.ToUpper() == "TRUE" || fromGlobalFlag.ToUpper() == "1") &&
                            (
                                !string.IsNullOrEmpty(fromGlobalRegionCode) == true ||
                                !string.IsNullOrEmpty(fromGlobalSubRegionCode) == true ||
                                !string.IsNullOrEmpty(fromCountryCode) == true ||
                                !string.IsNullOrEmpty(fromCityCode) == true ||
                                !string.IsNullOrEmpty(fromTravelPortCode) == true
                             )
                        )
                        {
                            returnMessage = "Row " + i + ": From routing has multiple values. Please provide a valid origination";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    //Validate Routings
                    if (!string.IsNullOrEmpty(toGlobalRegionCode))
                    {
                        GlobalRegion globalRegion = new GlobalRegion();
                        globalRegion = hierarchyRepository.GetGlobalRegion(toGlobalRegionCode);
                        if (globalRegion == null)
                        {
                            returnMessage = "Row " + i + ": To Global Region is invalid. Please provide a valid To Global Region";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(toGlobalSubRegionCode))
                    {
                        GlobalSubRegion globalSubRegion = new GlobalSubRegion();
                        globalSubRegion = hierarchyRepository.GetGlobalSubRegion(toGlobalSubRegionCode);
                        if (globalSubRegion == null)
                        {
                            returnMessage = "Row " + i + ": To Global Sub Region is invalid. Please provide a valid To Global Sub Region";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(toCountryCode))
                    {
                        Country country = new Country();
                        country = hierarchyRepository.GetCountry(toCountryCode);
                        if (country == null)
                        {
                            returnMessage = "Row " + i + ": To Country is invalid. Please provide a valid To Country";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(toCityCode))
                    {
                        CityRepository cityRepository = new CityRepository();
                        City city = cityRepository.GetCity(toCityCode);
                        if (city == null)
                        {
                            returnMessage = "Row " + i + ": To City is invalid. Please provide a valid To City";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(toTravelPortCode))
                    {
                        TravelPortRepository travelPortRepository = new TravelPortRepository();
                        TravelPort travelPort = travelPortRepository.GetTravelPort(toTravelPortCode);
                        if (travelPort == null)
                        {
                            returnMessage = "Row " + i + ": To Travel Port is invalid. Please provide a valid To Travel Port";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(toGlobalFlag) == true &&
                        string.IsNullOrEmpty(toGlobalRegionCode) == true &&
                        string.IsNullOrEmpty(toGlobalSubRegionCode) == true &&
                        string.IsNullOrEmpty(toCountryCode) == true &&
                        string.IsNullOrEmpty(toCityCode) == true &&
                        string.IsNullOrEmpty(toTravelPortCode) == true)
                    {
                        returnMessage = "Row " + i + ": To routing is missing. Please provide a valid destination";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    //Sanitise To Global Flag
                    if (!string.IsNullOrEmpty(toGlobalFlag) && toGlobalFlag != "0" && toGlobalFlag.ToUpper() != "FALSE" && toGlobalFlag.ToUpper() != "NULL" && toGlobalFlag.ToUpper() != "TRUE" && toGlobalFlag.ToUpper() != "1")
                    {
                        returnMessage = "Row " + i + ":To Global routing is invalid. Please provide a valid origination";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }
                    else
                    {
                        if (
                        (
                            string.IsNullOrEmpty(toGlobalFlag) == true ||
                            toGlobalFlag == "0" ||
                            toGlobalFlag.ToUpper() == "FALSE" ||
                            toGlobalFlag.ToUpper() == "NULL"
                        ) &&
                        string.IsNullOrEmpty(toGlobalRegionCode) == true &&
                        string.IsNullOrEmpty(toGlobalSubRegionCode) == true &&
                        string.IsNullOrEmpty(toCountryCode) == true &&
                        string.IsNullOrEmpty(toCityCode) == true &&
                        string.IsNullOrEmpty(toTravelPortCode) == true)
                        {
                            returnMessage = "Row " + i + ": To routing is missing. Please provide a valid destination";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }

                        if (
                            (toGlobalFlag.ToUpper() == "TRUE" || toGlobalFlag.ToUpper() == "1") &&
                            (
                                !string.IsNullOrEmpty(toGlobalRegionCode) == true ||
                                !string.IsNullOrEmpty(toGlobalSubRegionCode) == true ||
                                !string.IsNullOrEmpty(toCountryCode) == true ||
                                !string.IsNullOrEmpty(toCityCode) == true ||
                                !string.IsNullOrEmpty(toTravelPortCode) == true
                             )
                        )
                        {
                            returnMessage = "Row " + i + ": To Routing has multiple values. Please provide a valid destination";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(routingViceVersaFlag) == true)
                    {
                        returnMessage = "Row " + i + ": RoutingViceVersaFlag is missing. Please provide a valid Routing Vice Versa value";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
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

            PolicyAirVendorImportStep2VM preImportCheckResult = new PolicyAirVendorImportStep2VM();
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
                    from n in dbHierarchyDC.spDesktopDataAdmin_UpdatePolicyAirVendorGroupItemCount_v1(
                        policyGroupId,
                        System.Xml.Linq.XElement.Parse(doc.OuterXml),
                        adminUserGuid
                    )
                    select n).ToList();

                foreach (spDesktopDataAdmin_UpdatePolicyAirVendorGroupItemCount_v1Result message in output)
                {
                    returnMessages.Add(message.MessageText.ToString());
                }

                preImportCheckResult.FileBytes = array;

                preImportCheckResult.IsValidData = true;
            }

            return preImportCheckResult;

        }

        public PolicyAirVendorImportStep3VM Import(byte[] FileBytes, int policyGroupId)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            string fileToText = fileToText = enc.GetString(FileBytes);

            PolicyAirVendorImportStep3VM cdrPostImportResult = new PolicyAirVendorImportStep3VM();
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

                    string[] cells = line.Split(',');

                    //extract the data items from the file

                    string airStatusDescription = cells[0];     //Required: (eg.Preferred)
                    string supplierCode = cells[1];			    //Required: (eg.DL)
                    string ranking = cells[2];			        //(eg. 1 or NULL if status is other than Preferred)
                    string order = cells[3];			        //(from SequenceNumber)
                    string enabledFlag = cells[4];			    //(True / False)(1, 0)
                    string enabledDate = cells[5];			    //(YYYY / MM / DD)
                    string expiryDate = cells[6];			    //(YYYY / MM / DD)
                    string travelDateValidFrom = cells[7];		//(YYYY / MM / DD)
                    string travelDateValidTo = cells[8];        //(YYYY / MM / DD)

                    string fromGlobalFlag = cells[9];			//(True / False)(1, 0)
                    string fromGlobalRegionCode = cells[10];	//(eg.APAC)
                    string fromGlobalSubRegionCode = cells[11]; //(eg.AFRIC)
                    string fromCountryCode = cells[12];			//(eg.US)
                    string fromCityCode = cells[13];			//(eg.LON)
                    string fromTravelPortCode = cells[14];      //(eg.LHR)

                    string toGlobalFlag = cells[15];			//(True / False)(1, 0)
                    string toGlobalRegionCode = cells[16];		//(eg.APAC)
                    string toGlobalSubRegionCode = cells[17];	//(eg.AFRIC)
                    string toCountryCode = cells[18];			//(eg.US)
                    string toCityCode = cells[19];			    //(eg.LON)
                    string toTravelPortCode = cells[20];        //(eg.LHR)

                    string routingViceVersaFlag = cells[21];	//Required: (True / False)(1, 0)

                    //Build the XML Element for items

                    XmlElement xmlPAVItem = doc.CreateElement("PAVIG");

                    PolicyAirStatusRepository policyAirStatusRepository = new PolicyAirStatusRepository();
                    PolicyAirStatus policyAirStatus = policyAirStatusRepository.GetPolicyAirStatusByDescription(airStatusDescription);
                    
                    XmlElement xmlPolicyAirStatusId = doc.CreateElement("PolicyAirStatusId");
                    xmlPolicyAirStatusId.InnerText = policyAirStatus.PolicyAirStatusId.ToString();
                    xmlPAVItem.AppendChild(xmlPolicyAirStatusId);

                    XmlElement xmlSupplierCode = doc.CreateElement("SupplierCode");
                    xmlSupplierCode.InnerText = supplierCode;
                    xmlPAVItem.AppendChild(xmlSupplierCode);

                    XmlElement xmlRanking = doc.CreateElement("Ranking");
                    xmlRanking.InnerText = (ranking == "NULL") ? "" : ranking;
                    xmlPAVItem.AppendChild(xmlRanking);

                    XmlElement xmlOrder = doc.CreateElement("SequenceNumber");
                    xmlOrder.InnerText = order;
                    xmlPAVItem.AppendChild(xmlOrder);

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

                    XmlElement xmlFromGlobalFlag = doc.CreateElement("FromGlobalFlag");
                    xmlFromGlobalFlag.InnerText = (fromGlobalFlag == "") ? "0" : fromGlobalFlag;
                    xmlPAVItem.AppendChild(xmlFromGlobalFlag);

                    XmlElement xmlFromGlobalRegionCode = doc.CreateElement("FromGlobalRegionCode");
                    xmlFromGlobalRegionCode.InnerText = fromGlobalRegionCode;
                    xmlPAVItem.AppendChild(xmlFromGlobalRegionCode);

                    XmlElement xmlFromGlobalSubRegionCode = doc.CreateElement("FromGlobalSubRegionCode");
                    xmlFromGlobalSubRegionCode.InnerText = fromGlobalSubRegionCode;
                    xmlPAVItem.AppendChild(xmlFromGlobalSubRegionCode);

                    XmlElement xmlFromCountryCode = doc.CreateElement("FromCountryCode");
                    xmlFromCountryCode.InnerText = fromCountryCode;
                    xmlPAVItem.AppendChild(xmlFromCountryCode);

                    XmlElement xmlFromCityCode = doc.CreateElement("FromCityCode");
                    xmlFromCityCode.InnerText = fromCityCode;
                    xmlPAVItem.AppendChild(xmlFromCityCode);

                    XmlElement xmlFromTravelPortCode = doc.CreateElement("FromTravelPortCode");
                    xmlFromTravelPortCode.InnerText = fromTravelPortCode;
                    xmlPAVItem.AppendChild(xmlFromTravelPortCode);

                    XmlElement xmlToGlobalFlag = doc.CreateElement("ToGlobalFlag");
                    xmlToGlobalFlag.InnerText = (toGlobalFlag == "") ? "0" : toGlobalFlag;
                    xmlPAVItem.AppendChild(xmlToGlobalFlag);

                    XmlElement xmlToGlobalRegionCode = doc.CreateElement("ToGlobalRegionCode");
                    xmlToGlobalRegionCode.InnerText = toGlobalRegionCode;
                    xmlPAVItem.AppendChild(xmlToGlobalRegionCode);

                    XmlElement xmlToGlobalSubRegionCode = doc.CreateElement("ToGlobalSubRegionCode");
                    xmlToGlobalSubRegionCode.InnerText = toGlobalSubRegionCode;
                    xmlPAVItem.AppendChild(xmlToGlobalSubRegionCode);

                    XmlElement xmlToCountryCode = doc.CreateElement("ToCountryCode");
                    xmlToCountryCode.InnerText = toCountryCode;
                    xmlPAVItem.AppendChild(xmlToCountryCode);

                    XmlElement xmlToCityCode = doc.CreateElement("ToCityCode");
                    xmlToCityCode.InnerText = toCityCode;
                    xmlPAVItem.AppendChild(xmlToCityCode);

                    XmlElement xmlToTravelPortCode = doc.CreateElement("ToTravelPortCode");
                    xmlToTravelPortCode.InnerText = toTravelPortCode;
                    xmlPAVItem.AppendChild(xmlToTravelPortCode);

                    XmlElement xmlRoutingViceVersaFlag = doc.CreateElement("RoutingViceVersaFlag");
                    xmlRoutingViceVersaFlag.InnerText = routingViceVersaFlag;
                    xmlPAVItem.AppendChild(xmlRoutingViceVersaFlag);

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
                var output = (from n in dbHierarchyDC.spDesktopDataAdmin_UpdatePolicyAirVendorGroupItems_v1(
                policyGroupId,
                System.Xml.Linq.XElement.Parse(doc.OuterXml),
                adminUserGuid)
                              select n).ToList();

                int deletedItemCount = 0;
                int addedItemCount = 0;

                foreach (spDesktopDataAdmin_UpdatePolicyAirVendorGroupItems_v1Result message in output)
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
