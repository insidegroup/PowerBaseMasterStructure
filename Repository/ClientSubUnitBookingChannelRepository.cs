using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Xml;
using CWTDesktopDatabase.ViewModels;
using System.Text;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Repository
{
	public class BookingChannelRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of PageBookingChannel Items - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitBookingChannels_v1Result> PageBookingChannel(int page, string id, string sortField, int? sortOrder, string filter)
		{
			//get a page of records
			var result = db.spDesktopDataAdmin_SelectClientSubUnitBookingChannels_v1(id, sortField, sortOrder, page, filter ?? "").ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitBookingChannels_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get a Single Item
		public BookingChannel BookingChannel(int BookingChannelId)
		{
			return db.BookingChannels.SingleOrDefault(
				c => c.BookingChannelId == BookingChannelId
			);
		}

		//Add to DB
		public void Add(BookingChannelVM BookingChannelVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			//products to XML
			XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("ContentBookedItems");
			doc.AppendChild(root);

			if (BookingChannelVM.ContentBookedItems != null)
			{
				if (BookingChannelVM.ContentBookedItems.Count > 0)
				{
					XmlElement xmlContentBookedItems = doc.CreateElement("ContentBookedItems");
					foreach (string p in BookingChannelVM.ContentBookedItems)
					{
						if (!string.IsNullOrEmpty(p))
						{
							XmlElement xmlContentBookedItem = doc.CreateElement("ContentBookedItem");
							xmlContentBookedItem.InnerText = p;
							xmlContentBookedItems.AppendChild(xmlContentBookedItem);
						}
					}
					root.AppendChild(xmlContentBookedItems);
				}
			}

			db.spDesktopDataAdmin_InsertClientSubUnitBookingChannel_v1(
				BookingChannelVM.BookingChannel.ClientSubUnitGuid,
				BookingChannelVM.BookingChannel.GDSCode,
				BookingChannelVM.BookingChannel.BookingChannelTypeId,
				BookingChannelVM.BookingChannel.ProductChannelTypeId,
				BookingChannelVM.BookingChannel.DesktopUsedTypeId,
				BookingChannelVM.BookingChannel.BookingPseudoCityOrOfficeId,
				BookingChannelVM.BookingChannel.TicketingPseudoCityOrOfficeId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml), //ContentBookedItems
				adminUserGuid
			);
		}

		//Update in DB
		public void Update(BookingChannelVM BookingChannelVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			//products to XML
			XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("ContentBookedItems");
			doc.AppendChild(root);

			if (BookingChannelVM.ContentBookedItems != null)
			{
				if (BookingChannelVM.ContentBookedItems.Count > 0)
				{
					XmlElement xmlContentBookedItems = doc.CreateElement("ContentBookedItems");
					foreach (string p in BookingChannelVM.ContentBookedItems)
					{
						if (!string.IsNullOrEmpty(p))
						{
							XmlElement xmlContentBookedItem = doc.CreateElement("ContentBookedItem");
							xmlContentBookedItem.InnerText = p;
							xmlContentBookedItems.AppendChild(xmlContentBookedItem);
						}
					}
					root.AppendChild(xmlContentBookedItems);
				}
			}

			db.spDesktopDataAdmin_UpdateClientSubUnitBookingChannel_v1(
				BookingChannelVM.BookingChannel.BookingChannelId,
				BookingChannelVM.BookingChannel.ClientSubUnitGuid,
				BookingChannelVM.BookingChannel.GDSCode,
				BookingChannelVM.BookingChannel.BookingChannelTypeId,
				BookingChannelVM.BookingChannel.ProductChannelTypeId,
				BookingChannelVM.BookingChannel.DesktopUsedTypeId,
				BookingChannelVM.BookingChannel.BookingPseudoCityOrOfficeId,
				BookingChannelVM.BookingChannel.TicketingPseudoCityOrOfficeId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml), //ContentBookedItems
				adminUserGuid,
				BookingChannelVM.BookingChannel.VersionNumber
			);
		}

		//Delete From DB
		public void Delete(BookingChannel BookingChannel)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteClientSubUnitBookingChannel_v1(
				BookingChannel.BookingChannelId,
				BookingChannel.VersionNumber,
				adminUserGuid
			);
		}

        //Export Items to CSV
        public byte[] Export(string id)
        {
            StringBuilder sb = new StringBuilder();

            //Add Headers
            List<string> headers = new List<string>
            {
                "ClientSubUnitGuid",
                "GDSCode",
                "BookingChannel",
                "ProductChannel",
                "BookingPseudoCityOrOfficeId",
                "TicketingPseudoCityOrOfficeId",
                "DesktopUsed",
                "AdditionalBookingComment",
                "CreationTimestamp",
                "LastUpdateTimestamp"
            };

            sb.AppendLine(String.Join(",", headers.Select(x => x.ToString()).ToArray()));

            //Get ClientSubUnit
            ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
            ClientSubUnit clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

            //Add Items
            List<BookingChannel> bookingChannels = db.BookingChannels.Where(x => x.ClientSubUnitGuid == clientSubUnit.ClientSubUnitGuid).ToList();

            foreach (BookingChannel item in bookingChannels)
            {

                string date_format = "MM/dd/yy HH:mm";

                //BookingChannelTypeDescription
                string bookingChannelTypeDescription = string.Empty;
                if (item.BookingChannelTypeId != null)
                {
                    BookingChannelTypeRepository bookingChannelTypeRepository = new BookingChannelTypeRepository();
                    BookingChannelType bookingChannelType = bookingChannelTypeRepository.GetBookingChannelType(Int32.Parse(item.BookingChannelTypeId.ToString()));
                    if (bookingChannelType != null)
                    {
                        bookingChannelTypeDescription = bookingChannelType.BookingChannelTypeDescription;
                    }
                }

                //ProductChannelTypeDescription
                string productChannelTypeDescription = string.Empty;
                if (item.ProductChannelTypeId != null)
                {
                    ProductChannelTypeRepository productChannelTypeRepository = new ProductChannelTypeRepository();
                    ProductChannelType productChannelType = productChannelTypeRepository.GetProductChannelType(Int32.Parse(item.ProductChannelTypeId.ToString()));
                    if (productChannelType != null)
                    {
                        productChannelTypeDescription = productChannelType.ProductChannelTypeDescription;
                    }
                }

                //DesktopUsedTypeDescription
                string desktopUsedTypeDescription = string.Empty;
                if (item.DesktopUsedTypeId != null)
                {
                    DesktopUsedTypeRepository desktopUsedTypeRepository = new DesktopUsedTypeRepository();
                    DesktopUsedType desktopUsedType = desktopUsedTypeRepository.GetDesktopUsedType(Int32.Parse(item.DesktopUsedTypeId.ToString()));
                    if (desktopUsedType != null)
                    {
                        desktopUsedTypeDescription = desktopUsedType.DesktopUsedTypeDescription;
                    }
                }

                //AdditionalBookingComments
                StringBuilder additionalBookingCommentsList = new StringBuilder();
                AdditionalBookingCommentRepository additionalBookingCommentRepository = new AdditionalBookingCommentRepository();
                List<AdditionalBookingComment> additionalBookingComments = additionalBookingCommentRepository.GetAdditionalBookingComments(item.BookingChannelId);
                foreach (AdditionalBookingComment additionalBookingComment in additionalBookingComments)
                {
                    additionalBookingCommentsList.AppendFormat("{0} - {1}; ", additionalBookingComment.AdditionalBookingCommentDescription, additionalBookingComment.LanguageCode);
                }

                sb.AppendFormat(
                    "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",

                    clientSubUnit != null && !string.IsNullOrEmpty(clientSubUnit.ClientSubUnitGuid) ? clientSubUnit.ClientSubUnitGuid : "",
                    item.GDSCode != null && !string.IsNullOrEmpty(item.GDSCode) ? item.GDSCode : "",
                    bookingChannelTypeDescription != null && !string.IsNullOrEmpty(bookingChannelTypeDescription) ? bookingChannelTypeDescription : "",
                    productChannelTypeDescription != null && !string.IsNullOrEmpty(productChannelTypeDescription) ? productChannelTypeDescription : "",
                    item.BookingPseudoCityOrOfficeId != null && !string.IsNullOrEmpty(item.BookingPseudoCityOrOfficeId) ? item.BookingPseudoCityOrOfficeId : "",
                    item.TicketingPseudoCityOrOfficeId != null && !string.IsNullOrEmpty(item.TicketingPseudoCityOrOfficeId) ? item.TicketingPseudoCityOrOfficeId : "",
                    desktopUsedTypeDescription != null && !string.IsNullOrEmpty(desktopUsedTypeDescription) ? desktopUsedTypeDescription : "",
                    additionalBookingCommentsList != null && !string.IsNullOrEmpty(additionalBookingCommentsList.ToString()) ? string.Format("\"{0}\"", additionalBookingCommentsList) : "",
                    item.CreationTimestamp != null && item.CreationTimestamp.HasValue ? item.CreationTimestamp.Value.ToString(date_format) : "",
                    item.LastUpdateTimestamp != null && item.LastUpdateTimestamp.HasValue ? item.LastUpdateTimestamp.Value.ToString(date_format) : ""
                );

                sb.Append(Environment.NewLine);
            }

            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        private void ValidateLines(ref XmlDocument doc, string[] lines, ref List<string> returnMessages)
        {
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("BookingChannels");
            doc.AppendChild(root);

            string returnMessage;

            int i = 0;
            
            //loop through CSV lines
            foreach (string line in lines)
            {

                i++;

                if (i > 1) //ignore first line with titles
                {

                    Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                    String[] cells = csvParser.Split(line);
                    
                    //extract the data items from the file
                    string gdsCode = CWTStringHelpers.NullToEmpty(CWTStringHelpers.UnescapeQuotes(cells[0]));                                 //Required
                    string bookingChannelTypeDescription = CWTStringHelpers.NullToEmpty(CWTStringHelpers.UnescapeQuotes(cells[1]));           //Required
                    string productChannelTypeDescription = CWTStringHelpers.NullToEmpty(CWTStringHelpers.UnescapeQuotes(cells[2]));           //Required
                    string bookingPseudoCityOrOfficeId = CWTStringHelpers.NullToEmpty(CWTStringHelpers.UnescapeQuotes(cells[3]));
                    string ticketingPseudoCityOrOfficeId = CWTStringHelpers.NullToEmpty(CWTStringHelpers.UnescapeQuotes(cells[4]));
                    string desktopUsedTypeDescription = CWTStringHelpers.NullToEmpty(CWTStringHelpers.UnescapeQuotes(cells[5]));              //Required when Booking Channel = Offline
                    string additionalBookingCommentDescription = CWTStringHelpers.NullToEmpty(CWTStringHelpers.UnescapeQuotes(cells[6]));
                    string languageCode = CWTStringHelpers.NullToEmpty(CWTStringHelpers.UnescapeQuotes(cells[7]));

                    //Build the XML Element for items

                    XmlElement xmlBookingChannelItem = doc.CreateElement("BookingChannelItem");

                    //Validate data

                    /* GDS Code */

                    //Required
                    if (string.IsNullOrEmpty(gdsCode) == true)
                    {
                        returnMessage = "Row " + i + ": GDSCode is missing. Please provide a GDS Code";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }
                    else
                    {
                        //GDSCode must contain any one value from the GDSCode column of the GDS table
                        GDSRepository gdsRepository = new GDSRepository();
                        GDS gds = gdsRepository.GetGDS(gdsCode);
                        if (gds == null)
                        {
                            returnMessage = "Row " + i + ": GDSCode " + gdsCode + " is invalid. Please provide a valid GDS Code";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    XmlElement xmlGDSCode = doc.CreateElement("GDSCode");
                    xmlGDSCode.InnerText = gdsCode;
                    xmlBookingChannelItem.AppendChild(xmlGDSCode);

                    /* Booking Channel Type */
                    string bookingChannelTypeId = string.Empty;
                    BookingChannelTypeRepository bookingChannelTypeRepository = new BookingChannelTypeRepository();
                    BookingChannelType bookingChannelType = new BookingChannelType();

                    //Required
                    if (string.IsNullOrEmpty(bookingChannelTypeDescription) == true)
                    {
                        returnMessage = "Row " + i + ": Booking Channel  is missing. Please provide a Booking Channel";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }
                    else
                    {
                        //BookingChannelTypeDescription must contain any one value from the BookingChannelTypeDescription column of the BookingChannelType table
                        bookingChannelType = bookingChannelTypeRepository.GetBookingChannelTypeByDescription(bookingChannelTypeDescription);

                        if (bookingChannelType == null)
                        {
                            returnMessage = "Row " + i + ": BookingChannelTypeDescription " + bookingChannelTypeDescription + " is invalid. Please provide a valid BookingChannelType Code";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                        else
                        {
                            bookingChannelTypeId = bookingChannelType.BookingChannelTypeId.ToString();
                        }
                    }

                    XmlElement xmlBookingChannelTypeId = doc.CreateElement("BookingChannelTypeId");
                    xmlBookingChannelTypeId.InnerText = bookingChannelTypeId;
                    xmlBookingChannelItem.AppendChild(xmlBookingChannelTypeId);

                    /* Product Channel Type */
                    string productChannelTypeId = string.Empty;

                    ProductChannelTypeRepository productChannelTypeRepository = new ProductChannelTypeRepository();
                    ProductChannelType productChannelType = new ProductChannelType();
                    
                    //Required
                    if (string.IsNullOrEmpty(productChannelTypeDescription) == true)
                    {
                        returnMessage = "Row " + i + ": Product Channel is missing. Please provide a Product Channel";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }
                    else
                    {
                        //ProductChannelTypeDescription must contain any one value from the ProductChannelTypeDescription column of the ProductChannelType table
                        productChannelType = productChannelTypeRepository.GetProductChannelTypeByDescription(productChannelTypeDescription);
                        if (productChannelType == null)
                        {
                            returnMessage = "Row " + i + ": ProductChannelTypeDescription " + productChannelTypeDescription + " is invalid. Please provide a valid ProductChannelType Code";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                        else
                        {
                            productChannelTypeId = productChannelType.ProductChannelTypeId.ToString();
                        }
                    }

                    XmlElement xmlProductChannelTypeId = doc.CreateElement("ProductChannelTypeId");
                    xmlProductChannelTypeId.InnerText = productChannelTypeId;
                    xmlBookingChannelItem.AppendChild(xmlProductChannelTypeId);

                    //ProductChannelTypeId and BookingChannelTypeID must be present in the ProductChannelType table
                    if (bookingChannelType != null && bookingChannelType.BookingChannelTypeId > 0 && productChannelType != null && productChannelType.ProductChannelTypeId > 0)
                    {
                        ProductChannelType productChannelBookingChannelType = productChannelTypeRepository.GetProductChannelTypeBookingChannelType(bookingChannelType.BookingChannelTypeId, productChannelType.ProductChannelTypeId);
                        if (productChannelBookingChannelType == null)
                        {
                            returnMessage = "Row " + i + ": Product Channel and Booking Channel combination is invalid. Please provide a valid combination";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    /* DesktopUsedTypeDescription */
                    string desktopUsedTypeId = string.Empty;

                    //Required if BookingChannelTypeDescription is Offline
                    if (string.IsNullOrEmpty(desktopUsedTypeDescription) == true && bookingChannelTypeDescription.ToLower() == "offline")
                    {
                        returnMessage = "Row " + i + ": Desktop Used is required when Booking Channel is Offline. Please provide a valid Desktop Used or change Booking Channel";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }
                    else if(!string.IsNullOrEmpty(desktopUsedTypeDescription))
                    {
                        //DesktopUsed must contain any one value from the DesktopUsedTypeDescription column of the DesktopUsedType table
                        DesktopUsedTypeRepository desktopUsedTypeRepository = new DesktopUsedTypeRepository();
                        DesktopUsedType desktopUsedType = desktopUsedTypeRepository.GetDesktopUsedTypeByDescription(desktopUsedTypeDescription);
                        if (desktopUsedType == null)
                        {
                            returnMessage = "Row " + i + ": Desktop Used  " + desktopUsedTypeDescription + " is invalid. Please provide a valid Desktop Used";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                        else
                        {
                            desktopUsedTypeId = desktopUsedType.DesktopUsedTypeId.ToString();
                        }
                    }

                    XmlElement xmlDesktopUsedTypeId = doc.CreateElement("DesktopUsedTypeId");
                    xmlDesktopUsedTypeId.InnerText = desktopUsedTypeId;
                    xmlBookingChannelItem.AppendChild(xmlDesktopUsedTypeId);
                    
                    /* BookingPseudoCityOrOfficeId  */

                    int validBookingPseudoCityOrOfficeIdCount = db.ValidPseudoCityOrOfficeIds.Where(x => x.PseudoCityOrOfficeId == bookingPseudoCityOrOfficeId).Count();
                    if (!string.IsNullOrEmpty(bookingPseudoCityOrOfficeId) && validBookingPseudoCityOrOfficeIdCount == 0)
                    {
                        returnMessage = "Row " + i + ": BookingPseudoCityOrOfficeId " + bookingPseudoCityOrOfficeId + " is invalid. Please provide a valid BookingPseudoCityOrOfficeId";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    /* Trim if is not null */

                    if (!string.IsNullOrEmpty(bookingPseudoCityOrOfficeId))
                    {
                        bookingPseudoCityOrOfficeId = bookingPseudoCityOrOfficeId.Trim();
                    }

                    XmlElement xmlBookingPseudoCityOrOfficeId = doc.CreateElement("BookingPseudoCityOrOfficeId");
                    xmlBookingPseudoCityOrOfficeId.InnerText = bookingPseudoCityOrOfficeId;
                    xmlBookingChannelItem.AppendChild(xmlBookingPseudoCityOrOfficeId);

                    /* TicketingPseudoCityOrOfficeId   */

                    int validTicketingPseudoCityOrOfficeIdCount = db.ValidPseudoCityOrOfficeIds.Where(x => x.PseudoCityOrOfficeId == ticketingPseudoCityOrOfficeId).Count();
                    if (!string.IsNullOrEmpty(ticketingPseudoCityOrOfficeId) && validTicketingPseudoCityOrOfficeIdCount == 0)
                    {   
                        returnMessage = "Row " + i + ": TicketingPseudoCityOrOfficeId " + ticketingPseudoCityOrOfficeId + " is invalid. Please provide a valid TicketingPseudoCityOrOfficeId";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    /* Trim if is not null */

                    if (!string.IsNullOrEmpty(ticketingPseudoCityOrOfficeId))
                    {
                        ticketingPseudoCityOrOfficeId = ticketingPseudoCityOrOfficeId.Trim();
                    }

                    XmlElement xmlTicketingPseudoCityOrOfficeId = doc.CreateElement("TicketingPseudoCityOrOfficeId");
                    xmlTicketingPseudoCityOrOfficeId.InnerText = ticketingPseudoCityOrOfficeId;
                    xmlBookingChannelItem.AppendChild(xmlTicketingPseudoCityOrOfficeId);

                    /* AdditionalBookingComment  */

                    if (!string.IsNullOrEmpty(additionalBookingCommentDescription))
                    {
                        //AdditionalBookingComment is freeform text and will allow up to 1500 alphanumeric and allowable special characters. 
                        if (additionalBookingCommentDescription.Length > 1500)
                        {
                            returnMessage = "Row " + i + ": AdditionalBookingComment contains more than 1500 characters. Please provide a valid AdditionalBookingComment";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }

                        //Allowable special characters are: space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), ampersand and accented characters. 
                        string additionalBookingCommentPattern = @"^[À-ÿ\w\s\-\(\)\.\&\'\’_]+$";
                        if (!Regex.IsMatch(additionalBookingCommentDescription, additionalBookingCommentPattern, RegexOptions.IgnoreCase))
                        {
                            returnMessage = "Row " + i + ": AdditionalBookingComment contains invalid special characters. Please provide a valid AdditionalBookingComment";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    XmlElement xmlAdditionalBookingCommentDescription = doc.CreateElement("AdditionalBookingCommentDescription");
                    xmlAdditionalBookingCommentDescription.InnerText = additionalBookingCommentDescription;
                    xmlBookingChannelItem.AppendChild(xmlAdditionalBookingCommentDescription);

                    /* Language Code */

                    //Where the AdditionalBookingComment value exists, then the LanguageCode column must contain a LanguageCode value,
                    if (!string.IsNullOrEmpty(additionalBookingCommentDescription) && (languageCode == null || string.IsNullOrEmpty(languageCode)))
                    {
                        returnMessage = "Row " + i + ": AdditionalBookingComment provided, LanguageCode is mandatory. Please provide a valid LanguageCode";
                        if (!returnMessages.Contains(returnMessage))
                        {
                            returnMessages.Add(returnMessage);
                        }
                    }

                    //LanguageCode must contain any one value from the LanguageCode column of the Language table
                    if (!string.IsNullOrEmpty(languageCode))
                    {
                        LanguageRepository languageRepository = new LanguageRepository();
                        Language language = languageRepository.GetLanguage(languageCode);
                        if (language == null)
                        {
                            returnMessage = "Row " + i + ": LanguageCode " + languageCode + " is invalid. Please provide a valid Language Code";
                            if (!returnMessages.Contains(returnMessage))
                            {
                                returnMessages.Add(returnMessage);
                            }
                        }
                    }

                    XmlElement xmlLanguageCode = doc.CreateElement("LanguageCode");
                    xmlLanguageCode.InnerText = languageCode;
                    xmlBookingChannelItem.AppendChild(xmlLanguageCode);

                    //Attach the XML Element for an item to the Document
                    root.AppendChild(xmlBookingChannelItem);
                }
            }

            if (i == 0)
            {
                returnMessage = "There is no data in the file";
                returnMessages.Add(returnMessage);
            }
        }

        public BookingChannelImportStep2VM PreImportCheck(HttpPostedFileBase file, string clientSubUnitGuid)
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

                if (headers.Length < 8)
                {
                    string returnMessage = "File contains incorrect number of columns. File must contain columns for GDSCode, BookingChannel, ProductChannel, BookingPseudoCityOrOfficeId, TicketingPseudoCityOrOfficeId, DesktopUsed, AdditionalBookingComment, and LanguageCode";
                    returnMessages.Add(returnMessage);
                    validHeaders = false;
                }
            }

            //Validate CSV Lines
            if (validHeaders)
            {
                ValidateLines(ref doc, lines, ref returnMessages);
            }

            BookingChannelImportStep2VM preImportCheckResult = new BookingChannelImportStep2VM();
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
                    from n in db.spDesktopDataAdmin_UpdateClientSubUnitBookingChannelCount_v1(
                        clientSubUnitGuid,
                        System.Xml.Linq.XElement.Parse(doc.OuterXml),
                        adminUserGuid
                    )
                    select n).ToList();

                foreach (spDesktopDataAdmin_UpdateClientSubUnitBookingChannelCount_v1Result message in output)
                {
                    returnMessages.Add(message.MessageText.ToString());
                }

                preImportCheckResult.FileBytes = array;

                preImportCheckResult.IsValidData = true;
            }

            return preImportCheckResult;

        }

        public BookingChannelImportStep3VM Import(byte[] FileBytes, string clientSubUnitGuid)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            string fileToText = fileToText = enc.GetString(FileBytes);

            BookingChannelImportStep3VM cdrPostImportResult = new BookingChannelImportStep3VM();
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

            var output = (from n in db.spDesktopDataAdmin_UpdateClientSubUnitBookingChannels_v1(
                clientSubUnitGuid,
                System.Xml.Linq.XElement.Parse(doc.OuterXml),
                adminUserGuid)
                          select n).ToList();

            int deletedItemCount = 0;
            int addedItemCount = 0;

            foreach (spDesktopDataAdmin_UpdateClientSubUnitBookingChannels_v1Result message in output)
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
 
 
 