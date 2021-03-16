using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class FormOfPaymentTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());


        public IQueryable<FormOfPaymentType> GetAllFormOfPaymentTypes()
        {
            return db.FormOfPaymentTypes.OrderBy(c => c.FormOfPaymentTypeDescription);
        }

        public FormOfPaymentType GetFormOfPaymentType(int id)
        {
            return db.FormOfPaymentTypes.SingleOrDefault(c => c.FormOfPaymentTypeId == id);
        }

        internal List<FormOfPaymentType> LookUpFormOfPaymentTypes(string searchText, int maxResults)
        {
            var result = from n in GetAllFormOfPaymentTypes() where n.FormOfPaymentTypeDescription.Contains(searchText) orderby n.FormOfPaymentTypeDescription select n;
            return result.Take(maxResults).ToList();

        }

        //get one item by name
        public List<FormOfPaymentTypeJSON> GetFormOfPaymentTypeByName(string formOfPaymentTypeDescription)
        {
            FormOfPaymentTypeRepository formOfPaymentTypeRepository = new FormOfPaymentTypeRepository();
            var result = from n in formOfPaymentTypeRepository.GetAllFormOfPaymentTypes()
                         where n.FormOfPaymentTypeDescription.Trim().Equals(formOfPaymentTypeDescription)
                         select
                             new FormOfPaymentTypeJSON
                             {
                                 FormOfPaymentTypeId = n.FormOfPaymentTypeId,
                                 FormOfPaymentTypeDescription = n.FormOfPaymentTypeDescription
                             };
            return result.ToList();
        }
    }
}
