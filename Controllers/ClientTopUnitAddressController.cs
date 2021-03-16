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
    public class ClientTopUnitAddressController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
        ClientDetailClientTopUnitRepository clientDetailClientTopUnitRepository = new ClientDetailClientTopUnitRepository();
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

            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(id);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Populate View Model
            ClientTopUnitAddressesVM clientDetailAddressesVM = new ClientTopUnitAddressesVM();
            clientDetailAddressesVM.Addresses = clientDetailRepository.ListClientDetailAddresses(id, page ?? 1);
            clientDetailAddressesVM.ClientTopUnit = clientTopUnit;
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

            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(id);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;
            

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid))
            {
                ViewData["Access"] = "WriteAccess";
            }

            ClientTopUnitAddressVM clientTopUnitAddressVM = new ClientTopUnitAddressVM();
            clientTopUnitAddressVM.ClientTopUnit = clientTopUnit;
            clientTopUnitAddressVM.ClientDetail = clientDetail;

            Address address = new Address();
            clientTopUnitAddressVM.Address = address;

            CountryRepository countryRepository = new CountryRepository();
            clientTopUnitAddressVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName");

            MappingQualityRepository mappingQualityRepository = new MappingQualityRepository();
            clientTopUnitAddressVM.MappingQualityCodes = new SelectList(mappingQualityRepository.GetAllMappingQualities().ToList(), "MappingQualityCode", "MappingQualityCode", address.MappingQualityCode);

            return View(clientTopUnitAddressVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientTopUnitAddressVM clientTopUnitAddressVM)
        {
            int clientDetailId = clientTopUnitAddressVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;
            

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            //Check Exists
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<Address>(clientTopUnitAddressVM.Address, "Address");
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
                clientDetailAddressRepository.Add(clientTopUnitAddressVM.ClientDetail, clientTopUnitAddressVM.Address);
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
            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;
            

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientTopUnitAddressVM clientTopUnitAddressVM = new ClientTopUnitAddressVM();
            clientTopUnitAddressVM.ClientTopUnit = clientTopUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientTopUnitAddressVM.ClientDetail = clientDetail;

            Address address = new Address();
            address = addressRepository.GetAddress(clientDetailAddress.AddressId);
            clientTopUnitAddressVM.Address = address;

            CountryRepository countryRepository = new CountryRepository();
            clientTopUnitAddressVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", address.CountryCode);

            MappingQualityRepository mappingQualityRepository = new MappingQualityRepository();
            clientTopUnitAddressVM.MappingQualityCodes = new SelectList(mappingQualityRepository.GetAllMappingQualities().ToList(), "MappingQualityCode", "MappingQualityCode", address.MappingQualityCode);

            return View(clientTopUnitAddressVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientTopUnitAddressVM clientTopUnitAddressVM)
        {
            int clientDetailId = clientTopUnitAddressVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;
            

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            //Check Exists
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<Address>(clientTopUnitAddressVM.Address, "Address");
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
                addressRepository.Edit(clientTopUnitAddressVM.Address);
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
            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;
            

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientTopUnitAddressVM clientTopUnitAddressVM = new ClientTopUnitAddressVM();
            clientTopUnitAddressVM.ClientTopUnit = clientTopUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientTopUnitAddressVM.ClientDetail = clientDetail;

            Address address = new Address();
            address = addressRepository.GetAddress(clientDetailAddress.AddressId);
            addressRepository.EditForDisplay(address);
            clientTopUnitAddressVM.Address = address;
            
            return View(clientTopUnitAddressVM);
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
            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;
            

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid))
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
                    ViewData["ReturnURL"] = "/ClientTopUnitAddress.mvc/Delete/" + id.ToString();
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
            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;
            

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            ClientTopUnitAddressVM clientTopUnitAddressVM = new ClientTopUnitAddressVM();
            clientTopUnitAddressVM.ClientTopUnit = clientTopUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientTopUnitAddressVM.ClientDetail = clientDetail;

            Address address = new Address();
            address = addressRepository.GetAddress(clientDetailAddress.AddressId);
            addressRepository.EditForDisplay(address);
            clientTopUnitAddressVM.Address = address;

            return View(clientTopUnitAddressVM);
        }
    }
}
