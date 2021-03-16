using System.Transactions;

namespace CWTDesktopDatabase.Helpers
{
    /*
    * http://blogs.msdn.com/b/dbrowne/archive/2010/06/03/using-new-transactionscope-considered-harmful.aspx
     * To fix possible timeout and deadlock issues in Credit Card transactions
     * THese transactions span 2 databases
    */
    public class TransactionUtils
    {
        public static TransactionScope CreateTransactionScope()
        {
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadCommitted;
            transactionOptions.Timeout = TransactionManager.MaximumTimeout;
            return new TransactionScope(TransactionScopeOption.Required, transactionOptions);
        }
    }
}