using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Models
{
    partial class WizardDC
    {
    }
    //Success/Fail with Message and HTML retruned to JSONRequest
    public class WizardJSONResponse
    {
        public bool success { get; set; }
        public string html { get; set; }
        public string message { get; set; }

        public void SetResponse(bool b, string s, string m)
        {
            this.success = b;
            this.html = s;
            this.message = m;
        }
        public void SetResponse(bool b, string m)
        {
            this.success = b;
            this.message = m;
        }
        public void SetResponse(bool b)
        {
            this.success = b;
        }

    }

    //Success/Fail with Message
    public class WizardMessage
    {
        public bool success { get; set; }
        public string message { get; set; }
    }

    //List of <WizardMessage>
    public class WizardMessages
    {
        public List<WizardMessage> Messages { get; set; }

        public WizardMessages()
        {
            this.Messages = new List<WizardMessage>();
        }


        public void AddMessage(string message, bool success)
        {
            WizardMessage wizardMessage = new WizardMessage();
            wizardMessage.message = message;
            wizardMessage.success = success;
            Messages.Add(wizardMessage);
        }

    }
 
}