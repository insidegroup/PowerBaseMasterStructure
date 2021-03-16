using System.Linq;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class ContextRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public Context GetContext(int contextId)
        {
            return db.Contexts.SingleOrDefault(c => c.ContextId == contextId);
        }

        public IQueryable<Context> GetAllContexts()
        {
            return db.Contexts.OrderBy(c => c.ContextName);
        }
    }
}
