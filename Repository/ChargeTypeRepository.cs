using CWTDesktopDatabase.Models;
using System.Linq;

namespace CWTDesktopDatabase.Repository
{
    public class ChargeTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());


        public IQueryable<ChargeType> GetAllChargeTypes()
        {
            return db.ChargeTypes.OrderBy(c => c.ChargeTypeDescription);
        }

        public ChargeType GetChargeType(string chargeTypeCode)
        {
            return db.ChargeTypes.SingleOrDefault(c => c.ChargeTypeCode == chargeTypeCode);
        }
    }
}
