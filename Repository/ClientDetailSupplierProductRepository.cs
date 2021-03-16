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
    public class ClientDetailSupplierProductRepository
    {
        private ClientDetailDC db = new ClientDetailDC(Settings.getConnectionString());

        //Get one Item from ClientDetail
        public ClientDetailSupplierProduct GetClientDetailSupplierProduct(int clientDetailId, int productId, string supplierCode)
        {
            return db.ClientDetailSupplierProducts.SingleOrDefault(c => (c.ClientDetailId == clientDetailId) && (c.ProductId == productId) && (c.SupplierCode.Equals(supplierCode)));
        }

         //Add Data From Linked Tables for Display
        public void EditForDisplay(ClientDetailSupplierProduct clientDetailSupplierProduct)
        {
            SupplierRepository supplierRepository = new SupplierRepository();
            Supplier supplier = new Supplier();
            supplier = supplierRepository.GetSupplier(clientDetailSupplierProduct.SupplierCode, clientDetailSupplierProduct.ProductId);
            if (supplier != null)
            {
                clientDetailSupplierProduct.SupplierName = supplier.SupplierName;
            }
            ProductRepository productRepository = new ProductRepository();
            Product product = new Product();
            product = productRepository.GetProduct(clientDetailSupplierProduct.ProductId);
            if (product != null)
            {
                clientDetailSupplierProduct.ProductName = product.ProductName;
            }
            
        }
        //Add ClientDetail ESCInformation
        public void Add(ClientDetail clientDetail, ClientDetailSupplierProduct clientDetailSupplierProduct)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientDetailSupplierProduct_v1(
                clientDetail.ClientDetailId,
                clientDetailSupplierProduct.ProductId,
                clientDetailSupplierProduct.SupplierCode,
                adminUserGuid
            );
        }

        //Add ClientDetail ESCInformation
        public void Delete(ClientDetailSupplierProduct clientDetailSupplierProduct)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteClientDetailSupplierProduct_v1(
                clientDetailSupplierProduct.ClientDetailId,
                clientDetailSupplierProduct.ProductId,
                clientDetailSupplierProduct.SupplierCode,
                adminUserGuid,
                clientDetailSupplierProduct.VersionNumber
            );
        }
    }
}