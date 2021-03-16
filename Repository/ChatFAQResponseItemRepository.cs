using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Web.Security;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
    public class ChatFAQResponseItemRepository
    {
        private ChatFAQResponseGroupDC db = new ChatFAQResponseGroupDC(Settings.getConnectionString());

        //Get a Page of Chat FAQ Response Items - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectChatFAQResponseItems_v1Result> PageChatFAQResponseItems(int chatFAQResponseGroupId, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectChatFAQResponseItems_v1(chatFAQResponseGroupId, filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectChatFAQResponseItems_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
		public ChatFAQResponseItem GetItem(int chatFAQResponseId)
		{
			return db.ChatFAQResponseItems.SingleOrDefault(c => c.ChatFAQResponseItemId == chatFAQResponseId);
		}

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(ChatFAQResponseItem chatFAQResponseItem)
        {
            //ChatMessageFAQName
            if(chatFAQResponseItem.ChatMessageFAQId > 0)
            {
                ChatMessageFAQRepository chatMessageFAQRepository = new ChatMessageFAQRepository();
                ChatMessageFAQ chatMessageFAQ = chatMessageFAQRepository.GetChatMessageFAQ(chatFAQResponseItem.ChatMessageFAQId);
                if (chatMessageFAQ != null)
                {
                    chatFAQResponseItem.ChatMessageFAQName = chatMessageFAQ.ChatMessageFAQName;
                }
            }

            //Set LanguageName to en-gb
            LanguageRepository languageRepository = new LanguageRepository();
            Language language = languageRepository.GetLanguage("en-GB");
            if (language != null)
            {
                chatFAQResponseItem.LanguageCode = language.LanguageCode;
                chatFAQResponseItem.LanguageName = language.LanguageName;
            }
        }

        //Add Item 
        public void Add(ChatFAQResponseItem chatFAQResponseItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertChatFAQResponseItem_v1(
                chatFAQResponseItem.ChatFAQResponseGroupId,
                chatFAQResponseItem.ChatFAQResponseItemDescription,
                chatFAQResponseItem.ChatMessageFAQId,
                chatFAQResponseItem.LanguageCode,
                adminUserGuid
            );
        }

        //Edit Item 
        public void Edit(ChatFAQResponseItem chatFAQResponseItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateChatFAQResponseItem_v1(
                chatFAQResponseItem.ChatFAQResponseItemId,
                chatFAQResponseItem.ChatFAQResponseItemDescription,
                chatFAQResponseItem.ChatMessageFAQId,
                chatFAQResponseItem.LanguageCode,
                adminUserGuid,
                chatFAQResponseItem.VersionNumber
            );
        }

        //Delete Item
        public void Delete(ChatFAQResponseItem chatFAQResponseItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteChatFAQResponseItem_v1(
                chatFAQResponseItem.ChatFAQResponseItemId,
                adminUserGuid,
                chatFAQResponseItem.VersionNumber);
        }


		//Export Items to CSV
		public byte[] Export(int chatFAQResponseGroupId)
		{
			StringBuilder sb = new StringBuilder();

			//Add Headers
			List<string> headers = new List<string>
            {
                "ChatFAQResponseGroupName",
				"ChatMessageFAQId",
				"ChatMessageFAQName",				
				"ChatFAQResponseItemDescription",
				"CreationTimestamp",
				"CreationUserIdentifier",
				"LastUpdateTimestamp",
				"LastUpdateUserIdentifier"
            };

			sb.AppendLine(String.Join(",", headers.Select(x => x.ToString()).ToArray()));

			//Add Items
			List<ChatFAQResponseItem> chatFAQResponseItems = db.ChatFAQResponseItems.Where(x => x.ChatFAQResponseGroupId == chatFAQResponseGroupId).OrderBy(x => x.ChatFAQResponseItemId).ToList();

			foreach (ChatFAQResponseItem item in chatFAQResponseItems)
			{
				string date_format = "MM/dd/yy HH:mm";

				EditItemForDisplay(item);

				sb.AppendFormat(
					@"""{0}"",""{1}"",""{2}"",""{3}"",""{4}"",""{5}"",""{6}"",""{7}""",

					item.ChatFAQResponseGroup != null ? item.ChatFAQResponseGroup.ChatFAQResponseGroupName : "NULL",
					item.ChatMessageFAQId,
					item.ChatMessageFAQName != null ? item.ChatMessageFAQName : "NULL",
					item.ChatFAQResponseItemDescription != null ? item.ChatFAQResponseItemDescription : "NULL",
					item.CreationTimestamp != null && item.CreationTimestamp.HasValue ? item.CreationTimestamp.Value.ToString(date_format) : "NULL",
					item.CreationUserIdentifier != null ? item.CreationUserIdentifier : "NULL",
					item.LastUpdateTimestamp != null && item.LastUpdateTimestamp.HasValue ? item.LastUpdateTimestamp.Value.ToString(date_format) : "NULL",
					item.LastUpdateUserIdentifier != null ? item.LastUpdateUserIdentifier : "NULL"
				);

				sb.Append(Environment.NewLine);
			}

			return Encoding.UTF8.GetBytes(sb.ToString());
		}

		private void ValidateLines(ref XmlDocument doc, string[] lines, ref List<string> returnMessages)
		{
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("ChatFAQResponseItems");
			doc.AppendChild(root);

			string returnMessage;

			int i = 0;

			//Store Valid ClientSubUnits
			List<int> validChatMessageFAQIds = new List<int>();
			List<int> invalidChatMessageFAQIds = new List<int>();

			//loop through CSV lines
			foreach (string line in lines)
			{

				i++;

				if (i > 1) //ignore first line with titles
				{

					Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
					String[] cells = csvParser.Split(line);

					//extract the data items from the file
					string chatMessageFAQIdValue = CWTStringHelpers.NullToEmpty(CWTStringHelpers.UnescapeQuotes(cells[0]));					//Required
					string chatFAQResponseItemDescription = CWTStringHelpers.NullToEmpty(CWTStringHelpers.UnescapeQuotes(cells[1]));	//Required

					//Build the XML Element for items
					XmlElement xmlChatFAQResponseItem = doc.CreateElement("ChatFAQResponseItem");

					//Validate data

					/* ChatMessageFAQId */
					int chatMessageFAQId = 0;

					//Required
					if (string.IsNullOrEmpty(chatMessageFAQIdValue) == true || !int.TryParse(chatMessageFAQIdValue, out chatMessageFAQId))
					{
						returnMessage = "Row " + i + ": ChatMessageFAQId is missing. Please provide a Chat Message FAQ Id";
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}
					else
					{
						bool existingValidChatMessageFAQId = validChatMessageFAQIds.Contains(chatMessageFAQId);
						bool existingInvalidChatMessageFAQId = invalidChatMessageFAQIds.Contains(chatMessageFAQId);

						//Check Chat Message FAQ Id is valid
						if (existingInvalidChatMessageFAQId)
						{
							//Error: Chat Message FAQ Id is invalid
							returnMessage = string.Format("Row " + i + ": ChatMessageFAQId {0} is invalid. Please provide a valid Chat Message FAQ Id", chatMessageFAQId.ToString());
							if (!returnMessages.Contains(returnMessage))
							{
								returnMessages.Add(returnMessage);
							}
						}
						else if (existingValidChatMessageFAQId)
						{
							//Error: Chat Message FAQ Id is duplicated in file
							returnMessage = string.Format("Row " + i + ": ChatMessageFAQId {0} is not unique. Please remove duplicate Chat Message FAQ Id", chatMessageFAQId.ToString());
							if (!returnMessages.Contains(returnMessage))
							{
								returnMessages.Add(returnMessage);
							}

						} else {
							
							//Check if valid Chat Message FAQ
							ChatMessageFAQRepository chatMessageFAQRepository = new ChatMessageFAQRepository();
							ChatMessageFAQ chatMessageFAQ = chatMessageFAQRepository.GetChatMessageFAQ(chatMessageFAQId);
							
							//Error: Chat Message FAQ Id is invalid
							if (chatMessageFAQ == null)
							{
								returnMessage = string.Format("Row " + i + ": ChatMessageFAQId {0} is invalid. Please provide a valid Chat Message FAQ Id", chatMessageFAQId.ToString());
								if (!returnMessages.Contains(returnMessage))
								{
									returnMessages.Add(returnMessage);
								}

								if (!invalidChatMessageFAQIds.Contains(chatMessageFAQId))
								{
									invalidChatMessageFAQIds.Add(chatMessageFAQId);
								}
							}
							else
							{
								validChatMessageFAQIds.Add(chatMessageFAQId);
							}
						}
					}

					XmlElement xmlChatMessageFAQId = doc.CreateElement("ChatMessageFAQId");
					xmlChatMessageFAQId.InnerText = chatMessageFAQId.ToString();
					xmlChatFAQResponseItem.AppendChild(xmlChatMessageFAQId);

					//ChatFAQResponseItemDescription
					string dataRegex = @"^([À-ÿ\w\s\/\*\-\.\(\)\,\u0022\“\'\%\$\=\+\?\!\:\;\@\<\>\""]+)";

					if (string.IsNullOrEmpty(chatFAQResponseItemDescription.Trim()))
					{
						//Error: ChatFAQResponseItemDescription is missing
						returnMessage = string.Format("Row " + i + ": ChatFAQResponseItemDescription is missing. Please provide a Chat FAQ Response");
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}
					else if (chatFAQResponseItemDescription.Length > 400)
					{
						//Error: ChatFAQResponseItemDescription is too long
						returnMessage = string.Format("Row " + i + ": ChatFAQResponseItemDescription contains more than 400 characters. Please provide a valid Chat FAQ Response");
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}
					else if (!Regex.Match(chatFAQResponseItemDescription, dataRegex).Success)
					{
						//Error: ChatFAQResponseItemDescription contains invalid characters
						returnMessage = string.Format("Row " + i + ": ChatFAQResponseItemDescription contains invalid special characters. Please provide a valid Chat FAQ Response");
						if (!returnMessages.Contains(returnMessage))
						{
							returnMessages.Add(returnMessage);
						}
					}

					XmlElement xmlChatFAQResponseItemDescription = doc.CreateElement("ChatFAQResponseItemDescription");
					xmlChatFAQResponseItemDescription.InnerText = chatFAQResponseItemDescription;
					xmlChatFAQResponseItem.AppendChild(xmlChatFAQResponseItemDescription);

					//Attach the XML Element for an item to the Document
					root.AppendChild(xmlChatFAQResponseItem);
				}
			}

			if (i < 2)
			{
				returnMessage = "There is no data in the file";
				returnMessages.Add(returnMessage);
			}
		}

		public ChatFAQResponseItemImportStep2VM PreImportCheck(HttpPostedFileBase file, int chatFAQResponseGroupId)
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
					string returnMessage = "File contains incorrect number of columns. File must contain columns for ChatMessageFAQId and ChatFAQResponseItemDescription";
					returnMessages.Add(returnMessage);
					validHeaders = false;
				}
			}

			//Validate CSV Lines
			if (validHeaders)
			{
				ValidateLines(ref doc, lines, ref returnMessages);
			}

			ChatFAQResponseItemImportStep2VM preImportCheckResult = new ChatFAQResponseItemImportStep2VM();
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
					from n in db.spDesktopDataAdmin_UpdateChatFAQResponseItemCount_v1(
						chatFAQResponseGroupId,
						System.Xml.Linq.XElement.Parse(doc.OuterXml),
						adminUserGuid
					)
					select n).ToList();

				foreach (spDesktopDataAdmin_UpdateChatFAQResponseItemCount_v1Result message in output)
				{
					returnMessages.Add(message.MessageText.ToString());
				}

				preImportCheckResult.FileBytes = array;

				preImportCheckResult.IsValidData = true;

				preImportCheckResult.ChatFAQResponseGroupId = chatFAQResponseGroupId;
			}

			return preImportCheckResult;

		}

		public ChatFAQResponseItemImportStep3VM Import(byte[] FileBytes, int chatFAQResponseGroupId)
		{
			System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
			string fileToText = fileToText = enc.GetString(FileBytes);

			ChatFAQResponseItemImportStep3VM cdrPostImportResult = new ChatFAQResponseItemImportStep3VM();

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

			var output = (from n in db.spDesktopDataAdmin_UpdateChatFAQResponseItems_v1(
					chatFAQResponseGroupId,
					System.Xml.Linq.XElement.Parse(doc.OuterXml),
					adminUserGuid)
				select n).ToList();

			int addedItemCount = 0;

			foreach (spDesktopDataAdmin_UpdateChatFAQResponseItems_v1Result message in output)
			{
				returnMessages.Add(message.MessageText.ToString());
			}

			cdrPostImportResult.ReturnMessages = returnMessages;
			cdrPostImportResult.AddedItemCount = addedItemCount;

			return cdrPostImportResult;
		}
    }
}
