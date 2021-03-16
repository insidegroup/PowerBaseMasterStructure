using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Repository
{
	public class ClientTopUnitClientLocationRepository
    {
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of ClientDetails
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitClientLocations_v1Result> GetClientTopUnitClientLocations(string clientTopUnitGuid, string filter, string sortField, int sortOrder, int page)
        {
			var result = db.spDesktopDataAdmin_SelectClientTopUnitClientLocations_v1(clientTopUnitGuid, filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitClientLocations_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

		//Add Group
		public void Add(ClientTopUnitClientLocationVM clientTopUnitClientLocationVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertClientTopUnitClientLocation_v1(
				clientTopUnitClientLocationVM.ClientTopUnit.ClientTopUnitGuid,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.AddressLocationName,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.FirstAddressLine,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.SecondAddressLine,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.CityName,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.StateProvinceName,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.LatitudeDecimal,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.LongitudeDecimal,
				null,	//MappingQualityCode
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.PostalCode,
				false, //ReplicatedFromClientMaintenanceFlag
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.CountryCode,
				adminUserGuid,
                clientTopUnitClientLocationVM.ClientTopUnitClientLocation.Ranking
			);
		}

		//Edit Group
		public void Update(ClientTopUnitClientLocationVM clientTopUnitClientLocationVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateClientTopUnitClientLocation_v1(
				clientTopUnitClientLocationVM.ClientTopUnit.ClientTopUnitGuid,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.AddressId,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.AddressLocationName,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.FirstAddressLine,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.SecondAddressLine,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.CityName,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.StateProvinceName,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.LatitudeDecimal,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.LongitudeDecimal,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.PostalCode,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.CountryCode,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.VersionNumber,
				adminUserGuid,
                clientTopUnitClientLocationVM.ClientTopUnitClientLocation.Ranking
			);
		}

		//Delete Group
		public void Delete(ClientTopUnitClientLocationVM clientTopUnitClientLocationVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteClientTopUnitClientLocation_v1(
				clientTopUnitClientLocationVM.ClientTopUnit.ClientTopUnitGuid,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.AddressId,
				adminUserGuid,
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation.VersionNumber
			);
		}

		public ClientTopUnitClientLocation GetAddress(int addressId)
		{
			ClientTopUnitClientLocation clientTopUnitClientLocation = new ClientTopUnitClientLocation();
			Address address = db.Addresses.SingleOrDefault(c => c.AddressId == addressId);
			
			if(address != null) {
				clientTopUnitClientLocation.AddressId = address.AddressId;
				clientTopUnitClientLocation.AddressLocationName = address.AddressLocationName;
				clientTopUnitClientLocation.FirstAddressLine = address.FirstAddressLine;
				clientTopUnitClientLocation.SecondAddressLine = address.SecondAddressLine;
				clientTopUnitClientLocation.CityName = address.CityName;
				clientTopUnitClientLocation.StateProvinceName = address.StateProvinceName;
				clientTopUnitClientLocation.PostalCode = address.PostalCode;
                clientTopUnitClientLocation.Ranking = address.Ranking;


				if (address.CountryCode != null)
				{
					Country country = new Country();
					country = db.Countries.SingleOrDefault(c => c.CountryCode == address.CountryCode);
					clientTopUnitClientLocation.Country = country;
					clientTopUnitClientLocation.CountryName = country.CountryName;
					clientTopUnitClientLocation.CountryCode = country.CountryCode;
				}

				if (address.MappingQualityCode != null)
				{
					MappingQuality mappingQuality = new MappingQuality();
					mappingQuality = db.MappingQualities.SingleOrDefault(c => c.MappingQualityCode == address.MappingQualityCode);
					clientTopUnitClientLocation.MappingQualityDescription = mappingQuality.MappingQualityDescription;
				} 
				
				clientTopUnitClientLocation.LatitudeDecimal = address.LatitudeDecimal;
				clientTopUnitClientLocation.LongitudeDecimal = address.LongitudeDecimal;
				clientTopUnitClientLocation.VersionNumber = clientTopUnitClientLocation.VersionNumber;
			}

			return clientTopUnitClientLocation;
		}

		//Export Items to CSV
		public byte[] Export(string id)
		{
			StringBuilder sb = new StringBuilder();

			//Add Headers
			List<string> headers = new List<string>();
			headers.Add("AddressLocationName");
			headers.Add("FirstAddressLine");
			headers.Add("SecondAddressLine");
			headers.Add("CityName");
			headers.Add("PostalCode");
			headers.Add("CountryCode");
			headers.Add("StateProvinceName");
			headers.Add("LatitudeDecimal");
			headers.Add("LongitudeDecimal");
			headers.Add("Ranking");
			headers.Add("CreationTimestamp");
			headers.Add("LastUpdateTimestamp");

			sb.AppendLine(String.Join(",", headers.Select(x => x.ToString()).ToArray()));

			//Add Items
			List<ClientTopUnitClientLocation> clientTopUnitClientLocations = GetClientTopUnitClientLocationsExport(id);
			foreach (ClientTopUnitClientLocation item in clientTopUnitClientLocations)
			{
				string date_format = "MM/dd/yy HH:mm";

				sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
					!string.IsNullOrEmpty(item.AddressLocationName) ? CWTStringHelpers.EncloseCellValue(item.AddressLocationName) : "",
					!string.IsNullOrEmpty(item.FirstAddressLine) ? CWTStringHelpers.EncloseCellValue(item.FirstAddressLine) : "",
					!string.IsNullOrEmpty(item.SecondAddressLine) ? CWTStringHelpers.EncloseCellValue(item.SecondAddressLine) : "",
					!string.IsNullOrEmpty(item.CityName) ? CWTStringHelpers.EncloseCellValue(item.CityName) : "",
					!string.IsNullOrEmpty(item.PostalCode) ? CWTStringHelpers.EncloseCellValue(item.PostalCode) : "",
					!string.IsNullOrEmpty(item.CountryCode) ? item.CountryCode : "",
					!string.IsNullOrEmpty(item.StateProvinceName) ? item.StateProvinceName : "",
					item.LatitudeDecimal != null ? item.LatitudeDecimal.ToString() : "",
					item.LongitudeDecimal != null ? item.LongitudeDecimal.ToString() : "",
					item.Ranking != null ? item.Ranking.ToString() : "",
					item.CreationTimestamp.HasValue ? item.CreationTimestamp.Value.ToString(date_format) : "",
					item.LastUpdateTimestamp.HasValue ? item.LastUpdateTimestamp.Value.ToString(date_format) : ""
				);

				sb.Append(Environment.NewLine);
			}

            //return Encoding.ASCII.GetBytes(sb.ToString());
            return UTF8Encoding.UTF8.GetBytes(sb.ToString());
        }

		public List<ClientTopUnitClientLocation> GetClientTopUnitClientLocationsExport(string id)
		{
			return db.spDesktopDataAdmin_SelectClientTopUnitClientLocationsExport_v1(id)
				.Select(n => new ClientTopUnitClientLocation
				{
					AddressLocationName = n.AddressLocationName,
					FirstAddressLine = n.FirstAddressLine,
					SecondAddressLine = n.SecondAddressLine,
					CityName = n.CityName,
					PostalCode = n.PostalCode,
					CountryCode = n.CountryCode,
					StateProvinceName = n.StateProvinceName,
					LatitudeDecimal = n.LatitudeDecimal,
					LongitudeDecimal = n.LongitudeDecimal,
					Ranking = n.Ranking,
					CreationTimestamp = n.CreationTimeStamp,
					LastUpdateTimestamp = n.LastUpdateTimeStamp
				})
			  .ToList();
		}

		public ClientTopUnitImportStep2VM PreImportCheck(HttpPostedFileBase file, string clientTopUnitGuid)
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
                    string addressLocationName = CWTStringHelpers.UnescapeQuotes(cells[0]);          //Required
                    string firstAddressLine = CWTStringHelpers.UnescapeQuotes(cells[1]);             //Required
                    string secondAddressLine = CWTStringHelpers.UnescapeQuotes(cells[2]) ?? "";
                    string cityName = CWTStringHelpers.UnescapeQuotes(cells[3]);                     //Required
                    string postalCode = CWTStringHelpers.UnescapeQuotes(cells[4]);                   //Required
                    string countryCode = cells[5];                                  //Required (e.g.US)
                    string stateProvinceName = cells[6];                            //Required if CountryCode is in the StateProvince table (should contain valid StateProvinceCode)
                    string latitudeDecimal = cells[7];                              //Required
                    string longitudeDecimal = cells[8];                             //Required
                    string ranking = cells[9] ?? "";

                    //Build the XML Element for items

                    XmlElement xmlClientTopUnitLocationItem = doc.CreateElement("PHCRIG");

                    XmlElement xmlAddressLocationName = doc.CreateElement("AddressLocationName");
                    xmlAddressLocationName.InnerText = addressLocationName;
                    xmlClientTopUnitLocationItem.AppendChild(xmlAddressLocationName);

                    XmlElement xmlFirstAddressLine = doc.CreateElement("FirstAddressLine");
                    xmlFirstAddressLine.InnerText = firstAddressLine;
                    xmlClientTopUnitLocationItem.AppendChild(xmlFirstAddressLine);

                    XmlElement xmlSecondAddressLine = doc.CreateElement("SecondAddressLine");
                    xmlSecondAddressLine.InnerText = secondAddressLine;
                    xmlClientTopUnitLocationItem.AppendChild(xmlSecondAddressLine);

                    XmlElement xmlCityName = doc.CreateElement("CityName");
                    xmlCityName.InnerText = cityName;
                    xmlClientTopUnitLocationItem.AppendChild(xmlCityName);

                    XmlElement xmlPostalCode = doc.CreateElement("PostalCode");
                    xmlPostalCode.InnerText = postalCode;
                    xmlClientTopUnitLocationItem.AppendChild(xmlPostalCode);

                    XmlElement xmlCountryCode = doc.CreateElement("CountryCode");
                    xmlCountryCode.InnerText = countryCode;
                    xmlClientTopUnitLocationItem.AppendChild(xmlCountryCode);

                    XmlElement xmlStateProvinceName = doc.CreateElement("StateProvinceName");
                    xmlStateProvinceName.InnerText = stateProvinceName;
                    xmlClientTopUnitLocationItem.AppendChild(xmlStateProvinceName);

                    XmlElement xmlLatitudeDecimal = doc.CreateElement("LatitudeDecimal");
                    xmlLatitudeDecimal.InnerText = latitudeDecimal;
                    xmlClientTopUnitLocationItem.AppendChild(xmlLatitudeDecimal);

                    XmlElement xmlLongitudeDecimal = doc.CreateElement("LongitudeDecimal");
                    xmlLongitudeDecimal.InnerText = longitudeDecimal;
                    xmlClientTopUnitLocationItem.AppendChild(xmlLongitudeDecimal);

                    //Validate data
                    if (string.IsNullOrEmpty(addressLocationName) == true)
                    {
                        returnMessage = "Row " + i + ": AddressLocationName is missing. Please provide a Client Location Name";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    if (string.IsNullOrEmpty(firstAddressLine) == true)
                    {
                        returnMessage = "Row " + i + ": FirstAddressLine is missing. Please provide a First Address Line";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    if (string.IsNullOrEmpty(cityName) == true)
                    {
                        returnMessage = "Row " + i + ": CityName is missing. Please provide a City Name";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    if (string.IsNullOrEmpty(postalCode) == true)
                    {
                        returnMessage = "Row " + i + ": PostalCode is missing. Please provide a Postal Code";
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

                    if (string.IsNullOrEmpty(latitudeDecimal) == true)
                    {
                        returnMessage = "Row " + i + ": Latitude is missing. Please provide a valid Latitude";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    if (string.IsNullOrEmpty(longitudeDecimal) == true)
                    {
                        returnMessage = "Row " + i + ": Longitude is missing. Please provide a valid Longitude";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    //AddressLocationName is freeform text and will allow up to 150 alphanumeric and allowable special characters. 
                    if (addressLocationName.Length > 150)
                    {
                        returnMessage = "Row " + i + ": AddressLocationName contains more than 150 characters. Please provide a valid Address Location Name";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    //AddressLocationName allowable special characters are: space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), ampersand and accented characters.
                    string addressLocationNamePattern = @"^[À-ÿ\w\s-()\.\&\'\’_]+$";
                    if (!Regex.IsMatch(addressLocationName, addressLocationNamePattern, RegexOptions.IgnoreCase))
                    {
                        returnMessage = "Row " + i + ": AddressLocationName contains invalid special characters. Please provide a Address Location Name";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    //FirstAddressLine is freeform text and will allow up to 150 alphanumeric and allowable special characters. 
                    if (firstAddressLine.Length > 150)
                    {
                        returnMessage = "Row " + i + ": FirstAddressLine contains more than 150 characters. Please provide a valid First Address Line";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    //FirstAddressLine allowable special characters are: space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), ampersand and accented characters.
                    string firstAddressLinePattern = @"^([À-ÿ\w\s-()\*\.\&\'\’_\,\:\,\°\#\\\/]+)$";

                    if (!Regex.IsMatch(firstAddressLine, firstAddressLinePattern, RegexOptions.IgnoreCase))
                    {
                        returnMessage = "Row " + i + ": FirstAddressLine contains invalid special characters. Please provide a valid First Address Line";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    //SecondAddressLine is freeform text and will allow up to 150 alphanumeric and allowable special characters. 
                    if (secondAddressLine.Length > 150)
                    {
                        returnMessage = "Row " + i + ": SecondAddressLine contains more than 150 characters. Please provide a valid Second Address Line";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    //SecondAddressLine allowable special characters are: space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), ampersand and accented characters.
                    string secondAddressLinePattern = @"^([À-ÿ\w\s-()\*\.\&\'\’_\,\:\,\°\#\\\/]+)$";
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

                    //CityName allowable special characters are: space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), ampersand and accented characters.
                    string cityNamePattern = @"^[À-ÿ\w\s-()\.\&\'\’_]+$";
                    if (!Regex.IsMatch(cityName, cityNamePattern, RegexOptions.IgnoreCase))
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

                    //PostalCode allowable special characters are: space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), ampersand and accented characters.
                    string postalCodePattern = @"^[À-ÿ\w\s-()\.\&\'\’_]+$";
                    if (!Regex.IsMatch(postalCode, postalCodePattern, RegexOptions.IgnoreCase))
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
                    StateProvince stateProvince = stateProvinceRepository.GetStateProvinceByCountry(countryCode, stateProvinceName);
                    if ((stateProvinces.Count == 0 && !string.IsNullOrEmpty(stateProvinceName)) || (stateProvinces.Count > 0 && stateProvince == null))
                    {
                        returnMessage = "Row " + i + ": StateProvinceName " + stateProvinceName + " is invalid. Please provide a valid State/Province Code";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

					//LatitudeDecimal must be numerical
					float validLatitudeDecimal;
					if (!float.TryParse(latitudeDecimal, out validLatitudeDecimal))
					{
						returnMessage = "Row " + i + ": Latitude must be numerical. Please provide a valid Latitude";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//LatitudeDecimal positive and negative numerical values of up to two places prior to the decimal and seven places following the decimal
					//E.g -999.9999999 and 999.9999999
					string latitudeDecimalPattern = @"^[-+]?(\d{1,3}\.\d{1,7}|\d{1,3})?$";
					if (!Regex.IsMatch(latitudeDecimal, latitudeDecimalPattern, RegexOptions.IgnoreCase))
					{
						returnMessage = "Row " + i + ": Latitude must be between -999.9999999 and 999.9999999. Please provide a valid Latitude";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//LongitudeDecimal must be numerical
					float validLongitudeDecimal;
					if (!float.TryParse(longitudeDecimal, out validLongitudeDecimal))
					{
						returnMessage = "Row " + i + ": Longitude must be numerical. Please provide a valid Latitude";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//LongitudeDecimal positive and negative numerical values of up to three places prior to the decimal and seven places following the decimal
					//E.g -999.9999999 and 999.9999999
					string longitudeDecimalPattern = @"^[-+]?(\d{1,3}\.\d{1,7}|\d{1,3})?$";
					if (!Regex.IsMatch(longitudeDecimal, longitudeDecimalPattern, RegexOptions.IgnoreCase))
					{
						returnMessage = "Row " + i + ": Longitude must be between -999.9999999 and 999.9999999. Please provide a valid Longitude";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//Ranking may be empty or NULL or contain an integer from 1 to 9 inclusive
					string rankingPattern = @"\d{1,9}?";
					if (ranking.ToLower() != "null" && !string.IsNullOrEmpty(ranking) && !Regex.IsMatch(ranking, rankingPattern, RegexOptions.IgnoreCase))
					{
						returnMessage = "Row " + i + ": Ranking is from 1 to 9. Please provide a valid Rank";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					int validRanking;
					bool isRankingNumerical = Int32.TryParse(ranking, out validRanking);
					if (isRankingNumerical && ranking.ToLower() != "null" && !string.IsNullOrEmpty(ranking))
					{
						XmlElement xmlRanking = doc.CreateElement("Ranking");
						xmlRanking.InnerText = ranking;
						xmlClientTopUnitLocationItem.AppendChild(xmlRanking);
					}

					//Attach the XML Element for an item to the Document
					root.AppendChild(xmlClientTopUnitLocationItem);
				}
			}
			if (i == 0)
			{
				returnMessage = "There is no data in the file";
				returnMessages.Add(returnMessage);
			}

			ClientTopUnitImportStep2VM preImportCheckResult = new ClientTopUnitImportStep2VM();
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
					from n in db.spDesktopDataAdmin_UpdateClientTopUnitClientLocationCount_v1(
						clientTopUnitGuid,
						System.Xml.Linq.XElement.Parse(doc.OuterXml),
						adminUserGuid
					)
					select n).ToList();

				foreach (spDesktopDataAdmin_UpdateClientTopUnitClientLocationCount_v1Result message in output)
				{
					returnMessages.Add(message.MessageText.ToString());
				}

				preImportCheckResult.FileBytes = array;

				preImportCheckResult.IsValidData = true;
			}

			return preImportCheckResult;

		}

		public ClientTopUnitImportStep3VM Import(byte[] FileBytes, string clientTopUnitGuid)
		{
			System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
			string fileToText = fileToText = enc.GetString(FileBytes);

			ClientTopUnitImportStep3VM cdrPostImportResult = new ClientTopUnitImportStep3VM();
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
                    string addressLocationName = CWTStringHelpers.UnescapeQuotes(cells[0]);          //Required
                    string firstAddressLine = CWTStringHelpers.UnescapeQuotes(cells[1]);             //Required
                    string secondAddressLine = CWTStringHelpers.UnescapeQuotes(cells[2]) ?? "";
                    string cityName = CWTStringHelpers.UnescapeQuotes(cells[3]);                     //Required
                    string postalCode = CWTStringHelpers.UnescapeQuotes(cells[4]);                   //Required
                    string countryCode = cells[5];                                  //Required (e.g.US)
                    string stateProvinceName = cells[6];                            //Required if CountryCode is in the StateProvince table (should contain valid StateProvinceCode)
                    string latitudeDecimal = cells[7];                              //Required
                    string longitudeDecimal = cells[8];                             //Required
                    string ranking = cells[9] ?? "";

                    //Build the XML Element for items

                    XmlElement xmlClientTopUnitLocationItem = doc.CreateElement("PHCRIG");

					XmlElement xmlAddressLocationName = doc.CreateElement("AddressLocationName");
					xmlAddressLocationName.InnerText = addressLocationName;
					xmlClientTopUnitLocationItem.AppendChild(xmlAddressLocationName);

					XmlElement xmlFirstAddressLine = doc.CreateElement("FirstAddressLine");
					xmlFirstAddressLine.InnerText = firstAddressLine;
					xmlClientTopUnitLocationItem.AppendChild(xmlFirstAddressLine);

					XmlElement xmlSecondAddressLine = doc.CreateElement("SecondAddressLine");
					xmlSecondAddressLine.InnerText = secondAddressLine;
					xmlClientTopUnitLocationItem.AppendChild(xmlSecondAddressLine);

					XmlElement xmlCityName = doc.CreateElement("CityName");
					xmlCityName.InnerText = cityName;
					xmlClientTopUnitLocationItem.AppendChild(xmlCityName);

					XmlElement xmlPostalCode = doc.CreateElement("PostalCode");
					xmlPostalCode.InnerText = postalCode;
					xmlClientTopUnitLocationItem.AppendChild(xmlPostalCode);

					XmlElement xmlCountryCode = doc.CreateElement("CountryCode");
					xmlCountryCode.InnerText = countryCode;
					xmlClientTopUnitLocationItem.AppendChild(xmlCountryCode);

					if (stateProvinceName.ToLower() != "null" && !string.IsNullOrEmpty(stateProvinceName))
					{
						XmlElement xmlStateProvinceName = doc.CreateElement("StateProvinceName");
						xmlStateProvinceName.InnerText = stateProvinceName;
						xmlClientTopUnitLocationItem.AppendChild(xmlStateProvinceName);
					}
					
					XmlElement xmlLatitudeDecimal = doc.CreateElement("LatitudeDecimal");
					xmlLatitudeDecimal.InnerText = latitudeDecimal;
					xmlClientTopUnitLocationItem.AppendChild(xmlLatitudeDecimal);

					XmlElement xmlLongitudeDecimal = doc.CreateElement("LongitudeDecimal");
					xmlLongitudeDecimal.InnerText = longitudeDecimal;
					xmlClientTopUnitLocationItem.AppendChild(xmlLongitudeDecimal);

					int validRanking;
					bool isRankingNumerical = Int32.TryParse(ranking, out validRanking);
					if (isRankingNumerical && ranking.ToLower() != "null" && !string.IsNullOrEmpty(ranking))
					{
						XmlElement xmlRanking = doc.CreateElement("Ranking");
						xmlRanking.InnerText = ranking;
						xmlClientTopUnitLocationItem.AppendChild(xmlRanking);
					}

					//Attach the XML Element for an item to the Document
					root.AppendChild(xmlClientTopUnitLocationItem);

				}
			}

			//DB Check
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			var output = (from n in db.spDesktopDataAdmin_UpdateClientTopUnitClientLocations_v1(
				clientTopUnitGuid,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid)
						  select n).ToList();

			int deletedItemCount = 0;
			int addedItemCount = 0;

			foreach (spDesktopDataAdmin_UpdateClientTopUnitClientLocations_v1Result message in output)
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