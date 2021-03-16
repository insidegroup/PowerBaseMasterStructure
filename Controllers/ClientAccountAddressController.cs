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
    public class ClientAccountAddressController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        ClientAccountRepository clientAccountRepository = new ClientAccountRepository();
        ClientDetailClientAccountRepository clientDetailClientAccountRepository = new ClientDetailClientAccountRepository();
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

            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(id);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientAccount(can,ssc))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Populate View Model
            ClientAccountAddressesVM clientDetailAddressesVM = new ClientAccountAddressesVM();
            clientDetailAddressesVM.Addresses = clientDetailRepository.ListClientDetailAddresses(id, page ?? 1);
            clientDetailAddressesVM.ClientAccount = clientAccount;
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

            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(id);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Access"] = "WriteAccess";
            }

            ClientAccountAddressVM clientAccountAddressVM = new ClientAccountAddressVM();
            clientAccountAddressVM.ClientAccount = clientAccount;
            clientAccountAddressVM.ClientDetail = clientDetail;

            Address address = new Address();
            clientAccountAddressVM.Address = address;

            CountryRepository countryRepository = new CountryRepository();
            clientAccountAddressVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName");

            MappingQualityRepository mappingQualityRepository = new MappingQualityRepository();
            clientAccountAddressVM.MappingQualityCodes = new SelectList(mappingQualityRepository.GetAllMappingQualities().ToList(), "MappingQualityCode", "MappingQualityCode", address.MappingQualityCode);

            return View(clientAccountAddressVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientAccountAddressVM clientAccountAddressVM)
        {
            int clientDetailId = clientAccountAddressVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(clientDetailId);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<Address>(clientAccountAddressVM.Address, "Address");
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
                clientDetailAddressRepository.Add(clientAccountAddressVM.ClientDetail, clientAccountAddressVM.Address);
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
            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(clientDetailId);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientAccountAddressVM clientAccountAddressVM = new ClientAccountAddressVM();
            clientAccountAddressVM.ClientAccount = clientAccount;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientAccountAddressVM.ClientDetail = clientDetail;

            Address address = new Address();
            address = addressRepository.GetAddress(clientDetailAddress.AddressId);
            clientAccountAddressVM.Address = address;

            CountryRepository countryRepository = new CountryRepository();
            clientAccountAddressVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", address.CountryCode);

            MappingQualityRepository mappingQualityRepository = new MappingQualityRepository();
            clientAccountAddressVM.MappingQualityCodes = new SelectList(mappingQualityRepository.GetAllMappingQualities().ToList(), "MappingQualityCode", "MappingQualityCode", address.MappingQualityCode);

            return View(clientAccountAddressVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientAccountAddressVM clientAccountAddressVM)
        {
            int clientDetailId = clientAccountAddressVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(clientDetailId);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<Address>(clientAccountAddressVM.Address, "Address");
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
                addressRepository.Edit(clientAccountAddressVM.Address);
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
            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(clientDetailId);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientAccountAddressVM clientAccountAddressVM = new ClientAccountAddressVM();
            clientAccountAddressVM.ClientAccount = clientAccount;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientAccountAddressVM.ClientDetail = clientDetail;

            Address address = new Address();
            address = addressRepository.GetAddress(clientDetailAddress.AddressId);
            addressRepository.EditForDisplay(address);
            clientAccountAddressVM.Address = address;
            
            return View(clientAccountAddressVM);
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
            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(clientDetailId);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
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
                    ViewData["ReturnURL"] = "/ClientAccountAddress.mvc/Delete/" + id.ToString();
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
            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(clientDetailId);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            ClientAccountAddressVM clientAccountAddressVM = new ClientAccountAddressVM();
            clientAccountAddressVM.ClientAccount = clientAccount;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientAccountAddressVM.ClientDetail = clientDetail;

            Address address = new Address();
            address = addressRepository.GetAddress(clientDetailAddress.AddressId);
            addressRepository.EditForDisplay(address);
            clientAccountAddressVM.Address = address;

            return View(clientAccountAddressVM);
        }
    }
}
