using CWTDesktopDatabase.Models;
using System.Linq;

namespace CWTDesktopDatabase.Repository
{
    public class TransactionTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<TransactionType> GetAllTransactionTypes()
        {
            return db.TransactionTypes.OrderBy(c => c.TransactionTypeCode);
        }

        //public TransactionType GetTransactionType(string transactionTypeCode)
       // {
        //    return db.TransactionTypes.SingleOrDefault(c => c.TransactionTypeCode == transactionTypeCode);
       // }
    }
}