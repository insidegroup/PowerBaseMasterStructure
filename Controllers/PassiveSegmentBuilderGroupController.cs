using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class PassiveSegmentBuilderGroupController : Controller
    {
        //main repository
        ProductGroupRepository productGroupRepository = new ProductGroupRepository();
        ProductRepository productRepository = new ProductRepository();
        ProductGroupSubProductRepository productGroupSubProductRepository = new ProductGroupSubProductRepository();
        PassiveSegmentBuilderGroupRepository passiveSegmentBuilderGroupRepository = new PassiveSegmentBuilderGroupRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        private string groupName = "Passive Segment Builder";
        private int productGroupDomainTypeId = 1;

        // GET: /ListUnDeleted
        public ActionResult ListUnDeleted(string filter, int? page, string sortField, int? sortOrder)
        {
            //SortField
            if (sortField != "HierarchyType" && sortField != "HierarchyItem")
            {
                sortField = "ProductGroupName";
            }
            ViewData["CurrentSortField"] = sortField;

            //SortOrder
            if (sortOrder == 1)
            {
                ViewData["NewSortOrder"] = 0;
                ViewData["CurrentSortOrder"] = 1;
            }
            else
            {
                ViewData["NewSortOrder"] = 1;
                ViewData["CurrentSortOrder"] = 0;
                sortOrder = 0;
            }

            ProductGroupsVM productGroupsVM = new ProductGroupsVM();

            //Set Access Rights
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                productGroupsVM.HasDomainWriteAccess = true;
            }

			if (productGroupRepository != null)
			{
				var productGroups = productGroupRepository.PageProductGroups(false, productGroupDomainTypeId, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

				if (productGroups != null)
				{
					productGroupsVM.ProductGroups = productGroups;
				}
			}

			//return items
			return View(productGroupsVM);     
        }

        // GET: /ListDeleted
        public ActionResult ListDeleted(string filter, int? page, string sortField, int? sortOrder)
        {
            //SortField
            if (sortField != "HierarchyType" && sortField != "HierarchyItem")
            {
                sortField = "ProductGroupName";
            }
            ViewData["CurrentSortField"] = sortField;

            //SortOrder
            if (sortOrder == 1)
            {
                ViewData["NewSortOrder"] = 0;
                ViewData["CurrentSortOrder"] = 1;
            }
            else
            {
                ViewData["NewSortOrder"] = 1;
                ViewData["CurrentSortOrder"] = 0;
                sortOrder = 0;
            }

            ProductGroupsVM productGroupsVM = new ProductGroupsVM();

            //Set Access Rights
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                productGroupsVM.HasDomainWriteAccess = true;
            }

            //return items
            productGroupsVM.ProductGroups = productGroupRepository.PageProductGroups(true, productGroupDomainTypeId, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(productGroupsVM);     
        }

          // GET: /ListOrphaned
        public ActionResult ListOrphaned(string filter, int? page, string sortField, int? sortOrder)
        {
            //SortField
            sortField = "ProductGroupName";
            ViewData["CurrentSortField"] = sortField;

            //SortOrder
            if (sortOrder == 1)
            {
                ViewData["NewSortOrder"] = 0;
                ViewData["CurrentSortOrder"] = 1;
            }
            else
            {
                ViewData["NewSortOrder"] = 1;
                ViewData["CurrentSortOrder"] = 0;
            }

           ProductGroupsVM productGroupsVM = new ProductGroupsVM();

            //Set Access Rights
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                productGroupsVM.HasDomainWriteAccess = true;
            }

            //return items
            productGroupsVM.ProductGroupsOrphaned = productGroupRepository.PageOrphanedPolicyGroups(page ?? 1, productGroupDomainTypeId, filter ?? "", sortField, sortOrder ?? 0);
            return View(productGroupsVM);   
        }
        
        
        // GET: /View
        public ActionResult View(int id)
        {
            //Get Item From Database
            ProductGroup productGroup = new ProductGroup();
            productGroup = passiveSegmentBuilderGroupRepository.GetGroup(id);

            //Check Exists
            if (productGroup == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            PassiveSegmentBuilderGroupVM productGroupVM = new PassiveSegmentBuilderGroupVM();

            productGroupRepository.EditGroupForDisplay(productGroup);
            productGroupVM.ProductGroup = productGroup;

            List<SelectListItem> products = new List<SelectListItem>();
            products = productRepository.GetPassiveSegmentProducts(id);
            productGroupVM.Products = products;

            List<SelectListItem> subProducts = new List<SelectListItem>();
            subProducts = productGroupSubProductRepository.GetProductGroupSubProducts(id);
            productGroupVM.SubProducts = subProducts;

            return View(productGroupVM);
        }
        
        // GET: /Create
        public ActionResult Create()
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            PassiveSegmentBuilderGroupVM productGroupVM = new PassiveSegmentBuilderGroupVM();
            ProductGroup productGroup = new ProductGroup();

            productGroup.EnabledFlagNonNullable = true;
            productGroup.InheritFromParentFlag = true;
            productGroupVM.ProductGroup = productGroup;

            TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
            SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
            productGroupVM.HierarchyTypes = hierarchyTypesList;

            List<SelectListItem> products = new List<SelectListItem>();
            products = productRepository.GetPassiveSegmentProducts(null);
            productGroupVM.Products = products;

            List<SelectListItem> subProducts = new List<SelectListItem>();
            productGroupVM.SubProducts = subProducts;

            List<SelectListItem> availableSubProducts = new List<SelectListItem>();
            availableSubProducts = productGroupSubProductRepository.GetProductGroupAvailableSubProducts(null);
            productGroupVM.AvailableSubProducts = availableSubProducts;

            return View(productGroupVM);
        }

       
        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PassiveSegmentBuilderGroupVM passiveSegmentBuilderGroupVM)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //We need to extract group
            ProductGroup productGroup = new ProductGroup();
            productGroup = passiveSegmentBuilderGroupVM.ProductGroup;
            if (productGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Check Access Rights to Domain Hierarchy
            if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(productGroup.HierarchyType, productGroup.HierarchyCode, productGroup.SourceSystemCode, groupName))
            {
                ViewData["Message"] = "You cannot add to this hierarchy item";
                return View("Error");
            }

           //Update Model From Form + Validate against DB
	        try
            {
                UpdateModel<ProductGroup>(productGroup, "ProductGroup");
            }
	        catch
            {
                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        n += error.ErrorMessage;
                    }
                }
                ViewData["Message"] = "ValidationError : " + n;
                return View("Error");
            }


            //Database Update
            try
            {
                passiveSegmentBuilderGroupRepository.Add(passiveSegmentBuilderGroupVM);
            }
            catch (SqlException ex)
            {
                //Non-Unique Name
                if (ex.Message == "NonUniqueName")
                {
                    return View("NonUniqueNameError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            
            }

            return RedirectToAction("ListUnDeleted");
        }
        
       // GET: /Edit
       public ActionResult Edit(int id)
       {
           //Get Item From Database
           ProductGroup productGroup = new ProductGroup();
           productGroup = passiveSegmentBuilderGroupRepository.GetGroup(id);

           //Check Exists
           if (productGroup == null)
           {
               ViewData["ActionMethod"] = "EditGet";
               return View("RecordDoesNotExistError");
           }
           //Check Access
           RolesRepository rolesRepository = new RolesRepository();
           if (!rolesRepository.HasWriteAccessToProductGroup(id))
           {
               ViewData["Message"] = "You do not have access to this item";
               return View("Error");
           }

           PassiveSegmentBuilderGroupVM productGroupVM = new PassiveSegmentBuilderGroupVM();

           productGroupRepository.EditGroupForDisplay(productGroup);
           productGroupVM.ProductGroup = productGroup;
           
           TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
           SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
           productGroupVM.HierarchyTypes = hierarchyTypesList;

           List<SelectListItem> products = new List<SelectListItem>();
           products = productRepository.GetPassiveSegmentProducts(id);
           productGroupVM.Products = products;

           List<SelectListItem> subProducts = new List<SelectListItem>();
           subProducts = productGroupSubProductRepository.GetProductGroupSubProducts(id);
           productGroupVM.SubProducts = subProducts;

           List<SelectListItem> availableSubProducts = new List<SelectListItem>();
           availableSubProducts = productGroupSubProductRepository.GetProductGroupAvailableSubProducts(id);
           productGroupVM.AvailableSubProducts = availableSubProducts;

           return View(productGroupVM);
       }
      
      // POST: /Edit
      [HttpPost]
      [ValidateAntiForgeryToken]
       public ActionResult Edit(PassiveSegmentBuilderGroupVM passiveSegmentBuilderGroupVM)
      {
          //Get Item From Database
          ProductGroup productGroup = new ProductGroup();
          productGroup = passiveSegmentBuilderGroupRepository.GetGroup(passiveSegmentBuilderGroupVM.ProductGroup.ProductGroupId);

          //Check Exists
          if (productGroup == null)
          {
              ViewData["ActionMethod"] = "DeleteGet";
              return View("RecordDoesNotExistError");
          }
          //Check Access
          RolesRepository rolesRepository = new RolesRepository();
          if (!rolesRepository.HasWriteAccessToProductGroup(productGroup.ProductGroupId))
          {
              ViewData["Message"] = "You do not have access to this item";
              return View("Error");
          }
         //Update Model From Form + Validate against DB
          try
          {
              UpdateModel<ProductGroup>(productGroup, "ProductGroup");
          }
          catch
          {
              string n = "";
              foreach (ModelState modelState in ViewData.ModelState.Values)
              {
                  foreach (ModelError error in modelState.Errors)
                  {
                      n += error.ErrorMessage;
                  }
              }
              ViewData["Message"] = "ValidationError : " + n;
              return View("Error");
          }

           

          //Database Update
          try
          {
              passiveSegmentBuilderGroupRepository.Edit(passiveSegmentBuilderGroupVM);
          }
          catch (SqlException ex)
          {
              //Versioning Error
              if (ex.Message == "SQLVersioningError")
              {
                  ViewData["ReturnURL"] = "/ProductGroup.mvc/Edit/" + productGroup.ProductGroupId;
                  return View("VersionError");
              }
              LogRepository logRepository = new LogRepository();
              logRepository.LogError(ex.Message);

              ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
              return View("Error");            
          }
          return RedirectToAction("ListUnDeleted");
        }
       
       // GET: /Delete
	  [HttpGet]
	  public ActionResult Delete(int id)
       {
           //Get Item From Database
           ProductGroup productGroup = new ProductGroup();
           productGroup = passiveSegmentBuilderGroupRepository.GetGroup(id);

           //Check Exists
           if (productGroup == null || productGroup.DeletedFlag == true)
           {
               ViewData["ActionMethod"] = "DeleteGet";
               return View("RecordDoesNotExistError");
           }
           //Check Access
           RolesRepository rolesRepository = new RolesRepository();
           if (!rolesRepository.HasWriteAccessToProductGroup(id))
           {
               ViewData["Message"] = "You do not have access to this item";
               return View("Error");
           }

           PassiveSegmentBuilderGroupVM productGroupVM = new PassiveSegmentBuilderGroupVM();

           productGroupRepository.EditGroupForDisplay(productGroup);
           productGroupVM.ProductGroup = productGroup;

           List<SelectListItem> products = new List<SelectListItem>();
           products = productRepository.GetPassiveSegmentProducts(id);
           productGroupVM.Products = products;

           List<SelectListItem> subProducts = new List<SelectListItem>();
           subProducts = productGroupSubProductRepository.GetProductGroupSubProducts(id);
           productGroupVM.SubProducts = subProducts;

           return View(productGroupVM);
       }
        
       // POST: /Delete/5
       [HttpPost]
       [ValidateAntiForgeryToken]
       public ActionResult Delete(PassiveSegmentBuilderGroupVM passiveSegmentBuilderGroupVM)
       {
           //Check Valid Item passed in Form       
           if (passiveSegmentBuilderGroupVM.ProductGroup == null)
           {
               ViewData["ActionMethod"] = "DeletePost";
               return View("RecordDoesNotExistError");
           }

           //Get Item From Database
           ProductGroup productGroup = new ProductGroup();
           productGroup = passiveSegmentBuilderGroupRepository.GetGroup(passiveSegmentBuilderGroupVM.ProductGroup.ProductGroupId);

           //Check Exists in Databsase
           if (productGroup == null || productGroup.DeletedFlag == true)
           {
               ViewData["ActionMethod"] = "DeletePost";
               return View("RecordDoesNotExistError");
           }
           //Check Access
           RolesRepository rolesRepository = new RolesRepository();
           if (!rolesRepository.HasWriteAccessToProductGroup(productGroup.ProductGroupId))
           {
               ViewData["Message"] = "You do not have access to this item";
               return View("Error");
           }
           //Delete Form Item
           try
           {
               passiveSegmentBuilderGroupVM.ProductGroup.DeletedFlag = true;
               productGroupRepository.UpdateGroupDeletedStatus(passiveSegmentBuilderGroupVM.ProductGroup);
           }
           catch (SqlException ex)
           {
               //Versioning Error - go to standard versionError page
               if (ex.Message == "SQLVersioningError")
               {
                   ViewData["ReturnURL"] = "/ProductGroup.mvc/Delete/" + productGroup.ProductGroupId;
                   return View("VersionError");
               }

               LogRepository logRepository = new LogRepository();
               logRepository.LogError(ex.Message);

               ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
               return View("Error");            
           }
           return RedirectToAction("ListUnDeleted");
       }

      
       // GET: /UnDelete
       public ActionResult UnDelete(int id)
       {
           //Get Item From Database
           ProductGroup productGroup = new ProductGroup();
           productGroup = passiveSegmentBuilderGroupRepository.GetGroup(id);

           //Check Exists
           if (productGroup == null || productGroup.DeletedFlag == false)
           {
               ViewData["ActionMethod"] = "DeleteGet";
               return View("RecordDoesNotExistError");
           }
           //Check Access
           RolesRepository rolesRepository = new RolesRepository();
           if (!rolesRepository.HasWriteAccessToProductGroup(id))
           {
               ViewData["Message"] = "You do not have access to this item";
               return View("Error");
           }

           PassiveSegmentBuilderGroupVM productGroupVM = new PassiveSegmentBuilderGroupVM();

           productGroupRepository.EditGroupForDisplay(productGroup);
           productGroupVM.ProductGroup = productGroup;

           List<SelectListItem> products = new List<SelectListItem>();
           products = productRepository.GetPassiveSegmentProducts(id);
           productGroupVM.Products = products;

           List<SelectListItem> subProducts = new List<SelectListItem>();
           subProducts = productGroupSubProductRepository.GetProductGroupSubProducts(id);
           productGroupVM.SubProducts = subProducts;

           return View(productGroupVM);
       }

    
       // POST: /UnDelete/5
       [HttpPost]
       [ValidateAntiForgeryToken]
       public ActionResult UnDelete(PassiveSegmentBuilderGroupVM passiveSegmentBuilderGroupVM)
       {
           //Check Valid Item passed in Form       
           if (passiveSegmentBuilderGroupVM.ProductGroup == null)
           {
               ViewData["ActionMethod"] = "DeletePost";
               return View("RecordDoesNotExistError");
           }

           //Get Item From Database
           ProductGroup productGroup = new ProductGroup();
           productGroup = passiveSegmentBuilderGroupRepository.GetGroup(passiveSegmentBuilderGroupVM.ProductGroup.ProductGroupId);

           //Check Exists in Databsase
           if (productGroup == null || productGroup.DeletedFlag == false)
           {
               ViewData["ActionMethod"] = "DeletePost";
               return View("RecordDoesNotExistError");
           }
           //Check Access
           RolesRepository rolesRepository = new RolesRepository();
           if (!rolesRepository.HasWriteAccessToProductGroup(productGroup.ProductGroupId))
           {
               ViewData["Message"] = "You do not have access to this item";
               return View("Error");
           }
           //Delete Form Item
           try
           {
               passiveSegmentBuilderGroupVM.ProductGroup.DeletedFlag = false;
               productGroupRepository.UpdateGroupDeletedStatus(passiveSegmentBuilderGroupVM.ProductGroup);
           }
           catch (SqlException ex)
           {
               //Versioning Error - go to standard versionError page
               if (ex.Message == "SQLVersioningError")
               {
                   ViewData["ReturnURL"] = "/ProductGroup.mvc/UnDelete/" + productGroup.ProductGroupId;
                   return View("VersionError");
               }

               LogRepository logRepository = new LogRepository();
               logRepository.LogError(ex.Message);

               ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
               return View("Error");            
           }
           return RedirectToAction("ListUnDeleted");
       }

        
    }
}
