using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Repository
{
    public class PointOfSaleFeeLoadRepository
    {
        private PointOfSaleFeeLoadDataContext db = new PointOfSaleFeeLoadDataContext(Settings.getConnectionString());
        HierarchyRepository hierarchyRepository = new HierarchyRepository();

        //Get a Page of PointOfSaleFeeLoads - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPointOfSaleFeeLoads_v1Result> PagePointOfSaleFeeLoads(
            string clientTopUnitName,
            string clientSubUnitName,
            string travelerTypeName,
            int page,
            string sortField,
            int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPointOfSaleFeeLoads_v1(clientTopUnitName, clientSubUnitName, travelerTypeName, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPointOfSaleFeeLoads_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one PointOfSaleFeeLoad
        public PointOfSaleFeeLoad GetGroup(int pointOfSaleFeeLoadId)
        {
            return db.PointOfSaleFeeLoads.SingleOrDefault(c => c.PointOfSaleFeeLoadId == pointOfSaleFeeLoadId);
        }

        //Add Group
        public void Add(PointOfSaleFeeLoadVM pointOfSaleFeeLoadVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPointOfSaleFeeLoad_v1(
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.ClientTopUnitGuid,
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.ClientSubUnitGuid,
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.TravelerTypeGuid,
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.FeeLoadDescriptionTypeCode,
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.ProductId,
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.AgentInitiatedFlag,
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.TravelIndicator,
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.FeeLoadAmount,
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.FeeLoadCurrencyCode,
                adminUserGuid
            );
        }

        //Edit Group
        public void Edit(PointOfSaleFeeLoadVM pointOfSaleFeeLoadVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePointOfSaleFeeLoad_v1(
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.PointOfSaleFeeLoadId,
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.ClientTopUnitGuid,
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.ClientSubUnitGuid,
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.TravelerTypeGuid,
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.FeeLoadDescriptionTypeCode,
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.ProductId,
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.AgentInitiatedFlag,
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.TravelIndicator,
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.FeeLoadAmount,
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.FeeLoadCurrencyCode,
                adminUserGuid,
                pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.VersionNumber
            );
        }

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(PointOfSaleFeeLoad pointOfSaleFeeLoad)
        {
            //ClientTopUnit
            if (pointOfSaleFeeLoad.ClientTopUnitGuid != null)
            {
                ClientTopUnitRepository ClientTopUnitRepository = new ClientTopUnitRepository();
                ClientTopUnit clientTopUnit = ClientTopUnitRepository.GetClientTopUnit(pointOfSaleFeeLoad.ClientTopUnitGuid);
                if (clientTopUnit != null)
                {
                    pointOfSaleFeeLoad.ClientTopUnitName = clientTopUnit.ClientTopUnitName;
                }
            }

            //ClientSubUnit
            if (pointOfSaleFeeLoad.ClientSubUnitGuid != null)
            {
                ClientSubUnitRepository ClientSubUnitRepository = new ClientSubUnitRepository();
                ClientSubUnit clientSubUnit = ClientSubUnitRepository.GetClientSubUnit(pointOfSaleFeeLoad.ClientSubUnitGuid);
                if (clientSubUnit != null)
                {
                    pointOfSaleFeeLoad.ClientSubUnitName = clientSubUnit.ClientSubUnitName;
                }
            }

            //TravelerType
            if (pointOfSaleFeeLoad.TravelerTypeGuid != null)
            {
                TravelerTypeRepository TravelerTypeRepository = new TravelerTypeRepository();
                TravelerType TravelerType = TravelerTypeRepository.GetTravelerType(pointOfSaleFeeLoad.TravelerTypeGuid);
                if (TravelerType != null)
                {
                    pointOfSaleFeeLoad.TravelerTypeName = TravelerType.TravelerTypeName;
                }
            }

            //Product
            if (pointOfSaleFeeLoad.ProductId > 0)
            {
                ProductRepository productRepository = new ProductRepository();
                Product product = productRepository.GetProduct(pointOfSaleFeeLoad.ProductId);
                if (product != null)
                {
                    pointOfSaleFeeLoad.Product = product;
                }
            }
        }

        //Delete From DB
        public void Delete(PointOfSaleFeeLoad pointOfSaleFeeLoad)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePointOfSaleFeeLoad_v1(
                pointOfSaleFeeLoad.PointOfSaleFeeLoadId,
                pointOfSaleFeeLoad.ClientTopUnitGuid,
                pointOfSaleFeeLoad.ClientSubUnitGuid,
                pointOfSaleFeeLoad.TravelerTypeGuid,
                pointOfSaleFeeLoad.FeeLoadDescriptionTypeCode,
                pointOfSaleFeeLoad.ProductId,
                pointOfSaleFeeLoad.AgentInitiatedFlag,
                pointOfSaleFeeLoad.TravelIndicator,
                adminUserGuid,
                pointOfSaleFeeLoad.VersionNumber
            );
        }
    }
}