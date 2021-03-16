using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;


namespace CWTDesktopDatabase.Controllers
{
    public class PolicySupplierDealCodeController : Controller
    {
        //main repositories
        PolicySupplierDealCodeRepository policySupplierDealCodeRepository = new PolicySupplierDealCodeRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();

        // GET: /ClientDetailAddress/
        public ActionResult List(string filter, int id, int? page, string sortField, int? sortOrder)
        {

            //Check Exists
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id);
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent Information
            ViewData["PolicyGroupID"] = id;
            ViewData["PolicyGroupName"] = policyGroupRepository.GetGroup(id).PolicyGroupName;


            //SortField
            if (sortField != "PolicySupplierDealCodeTypeDescription" && sortField != "GDSName" && sortField != "SupplierName" && sortField != "PolicyLocationName")
            {
                sortField = "PolicySupplierDealCodeValue";
            }
            ViewData["CurrentSortField"] = sortField;

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

            var items = policySupplierDealCodeRepository.PagePolicySupplierDealCodes(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(items);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {

            PolicyGroup policyGroup = policyGroupRepository.GetGroup(id);

            //Check Exists
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
           
			//Populate List of GDSs
            GDSRepository gdsRepository = new GDSRepository();
            SelectList gdss = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");
            ViewData["GDSList"] = gdss;

            //Populate List of PolicySupplierDealCodeTypes
            PolicySupplierDealCodeTypeRepository policySupplierDealCodeType = new PolicySupplierDealCodeTypeRepository();
            SelectList policySupplierDealCodeTypes = new SelectList(policySupplierDealCodeType.GetAllPolicySupplierDealCodeTypes().ToList(), "PolicySupplierDealCodeTypeId", "PolicySupplierDealCodeTypeDescription");
            ViewData["PolicySupplierDealCodeTypeList"] = policySupplierDealCodeTypes;

            //Populate List of Products
            ProductRepository productRepository = new ProductRepository();
            SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
            ViewData["ProductList"] = products;

			//Populate List of Policy Locations
			PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
			SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
			ViewData["PolicyLocationList"] = policyLocations;

			//Populate List of Travel Indicators
			TravelIndicatorRepository travelIndicatorRepository = new TravelIndicatorRepository();
			SelectList travelIndicators = new SelectList(travelIndicatorRepository.GetAllTravelIndicators().ToList(), "TravelIndicator1", "TravelIndicatorDescription");
			ViewData["TravelIndicatorList"] = travelIndicators;

			//Populate Blank List of Tour Code Types (Chosen with GDS Selection)
			TourCodeTypeRepository tourCodeTypeRepository = new TourCodeTypeRepository();
			List<TourCodeType> defaultList = new List<TourCodeType>();
			SelectList tourCodeTypes = new SelectList(defaultList, "TourCodeTypeId", "TourCodeTypeDescription");
			ViewData["TourCodeTypeList"] = tourCodeTypes;

            //populate new item with known PolicyGroup Information           
            PolicySupplierDealCode policySupplierDealCode = new PolicySupplierDealCode(); 
            policySupplierDealCode.PolicyGroupId = id;
            policySupplierDealCode.PolicyGroupName = policyGroup.PolicyGroupName;
            policySupplierDealCode.EnabledFlagNonNullable = true;

            //Show 'Create' Form
            return View(policySupplierDealCode);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Create(PolicySupplierDealCode policySupplierDealCode, FormCollection formCollection)
        {

			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policySupplierDealCode.PolicyGroupId);

            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policySupplierDealCode.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

			//Create PolicySupplierDealCodeOSIs from Post values
			System.Data.Linq.EntitySet<PolicySupplierDealCodeOSI> PolicySupplierDealCodeOSIs = new System.Data.Linq.EntitySet<PolicySupplierDealCodeOSI>();
			foreach (string key in formCollection)
			{

				if (key.StartsWith("PolicySupplierDealCodeOSI") && !string.IsNullOrEmpty(formCollection[key]))
				{

					PolicySupplierDealCodeOSI policySupplierDealCodeOSI = new PolicySupplierDealCodeOSI()
					{
						PolicySupplierDealCodeOSIDescription = formCollection[key],
						PolicySupplierDealCodeOSISequenceNumber = int.Parse(key.Replace("PolicySupplierDealCodeOSI_", ""))
					};

					PolicySupplierDealCodeOSIs.Add(policySupplierDealCodeOSI);
				}
			}

			if (PolicySupplierDealCodeOSIs != null && PolicySupplierDealCodeOSIs.Count > 0)
			{
				policySupplierDealCode.PolicySupplierDealCodeOSIs = PolicySupplierDealCodeOSIs;
			}
			
			//Update Model from Form
			try
			{
				UpdateModel(policySupplierDealCode, "PolicySupplierDealCode");
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

			//Update Database
            try
            {
                policySupplierDealCodeRepository.Add(policySupplierDealCode);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("List", new { id = policySupplierDealCode.PolicyGroupId });
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Get PolicySupplierDealCode
            PolicySupplierDealCode policySupplierDealCode = new PolicySupplierDealCode();
            policySupplierDealCode = policySupplierDealCodeRepository.GetPolicySupplierDealCode(id);

            //Check Exists
            if (policySupplierDealCode == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policySupplierDealCode.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Populate List of GDSs
            GDSRepository gdsRepository = new GDSRepository();
            SelectList gdss = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");
            ViewData["GDSList"] = gdss;

            //Populate List of Products
            ProductRepository productRepository = new ProductRepository();
            SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
            ViewData["ProductList"] = products;

            //Populate List of PolicySupplierDealCodeTypes
            PolicySupplierDealCodeTypeRepository policySupplierDealCodeType = new PolicySupplierDealCodeTypeRepository();
            SelectList policySupplierDealCodeTypes = new SelectList(policySupplierDealCodeType.GetAllPolicySupplierDealCodeTypes().ToList(), "PolicySupplierDealCodeTypeId", "PolicySupplierDealCodeTypeDescription");
            ViewData["PolicySupplierDealCodeTypeList"] = policySupplierDealCodeTypes;

            //Populate List of Policy Locations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
            ViewData["PolicyLocationList"] = policyLocations;

			//Populate List of Travel Indicators
			TravelIndicatorRepository travelIndicatorRepository = new TravelIndicatorRepository();
			SelectList travelIndicators = new SelectList(travelIndicatorRepository.GetAllTravelIndicators().ToList(), "TravelIndicator1", "TravelIndicatorDescription", policySupplierDealCode.TravelIndicator);
			ViewData["TravelIndicatorList"] = travelIndicators;

			//Populate List of Tour Code Types
			TourCodeTypeRepository tourCodeTypeRepository = new TourCodeTypeRepository();
			SelectList tourCodeTypes = new SelectList(tourCodeTypeRepository.GetTourCodeTypesForGDS(policySupplierDealCode.GDSCode).ToList(), "TourCodeTypeId", "TourCodeTypeDescription", policySupplierDealCode.TourCodeTypeId);
			ViewData["TourCodeTypeList"] = tourCodeTypes;
			
			//return edit form
            policySupplierDealCodeRepository.EditItemForDisplay(policySupplierDealCode);
            return View(policySupplierDealCode);
        }

        // POST /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Edit(PolicySupplierDealCode policySupplierDealCode, FormCollection formCollection)
        {
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policySupplierDealCode.PolicyGroupId);

            //Check Exists
			if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policySupplierDealCode.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

			//Create PolicySupplierDealCodeOSIs from Post values
			System.Data.Linq.EntitySet<PolicySupplierDealCodeOSI> PolicySupplierDealCodeOSIs = new System.Data.Linq.EntitySet<PolicySupplierDealCodeOSI>();

			foreach (string key in formCollection)
			{

				if (key.StartsWith("PolicySupplierDealCodeOSI") && !string.IsNullOrEmpty(formCollection[key]))
				{

					PolicySupplierDealCodeOSI policySupplierDealCodeOSI = new PolicySupplierDealCodeOSI()
					{
						PolicySupplierDealCodeOSIDescription = formCollection[key],
						PolicySupplierDealCodeOSISequenceNumber = int.Parse(key.Replace("PolicySupplierDealCodeOSI_", ""))
					};

					PolicySupplierDealCodeOSIs.Add(policySupplierDealCodeOSI);
				}
			}

			if (PolicySupplierDealCodeOSIs != null && PolicySupplierDealCodeOSIs.Count > 0)
			{
				policySupplierDealCode.PolicySupplierDealCodeOSIs = PolicySupplierDealCodeOSIs;
			}

			//Update PolicySupplierDealCode Model From Form
			try
			{
				UpdateModel(policySupplierDealCode, "PolicySupplierDealCode");
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
                policySupplierDealCodeRepository.Update(policySupplierDealCode);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicySupplierDealCode.mvc/Edit/" + policySupplierDealCode.PolicySupplierDealCodeId;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            //Return to Form
            return RedirectToAction("List", new { id = policySupplierDealCode.PolicyGroupId });

        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get PolicySupplierDealCode
            PolicySupplierDealCode policySupplierDealCode = new PolicySupplierDealCode();
            policySupplierDealCode = policySupplierDealCodeRepository.GetPolicySupplierDealCode(id);

            //Check Exists
            if (policySupplierDealCode == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policySupplierDealCode.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            
            //Populate new PolicyCarVendorGroupItem with known PolicyGroup Information
            policySupplierDealCodeRepository.EditItemForDisplay(policySupplierDealCode);

            //Show 'Create' Form
            return View(policySupplierDealCode);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            PolicySupplierDealCode policySupplierDealCode = new PolicySupplierDealCode();
            policySupplierDealCode = policySupplierDealCodeRepository.GetPolicySupplierDealCode(id);

            //Check Exists
            if (policySupplierDealCode == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policySupplierDealCode.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                policySupplierDealCode.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policySupplierDealCodeRepository.Delete(policySupplierDealCode);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicySupplierDealCode.mvc/Delete/" + policySupplierDealCode.PolicySupplierDealCodeTypeId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = policySupplierDealCode.PolicyGroupId });
        }

		public ActionResult View(int id)
		{
			//Get PolicySupplierDealCode
			PolicySupplierDealCode policySupplierDealCode = new PolicySupplierDealCode();
			policySupplierDealCode = policySupplierDealCodeRepository.GetPolicySupplierDealCode(id);

			//Check Exists
			if (policySupplierDealCode == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			//Populate new PolicyCarVendorGroupItem with known PolicyGroup Information
			policySupplierDealCodeRepository.EditItemForDisplay(policySupplierDealCode);

			return View(policySupplierDealCode);
		}

		//Update Select List
		[HttpPost]
		public JsonResult GetTourCodeTypes(string gdsCode)
		{
			TourCodeTypeRepository tourCodeTypeRepository = new TourCodeTypeRepository();
			var result = tourCodeTypeRepository.GetTourCodeTypesForGDS(gdsCode);
			return Json(result);
		}

		// GET: /Export
		public ActionResult Export(int id)
		{
			//Check Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(id);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "ExportGet";
				return View("RecordDoesNotExistError");
			}

			//Get CSV Data
			byte[] csvData = policySupplierDealCodeRepository.Export(id);
			return File(csvData, "text/csv", "Policy Supplier Deal Code Items Export.csv");
		}
    }
}
