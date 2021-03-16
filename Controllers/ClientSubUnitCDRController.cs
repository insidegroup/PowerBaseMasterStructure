using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Models;
using System.Data.SqlClient;
using System.IO;

namespace CWTDesktopDatabase.Controllers
{
    public class ClientSubUnitCDRController : Controller
    {
        ClientSubUnitCDRRepository clientSubUnitCDRRepository = new ClientSubUnitCDRRepository();
        CreditCardRepository creditCardRepository = new CreditCardRepository();
        ClientAccountRepository clientAccountRepository = new ClientAccountRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientDefinedReferenceItemRepository clientDefinedReferenceItemRepository = new ClientDefinedReferenceItemRepository();
        
        public ActionResult List(string filter, int? page, string id, string sortField, int? sortOrder)
        {
            //SortField + SortOrder settings
			if (string.IsNullOrEmpty(sortField))
            {
				sortField = "RelatedToValue";
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


            ClientSubUnitCDRsVM clientSubUnitCDRsVM = new ClientSubUnitCDRsVM();
            clientSubUnitCDRsVM.ClientDefinedReferences = clientSubUnitCDRRepository.PageClientSubUnitCDRs(page ?? 1,  filter ?? "", id, sortField, sortOrder);

            RolesRepository rolesRepository = new RolesRepository();
            clientSubUnitCDRsVM.HasWriteAccess = rolesRepository.HasWriteAccessToClientSubUnit(id);
            clientSubUnitCDRsVM.HasCDRLinkImportAccess = rolesRepository.HasWriteAccessToCDRLinkImport();

			/*
			DisplayName
			The ClientSubUnit may only be asscoiated with one Itemtype of ClientDefinedReference
			Therefore, we show Itemtype value if it has ClientDefinedReferenceItems assigned, otherwise a list of available Itemtype
			*/
			ClientDefinedReferenceItemRepository cdrItemRepository = new ClientDefinedReferenceItemRepository();
            ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			if (clientSubUnitCDRsVM.ClientDefinedReferences.Count == 0)
			{
				clientSubUnitCDRsVM.ClientDefinedReferenceItemId = null;
			}
			else
			{
				clientSubUnitCDRsVM.DisplayName = clientSubUnitCDRsVM.ClientDefinedReferences[0].DisplayName;
			}

			//ClientSubUnit
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}
			
            clientSubUnitCDRsVM.ClientSubUnit = clientSubUnit;

			//RelatedToDisplayName
			if (clientSubUnitCDRsVM.ClientDefinedReferences.Count > 0)
			{
				int clientSubUnitClientDefinedReferenceId = clientSubUnitCDRsVM.ClientDefinedReferences[0].ClientSubUnitClientDefinedReferenceId;

				ClientSubUnitCDRItemRepository clientSubUnitCDRItemRepository = new ClientSubUnitCDRItemRepository();
				
				List<ClientSubUnitClientDefinedReferenceItem> clientSubUnitClientDefinedReferenceItems = new List<ClientSubUnitClientDefinedReferenceItem>();
				clientSubUnitClientDefinedReferenceItems = clientSubUnitCDRItemRepository.GetClientSubUnitCDRItems(clientSubUnitClientDefinedReferenceId);

				if (clientSubUnitClientDefinedReferenceItems != null && clientSubUnitClientDefinedReferenceItems.Count > 0)
				{
					clientSubUnitCDRsVM.RelatedToDisplayName = clientSubUnitClientDefinedReferenceItems[0].RelatedToDisplayName;
				}
			}

            //return items
            return View(clientSubUnitCDRsVM);
        }

		[HttpGet]
		public ActionResult Create(ClientSubUnitCDRsVM clientSubUnitCDRsVM, string btnSubmit)
		{
			string clientSubUnitGuid = clientSubUnitCDRsVM.ClientSubUnitGuid;
			string displayName = clientSubUnitCDRsVM.DisplayName;
			string relatedToDisplayName = clientSubUnitCDRsVM.RelatedToDisplayName ?? "";

			if (btnSubmit == "Import")
			{
				return RedirectToAction("ImportStep1", new { id = clientSubUnitGuid, displayName = displayName, relatedToDisplayName = relatedToDisplayName });
			}

			//Get ClientSubUnit
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}



			//Check Exists
			//if (clientdefinedReferenceItem2 == null)
			//{
			//    ViewData["ActionMethod"] = "CreateGet";
			//    return View("RecordDoesNotExistError");
			//}


			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientSubUnitCDRVM clientSubUnitCDRVM = new ClientSubUnitCDRVM();
			clientSubUnitCDRVM.ClientSubUnit = clientSubUnit;

			//Get ClientSubUnit/ClientDefinedReferenceItem
			ClientDefinedReferenceItem clientdefinedReferenceItem2 = new ClientDefinedReferenceItem();
			//if(string.IsNullOrEmpty(displayName)){
			//clientdefinedReferenceItem2. = ClientDefinedReferenceItemId;// clientDefinedReferenceItemRepository.GetCSUClientDefinedReferenceItemByCDRId(clientSubUnitGuid, ClientDefinedReferenceItemId);
			//     ClientSubUnitClientDefinedReference clientSubUnitClientDefinedReference = new ClientSubUnitClientDefinedReference();
			//     clientSubUnitClientDefinedReference.DisplayName = displayName;
			//     clientSubUnitCDRVM.ClientSubUnitClientDefinedReference = clientSubUnitClientDefinedReference;
			//}else{

			//   clientdefinedReferenceItem2 = clientDefinedReferenceItemRepository.GetCSUClientDefinedReferenceItem(clientSubUnitGuid,displayName);
			//   clientSubUnitCDRVM.ClientDefinedReferenceItemId = clientdefinedReferenceItem2.ClientDefinedReferenceItemId;
			//}

			//Add MaskedCreditCardNumber and CreditCardHolderName into Select List
			CreditCardClientSubUnitRepository creditCardClientSubUnitRepository = new CreditCardClientSubUnitRepository();
			List<CreditCard> creditCards = creditCardClientSubUnitRepository.GetAllCreditCardsBySubUnit(clientSubUnitGuid).ToList();
			List<SelectListItem> creditCardList = new List<SelectListItem>();
			foreach (CreditCard creditCardItem in creditCards)
			{
				creditCardList.Add(new SelectListItem
				{
					Value = creditCardItem.CreditCardId.ToString(),
					Text = string.Format("{0}\xA0\xA0\xA0\xA0\xA0\xA0\xA0\xA0\xA0\xA0\xA0\xA0{1}", creditCardItem.MaskedCreditCardNumber, creditCardItem.CreditCardHolderName) //Space out items
				});
			}
			clientSubUnitCDRVM.CreditCardSelectList = new SelectList(creditCardList, "Value", "Text");

			ClientSubUnitClientAccountRepository clientSubUnitClientAccountRepository = new ClientSubUnitClientAccountRepository();
			clientSubUnitCDRVM.ClientAccountSelectList = clientSubUnitClientAccountRepository.GetClientAccountsBySubUnit(clientSubUnitGuid, "");
			clientSubUnitCDRVM.CreditCardValidTo = null;

			//Show Create Form
			return View(clientSubUnitCDRVM);
		}

        [HttpPost]
		[ValidateAntiForgeryToken]
        public ActionResult Create( ClientSubUnitCDRVM clientSubUnitCDRVM)
        {
            //Validate data against Tables
            if (!ModelState.IsValid)
            {
                string clientSubUnitGuid = clientSubUnitCDRVM.ClientSubUnit.ClientSubUnitGuid;

                //Get ClientSubUnit
                ClientSubUnit clientSubUnit = new ClientSubUnit();
                clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

                 ClientTopUnit clientTopUnit = new ClientTopUnit();
                 ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
                clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientSubUnit.ClientTopUnitGuid);

                clientSubUnit.ClientTopUnit = clientTopUnit;
                clientSubUnitCDRVM.ClientSubUnit = clientSubUnit;                
                clientSubUnitCDRVM.ClientSubUnit = clientSubUnit;

                CreditCardClientSubUnitRepository creditCardClientSubUnitRepository = new CreditCardClientSubUnitRepository();
                clientSubUnitCDRVM.CreditCardSelectList = new SelectList(creditCardClientSubUnitRepository.GetAllCreditCardsBySubUnit(clientSubUnitGuid).ToList(), "CreditCardId", "MaskedCreditCardNumber");

                ClientSubUnitClientAccountRepository clientSubUnitClientAccountRepository = new ClientSubUnitClientAccountRepository();
                clientSubUnitCDRVM.ClientAccountSelectList = clientSubUnitClientAccountRepository.GetClientAccountsBySubUnit(clientSubUnitGuid, "");

                //ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
                //clientSubUnitCDRVM.ClientSubUnit.ClientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientSubUnitCDRVM.ClientSubUnit.ClientSubUnitGuid);
                return View(clientSubUnitCDRVM);
            }
            CreditCard creditCard = new CreditCard();
            int? creditCardId = clientSubUnitCDRVM.ClientSubUnitClientDefinedReference.CreditCardId;
            if(creditCardId != null){
                creditCard = creditCardRepository.GetCreditCard((int)creditCardId,false);
            }

            ClientAccount clientAccount = new ClientAccount();
            string clientAccountNumber = null;
            string sourceSystemCode = null;
            if(!String.IsNullOrEmpty(clientSubUnitCDRVM.ClientSubUnitClientDefinedReference.ClientAccountNumberSourceSystemCode)){
                clientAccountNumber = clientSubUnitCDRVM.ClientSubUnitClientDefinedReference.ClientAccountNumberSourceSystemCode.Split(new[] { '|' })[0];
                sourceSystemCode = clientSubUnitCDRVM.ClientSubUnitClientDefinedReference.ClientAccountNumberSourceSystemCode.Split(new[] { '|' })[1];
                clientAccount = clientAccountRepository.GetClientAccount(clientAccountNumber, sourceSystemCode);
            }

            //Check Exists (at least one
            if (creditCard == null && clientAccount==null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitCDRVM.ClientSubUnit.ClientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            ClientSubUnitClientDefinedReference clientSubUnitClientDefinedReference = new ClientSubUnitClientDefinedReference();
            clientSubUnitClientDefinedReference.ClientSubUnitGuid = clientSubUnitCDRVM.ClientSubUnit.ClientSubUnitGuid;
            
            clientSubUnitClientDefinedReference.Value = clientSubUnitCDRVM.ClientSubUnitClientDefinedReference.Value;
            clientSubUnitClientDefinedReference.ClientAccountNumber = clientAccountNumber;
            clientSubUnitClientDefinedReference.SourceSystemCode = sourceSystemCode;
            clientSubUnitClientDefinedReference.CreditCardId = creditCardId;

            ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
            ClientDefinedReferenceItemRepository clientDefinedReferenceItemRepository = new ClientDefinedReferenceItemRepository();
            //clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(clientSubUnitCDRVM.ClientDefinedReferenceItemId);
            //if(clientDefinedReferenceItem != null){
                clientSubUnitClientDefinedReference.DisplayName = clientSubUnitCDRVM.DisplayName;
            //}


            try
            {
                clientSubUnitCDRRepository.Add(clientSubUnitClientDefinedReference);
            }
             catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id =clientSubUnitCDRVM.ClientSubUnit.ClientSubUnitGuid});
        }

        public ActionResult Edit(int id)
        {
            //Get ClientSubUnitTelephony
            ClientSubUnitClientDefinedReference clientSubUnitClientDefinedReference = new ClientSubUnitClientDefinedReference();
            clientSubUnitClientDefinedReference = clientSubUnitCDRRepository.GetClientSubUnitCDR(id);

            //Check Exists
            if (clientSubUnitClientDefinedReference == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitClientDefinedReference.ClientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitCDRVM clientSubUnitCDRVM = new ClientSubUnitCDRVM();
            

            CreditCard creditCard = new CreditCard();
            CreditCardRepository creditCardRepository = new CreditCardRepository();
            if(clientSubUnitClientDefinedReference.CreditCardId == null){
                clientSubUnitClientDefinedReference.CreditCard = creditCard;
            }else{
                creditCard = creditCardRepository.GetCreditCard((int)clientSubUnitClientDefinedReference.CreditCardId,false);
                clientSubUnitCDRVM.CreditCardValidTo = creditCard.CreditCardValidTo;
            }
            
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnitCDRVM.ClientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitClientDefinedReference.ClientSubUnitGuid);

			//Add MaskedCreditCardNumber and CreditCardHolderName into Select List
			CreditCardClientSubUnitRepository creditCardClientSubUnitRepository = new CreditCardClientSubUnitRepository();
			List<CreditCard> creditCards = creditCardClientSubUnitRepository.GetAllCreditCardsBySubUnit(clientSubUnitClientDefinedReference.ClientSubUnitGuid).ToList();
			List<SelectListItem> creditCardList = new List<SelectListItem>();
			foreach (CreditCard creditCardItem in creditCards)
			{
				creditCardList.Add(new SelectListItem
				{
					Value = creditCardItem.CreditCardId.ToString(),
					Text = string.Format("{0}\xA0\xA0\xA0\xA0\xA0\xA0\xA0\xA0\xA0\xA0\xA0\xA0{1}", creditCardItem.MaskedCreditCardNumber, creditCardItem.CreditCardHolderName) //Space out items
				});
			}
			clientSubUnitCDRVM.CreditCardSelectList = new SelectList(creditCardList, "Value", "Text");
			
			ClientSubUnitClientAccountRepository clientSubUnitClientAccountRepository = new ClientSubUnitClientAccountRepository();
            string modifiedClientAccountNumber = clientSubUnitClientDefinedReference.ClientAccountNumber + "|" + clientSubUnitClientDefinedReference.SourceSystemCode;
            clientSubUnitCDRVM.ClientAccountSelectList = clientSubUnitClientAccountRepository.GetClientAccountsBySubUnit(clientSubUnitClientDefinedReference.ClientSubUnitGuid, modifiedClientAccountNumber.ToString());

            clientSubUnitClientDefinedReference.ClientAccountNumberSourceSystemCode = modifiedClientAccountNumber;
            clientSubUnitCDRVM.ClientSubUnitClientDefinedReference = clientSubUnitClientDefinedReference;

            //Show Create Form
            return View(clientSubUnitCDRVM);
        }

        [HttpPost][ValidateAntiForgeryToken]
        public ActionResult Edit(ClientSubUnitCDRVM clientSubUnitCDRVM)
        {
            if (!ModelState.IsValid)
            {
                string clientSubUnitGuid = clientSubUnitCDRVM.ClientSubUnit.ClientSubUnitGuid;

                //Get ClientSubUnit
                ClientSubUnit clientSubUnit = new ClientSubUnit();
                clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

                 ClientTopUnit clientTopUnit = new ClientTopUnit();
                 ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
                clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientSubUnit.ClientTopUnitGuid);

                clientSubUnit.ClientTopUnit = clientTopUnit;
                clientSubUnitCDRVM.ClientSubUnit = clientSubUnit;


                
                clientSubUnitCDRVM.ClientSubUnit = clientSubUnit;

                CreditCardClientSubUnitRepository creditCardClientSubUnitRepository = new CreditCardClientSubUnitRepository();
                clientSubUnitCDRVM.CreditCardSelectList = new SelectList(creditCardClientSubUnitRepository.GetAllCreditCardsBySubUnit(clientSubUnitGuid).ToList(), "CreditCardId", "MaskedCreditCardNumber");

                ClientSubUnitClientAccountRepository clientSubUnitClientAccountRepository = new ClientSubUnitClientAccountRepository();
                clientSubUnitCDRVM.ClientAccountSelectList = clientSubUnitClientAccountRepository.GetClientAccountsBySubUnit(clientSubUnitGuid, "");

                //ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
                //clientSubUnitCDRVM.ClientSubUnit.ClientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientSubUnitCDRVM.ClientSubUnit.ClientSubUnitGuid);
                return View(clientSubUnitCDRVM);
            }

            CreditCard creditCard = new CreditCard();
            int? creditCardId = clientSubUnitCDRVM.ClientSubUnitClientDefinedReference.CreditCardId;
            if(creditCardId != null){
                creditCard = creditCardRepository.GetCreditCard((int)creditCardId,false);
            }

           ClientAccount clientAccount = new ClientAccount();
            string clientAccountNumber = null;
            string sourceSystemCode = null;
            if(!String.IsNullOrEmpty(clientSubUnitCDRVM.ClientSubUnitClientDefinedReference.ClientAccountNumberSourceSystemCode)){
                clientAccountNumber = clientSubUnitCDRVM.ClientSubUnitClientDefinedReference.ClientAccountNumberSourceSystemCode.Split(new[] { '|' })[0];
                sourceSystemCode = clientSubUnitCDRVM.ClientSubUnitClientDefinedReference.ClientAccountNumberSourceSystemCode.Split(new[] { '|' })[1];
                clientAccount = clientAccountRepository.GetClientAccount(clientAccountNumber, sourceSystemCode);
            }

            //Check Exists (at least one
            if (creditCard == null && clientAccount==null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitCDRVM.ClientSubUnit.ClientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitClientDefinedReference clientSubUnitClientDefinedReference = new ClientSubUnitClientDefinedReference();
            clientSubUnitClientDefinedReference.ClientSubUnitGuid = clientSubUnitCDRVM.ClientSubUnit.ClientSubUnitGuid;
            clientSubUnitClientDefinedReference.Value = clientSubUnitCDRVM.ClientSubUnitClientDefinedReference.Value;
            clientSubUnitClientDefinedReference.VersionNumber = clientSubUnitCDRVM.ClientSubUnitClientDefinedReference.VersionNumber;
            clientSubUnitClientDefinedReference.ClientAccountNumber = clientAccountNumber;
            clientSubUnitClientDefinedReference.SourceSystemCode = sourceSystemCode;
            if(creditCardId != null){
            clientSubUnitClientDefinedReference.CreditCardId = creditCard.CreditCardId;
            }
            clientSubUnitClientDefinedReference.ClientSubUnitClientDefinedReferenceId = clientSubUnitCDRVM.ClientSubUnitClientDefinedReference.ClientSubUnitClientDefinedReferenceId;
            clientSubUnitClientDefinedReference.VersionNumber = clientSubUnitCDRVM.ClientSubUnitClientDefinedReference.VersionNumber;

            



            try
            {
                 clientSubUnitCDRRepository.Edit(clientSubUnitClientDefinedReference);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
           


           


            return RedirectToAction("List", new {id =clientSubUnitCDRVM.ClientSubUnit.ClientSubUnitGuid});
        }

        public ActionResult View (int id)
        {
            //Get ClientSubUnitTelephony
            ClientSubUnitClientDefinedReference clientSubUnitClientDefinedReference = new ClientSubUnitClientDefinedReference();
            clientSubUnitClientDefinedReference = clientSubUnitCDRRepository.GetClientSubUnitCDR(id);

            //Check Exists
            if (clientSubUnitClientDefinedReference == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            
            if(clientSubUnitClientDefinedReference.ClientAccountNumber == null){
                ClientAccount clientAccount = new ClientAccount();
                clientSubUnitClientDefinedReference.ClientAccount = clientAccount;
            }

            if(clientSubUnitClientDefinedReference.CreditCardId == null){
                CreditCard creditCard = new CreditCard();
                clientSubUnitClientDefinedReference.CreditCard = creditCard;
            }

            ClientSubUnitCDRVM clientSubUnitCDRVM = new ClientSubUnitCDRVM();
            clientSubUnitCDRVM.ClientSubUnitClientDefinedReference = clientSubUnitClientDefinedReference;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnitCDRVM.ClientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitClientDefinedReference.ClientSubUnitGuid);

            //Show Create Form
            return View(clientSubUnitCDRVM);
        }

		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get ClientSubUnitTelephony
            ClientSubUnitClientDefinedReference clientSubUnitClientDefinedReference = new ClientSubUnitClientDefinedReference();
            clientSubUnitClientDefinedReference = clientSubUnitCDRRepository.GetClientSubUnitCDR(id);

            //Check Exists
            if (clientSubUnitClientDefinedReference == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            if(clientSubUnitClientDefinedReference.ClientAccountNumber == null){
                ClientAccount clientAccount = new ClientAccount();
                clientSubUnitClientDefinedReference.ClientAccount = clientAccount;
            }
            if(clientSubUnitClientDefinedReference.CreditCardId == null){
                CreditCard creditCard = new CreditCard();
                clientSubUnitClientDefinedReference.CreditCard = creditCard;
            }

            ClientSubUnitCDRVM clientSubUnitCDRVM = new ClientSubUnitCDRVM();
            clientSubUnitCDRVM.ClientSubUnitClientDefinedReference = clientSubUnitClientDefinedReference;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnitCDRVM.ClientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitClientDefinedReference.ClientSubUnitGuid);

            //Show Create Form
            return View(clientSubUnitCDRVM);
        }

        [HttpPost][ValidateAntiForgeryToken]
        public ActionResult Delete(ClientSubUnitCDRVM clientSubUnitCDRVM)
        {
            int id = clientSubUnitCDRVM.ClientSubUnitClientDefinedReference.ClientSubUnitClientDefinedReferenceId;
            ClientSubUnitClientDefinedReference clientSubUnitClientDefinedReference = new ClientSubUnitClientDefinedReference();
            clientSubUnitClientDefinedReference = clientSubUnitCDRRepository.GetClientSubUnitCDR(id);

            //Check Exists (at least one
            if (clientSubUnitClientDefinedReference==null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitCDRVM.ClientSubUnit.ClientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            try
            {
                 clientSubUnitCDRRepository.Delete(clientSubUnitClientDefinedReference);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

			return RedirectToAction("List", new { id = clientSubUnitCDRVM.ClientSubUnit.ClientSubUnitGuid });
        }

		public ActionResult ImportStep1(string id, string displayName, string relatedToDisplayName = "")
        {

            CDRImportStep1WithFileVM cdrLinkImportFileVM = new CDRImportStep1WithFileVM();
            cdrLinkImportFileVM.ClientSubUnitGuid = id;
            cdrLinkImportFileVM.DisplayName = displayName;
			cdrLinkImportFileVM.RelatedToDisplayName = relatedToDisplayName;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

            cdrLinkImportFileVM.ClientSubUnit = clientSubUnit;

            return View(cdrLinkImportFileVM);
        }

        [HttpPost]
		public ActionResult ImportStep1(CDRImportStep1WithFileVM csvfile)
        {
            //used for return only
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csvfile.ClientSubUnitGuid);
            ClientTopUnit clientTopUnit = new ClientTopUnit();
            ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientSubUnit.ClientTopUnitGuid);
            clientSubUnit.ClientTopUnit = clientTopUnit;
            csvfile.ClientSubUnit = clientSubUnit;


            if (!ModelState.IsValid)
            {

                return View(csvfile);
            }
            string fileExtension = Path.GetExtension(csvfile.File.FileName);
            if (fileExtension != ".csv" && fileExtension != ".xls" && fileExtension != ".xlsx")
            {
                ModelState.AddModelError("file", csvfile.File.ContentType);
                return View(csvfile);
            }

            if (csvfile.File.ContentLength > 0)
            {
                CDRImportStep2VM preImportCheckResult = new CDRImportStep2VM();
                List<string> returnMessages = new List<string>();

				preImportCheckResult = clientSubUnitCDRRepository.PreImportCheck(csvfile.File, csvfile.ClientSubUnitGuid, csvfile.DisplayName, csvfile.RelatedToDisplayName);
                
                CDRImportStep1VM preImportCheckResultVM = new CDRImportStep1VM();
                preImportCheckResultVM.ClientSubUnit = clientSubUnit;
                preImportCheckResultVM.CDRImportStep2VM = preImportCheckResult;
                preImportCheckResultVM.DisplayName = csvfile.DisplayName;
				preImportCheckResultVM.RelatedToDisplayName = csvfile.RelatedToDisplayName;
                preImportCheckResultVM.ClientSubUnitGuid = csvfile.ClientSubUnitGuid;

                TempData["PreImportCheckResultVM"] = preImportCheckResultVM;
                return RedirectToAction("ImportStep2");
            }

            return View();
        }

        public ActionResult ImportStep2()
        {
            CDRImportStep1VM preImportCheckResultVM = new CDRImportStep1VM();
            preImportCheckResultVM = (CDRImportStep1VM)TempData["PreImportCheckResultVM"];
            return View(preImportCheckResultVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportStep2(CDRImportStep1VM preImportCheckResultVM)
        {
            //PreImport Check Results (check has passed)
            CDRImportStep2VM preImportCheckResult = new CDRImportStep2VM();
            preImportCheckResult = preImportCheckResultVM.CDRImportStep2VM;

            //Do the Import, return results
            CDRImportStep3VM cdrPostImportResult = new CDRImportStep3VM();
			cdrPostImportResult = clientSubUnitCDRRepository.Import(
				preImportCheckResult.FileBytes, 
				preImportCheckResultVM.ClientSubUnitGuid, 
				preImportCheckResultVM.DisplayName, 
				preImportCheckResultVM.RelatedToDisplayName
			);

            cdrPostImportResult.ClientSubUnitGuid = preImportCheckResultVM.ClientSubUnitGuid;
            TempData["CdrPostImportResult"] = cdrPostImportResult;

            //Pass Results to Next Page
            return RedirectToAction("ImportStep3");

        }

        public ActionResult ImportStep3()
        {
            //Display Results of Import
            CDRImportStep3VM cdrPostImportResult = new CDRImportStep3VM();
            cdrPostImportResult = (CDRImportStep3VM)TempData["CdrPostImportResult"];

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(cdrPostImportResult.ClientSubUnitGuid);
            cdrPostImportResult.ClientSubUnit = clientSubUnit;
            return View(cdrPostImportResult);
        }
    }
}
