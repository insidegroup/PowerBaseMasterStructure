using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Xml;
using System.Text;


namespace CWTDesktopDatabase.Repository
{
    public class PolicySupplierDealCodeRepository
    {
        private PolicySupplierDealCodeDC db = new PolicySupplierDealCodeDC(Settings.getConnectionString());

        //Get a Page of PolicySupplierDealCodes - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicySupplierDealCodes_v1Result> PagePolicySupplierDealCodes(int policyGroupId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectPolicySupplierDealCodes_v1(policyGroupId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicySupplierDealCodes_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
        public PolicySupplierDealCode GetPolicySupplierDealCode(int policySupplierDealCodeId)
        {
            return db.PolicySupplierDealCodes.SingleOrDefault(c => c.PolicySupplierDealCodeId == policySupplierDealCodeId);
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicySupplierDealCode policySupplierDealCode)
        {
            //PolicySupplierDealCodeType    
            PolicySupplierDealCodeTypeRepository policySupplierDealCodeTypeRepository = new PolicySupplierDealCodeTypeRepository();
            PolicySupplierDealCodeType policySupplierDealCodeType = new PolicySupplierDealCodeType();
            policySupplierDealCodeType = policySupplierDealCodeTypeRepository.GetPolicySupplierDealCodeType(policySupplierDealCode.PolicySupplierDealCodeTypeId);
            if (policySupplierDealCodeType != null)
            {
                policySupplierDealCode.PolicySupplierDealCodeTypeDescription = policySupplierDealCodeType.PolicySupplierDealCodeTypeDescription;
            }

            //GDS
            GDSRepository gdsRepository = new GDSRepository();
            GDS gds = new GDS();
            gds = gdsRepository.GetGDS(policySupplierDealCode.GDSCode);
            if (gds != null)
            {
                policySupplierDealCode.GDSName = gds.GDSName;
            }

            //PolicyLocation
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            PolicyLocation policyLocation = new PolicyLocation();

            policyLocation = policyLocationRepository.GetPolicyLocation((int)policySupplierDealCode.PolicyLocationId);
            if (policyLocation != null)
            {
                policySupplierDealCode.PolicyLocationName = policyLocation.PolicyLocationName;
            }

            //Supplier
            SupplierRepository supplierRepository = new SupplierRepository();
            Supplier supplier = new Supplier();
            supplier = supplierRepository.GetSupplier(policySupplierDealCode.SupplierCode, policySupplierDealCode.ProductId);
            if (supplier != null)
            {
                policySupplierDealCode.SupplierName = supplier.SupplierName;
            }

            //EnabledFlag is nullable
            if (policySupplierDealCode.EnabledFlag != true)
            {
                policySupplierDealCode.EnabledFlag = false;
            }
            policySupplierDealCode.EnabledFlagNonNullable = (bool)policySupplierDealCode.EnabledFlag;

			//OSIFlag is nullable
            if (policySupplierDealCode.OSIFlag != true)
            {
				policySupplierDealCode.OSIFlag = false;
            }
			policySupplierDealCode.OSIFlagNonNullable = (bool)policySupplierDealCode.OSIFlag;
			
			//Product
            ProductRepository productRepository = new ProductRepository();
            Product product = new Product();
            product = productRepository.GetProduct(policySupplierDealCode.ProductId);
            if (product != null)
            {
                policySupplierDealCode.ProductName = product.ProductName;
            }

            //PolicyGroup
            PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
            PolicyGroup policyGroup = policyGroupRepository.GetGroup(policySupplierDealCode.PolicyGroupId);
            policySupplierDealCode.PolicyGroupName = policyGroup.PolicyGroupName;

			//Tour Code Type
			TourCodeTypeRepository tourCodeTypeRepository = new TourCodeTypeRepository();
			TourCodeType tourCodeType = tourCodeTypeRepository.GetTourCodeType(policySupplierDealCode.TourCodeTypeId ?? 0);
			if (tourCodeType != null)
			{
				policySupplierDealCode.TourCodeType = tourCodeType;
			}
        }

        //Add
        public void Add(PolicySupplierDealCode policySupplierDealCode)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			//PolicySupplierDealCodeOSIs to XML
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("PolicySupplierDealCodeOSIs");
			doc.AppendChild(root);

			if (policySupplierDealCode.PolicySupplierDealCodeOSIs != null)
			{
				foreach (PolicySupplierDealCodeOSI policySupplierDealCodeOSI in policySupplierDealCode.PolicySupplierDealCodeOSIs)
				{
					if (policySupplierDealCodeOSI != null)
					{
						if (policySupplierDealCodeOSI.PolicySupplierDealCodeOSIDescription != null && policySupplierDealCodeOSI.PolicySupplierDealCodeOSIDescription != null)
						{
							XmlElement xmlPolicySupplierDealCodeOSI = doc.CreateElement("PolicySupplierDealCodeOSI");

							XmlElement xmlPolicySupplierDealCodeOSIDescription = doc.CreateElement("PolicySupplierDealCodeOSIDescription");
							xmlPolicySupplierDealCodeOSIDescription.InnerText = policySupplierDealCodeOSI.PolicySupplierDealCodeOSIDescription;
							xmlPolicySupplierDealCodeOSI.AppendChild(xmlPolicySupplierDealCodeOSIDescription);

							XmlElement xmlPolicySupplierDealCodeOSISequenceNumber = doc.CreateElement("PolicySupplierDealCodeOSISequenceNumber");
							xmlPolicySupplierDealCodeOSISequenceNumber.InnerText = policySupplierDealCodeOSI.PolicySupplierDealCodeOSISequenceNumber.ToString();
							xmlPolicySupplierDealCodeOSI.AppendChild(xmlPolicySupplierDealCodeOSISequenceNumber);

							root.AppendChild(xmlPolicySupplierDealCodeOSI);
						}
					}
				}
			} 
			
			db.spDesktopDataAdmin_InsertPolicySupplierDealCode_v1(
                policySupplierDealCode.SupplierCode,
                policySupplierDealCode.PolicySupplierDealCodeValue,
                policySupplierDealCode.PolicySupplierDealCodeDescription,
                policySupplierDealCode.GDSCode,
                policySupplierDealCode.ProductId,
                policySupplierDealCode.PolicySupplierDealCodeTypeId,
                policySupplierDealCode.PolicyGroupId,
                policySupplierDealCode.EnabledFlagNonNullable,
                policySupplierDealCode.EnabledDate,
                policySupplierDealCode.PolicyLocationId,
				policySupplierDealCode.ExpiryDate,
				policySupplierDealCode.TravelIndicator,
				policySupplierDealCode.TourCodeTypeId,
				policySupplierDealCode.Endorsement,
				policySupplierDealCode.EndorsementOverride,
				policySupplierDealCode.OSIFlagNonNullable,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
                adminUserGuid
            );

        }

        //Add
        public void Update(PolicySupplierDealCode policySupplierDealCode)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			//PolicySupplierDealCodeOSIs to XML
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("PolicySupplierDealCodeOSIs");
			doc.AppendChild(root);

			if (policySupplierDealCode.PolicySupplierDealCodeOSIs != null)
			{
				foreach (PolicySupplierDealCodeOSI policySupplierDealCodeOSI in policySupplierDealCode.PolicySupplierDealCodeOSIs)
				{
					if (policySupplierDealCodeOSI != null)
					{
						if (policySupplierDealCodeOSI.PolicySupplierDealCodeOSIDescription != null && policySupplierDealCodeOSI.PolicySupplierDealCodeOSIDescription != null)
						{
							XmlElement xmlPolicySupplierDealCodeOSI = doc.CreateElement("PolicySupplierDealCodeOSI");

							XmlElement xmlPolicySupplierDealCodeOSIDescription = doc.CreateElement("PolicySupplierDealCodeOSIDescription");
							xmlPolicySupplierDealCodeOSIDescription.InnerText = policySupplierDealCodeOSI.PolicySupplierDealCodeOSIDescription;
							xmlPolicySupplierDealCodeOSI.AppendChild(xmlPolicySupplierDealCodeOSIDescription);

							XmlElement xmlPolicySupplierDealCodeOSISequenceNumber = doc.CreateElement("PolicySupplierDealCodeOSISequenceNumber");
							xmlPolicySupplierDealCodeOSISequenceNumber.InnerText = policySupplierDealCodeOSI.PolicySupplierDealCodeOSISequenceNumber.ToString();
							xmlPolicySupplierDealCodeOSI.AppendChild(xmlPolicySupplierDealCodeOSISequenceNumber);

							root.AppendChild(xmlPolicySupplierDealCodeOSI);
						}
					}
				}
			} 


			db.spDesktopDataAdmin_UpdatePolicySupplierDealCode_v1(
                policySupplierDealCode.PolicySupplierDealCodeId,
                policySupplierDealCode.SupplierCode,
                policySupplierDealCode.PolicySupplierDealCodeValue,
                policySupplierDealCode.PolicySupplierDealCodeDescription,
                policySupplierDealCode.GDSCode,
                policySupplierDealCode.ProductId,
                policySupplierDealCode.PolicySupplierDealCodeTypeId,
                policySupplierDealCode.PolicyGroupId,
                policySupplierDealCode.EnabledFlagNonNullable,
                policySupplierDealCode.EnabledDate,
                policySupplierDealCode.ExpiryDate,
				policySupplierDealCode.PolicyLocationId, 
				policySupplierDealCode.TravelIndicator,
				policySupplierDealCode.TourCodeTypeId,
				policySupplierDealCode.Endorsement,
				policySupplierDealCode.EndorsementOverride,
				policySupplierDealCode.OSIFlagNonNullable,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
                adminUserGuid,
                policySupplierDealCode.VersionNumber
            );

        }

        //Delete
        public void Delete(PolicySupplierDealCode policySupplierDealCode)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicySupplierDealCode_v1(
                policySupplierDealCode.PolicySupplierDealCodeId,
                adminUserGuid,
                policySupplierDealCode.VersionNumber
                );
        }

		//Export Items to CSV
		public byte[] Export(int id)
		{
			StringBuilder sb = new StringBuilder();

			//Add Headers
			List<string> headers = new List<string>();
			headers.Add("Policy Group Name");
			headers.Add("Deal Code Value");
			headers.Add("Deal Code Description");
			headers.Add("Deal Code Type Description");
			headers.Add("Location Name");
			headers.Add("Location Code");
			headers.Add("GDS Name ");
			headers.Add("Product Name");
			headers.Add("Supplier Name");
			headers.Add("Supplier Code");
			headers.Add("Travel Indicator Description");
			headers.Add("Travel Indicator");
			headers.Add("Endorsement");
			headers.Add("Endorsement Override");
			headers.Add("OSI 1");
			headers.Add("Enabled Flag");
			headers.Add("Enabled Date");
			headers.Add("Expiry Date");
			headers.Add("Creation TimeStamp");
			headers.Add("Last Update Time Stamp");

			sb.AppendLine(String.Join(",", headers.Select(x => x.ToString()).ToArray()));

			//Add Items
			List<PolicySupplierDealCode> policySupplierDealCodes = db.PolicySupplierDealCodes.Where(x => x.PolicyGroupId == id).ToList();
			foreach (PolicySupplierDealCode item in policySupplierDealCodes)
			{

				//Edit Item
				EditItemForDisplay(item);

				//Location
				PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
				PolicyLocation policyLocation = new PolicyLocation();
				policyLocation = policyLocationRepository.GetPolicyLocation(Int32.Parse(item.PolicyLocationId.ToString()));
				if (policyLocation != null)
				{
					policyLocationRepository.EditForDisplay(policyLocation);
				}

				//TravelIndicator
				TravelIndicatorRepository travelIndicatorRepository = new TravelIndicatorRepository();
				TravelIndicator travelIndicator = new TravelIndicator();
				if (item.TravelIndicator != null)
				{
					travelIndicator = travelIndicatorRepository.GetTravelIndicator(item.TravelIndicator);
				}

				//PolicySupplierDealCodeOSI
				PolicySupplierDealCodeOSI policySupplierDealCodeOSI = null;
				if (item.PolicySupplierDealCodeOSIs != null)
				{
					policySupplierDealCodeOSI = item.PolicySupplierDealCodeOSIs.FirstOrDefault();
				}

				string date_format = "MM/dd/yy HH:mm";

				sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19}",
					!string.IsNullOrEmpty(item.PolicyGroupName) ? item.PolicyGroupName : "",
					!string.IsNullOrEmpty(item.PolicySupplierDealCodeValue) ? item.PolicySupplierDealCodeValue : "",
					!string.IsNullOrEmpty(item.PolicySupplierDealCodeDescription) ? item.PolicySupplierDealCodeDescription : "",
					!string.IsNullOrEmpty(item.PolicySupplierDealCodeTypeDescription) ? item.PolicySupplierDealCodeTypeDescription : "",

					!string.IsNullOrEmpty(policyLocation.LocationName) ? policyLocation.LocationName : "",
					!string.IsNullOrEmpty(policyLocation.LocationCode) ? policyLocation.LocationCode : "",

					!string.IsNullOrEmpty(item.GDSName) ? item.GDSName : "",

					!string.IsNullOrEmpty(item.ProductName) ? item.ProductName : "",
					!string.IsNullOrEmpty(item.SupplierName) ? item.SupplierName : "",
					!string.IsNullOrEmpty(item.SupplierCode) ? item.SupplierCode : "",

					//Valid when Deal Code Type = Tour Code
					travelIndicator != null && !string.IsNullOrEmpty(travelIndicator.TravelIndicatorDescription) ? travelIndicator.TravelIndicatorDescription : "",
					item.TravelIndicator != null && !string.IsNullOrEmpty(item.TravelIndicator) ? item.TravelIndicator : "",
					item.Endorsement != null && !string.IsNullOrEmpty(item.Endorsement) ? item.Endorsement : "",
					item.EndorsementOverride != null && !string.IsNullOrEmpty(item.EndorsementOverride) ? item.EndorsementOverride : "",
					
					policySupplierDealCodeOSI != null ? policySupplierDealCodeOSI.PolicySupplierDealCodeOSIDescription : "",
					item.EnabledFlag == true ? "True" : "False",
					item.EnabledDate.HasValue ? item.EnabledDate.Value.ToString(date_format) : "",
					item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString(date_format) : "",
					item.CreationTimestamp.HasValue ? item.CreationTimestamp.Value.ToString(date_format) : "",
					item.LastUpdateTimestamp.HasValue ? item.LastUpdateTimestamp.Value.ToString(date_format) : ""
				);

				sb.Append(Environment.NewLine);
			}

			return Encoding.ASCII.GetBytes(sb.ToString());
		}
    }
}
