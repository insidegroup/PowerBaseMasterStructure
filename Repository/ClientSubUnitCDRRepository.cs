using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Text.RegularExpressions;
using System.Xml;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
    public class ClientSubUnitCDRRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
        
        //Get a Page of ClientSubUnitTelephony Items - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitCDR_v1Result> PageClientSubUnitCDRs(int page, string filter, string id, string sortField, int? sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectClientSubUnitCDR_v1(filter, id, sortField, sortOrder, page).ToList();
            

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitCDR_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Single Item
        public ClientSubUnitClientDefinedReference GetClientSubUnitCDR(int clientSubUnitClientDefinedReferenceId)
        {
            return db.ClientSubUnitClientDefinedReferences.SingleOrDefault(c => c.ClientSubUnitClientDefinedReferenceId == clientSubUnitClientDefinedReferenceId);
        }

        //Add to DB
        public void Add(ClientSubUnitClientDefinedReference csuCDR)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            
            db.spDesktopDataAdmin_InsertClientSubUnitClientDefinedReference_v1(
                csuCDR.ClientSubUnitGuid,
                csuCDR.DisplayName ,
                csuCDR.Value,
                csuCDR.SourceSystemCode,
                csuCDR.ClientAccountNumber,
                csuCDR.CreditCardId,
                adminUserGuid
            );
            

        }

        //Update DB
        public void Edit(ClientSubUnitClientDefinedReference clientSubUnitCDR){
             string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            
            db.spDesktopDataAdmin_UpdateClientSubUnitClientDefinedReference_v1(
                clientSubUnitCDR.ClientSubUnitClientDefinedReferenceId ,
                clientSubUnitCDR.Value ,
                clientSubUnitCDR.SourceSystemCode ,
                clientSubUnitCDR.ClientAccountNumber,
                clientSubUnitCDR.CreditCardId,
                adminUserGuid,
                clientSubUnitCDR.VersionNumber
            );
        }

         //Delete From DB
        public void Delete(ClientSubUnitClientDefinedReference csuCDR)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteClientSubUnitClientDefinedReference_v1(
                csuCDR.ClientSubUnitClientDefinedReferenceId,
                adminUserGuid,
                csuCDR.VersionNumber
            );
        }

		public CDRImportStep2VM PreImportCheck(HttpPostedFileBase file, string clientSubUnitGuid, string displayName, string relatedToDisplayName = "")
		{

			//convert file to string so that we can parse
			int length = file.ContentLength;
			byte[] tempFile = new byte[length];
			file.InputStream.Read(tempFile, 0, length);
			byte[] array = tempFile.ToArray();
			System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
			string fileToText = fileToText = enc.GetString(array);

			/*
			Regex to match on CDRValue
			The following Special Characters are currently allowed in CDR fields:
			Alpha (A-Z), numeric (0-9), ampersand (&), asterisk (*), at sign (@), hypen (-), forward slash (/), period (.), 
			underscore (_), equal (=), colon (:), open/closed brackets ( ), space
			*/
			//string dataRegex = @"[^(\w|\-|@|*|(|)| |=|:|/)]+";
			string dataRegex = @"[^(\w| |\-|\@|\*|\&|\(|\)|\=|\:|\/|\.)]+";

			// Create the xml document container, this will be used to store the data after the Regex checks
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("CSUCDRs");
			doc.AppendChild(root);

			//Client SubUnit
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			ClientSubUnitClientAccountRepository clientSubUnitClientAccountRepository = new ClientSubUnitClientAccountRepository();
			ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);
			string clientSubUnitName = clientSubUnit.ClientSubUnitName;

			List<string> returnMessages = new List<string>();
			string returnMessage;
			int i = 0;

			//Store Valid ClientSubUnitClientAccounts
			List<ClientSubUnitClientAccount> validClientSubUnitClientAccounts = new List<ClientSubUnitClientAccount>();
			List<ClientSubUnitClientAccountImport> invalidClientSubUnitClientAccounts = new List<ClientSubUnitClientAccountImport>();
			
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
					string fileCostCentre = cells[0];                      //CDRValue (Cost Centre)
					string fileSourceSystemCode = cells[1];                //SourceSystemCode
					string fileClientAccountNumber = cells[2];             //CWT Account No (Air/Rail)

					//Credit Card No field is an optional field on the .csv file
					string fileCreditCardNumber = string.Empty;
					if (cells.Length >= 4)
					{
						fileCreditCardNumber = cells[3] ?? "";
					}

					//The RelatedToValue field is an optional field on the .csv file.  If there is no CDR Display Name value entered in the Validate CDR field,
					//then the Import should not look for a RelatedToValue column on the .csv file
					string fileRelatedToValue = string.Empty;
					if (cells.Length >= 5)
					{
						fileRelatedToValue = cells[4] ?? "";
					}

					//Build the XML Element for a CDR
					XmlElement xmlCDRItem = doc.CreateElement("CSUCDR");

					//Populate the XML Element for a CDR
					//This XMLDoc is only used if the File Passes Validation
					XmlElement xmlSourceSystemCode = doc.CreateElement("SourceSystemCode");
					xmlSourceSystemCode.InnerText = fileSourceSystemCode;
					xmlCDRItem.AppendChild(xmlSourceSystemCode);

					XmlElement xmlClientAccountNumber = doc.CreateElement("ClientAccountNumber");
					xmlClientAccountNumber.InnerText = fileClientAccountNumber;
					xmlCDRItem.AppendChild(xmlClientAccountNumber);

					XmlElement xmlCostCentre = doc.CreateElement("CostCentre");
					xmlCostCentre.InnerText = fileCostCentre;
					xmlCDRItem.AppendChild(xmlCostCentre);

					//Check for numerical credit card
					if (!string.IsNullOrEmpty(fileCreditCardNumber))
					{
						int creditCardId;
						if (!Int32.TryParse(fileCreditCardNumber, out creditCardId))
						{
							fileCreditCardNumber = "";
						}

						XmlElement xmlCreditCardId = doc.CreateElement("CreditCardId");
						xmlCreditCardId.InnerText = !string.IsNullOrEmpty(fileCreditCardNumber) ? fileCreditCardNumber : "";
						xmlCDRItem.AppendChild(xmlCreditCardId);
					}

					if (!string.IsNullOrEmpty(fileRelatedToValue) && !string.IsNullOrEmpty(relatedToDisplayName))
					{
						XmlElement xmlRelatedToDisplayName = doc.CreateElement("RelatedToDisplayName");
						xmlRelatedToDisplayName.InnerText = !string.IsNullOrEmpty(relatedToDisplayName) ? relatedToDisplayName : "";
						xmlCDRItem.AppendChild(xmlRelatedToDisplayName);

						XmlElement xmlRelatedToValue = doc.CreateElement("RelatedToValue");
						xmlRelatedToValue.InnerText = !string.IsNullOrEmpty(fileRelatedToValue) ? fileRelatedToValue : "";
						xmlCDRItem.AppendChild(xmlRelatedToValue);
					}

					//Attach the XML Element for a CDR to the Document
					root.AppendChild(xmlCDRItem);

					bool existingValidClient = validClientSubUnitClientAccounts.Where(x => 
						x.ClientAccountNumber == fileClientAccountNumber &&
						x.SourceSystemCode == fileSourceSystemCode &&
						x.ClientSubUnitGuid == clientSubUnitGuid
					).Count() > 0;

					bool existingInvalidClient = invalidClientSubUnitClientAccounts.Where(x => 
						x.ClientAccountNumber == fileClientAccountNumber &&
						x.SourceSystemCode == fileSourceSystemCode &&
						x.ClientSubUnitGuid == clientSubUnitGuid
					).Count() > 0;

					//Check Client Account belongs to this Client
					if (existingInvalidClient)
					{
						//Error: Account and SourceSystemCode have been detected in the data file that are not associated to the ClientSubUnit
						returnMessage = "Account '" + fileClientAccountNumber + "' and SourceSystemCode '" + fileSourceSystemCode + "' have been detected in the data file that are not associated to the ClientSubUnit that you have selected ('" + clientSubUnitName + "'). Please either associate the Account and SourceSystemCode to the chosen ClientSubUnit or update the data file with the correct Account and SourceSystemCode";
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
						ClientSubUnitClientAccount clientSubUnitClientAccount = clientSubUnitClientAccountRepository.GetClientSubUnitClientAccount(
							fileClientAccountNumber,
							fileSourceSystemCode,
							clientSubUnitGuid
						);
						if (clientSubUnitClientAccount == null)
						{
							//Error: Account and SourceSystemCode have been detected in the data file that are not associated to the ClientSubUnit
							returnMessage = "Account '" + fileClientAccountNumber + "' and SourceSystemCode '" + fileSourceSystemCode + "' have been detected in the data file that are not associated to the ClientSubUnit that you have selected ('" + clientSubUnitName + "'). Please either associate the Account and SourceSystemCode to the chosen ClientSubUnit or update the data file with the correct Account and SourceSystemCode";
							if (!returnMessages.Contains(returnMessage))
							{
								returnMessages.Add(returnMessage);
							}

							ClientSubUnitClientAccountImport clientSubUnitClientAccountImport = new ClientSubUnitClientAccountImport()
							{
								ClientAccountNumber = fileClientAccountNumber,
								SourceSystemCode = fileSourceSystemCode,
								ClientSubUnitGuid = clientSubUnitGuid,
							};

							if (!invalidClientSubUnitClientAccounts.Contains(clientSubUnitClientAccountImport))
							{
								invalidClientSubUnitClientAccounts.Add(clientSubUnitClientAccountImport);
							}
						}
						else
						{
							if (!validClientSubUnitClientAccounts.Contains(clientSubUnitClientAccount))
							{
								validClientSubUnitClientAccounts.Add(clientSubUnitClientAccount);
							}
						}
					}

					//Error: There is a value that exceeds the maximum length of 50 characters
					if (fileCostCentre.Length > 50)
					{
						returnMessage = fileCostCentre + "is a VALUE that exceeds the maximum Value length of 50 characters. Please update the data file or select a new data file.";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					////Error: There is a credit card id that is not present in the database
					//if (!string.IsNullOrEmpty(fileCreditCardNumber))
					//{
					//	int creditCardId = Int32.Parse(fileCreditCardNumber);
					//	CreditCardRepository creditCardRepository = new CreditCardRepository();
					//	CreditCard creditCard = creditCardRepository.GetCreditCard(creditCardId, false);
					//	if (creditCard == null)
					//	{
					//		returnMessage = "Credit Card Id  " + fileCreditCardNumber + " has been detected in the file but is not present in the database. Please check the value and try again";
					//		if (!returnMessages.Contains(returnMessage))
					//		{
					//			returnMessages.Add(returnMessage);
					//			break;
					//		}
					//	}
					//}

					//Error: An invalid character that has been detected within the data file
					if (Regex.Match(fileCostCentre, dataRegex).Success)
					{
						foreach (Match n in Regex.Matches(fileCostCentre, dataRegex))
						{
							returnMessage = n.Groups[0].Value + " is an invalid character that has been detected within the data file. Please update the data file or select a new data file to import.";
							if (!returnMessages.Contains(returnMessage))
							{
								returnMessages.Add(returnMessage);
							}
						}
					}

					//Error: If a user doesn’t include the Validate CDR field on the UI, and there are RelatedToValues listed on the Import,  
					//the Import should fail and give the user an error message that Validate CDR value missing.
					if (!string.IsNullOrEmpty(fileRelatedToValue) && string.IsNullOrEmpty(relatedToDisplayName))
					{
						returnMessage = "A value in the RelatedToValue column has been detected within the data file but the Validate CDR value missing. Please go back and enter a Validate CDR value or remove the RelatedToValue from the data file.";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					//Error: If a user enters a value in the Validate CDR field on the UI, the Import file must include RelatedToValues else 
					// the Import should fail and give the user an error message that RelatedToValues missing on the Import
					if (string.IsNullOrEmpty(fileRelatedToValue) && !string.IsNullOrEmpty(relatedToDisplayName))
					{
						returnMessage = "A Validate CDR has been detected but the RelatedToValue is missing. Please go back and remove the Validate CDR value or add the RelatedToValue data into the data file.";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}
				}
			}
			if (i == 0)
			{
				returnMessage = "There is no data in the file";
				returnMessages.Add(returnMessage);
			}

			CDRImportStep2VM preImportCheckResult = new CDRImportStep2VM();
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
					from n in db.spDesktopDataAdmin_UpdateClientSubUnitClientDefinedReferencesCount_v1(
						clientSubUnit.ClientSubUnitGuid,
						displayName,
						System.Xml.Linq.XElement.Parse(doc.OuterXml),
						adminUserGuid
					)
					select n).ToList();

				foreach (spDesktopDataAdmin_UpdateClientSubUnitClientDefinedReferencesCount_v1Result message in output)
				{
					returnMessages.Add(message.MessageText.ToString());
				}

				preImportCheckResult.FileBytes = array;

				preImportCheckResult.IsValidData = true;
			}

			return preImportCheckResult;

		}

		public CDRImportStep3VM Import(byte[] FileBytes, string clientSubUnitGuid, string displayName, string relatedToDisplayName = "")
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            string fileToText = fileToText = enc.GetString(FileBytes);

			/*
			Regex to match on each line (5 items separated by commas, followed by a new line - last 2 items are optional)
			CDR Value, SourceSystemCode, ClientAccountNumber, CreditCardID (optional), RelatedToValue (optional)
			*/

			CDRImportStep3VM cdrPostImportResult = new CDRImportStep3VM();

            // Create the xml document container, this will be used to store the data after the Regex checks
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("CSUCDRs");
            doc.AppendChild(root);
			

			ClientSubUnit clientSubUnit = new ClientSubUnit();
            ClientSubUnitClientAccountRepository clientSubUnitClientAccountRepository = new ClientSubUnitClientAccountRepository();
            ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

			//Counters are above loop, every time we call proc,we have to sum values (old+new)
			int deletedItemCount = 0;
			int addedItemCount = 0;

			//declare step flag for proc
			int step = 0;

			//Split the CSV into lines
			string[] lines = fileToText.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			//Max size of chunk
			int MaxSizeOfChunk = 10;
			//Lines buffer
			string[] linesBuffer;

			//begin loop for divide into chunks
			for (int j = 0; j < lines.Length; j += MaxSizeOfChunk)
			{
				int i = 0;

				//Set MaxSizeOfChunk for last iteration
				if (lines.Length - j < MaxSizeOfChunk)
				{
					MaxSizeOfChunk = lines.Length - j;
				}
				//Copy from source (lines), array size(chunk), into buffer array
				linesBuffer = new string[MaxSizeOfChunk];
				Array.Copy(lines, j, linesBuffer, 0, MaxSizeOfChunk);

				//loop through CSV lines of buffer
				foreach (string line in linesBuffer)
				{

					if (i != j) //ignore first line with titles-we have to avoid first line, position (0,0)
					{

						string[] cells = line.Split(',');

						//extract the data items from the file
						string fileCostCentre = cells[0];                      //CDRValue (Cost Centre)
						string fileSourceSystemCode = cells[1];                //SourceSystemCode
						string fileClientAccountNumber = cells[2];             //CWT Account No (Air/Rail)

						//Credit Card No field is an optional field on the .csv file
						string fileCreditCardNumber = string.Empty;
						if (cells.Length >= 4)
						{
							fileCreditCardNumber = cells[3] ?? "";
						}

						//The RelatedToValue field is an optional field on the .csv file.  If there is no CDR Display Name value entered in the Validate CDR field,
						//then the Import should not look for a RelatedToValue column on the .csv file
						string fileRelatedToValue = string.Empty;
						if (cells.Length >= 5)
						{
							fileRelatedToValue = cells[4] ?? "";
						}

						//Build the XML Element for a CDR
						XmlElement xmlCDRItem = doc.CreateElement("CSUCDR");

						//Populate the XML Element for a CDR
						//This XMLDoc is only used if the File Passes Validation
						XmlElement xmlSourceSystemCode = doc.CreateElement("SourceSystemCode");
						xmlSourceSystemCode.InnerText = fileSourceSystemCode;
						xmlCDRItem.AppendChild(xmlSourceSystemCode);

						XmlElement xmlClientAccountNumber = doc.CreateElement("ClientAccountNumber");
						xmlClientAccountNumber.InnerText = fileClientAccountNumber;
						xmlCDRItem.AppendChild(xmlClientAccountNumber);

						XmlElement xmlCostCentre = doc.CreateElement("CostCentre");
						xmlCostCentre.InnerText = fileCostCentre;
						xmlCDRItem.AppendChild(xmlCostCentre);

						//Check for numerical credit card
						if (!string.IsNullOrEmpty(fileCreditCardNumber))
						{
							int creditCardId;
							if (!Int32.TryParse(fileCreditCardNumber, out creditCardId))
							{
								fileCreditCardNumber = "";
							}

							XmlElement xmlCreditCardId = doc.CreateElement("CreditCardId");
							xmlCreditCardId.InnerText = !string.IsNullOrEmpty(fileCreditCardNumber) ? fileCreditCardNumber : "";
							xmlCDRItem.AppendChild(xmlCreditCardId);
						}

						if (!string.IsNullOrEmpty(fileRelatedToValue) && !string.IsNullOrEmpty(relatedToDisplayName))
						{
							XmlElement xmlRelatedToDisplayName = doc.CreateElement("RelatedToDisplayName");
							xmlRelatedToDisplayName.InnerText = !string.IsNullOrEmpty(relatedToDisplayName) ? relatedToDisplayName : "";
							xmlCDRItem.AppendChild(xmlRelatedToDisplayName);

							XmlElement xmlRelatedToValue = doc.CreateElement("RelatedToValue");
							xmlRelatedToValue.InnerText = !string.IsNullOrEmpty(fileRelatedToValue) ? fileRelatedToValue : "";
							xmlCDRItem.AppendChild(xmlRelatedToValue);
						}

						//Attach the XML Element for a CDR to the Document
						root.AppendChild(xmlCDRItem);
						
					}
					
					i++;
				}



				//DB Check
				string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

				//Extra param added to proc, send 0 if first page, 1 others(if it is first page in that case we delete in proc)
				step = (j == 0 ? 0 : 1); 

				var output = (from n in db.spDesktopDataAdmin_UpdateClientSubUnitClientDefinedReferences_v1(
					clientSubUnit.ClientSubUnitGuid,
					displayName,
					System.Xml.Linq.XElement.Parse(doc.OuterXml),
					adminUserGuid,
					step)
							  select n).ToList();

				

				foreach (spDesktopDataAdmin_UpdateClientSubUnitClientDefinedReferences_v1Result message in output)
				{
					deletedItemCount += message.DeletedCount;
					addedItemCount += message.AddedCount;
				}

				//empty xml nodes into root, ready for next iteration
				root.RemoveAll();
			}
			//end loop for divide into chunks

			cdrPostImportResult.ClientSubUnitGuid = clientSubUnitGuid;
            cdrPostImportResult.DisplayName = displayName;
            cdrPostImportResult.AddedItemCount = addedItemCount;
            cdrPostImportResult.DeletedItemCount = deletedItemCount;
            return cdrPostImportResult;
        }
    }
}   