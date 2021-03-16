using CWTDesktopDatabase.Models;
using System.Linq;

namespace CWTDesktopDatabase.Repository
{
    public class ClientFeeOutputRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get one Item
        public ClientFeeOutput GetItem(int clientFeeOutputId)
        {
            return db.ClientFeeOutputs.SingleOrDefault(c => (c.ClientFeeOutputId == clientFeeOutputId));
        }
    }
}