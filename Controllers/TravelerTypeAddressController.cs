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
    public class TravelerTypeAddressController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientSubUnitTravelerTypeRepository clientSubUnitTravelerTypeRepository = new ClientSubUnitTravelerTypeRepository();
        ClientDetailTravelerTypeRepository clientDetailTravelerTypeRepository = new ClientDetailTravelerTypeRepository();
        ClientDetailAddressRepository clientDetailAddressRepository = new ClientDetailAddressRepository();
        AddressRepository addressRepository = new AddressRepository();

        // GET: /List
        public ActionResult List(string csu, string tt, int id, int? page)
        {
            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(id);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }

            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu,tt);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Populate View Model
            TravelerTypeAddressesVM travelerTypeAddressesVM = new TravelerTypeAddressesVM();
            travelerTypeAddressesVM.Addresses = clientDetailRepository.ListClientDetailAddresses(id, page ?? 1);

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeAddressesVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);
            travelerTypeAddressesVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeAddressesVM.TravelerType = travelerType;

            return View(travelerTypeAddressesVM);
        }
        
        // GET: /Create
        public ActionResult Create(int id, string csu, string tt)
        {
            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(id);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Access"] = "WriteAccess";
            }

            TravelerTypeAddressVM travelerTypeAddressVM = new TravelerTypeAddressVM();

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeAddressVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);
            travelerTypeAddressVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeAddressVM.TravelerType = travelerType;

            Address address = new Address();
            travelerTypeAddressVM.Address = address;

            CountryRepository countryRepository = new CountryRepository();
            travelerTypeAddressVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName");

            MappingQualityRepository mappingQualityRepository = new MappingQualityRepository();
            travelerTypeAddressVM.MappingQualityCodes = new SelectList(mappingQualityRepository.GetAllMappingQualities().ToList(), "MappingQualityCode", "MappingQualityCode", address.MappingQualityCode);

            return View(travelerTypeAddressVM);
        }
       
        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TravelerTypeAddressVM travelerTypeAddressVM)
        {
            int clientDetailId = travelerTypeAddressVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            string csu = travelerTypeAddressVM.ClientSubUnit.ClientSubUnitGuid;
            string tt = travelerTypeAddressVM.TravelerType.TravelerTypeGuid;

            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientSubUnitTravelerType == null)
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
                TryUpdateModel<Address>(travelerTypeAddressVM.Address, "Address");
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
                clientDetailAddressRepository.Add(travelerTypeAddressVM.ClientDetail, travelerTypeAddressVM.Address);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = clientDetailId, csu = csu, tt=tt });
        }
        
        // GET: /Edit
        public ActionResult Edit(string csu, int id)
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
           ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
           clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(clientDetailId);

           //Check Exists
           if (clientDetailTravelerType == null)
           {
               ViewData["ActionMethod"] = "EditGet";
               return View("RecordDoesNotExistError");
           }

           string tt = clientDetailTravelerType.TravelerTypeGuid;
           ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
           clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

           //Check Exists
           if (clientDetailTravelerType == null)
           {
               ViewData["ActionMethod"] = "EditGet";
               return View("RecordDoesNotExistError");
           }

           //Access Rights
           RolesRepository rolesRepository = new RolesRepository();
           if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
           {
               ViewData["Message"] = "You do not have access to this item";
               return View("Error");
           }           

           TravelerTypeAddressVM travelerTypeAddressVM = new TravelerTypeAddressVM();

           ClientSubUnit clientSubUnit = new ClientSubUnit();
           clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu); 
           travelerTypeAddressVM.ClientSubUnit = clientSubUnit;

           ClientDetail clientDetail = new ClientDetail();
           clientDetail = clientDetailRepository.GetGroup(clientDetailId);
           travelerTypeAddressVM.ClientDetail = clientDetail;

           TravelerType travelerType = new TravelerType();
           travelerType = travelerTypeRepository.GetTravelerType(tt);
           travelerTypeAddressVM.TravelerType = travelerType;

           Address address = new Address();
           address = addressRepository.GetAddress(clientDetailAddress.AddressId);
           travelerTypeAddressVM.Address = address;

           CountryRepository countryRepository = new CountryRepository();
           travelerTypeAddressVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", address.CountryCode);

           MappingQualityRepository mappingQualityRepository = new MappingQualityRepository();
           travelerTypeAddressVM.MappingQualityCodes = new SelectList(mappingQualityRepository.GetAllMappingQualities().ToList(), "MappingQualityCode", "MappingQualityCode", address.MappingQualityCode);

           return View(travelerTypeAddressVM);
       }
       
        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TravelerTypeAddressVM travelerTypeAddressVM)
       {
           int clientDetailId = travelerTypeAddressVM.ClientDetail.ClientDetailId;

           ClientDetail clientDetail = new ClientDetail();
           clientDetail = clientDetailRepository.GetGroup(clientDetailId);

           //Check Exists
           if (clientDetail == null)
           {
               ViewData["ActionMethod"] = "CreatePost";
               return View("RecordDoesNotExistError");
           }

           ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
           clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(clientDetailId);

           //Check Exists
           if (clientDetailTravelerType == null)
           {
               ViewData["ActionMethod"] = "CreatePost";
               return View("RecordDoesNotExistError");
           }

           string csu = travelerTypeAddressVM.ClientSubUnit.ClientSubUnitGuid;
           string tt = travelerTypeAddressVM.TravelerType.TravelerTypeGuid;

           ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
           clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

           //Check Exists
           if (clientSubUnitTravelerType == null)
           {
               ViewData["ActionMethod"] = "ListSubMenu";
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
               TryUpdateModel<Address>(travelerTypeAddressVM.Address, "Address");
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
               addressRepository.Edit(travelerTypeAddressVM.Address);
           }
           catch (SqlException ex)
           {
               LogRepository logRepository = new LogRepository();
               logRepository.LogError(ex.Message);

               ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
               return View("Error");
           }


           return RedirectToAction("List", new { id = clientDetailId, csu = csu, tt = tt });
       }
  
        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(string csu, int id)
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
          ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
          clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(clientDetailId);

          //Check Exists
          if (clientDetailTravelerType == null)
          {
              ViewData["ActionMethod"] = "EditGet";
              return View("RecordDoesNotExistError");
          }

          string tt = clientDetailTravelerType.TravelerTypeGuid;
          ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
          clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

          //Check Exists
          if (clientDetailTravelerType == null)
          {
              ViewData["ActionMethod"] = "EditGet";
              return View("RecordDoesNotExistError");
          }

          //Access Rights
          RolesRepository rolesRepository = new RolesRepository();
          if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
          {
              ViewData["Message"] = "You do not have access to this item";
              return View("Error");
          }

 
          TravelerTypeAddressVM travelerTypeAddressVM = new TravelerTypeAddressVM();

          ClientSubUnit clientSubUnit = new ClientSubUnit();
          clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
          travelerTypeAddressVM.ClientSubUnit = clientSubUnit;

          ClientDetail clientDetail = new ClientDetail();
          clientDetail = clientDetailRepository.GetGroup(clientDetailId);
          travelerTypeAddressVM.ClientDetail = clientDetail;

          TravelerType travelerType = new TravelerType();
          travelerType = travelerTypeRepository.GetTravelerType(tt);
          travelerTypeAddressVM.TravelerType = travelerType;

          Address address = new Address();
          address = addressRepository.GetAddress(clientDetailAddress.AddressId);
          addressRepository.EditForDisplay(address);
          travelerTypeAddressVM.Address = address;

          return View(travelerTypeAddressVM);
      }
        
        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(TravelerTypeAddressVM travelerTypeAddressVM, FormCollection collection)
    {
        int addressId = travelerTypeAddressVM.Address.AddressId;

        Address address = new Address();
        address = addressRepository.GetAddress(addressId);

        if (address == null)
        {
            ViewData["ActionMethod"] = "DeletePost";
            return View("RecordDoesNotExistError");
        }

        int clientDetailId = travelerTypeAddressVM.ClientDetail.ClientDetailId;
        ClientDetail clientDetail = new ClientDetail();
        clientDetail = clientDetailRepository.GetGroup(clientDetailId);

        //Check Exists
        if (clientDetail == null)
        {
            ViewData["ActionMethod"] = "DeletePost";
            return View("RecordDoesNotExistError");
        }

        ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
        clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(clientDetailId);

        //Check Exists
        if (clientDetailTravelerType == null)
        {
            ViewData["ActionMethod"] = "DeletePost";
            return View("RecordDoesNotExistError");
        }

        string csu = travelerTypeAddressVM.ClientSubUnit.ClientSubUnitGuid;
        string tt = travelerTypeAddressVM.TravelerType.TravelerTypeGuid;

        ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
        clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

        //Check Exists
        if (clientSubUnitTravelerType == null)
        {
            ViewData["ActionMethod"] = "DeletePost";
            return View("RecordDoesNotExistError");
        }

        //Access Rights
        RolesRepository rolesRepository = new RolesRepository();
        if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
        {
            ViewData["Message"] = "You do not have access to this item";
            return View("Error");
        }

       

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
                ViewData["ReturnURL"] = "/TravelerTypeAddress.mvc/Delete/id=" + addressId.ToString() + "&csu=" + csu;
                return View("VersionError");
            }
            //Generic Error
            ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
            return View("Error");
        }


        return RedirectToAction("List", new { id = clientDetailId, csu = csu, tt = tt });
    }
        
        // GET: /View
        public ActionResult View(string csu, int id)
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
            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            string tt = clientDetailTravelerType.TravelerTypeGuid;
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            TravelerTypeAddressVM travelerTypeAddressVM = new TravelerTypeAddressVM();

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeAddressVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            travelerTypeAddressVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeAddressVM.TravelerType = travelerType;

            Address address = new Address();
            address = addressRepository.GetAddress(clientDetailAddress.AddressId);
            addressRepository.EditForDisplay(address);
            travelerTypeAddressVM.Address = address;

            return View(travelerTypeAddressVM);
        }
    }
}
