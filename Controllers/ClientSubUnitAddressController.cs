using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.Helpers;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class ClientSubUnitAddressController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientDetailClientSubUnitRepository clientDetailClientSubUnitRepository = new ClientDetailClientSubUnitRepository();
        ClientDetailAddressRepository clientDetailAddressRepository = new ClientDetailAddressRepository();
        AddressRepository addressRepository = new AddressRepository();

        // GET: /List
        public ActionResult List(int id, int? page)
        {
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(id);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            string clientSubUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Populate View Model
            ClientSubUnitAddressesVM clientDetailAddressesVM = new ClientSubUnitAddressesVM();
            clientDetailAddressesVM.Addresses = clientDetailRepository.ListClientDetailAddresses(id, page ?? 1);
            clientDetailAddressesVM.ClientSubUnit = clientSubUnit;
            clientDetailAddressesVM.ClientDetail = clientDetail;

            return View(clientDetailAddressesVM);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(id);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            string clientSubUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Access"] = "WriteAccess";
            }

            ClientSubUnitAddressVM clientSubUnitAddressVM = new ClientSubUnitAddressVM();
            clientSubUnitAddressVM.ClientSubUnit = clientSubUnit;
            clientSubUnitAddressVM.ClientDetail = clientDetail;

            Address address = new Address();
            clientSubUnitAddressVM.Address = address;

            CountryRepository countryRepository = new CountryRepository();
            clientSubUnitAddressVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName");

            MappingQualityRepository mappingQualityRepository = new MappingQualityRepository();
            clientSubUnitAddressVM.MappingQualityCodes = new SelectList(mappingQualityRepository.GetAllMappingQualities().ToList(), "MappingQualityCode", "MappingQualityCode", address.MappingQualityCode);

            return View(clientSubUnitAddressVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientSubUnitAddressVM clientSubUnitAddressVM)
        {
            int clientDetailId = clientSubUnitAddressVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            string clientSubUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<Address>(clientSubUnitAddressVM.Address, "Address");
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

            

            try
            {
                clientDetailAddressRepository.Add(clientSubUnitAddressVM.ClientDetail, clientSubUnitAddressVM.Address);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = clientDetailId });
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            ClientDetailAddress clientDetailAddress = new ClientDetailAddress();
            clientDetailAddress = clientDetailAddressRepository.GetAddressClientDetail(id);
            
            //Check Exists
            if (clientDetailAddress == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            int clientDetailId = clientDetailAddress.ClientDetailId;
            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            string clientSubUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitAddressVM clientSubUnitAddressVM = new ClientSubUnitAddressVM();
            clientSubUnitAddressVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientSubUnitAddressVM.ClientDetail = clientDetail;

            Address address = new Address();
            address = addressRepository.GetAddress(clientDetailAddress.AddressId);
            clientSubUnitAddressVM.Address = address;

            CountryRepository countryRepository = new CountryRepository();
            clientSubUnitAddressVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", address.CountryCode);

            MappingQualityRepository mappingQualityRepository = new MappingQualityRepository();
            clientSubUnitAddressVM.MappingQualityCodes = new SelectList(mappingQualityRepository.GetAllMappingQualities().ToList(), "MappingQualityCode", "MappingQualityCode", address.MappingQualityCode);

            return View(clientSubUnitAddressVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientSubUnitAddressVM clientSubUnitAddressVM)
        {
            int clientDetailId = clientSubUnitAddressVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            string clientSubUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<Address>(clientSubUnitAddressVM.Address, "Address");
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



            try
            {
                addressRepository.Edit(clientSubUnitAddressVM.Address);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = clientDetailId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            ClientDetailAddress clientDetailAddress = new ClientDetailAddress();
            clientDetailAddress = clientDetailAddressRepository.GetAddressClientDetail(id);

            //Check Exists
            if (clientDetailAddress == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            int clientDetailId = clientDetailAddress.ClientDetailId;
            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            string clientSubUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitAddressVM clientSubUnitAddressVM = new ClientSubUnitAddressVM();
            clientSubUnitAddressVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientSubUnitAddressVM.ClientDetail = clientDetail;

            Address address = new Address();
            address = addressRepository.GetAddress(clientDetailAddress.AddressId);
            addressRepository.EditForDisplay(address);
            clientSubUnitAddressVM.Address = address;
            
            return View(clientSubUnitAddressVM);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            ClientDetailAddress clientDetailAddress = new ClientDetailAddress();
            clientDetailAddress = clientDetailAddressRepository.GetAddressClientDetail(id);

            //Check Exists
            if (clientDetailAddress == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            int clientDetailId = clientDetailAddress.ClientDetailId;
            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            string clientSubUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            Address address = new Address();
            address = addressRepository.GetAddress(id);

            //Delete Item
            try
            {
                address.VersionNumber = Int32.Parse(collection["Address.VersionNumber"]);
                addressRepository.Delete(address);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientSubUnitAddress.mvc/Delete/" + id.ToString();
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = clientDetailId });
        }

        // GET: /View
        public ActionResult View(int id)
        {
            ClientDetailAddress clientDetailAddress = new ClientDetailAddress();
            clientDetailAddress = clientDetailAddressRepository.GetAddressClientDetail(id);

            //Check Exists
            if (clientDetailAddress == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            int clientDetailId = clientDetailAddress.ClientDetailId;
            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            string clientSubUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            ClientSubUnitAddressVM clientSubUnitAddressVM = new ClientSubUnitAddressVM();
            clientSubUnitAddressVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientSubUnitAddressVM.ClientDetail = clientDetail;

            Address address = new Address();
            address = addressRepository.GetAddress(clientDetailAddress.AddressId);
            addressRepository.EditForDisplay(address);
            clientSubUnitAddressVM.Address = address;

            return View(clientSubUnitAddressVM);
        }
    }
}
