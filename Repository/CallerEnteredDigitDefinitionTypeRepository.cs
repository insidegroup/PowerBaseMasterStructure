using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class CallerEnteredDigitDefinitionTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<CallerEnteredDigitDefinitionType> GetAllCallerEnteredDigitDefinitionTypes()
        {
            return db.CallerEnteredDigitDefinitionTypes.OrderBy(c => c.CallerEnteredDigitDefinitionTypeDescription);
        }

        public CallerEnteredDigitDefinitionType GetCallerEnteredDigitDefinitionType(int callerEnteredDigitDefinitionTypeId)
        {
            return db.CallerEnteredDigitDefinitionTypes.SingleOrDefault(c => c.CallerEnteredDigitDefinitionTypeId == callerEnteredDigitDefinitionTypeId);
        }

    }
}