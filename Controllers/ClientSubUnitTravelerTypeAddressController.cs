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
    public class ClientSubUnitTravelerTypeAddressController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientDetailClientSubUnitTravelerTypeRepository clientDetailClientSubUnitTravelerTypeRepository = new ClientDetailClientSubUnitTravelerTypeRepository();
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
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(id);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Populate View Model
            ClientSubUnitTravelerTypeAddressesVM clientSubUnitTravelerTypeAddressesVM = new ClientSubUnitTravelerTypeAddressesVM();
            clientSubUnitTravelerTypeAddressesVM.Addresses = clientDetailRepository.ListClientDetailAddresses(id, page ?? 1);
            clientSubUnitTravelerTypeAddressesVM.ClientSubUnit = clientSubUnit;      
            clientSubUnitTravelerTypeAddressesVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeAddressesVM.TravelerType = travelerType;

            return View(clientSubUnitTravelerTypeAddressesVM);
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

            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(id);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;
            
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Access"] = "WriteAccess";
            }

            ClientSubUnitTravelerTypeAddressVM clientSubUnitTravelerTypeAddressVM = new ClientSubUnitTravelerTypeAddressVM();
            clientSubUnitTravelerTypeAddressVM.ClientSubUnit = clientSubUnit;
            clientSubUnitTravelerTypeAddressVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeAddressVM.TravelerType = travelerType;

            Address address = new Address();
            clientSubUnitTravelerTypeAddressVM.Address = address;

            CountryRepository countryRepository = new CountryRepository();
            clientSubUnitTravelerTypeAddressVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName");

            MappingQualityRepository mappingQualityRepository = new MappingQualityRepository();
            clientSubUnitTravelerTypeAddressVM.MappingQualityCodes = new SelectList(mappingQualityRepository.GetAllMappingQualities().ToList(), "MappingQualityCode", "MappingQualityCode", address.MappingQualityCode);

            return View(clientSubUnitTravelerTypeAddressVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientSubUnitTravelerTypeAddressVM clientSubUnitTravelerTypeAddressVM)
        {
            int clientDetailId = clientSubUnitTravelerTypeAddressVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<Address>(clientSubUnitTravelerTypeAddressVM.Address, "Address");
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
                clientDetailAddressRepository.Add(clientSubUnitTravelerTypeAddressVM.ClientDetail, clientSubUnitTravelerTypeAddressVM.Address);
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
            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitTravelerTypeAddressVM clientSubUnitTravelerTypeAddressVM = new ClientSubUnitTravelerTypeAddressVM();
            clientSubUnitTravelerTypeAddressVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientSubUnitTravelerTypeAddressVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeAddressVM.TravelerType = travelerType;

            Address address = new Address();
            address = addressRepository.GetAddress(clientDetailAddress.AddressId);
            clientSubUnitTravelerTypeAddressVM.Address = address;

            CountryRepository countryRepository = new CountryRepository();
            clientSubUnitTravelerTypeAddressVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", address.CountryCode);

            MappingQualityRepository mappingQualityRepository = new MappingQualityRepository();
            clientSubUnitTravelerTypeAddressVM.MappingQualityCodes = new SelectList(mappingQualityRepository.GetAllMappingQualities().ToList(), "MappingQualityCode", "MappingQualityCode", address.MappingQualityCode);

            return View(clientSubUnitTravelerTypeAddressVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientSubUnitTravelerTypeAddressVM clientSubUnitTravelerTypeAddressVM)
        {
            int clientDetailId = clientSubUnitTravelerTypeAddressVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<Address>(clientSubUnitTravelerTypeAddressVM.Address, "Address");
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
                addressRepository.Edit(clientSubUnitTravelerTypeAddressVM.Address);
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
            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;
            
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitTravelerTypeAddressVM clientSubUnitTravelerTypeAddressVM = new ClientSubUnitTravelerTypeAddressVM();
            clientSubUnitTravelerTypeAddressVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientSubUnitTravelerTypeAddressVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeAddressVM.TravelerType = travelerType;


            Address address = new Address();
            address = addressRepository.GetAddress(clientDetailAddress.AddressId);
            addressRepository.EditForDisplay(address);
            clientSubUnitTravelerTypeAddressVM.Address = address;
            
            return View(clientSubUnitTravelerTypeAddressVM);
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
            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
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
            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            ClientSubUnitTravelerTypeAddressVM clientSubUnitTravelerTypeAddressVM = new ClientSubUnitTravelerTypeAddressVM();
            clientSubUnitTravelerTypeAddressVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientSubUnitTravelerTypeAddressVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeAddressVM.TravelerType = travelerType;

            Address address = new Address();
            address = addressRepository.GetAddress(clientDetailAddress.AddressId);
            addressRepository.EditForDisplay(address);
            clientSubUnitTravelerTypeAddressVM.Address = address;

            return View(clientSubUnitTravelerTypeAddressVM);
        }
    }
}
