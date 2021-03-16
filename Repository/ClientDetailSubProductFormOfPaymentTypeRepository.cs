using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System.Linq.Dynamic;
using CWTDesktopDatabase.Helpers;
namespace CWTDesktopDatabase.Repository
{
    public class ClientDetailSubProductFormOfPaymentTypeRepository
    {
        private ClientDetailDC db = new ClientDetailDC(Settings.getConnectionString());

         //Add Data From Linked Tables for Display
        public void EditForDisplay(ClientDetailSubProductFormOfPaymentType subProductFormOfPaymentType)
        {
            FormOfPaymentTypeRepository formOfPaymentTypeRepository = new FormOfPaymentTypeRepository();
            FormOfPaymentType formOfPaymentType = new FormOfPaymentType();
            formOfPaymentType = formOfPaymentTypeRepository.GetFormOfPaymentType(subProductFormOfPaymentType.FormOfPaymentTypeId);
            if (formOfPaymentType != null)
            {
                subProductFormOfPaymentType.FormOfPaymentTypeDescription = formOfPaymentType.FormOfPaymentTypeDescription;
            }

            SubProductRepository subProductRepository = new SubProductRepository();
            SubProduct subProduct = new SubProduct();
            subProduct = subProductRepository.GetSubProduct(subProductFormOfPaymentType.SubProductId);
            if (subProduct != null)
            {
                subProductFormOfPaymentType.SubProductName = subProduct.SubProductName;
            }
        }
        //SubProducts not used by this clientDetail
        public List<SubProduct> GetUnUsedSubProducts(int clientDetailId)
        {
            var result = from n in db.spDesktopDataAdmin_SelectClientDetailAvailableSubProducts_v1(clientDetailId)
                         select new SubProduct
                         {
                             SubProductId = n.SubProductId,
                             SubProductName = n.SubProductName
                         };
            return result.ToList();
        }

        //Add ClientDetail Contact
        public void Add(ClientDetail clientDetail, ClientDetailSubProductFormOfPaymentType clientDetailSubProductFormOfPaymentType)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientDetailSubProductFormOfPaymentType_v1(
                clientDetail.ClientDetailId,
                clientDetailSubProductFormOfPaymentType.SubProductId,
                clientDetailSubProductFormOfPaymentType.FormOfPaymentTypeId,
                adminUserGuid
            );

        }

         //Add ClientDetail ESCInformation
        public void Delete(ClientDetailSubProductFormOfPaymentType clientDetailSubProductFormOfPaymentType)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteClientDetailSubProductFormOfPaymentType_v1(
                clientDetailSubProductFormOfPaymentType.ClientDetailId,
                clientDetailSubProductFormOfPaymentType.SubProductId,
                clientDetailSubProductFormOfPaymentType.FormOfPaymentTypeId,
                adminUserGuid,
                clientDetailSubProductFormOfPaymentType.VersionNumber
            );
        }


        //Get one Item from Contact
        public ClientDetailSubProductFormOfPaymentType GetClientDetailSubProductFormOfPaymentType(int subProductId, int clientDetailId)
        {
            return db.ClientDetailSubProductFormOfPaymentTypes.SingleOrDefault(c => (c.SubProductId == subProductId) && (c.ClientDetailId == clientDetailId));
        }
       
         
    }
}
