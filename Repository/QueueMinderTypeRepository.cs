using System.Linq;
using System.Linq.Dynamic;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.Repository
{
    public class QueueMinderTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<QueueMinderType> GetAllQueueMinderTypes()
        {
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToReferenceInfo())
            {
                return db.QueueMinderTypes.OrderBy("QueueMinderTypeDescription");
            }
            else
            {
                //DONT SHOW 17,18,19
                int[] exceptionList = new int[] { 17,18,19 };
                var results = from p in db.QueueMinderTypes where !exceptionList.Contains(p.QueueMinderTypeId) select p;
                return results;

            }

        }
        public QueueMinderType GetQueueMinderType(int queueMinderTypeId)
        {
            return db.QueueMinderTypes.SingleOrDefault(c => c.QueueMinderTypeId == queueMinderTypeId);
        }
    }
}
