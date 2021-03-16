using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class ChatMessageFAQVM : CWTBaseViewModel
	{
		public ChatMessageFAQ ChatMessageFAQ { get; set; }

        public bool AllowDelete { get; set; }
        public List<ChatMessageFAQReference> ChatMessageFAQReferences { get; set; }

        public ChatMessageFAQVM()
		{
		}

		public ChatMessageFAQVM(ChatMessageFAQ chatMessageFAQ)
		{
			ChatMessageFAQ = chatMessageFAQ;
		}
	}
}