using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.ViewModels;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Repository
{
    public class ClientSubUnitContactRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List of Contacts in a ClientSubUnit - Paged
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitContacts_v1Result> PageClientSubUnitContacts(string clientSubUnitGuid, string filter, string sortField, int sortOrder, int page)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectClientSubUnitContacts_v1(clientSubUnitGuid, filter, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitContacts_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;

		}

		//Get Contact
		public Contact GetContact(int id)
		{
			return db.Contacts.SingleOrDefault(c => c.ContactId == id);
		}

		//Add Data From Linked Tables for Display
		public void EditForDisplay(Contact contact)
		{
			ContactType contactType = new ContactType();
			contactType = db.ContactTypes.SingleOrDefault(c => c.ContactTypeId == contact.ContactTypeId);
			if (contactType != null)
			{
				contact.ContactType = contactType;
			}
		}

		//Add Contact
		public void Add(ContactVM contactVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertClientSubUnitContact_v1(
				contactVM.ClientSubUnit.ClientSubUnitGuid,
				contactVM.Contact.ContactTypeId,
				contactVM.Contact.LastName,
				contactVM.Contact.FirstName,
				contactVM.Contact.MiddleName, 
				contactVM.Contact.PrimaryTelephoneNumber,
				contactVM.Contact.EmergencyTelephoneNumber,
				contactVM.Contact.EmailAddress,
				contactVM.Contact.FirstAddressLine,
				contactVM.Contact.SecondAddressLine,
				contactVM.Contact.CityName,
				contactVM.Contact.StateProvinceName,
				contactVM.Contact.PostalCode,
				contactVM.Contact.CountryCode,
				contactVM.Contact.ResponsibilityDescription,
				adminUserGuid
			);

		}

		//Edit Contact
		public void Edit(ContactVM contactVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateClientSubUnitContact_v1(
				contactVM.Contact.ContactId, 
				contactVM.ClientSubUnit.ClientSubUnitGuid,
				contactVM.Contact.ContactTypeId,
				contactVM.Contact.LastName,
				contactVM.Contact.FirstName,
				contactVM.Contact.MiddleName,
				contactVM.Contact.PrimaryTelephoneNumber,
				contactVM.Contact.EmergencyTelephoneNumber,
				contactVM.Contact.EmailAddress,
				contactVM.Contact.FirstAddressLine,
				contactVM.Contact.SecondAddressLine,
				contactVM.Contact.CityName,
				contactVM.Contact.StateProvinceName,
				contactVM.Contact.PostalCode,
				contactVM.Contact.CountryCode,
				contactVM.Contact.ResponsibilityDescription,
				adminUserGuid,
				contactVM.Contact.VersionNumber
			);

		}

        //Delete Contact
        public void Delete(Contact contact)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteClientSubUnitContact_v1(
                contact.ContactId,
                adminUserGuid,
                contact.VersionNumber 
            );

        }

		//Export Items to CSV
		public byte[] Export(string id)
		{
			StringBuilder sb = new StringBuilder();

			//Add Headers
			List<string> headers = new List<string>();
	
			headers.Add("ContactTypeName");
			headers.Add("LastName");
			headers.Add("FirstName");
			headers.Add("PrimaryTelephoneNumber");
			headers.Add("EmergencyTelephoneNumber");
			headers.Add("EmailAddress");
			headers.Add("CountryCode");
			headers.Add("FirstAddressLine");
			headers.Add("SecondAddressLine");
			headers.Add("CityName");
			headers.Add("StateProvinceName");
			headers.Add("PostalCode");
			headers.Add("ResponsibilityDescription");
			headers.Add("Creation TimeStamp");
			headers.Add("Last Update Time Stamp");

			sb.AppendLine(String.Join(",", headers.Select(x => x.ToString()).ToArray()));

			//Add Items
			List<Contact> clientSubUnitContacts = GetClientSubUnitContactsExport(id);
			foreach (Contact item in clientSubUnitContacts)
			{
				string date_format = "MM/dd/yy HH:mm";

				sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}",
					!string.IsNullOrEmpty(item.ContactTypeName) ? CWTStringHelpers.EncloseCellValue(item.ContactTypeName) : "",
					!string.IsNullOrEmpty(item.LastName) ? CWTStringHelpers.EncloseCellValue(item.LastName) : "",
					!string.IsNullOrEmpty(item.FirstName) ? CWTStringHelpers.EncloseCellValue(item.FirstName) : "",
					!string.IsNullOrEmpty(item.PrimaryTelephoneNumber) ? CWTStringHelpers.EncloseCellValue(item.PrimaryTelephoneNumber) : "",
					!string.IsNullOrEmpty(item.EmergencyTelephoneNumber) ? CWTStringHelpers.EncloseCellValue(item.EmergencyTelephoneNumber) : "",
					!string.IsNullOrEmpty(item.EmailAddress) ? CWTStringHelpers.EncloseCellValue(item.EmailAddress) : "",
					!string.IsNullOrEmpty(item.CountryCode) ? CWTStringHelpers.EncloseCellValue(item.CountryCode) : "",
					!string.IsNullOrEmpty(item.FirstAddressLine) ? CWTStringHelpers.EncloseCellValue(item.FirstAddressLine) : "",
					!string.IsNullOrEmpty(item.SecondAddressLine) ? CWTStringHelpers.EncloseCellValue(item.SecondAddressLine) : "",
					!string.IsNullOrEmpty(item.CityName) ? CWTStringHelpers.EncloseCellValue(item.CityName) : "",
					!string.IsNullOrEmpty(item.StateProvinceName) ? CWTStringHelpers.EncloseCellValue(item.StateProvinceName) : "", 
					!string.IsNullOrEmpty(item.PostalCode) ? CWTStringHelpers.EncloseCellValue(item.PostalCode) : "",
					!string.IsNullOrEmpty(item.ResponsibilityDescription) ? CWTStringHelpers.EncloseCellValue(item.ResponsibilityDescription) : "",
					item.CreationTimestamp.HasValue ? item.CreationTimestamp.Value.ToString(date_format) : "",
					item.LastUpdateTimestamp.HasValue ? item.LastUpdateTimestamp.Value.ToString(date_format) : ""
				);

				sb.Append(Environment.NewLine);
			}

			return UTF8Encoding.UTF8.GetBytes(sb.ToString());
		}

		public List<Contact> GetClientSubUnitContactsExport(string id)
		{
			return db.spDesktopDataAdmin_SelectClientSubUnitContactsExport_v1(id)
				.Select(n => new Contact
				{
					ContactTypeName = n.ContactTypeName,
					FirstName = n.FirstName,
					LastName = n.LastName,
					PrimaryTelephoneNumber = n.PrimaryTelephoneNumber,
					EmergencyTelephoneNumber = n.EmergencyTelephoneNumber,
					EmailAddress = n.EmailAddress,
					FirstAddressLine = n.FirstAddressLine,
					SecondAddressLine = n.SecondAddressLine,
					CityName = n.CityName,
					PostalCode = n.PostalCode,
					CountryCode = n.CountryCode,
					StateProvinceName = n.StateProvinceName,
					ResponsibilityDescription = n.ResponsibilityDescription,
					CreationTimestamp = n.CreationTimestamp,
					LastUpdateTimestamp = n.LastUpdateTimestamp
				})
			  .ToList();
		}

		public ClientSubUnitContactImportStep2VM PreImportCheck(HttpPostedFileBase file, string clientSubUnitGuid)
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

					Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
					String[] cells = csvParser.Split(line);

					//extract the data items from the file
					string contactTypeName = CWTStringHelpers.UnescapeQuotes(cells[0]);						//Required
					string lastName = CWTStringHelpers.UnescapeQuotes(cells[1]);							//Required
					string firstName = CWTStringHelpers.UnescapeQuotes(cells[2]) ?? "";						//Required
					string primaryTelephoneNumber = CWTStringHelpers.UnescapeQuotes(cells[3]);				//Required
					string emergencyTelephoneNumber = CWTStringHelpers.UnescapeQuotes(cells[4]);
					string emailAddress = CWTStringHelpers.UnescapeQuotes(cells[5]) ?? "";					//Required
					string countryCode = CWTStringHelpers.UnescapeQuotes(cells[6]) ?? "";					//Required (e.g.US)
					string firstAddressLine = CWTStringHelpers.UnescapeQuotes(cells[7]) ?? "";
					string secondAddressLine = CWTStringHelpers.UnescapeQuotes(cells[8]) ?? "";
					string cityName = CWTStringHelpers.UnescapeQuotes(cells[9]) ?? "";
					string stateProvinceCode = CWTStringHelpers.UnescapeQuotes(cells[10]) ?? "";			//Required if CountryCode is in the StateProvince table
					string postalCode = CWTStringHelpers.UnescapeQuotes(cells[11]) ?? "";
					string responsibilityDescription = CWTStringHelpers.UnescapeQuotes(cells[12]) ?? "";	//Required

					ContactType contactType = null;
					
					//Required Fields

					if (string.IsNullOrEmpty(contactTypeName) == true)
					{
						returnMessage = "Row " + i + ": ContactTypeName is missing. Please provide a Contact Type";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					if (string.IsNullOrEmpty(lastName) == true)
					{
						returnMessage = "Row " + i + ": LastName is missing. Please provide a Last Name";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					if (string.IsNullOrEmpty(firstName) == true)
					{
						returnMessage = "Row " + i + ": FirstName is missing. Please provide a First Name";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					if (string.IsNullOrEmpty(primaryTelephoneNumber) == true)
					{
						returnMessage = "Row " + i + ": PrimaryTelephoneNumber is missing. Please provide a Phone Number";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					if (string.IsNullOrEmpty(emailAddress) == true)
					{
						returnMessage = "Row " + i + ": EmailAddress is missing. Please provide an Email address";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					if (string.IsNullOrEmpty(countryCode) == true)
					{
						returnMessage = "Row " + i + ": CountryCode is missing. Please provide a valid Country Code";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					if (string.IsNullOrEmpty(responsibilityDescription) == true)
					{
						returnMessage = "Row " + i + ": ResponsibilityDescription is missing. Please provide a valid Responsibility";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//Field Validations

					//ContactTypeName must contain any one value from the ContactTypeName column of the ContactType table
					if (!string.IsNullOrEmpty(contactTypeName))
					{
						ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
						contactType = contactTypeRepository.GetContactTypeByName(contactTypeName);
						if (contactType == null)
						{
							returnMessage = "Row " + i + ": ContactTypeName " + contactTypeName + " is invalid. Please provide a valid Contact Type";
							if (!returnMessages.Contains(returnMessage))
							{
								returnMessages.Add(returnMessage);
							}
						}
					}

					//LastName is freeform text and allowable special characters are: space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), and accented characters.
					string lastNamePattern = @"^([À-ÿ\w\s-()*@\.']+)$";
					if (!string.IsNullOrEmpty(lastName) && !Regex.IsMatch(lastName, lastNamePattern, RegexOptions.IgnoreCase))
					{
						returnMessage = "Row " + i + ": LastName contains invalid special characters. Please provide a valid Last Name";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//LastName will allow up to 50 alphanumeric and allowable special characters. 
					if (!string.IsNullOrEmpty(lastName) && lastName.Length > 50)
					{
						returnMessage = "Row " + i + ": LastName contains more than 50 characters. Please provide a valid Last Name";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//FirstName is freeform text and allowable special characters are: space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), and accented characters.
					string firstNamePattern = @"^([À-ÿ\w\s-()*@\.']+)$";
					if (!string.IsNullOrEmpty(firstName) && !Regex.IsMatch(firstName, firstNamePattern, RegexOptions.IgnoreCase))
					{
						returnMessage = "Row " + i + ": FirstName contains invalid special characters. Please provide a valid First Name";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//FirstName will allow up to 50 alphanumeric and allowable special characters. 
					if (!string.IsNullOrEmpty(firstName) && firstName.Length > 50)
					{
						returnMessage = "Row " + i + ": FirstName contains more than 50 characters. Please provide a valid First Name";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//PrimaryTelephoneNumber is freeform text and will allow up to 20 digits
					string primaryTelephoneNumberPattern = @"^[0-9]+$";
					if (!string.IsNullOrEmpty(primaryTelephoneNumber) && !Regex.IsMatch(primaryTelephoneNumber, primaryTelephoneNumberPattern, RegexOptions.IgnoreCase))
					{
						returnMessage = "Row " + i + ": PrimaryTelephoneNumber contains invalid characters. Please provide a valid Phone";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//PrimaryTelephoneNumber will allow up to 20 alphanumeric and allowable special characters. 
					if (!string.IsNullOrEmpty(primaryTelephoneNumber) && primaryTelephoneNumber.Length > 20)
					{
						returnMessage = "Row " + i + ": PrimaryTelephoneNumber contains more than 20 characters. Please provide a valid Phone";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//EmergencyTelephoneNumber is freeform text and will allow up to 20 digits
					string emergencyTelephoneNumberPattern = @"^[0-9]+$";
					if (!string.IsNullOrEmpty(emergencyTelephoneNumber) && !Regex.IsMatch(emergencyTelephoneNumber, emergencyTelephoneNumberPattern, RegexOptions.IgnoreCase))
					{
						returnMessage = "Row " + i + ": EmergencyTelephoneNumber contains invalid characters. Please provide a valid Emergency Phone";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//EmergencyTelephoneNumber will allow up to 20 alphanumeric and allowable special characters. 
					if (!string.IsNullOrEmpty(emergencyTelephoneNumber) && emergencyTelephoneNumber.Length > 20)
					{
						returnMessage = "Row " + i + ": EmergencyTelephoneNumber contains more than 20 characters. Please provide a valid Emergency Phone";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//EmailAddress is freeform text and will allow up to 100 alphanumeric and allowable special characters. Allowable special characters are: 
					//space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), accented characters, and @ symbol
					string emailAddressPattern = @"^([À-ÿ\w\s-()*@\.]+)$";
					if (!string.IsNullOrEmpty(emailAddress) && !Regex.IsMatch(emailAddress, emailAddressPattern, RegexOptions.IgnoreCase))
					{
						returnMessage = "Row " + i + ": EmailAddress contains invalid characters. Please provide a valid Email";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//EmailAddress will allow up to 20 alphanumeric and allowable special characters. 
					if (!string.IsNullOrEmpty(emailAddress) && emailAddress.Length > 100)
					{
						returnMessage = "Row " + i + ": EmailAddress contains more than 100 characters. Please provide a valid Email";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//FirstAddressLine is freeform text and will allow up to 80 alphanumeric and allowable special characters. 
					if (firstAddressLine.Length > 80)
					{
						returnMessage = "Row " + i + ": FirstAddressLine contains more than 80 characters. Please provide a valid First Address Line";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//FirstAddressLine allowable special characters are: space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), 
					//ampersand and accented characters.
					string firstAddressLinePattern = @"^([À-ÿ\w\s-\\\/'()\*\.\,\&\:\,\°\#]+)$";
					if (!string.IsNullOrEmpty(firstAddressLine) && !Regex.IsMatch(firstAddressLine, firstAddressLinePattern, RegexOptions.IgnoreCase))
					{
						returnMessage = "Row " + i + ": FirstAddressLine contains invalid special characters. Please provide a valid First Address Line";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//SecondAddressLine is freeform text and will allow up to 80 alphanumeric and allowable special characters. 
					if (secondAddressLine.Length > 80)
					{
						returnMessage = "Row " + i + ": SecondAddressLine contains more than 80 characters. Please provide a valid Second Address Line";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//SecondAddressLine allowable special characters are: space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), 
					//ampersand and accented characters.
					string secondAddressLinePattern = @"^([À-ÿ\w\s-\\\/'()\*\.\,\&\:\,\°\#]+)$";
					if (!string.IsNullOrEmpty(secondAddressLine) && !Regex.IsMatch(secondAddressLine, secondAddressLinePattern, RegexOptions.IgnoreCase))
					{
						returnMessage = "Row " + i + ": SecondAddressLine contains invalid special characters. Please provide a valid Second Address Line";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//CityName is freeform text and will allow up to 40 alphanumeric and allowable special characters. 
					if (cityName.Length > 40)
					{
						returnMessage = "Row " + i + ": CityName contains more than 40 characters. Please provide a valid City Name";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//CityName allowable special characters are: space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), 
					//ampersand and accented characters.
					string cityNamePattern = @"^([À-ÿ\w\s-\\\/'()*.,]+)$";
					if (!string.IsNullOrEmpty(cityName) && !Regex.IsMatch(cityName, cityNamePattern, RegexOptions.IgnoreCase))
					{
						returnMessage = "Row " + i + ": CityName contains invalid special characters. Please provide a valid City Name";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//PostalCode is freeform text and will allow up to 30 alphanumeric and allowable special characters. 
					if (postalCode.Length > 30)
					{
						returnMessage = "Row " + i + ": CityName contains more than 30 characters. Please provide a valid Postal Code";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//PostalCode allowable special characters are: space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), 
					//ampersand and accented characters.
					string postalCodePattern = @"^([\w\s-()*]+)$";
					if (!string.IsNullOrEmpty(postalCode) && !Regex.IsMatch(postalCode, postalCodePattern, RegexOptions.IgnoreCase))
					{
						returnMessage = "Row " + i + ": CityName contains invalid special characters. Please provide a valid Postal Code";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//CountryCode must contain any one value from the CountryCode column of the Country table
					CountryRepository countryRepository = new CountryRepository();
					Country country = countryRepository.GetCountry(countryCode);
					if (country == null)
					{
						returnMessage = "Row " + i + ": CountryCode " + countryCode + " is invalid. Please provide a valid Country Code";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//Where the CountryCode value in the row exists in the StateProvince table then the StateProvinceName column 
					//in the data import file must contain a StateProvinceCode value for the relevant CountryCode. 
					StateProvinceRepository stateProvinceRepository = new StateProvinceRepository();
					List<StateProvince> stateProvinces = stateProvinceRepository.GetStateProvincesByCountryCode(countryCode);
					StateProvince stateProvince = stateProvinceRepository.GetStateProvinceByCountry(countryCode, stateProvinceCode);
					if ((stateProvinces.Count == 0 && !string.IsNullOrEmpty(stateProvinceCode)) || (stateProvinces.Count > 0 && stateProvince == null))
					{
						returnMessage = "Row " + i + ": StateProvinceName " + stateProvinceCode + " is invalid. Please provide a valid State/Province Code";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//ResponsibilityDescription is freeform text and will allow up to 100 alphanumeric and allowable special characters. 
					if (responsibilityDescription.Length > 100)
					{
						returnMessage = "Row " + i + ": ResponsibilityDescription contains more than 100 characters. Please provide a valid Responsibility";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//PostalCode allowable special characters are: space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), 
					//ampersand and accented characters.
					string responsibilityDescriptionPattern = @"^([À-ÿ\w\s-\\\/'()*.,]+)$";
					if (!string.IsNullOrEmpty(responsibilityDescription) && !Regex.IsMatch(responsibilityDescription, responsibilityDescriptionPattern, RegexOptions.IgnoreCase))
					{
						returnMessage = "Row " + i + ": ResponsibilityDescription contains invalid special characters. Please provide a valid Postal Responsibility";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//Build the XML Element for items

					XmlElement xmlClientSubUnitContactItem = doc.CreateElement("PHCRIG");

					if (contactType != null)
					{
						XmlElement xmlContactTypeId = doc.CreateElement("ContactTypeId");
						xmlContactTypeId.InnerText = contactType.ContactTypeId.ToString();
						xmlClientSubUnitContactItem.AppendChild(xmlContactTypeId);
					}

					XmlElement xmlLastName = doc.CreateElement("LastName");
					xmlLastName.InnerText = lastName;
					xmlClientSubUnitContactItem.AppendChild(xmlLastName);

					XmlElement xmlFirstName = doc.CreateElement("FirstName");
					xmlFirstName.InnerText = firstName;
					xmlClientSubUnitContactItem.AppendChild(xmlFirstName);

					XmlElement xmlPrimaryTelephoneNumber = doc.CreateElement("PrimaryTelephoneNumber");
					xmlPrimaryTelephoneNumber.InnerText = primaryTelephoneNumber;
					xmlClientSubUnitContactItem.AppendChild(xmlPrimaryTelephoneNumber);

					XmlElement xmlEmergencyTelephoneNumber = doc.CreateElement("EmergencyTelephoneNumber");
					xmlEmergencyTelephoneNumber.InnerText = emergencyTelephoneNumber;
					xmlClientSubUnitContactItem.AppendChild(xmlEmergencyTelephoneNumber);

					XmlElement xmlEmailAddress = doc.CreateElement("EmailAddress");
					xmlEmailAddress.InnerText = emailAddress;
					xmlClientSubUnitContactItem.AppendChild(xmlEmailAddress);

					XmlElement xmlCountryCode = doc.CreateElement("CountryCode");
					xmlCountryCode.InnerText = countryCode;
					xmlClientSubUnitContactItem.AppendChild(xmlCountryCode);

					XmlElement xmlFirstAddressLine = doc.CreateElement("FirstAddressLine");
					xmlFirstAddressLine.InnerText = firstAddressLine;
					xmlClientSubUnitContactItem.AppendChild(xmlFirstAddressLine); ;

					XmlElement xmlSecondAddressLine = doc.CreateElement("SecondAddressLine");
					xmlSecondAddressLine.InnerText = secondAddressLine;
					xmlClientSubUnitContactItem.AppendChild(xmlSecondAddressLine);

					XmlElement xmlCityName = doc.CreateElement("CityName");
					xmlCityName.InnerText = cityName;
					xmlClientSubUnitContactItem.AppendChild(xmlCityName);

					XmlElement xmlStateProvinceCode = doc.CreateElement("StateProvinceCode");
					xmlStateProvinceCode.InnerText = stateProvinceCode;
					xmlClientSubUnitContactItem.AppendChild(xmlStateProvinceCode);

					XmlElement xmlPostalCode = doc.CreateElement("PostalCode");
					xmlPostalCode.InnerText = postalCode;
					xmlClientSubUnitContactItem.AppendChild(xmlPostalCode);

					XmlElement xmlResponsibilityDescription = doc.CreateElement("ResponsibilityDescription");
					xmlResponsibilityDescription.InnerText = responsibilityDescription;
					xmlClientSubUnitContactItem.AppendChild(xmlResponsibilityDescription);

					//Attach the XML Element for an item to the Document
					root.AppendChild(xmlClientSubUnitContactItem);
				}
			}

			if (i < 2) //Columns and first row must be included
			{
				returnMessage = "There is no data in the file";
				returnMessages.Add(returnMessage);
			}

			ClientSubUnitContactImportStep2VM preImportCheckResult = new ClientSubUnitContactImportStep2VM();
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
					from n in db.spDesktopDataAdmin_UpdateClientSubUnitContactCount_v1(
						clientSubUnitGuid,
						System.Xml.Linq.XElement.Parse(doc.OuterXml),
						adminUserGuid
					)
					select n).ToList();

				foreach (spDesktopDataAdmin_UpdateClientSubUnitContactCount_v1Result message in output)
				{
					returnMessages.Add(message.MessageText.ToString());
				}

				preImportCheckResult.FileBytes = array;

				preImportCheckResult.IsValidData = true;
			}

			return preImportCheckResult;

		}

		public ClientSubUnitContactImportStep3VM Import(byte[] FileBytes, string clientSubUnitGuid)
		{
			System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
			string fileToText = fileToText = enc.GetString(FileBytes);

			ClientSubUnitContactImportStep3VM cdrPostImportResult = new ClientSubUnitContactImportStep3VM();
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

					Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
					String[] cells = csvParser.Split(line);

					//extract the data items from the file
					string contactTypeName = CWTStringHelpers.UnescapeQuotes(cells[0]);						//Required
					string lastName = CWTStringHelpers.UnescapeQuotes(cells[1]);							//Required
					string firstName = CWTStringHelpers.UnescapeQuotes(cells[2]) ?? "";						//Required
					string primaryTelephoneNumber = CWTStringHelpers.UnescapeQuotes(cells[3]);				//Required
					string emergencyTelephoneNumber = CWTStringHelpers.UnescapeQuotes(cells[4]);			//Required
					string emailAddress = CWTStringHelpers.UnescapeQuotes(cells[5]) ?? "";					//Required
					string countryCode = CWTStringHelpers.UnescapeQuotes(cells[6]) ?? "";					//Required (e.g.US)
					string firstAddressLine = CWTStringHelpers.UnescapeQuotes(cells[7]) ?? "";
					string secondAddressLine = CWTStringHelpers.UnescapeQuotes(cells[8]) ?? "";
					string cityName = CWTStringHelpers.UnescapeQuotes(cells[9]) ?? "";
					string stateProvinceCode = CWTStringHelpers.UnescapeQuotes(cells[10]) ?? "";			//Required if CountryCode is in the StateProvince table
					string postalCode = CWTStringHelpers.UnescapeQuotes(cells[11]) ?? "";
					string responsibilityDescription = CWTStringHelpers.UnescapeQuotes(cells[12]) ?? "";	//Required

					//Build the XML Element for items

					XmlElement xmlClientSubUnitContactItem = doc.CreateElement("PHCRIG");

					ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
					ContactType contactType = contactTypeRepository.GetContactTypeByName(contactTypeName);
					if (contactType != null)
					{
						XmlElement xmlContactTypeId = doc.CreateElement("ContactTypeId");
						xmlContactTypeId.InnerText = contactType.ContactTypeId.ToString();
						xmlClientSubUnitContactItem.AppendChild(xmlContactTypeId);
					}

					XmlElement xmlLastName = doc.CreateElement("LastName");
					xmlLastName.InnerText = lastName;
					xmlClientSubUnitContactItem.AppendChild(xmlLastName);

					XmlElement xmlFirstName = doc.CreateElement("FirstName");
					xmlFirstName.InnerText = firstName;
					xmlClientSubUnitContactItem.AppendChild(xmlFirstName);

					XmlElement xmlPrimaryTelephoneNumber = doc.CreateElement("PrimaryTelephoneNumber");
					xmlPrimaryTelephoneNumber.InnerText = primaryTelephoneNumber;
					xmlClientSubUnitContactItem.AppendChild(xmlPrimaryTelephoneNumber);

					XmlElement xmlEmergencyTelephoneNumber = doc.CreateElement("EmergencyTelephoneNumber");
					xmlEmergencyTelephoneNumber.InnerText = emergencyTelephoneNumber;
					xmlClientSubUnitContactItem.AppendChild(xmlEmergencyTelephoneNumber);

					XmlElement xmlEmailAddress = doc.CreateElement("EmailAddress");
					xmlEmailAddress.InnerText = emailAddress;
					xmlClientSubUnitContactItem.AppendChild(xmlEmailAddress);

					XmlElement xmlCountryCode = doc.CreateElement("CountryCode");
					xmlCountryCode.InnerText = countryCode;
					xmlClientSubUnitContactItem.AppendChild(xmlCountryCode);

					XmlElement xmlFirstAddressLine = doc.CreateElement("FirstAddressLine");
					xmlFirstAddressLine.InnerText = firstAddressLine;
					xmlClientSubUnitContactItem.AppendChild(xmlFirstAddressLine); ;

					XmlElement xmlSecondAddressLine = doc.CreateElement("SecondAddressLine");
					xmlSecondAddressLine.InnerText = secondAddressLine;
					xmlClientSubUnitContactItem.AppendChild(xmlSecondAddressLine);

					XmlElement xmlCityName = doc.CreateElement("CityName");
					xmlCityName.InnerText = cityName;
					xmlClientSubUnitContactItem.AppendChild(xmlCityName);

					if (stateProvinceCode.ToLower() != "null" && !string.IsNullOrEmpty(stateProvinceCode))
					{
						XmlElement xmlStateProvinceCode = doc.CreateElement("StateProvinceCode");
						xmlStateProvinceCode.InnerText = stateProvinceCode;
						xmlClientSubUnitContactItem.AppendChild(xmlStateProvinceCode);
					}

					XmlElement xmlPostalCode = doc.CreateElement("PostalCode");
					xmlPostalCode.InnerText = postalCode;
					xmlClientSubUnitContactItem.AppendChild(xmlPostalCode);

					XmlElement xmlResponsibilityDescription = doc.CreateElement("ResponsibilityDescription");
					xmlResponsibilityDescription.InnerText = responsibilityDescription;
					xmlClientSubUnitContactItem.AppendChild(xmlResponsibilityDescription);


					//Attach the XML Element for an item to the Document
					root.AppendChild(xmlClientSubUnitContactItem);

				}
			}

			//DB Check
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			var output = (from n in db.spDesktopDataAdmin_UpdateClientSubUnitContacts_v1(
				clientSubUnitGuid,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid)
						  select n).ToList();

			int deletedItemCount = 0;
			int addedItemCount = 0;

			foreach (spDesktopDataAdmin_UpdateClientSubUnitContacts_v1Result message in output)
			{
				returnMessages.Add(message.MessageText.ToString());
			}

			cdrPostImportResult.ReturnMessages = returnMessages;
			cdrPostImportResult.AddedItemCount = addedItemCount;
			cdrPostImportResult.DeletedItemCount = deletedItemCount;

			return cdrPostImportResult;
		}
    }
}