using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class TicketTypeRepository
    {
        private TicketTypeDC db = new TicketTypeDC(Settings.getConnectionString());

        public IQueryable<TicketType> GetAllTicketTypes()
        {
            return db.TicketTypes.OrderBy(t => t.TicketTypeDescription);
        }

        public TicketType GetTicketType(int ticketTypeId)
        {
            return db.TicketTypes.SingleOrDefault(c => c.TicketTypeId == ticketTypeId);
        }
    }
}
