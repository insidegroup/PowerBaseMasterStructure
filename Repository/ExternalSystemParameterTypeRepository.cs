using System.Linq;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class ExternalSystemParameterTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
        public IQueryable<ExternalSystemParameterType> GetAllExternalSystemParameterTypes()
        {
            return db.ExternalSystemParameterTypes;
        }

        public ExternalSystemParameterType GetExternalSystemParameterType(int? id)
        {
            return db.ExternalSystemParameterTypes.SingleOrDefault(c => c.ExternalSystemParameterTypeId == id);
        }
    }
}
