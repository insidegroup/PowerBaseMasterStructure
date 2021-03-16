using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ClientTopUnitMatrixDPCodeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of ClientTopUnitTelephony Items - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitMatrixDPCodes_v1Result> PageClientTopUnitMatrixDPCodes(int page, string id, string sortField, int? sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectClientTopUnitMatrixDPCodes_v1(id, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitMatrixDPCodes_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        public Dictionary<string, string> GetClientTopUnitMatrixDPCodeHierarchies()
        {
            Dictionary<string, string> hierarchies = new Dictionary<string, string>();

            hierarchies.Add("ClientSubUnit", "Client SubUnit");
            hierarchies.Add("TravelerType", "Traveler Type");

            return hierarchies;
        }

        //Get one Commissionable Route Group
        public ClientTopUnitMatrixDPCode GetGroup(string hierarchyCode, string hierarchyType)
        {
            ClientTopUnitMatrixDPCode clientTopUnitMatrixDPCode = new ClientTopUnitMatrixDPCode();

            if (hierarchyType == "ClientSubUnit")
            {
                MatrixDPCodeByClientSubUnit matrixDPCodeByClientSubUnit = db.MatrixDPCodeByClientSubUnits.SingleOrDefault(c => c.ClientSubUnitGuid == hierarchyCode);
                if(matrixDPCodeByClientSubUnit != null)
                {
                    clientTopUnitMatrixDPCode.HierarchyType = hierarchyType;
                    clientTopUnitMatrixDPCode.HierarchyCode = matrixDPCodeByClientSubUnit.ClientSubUnitGuid;
                    clientTopUnitMatrixDPCode.MatrixDPCode = matrixDPCodeByClientSubUnit.MatrixDPCode;
                    clientTopUnitMatrixDPCode.VersionNumber = matrixDPCodeByClientSubUnit.VersionNumber;

                    ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
                    ClientSubUnit clientSubUnit = clientSubUnitRepository.GetClientSubUnit(matrixDPCodeByClientSubUnit.ClientSubUnitGuid);
                    if(clientSubUnit != null)
                    {
                        clientTopUnitMatrixDPCode.HierarchyItem = clientSubUnit.ClientSubUnitName;
                    }
                }
            }
            else
            {
                MatrixDPCodeByTravelerType matrixDPCodeByTravelerType = db.MatrixDPCodeByTravelerTypes.SingleOrDefault(c => c.TravelerTypeGuid == hierarchyCode);
                if (matrixDPCodeByTravelerType != null)
                {
                    clientTopUnitMatrixDPCode.HierarchyType = hierarchyType;
                    clientTopUnitMatrixDPCode.HierarchyCode = matrixDPCodeByTravelerType.TravelerTypeGuid;
                    clientTopUnitMatrixDPCode.MatrixDPCode = matrixDPCodeByTravelerType.MatrixDPCode;
                    clientTopUnitMatrixDPCode.VersionNumber = matrixDPCodeByTravelerType.VersionNumber;

                    TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
                    TravelerType travelerType = travelerTypeRepository.GetTravelerType(matrixDPCodeByTravelerType.TravelerTypeGuid);
                    if (travelerType != null)
                    {
                        clientTopUnitMatrixDPCode.HierarchyItem = travelerType.TravelerTypeName;
                    }
                }
            }
            return clientTopUnitMatrixDPCode;
        }

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(ClientTopUnitMatrixDPCode group)
        {

        }

        //Add Group
        public void Add(ClientTopUnitMatrixDPCode group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientTopUnitMatrixDPCode_v1(
                group.HierarchyCode,
                group.HierarchyType,
                group.MatrixDPCode,
                adminUserGuid
            );
        }

        //Edit Group
        public void Edit(ClientTopUnitMatrixDPCode group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientTopUnitMatrixDPCode_v1(
                group.HierarchyCode,
                group.HierarchyType,
                group.MatrixDPCode,
                adminUserGuid,
                group.VersionNumber
            );
        }

        //Delete Group
        public void Delete(ClientTopUnitMatrixDPCode group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteClientTopUnitMatrixDPCode_v1(
                group.HierarchyCode,
                group.HierarchyType,
                adminUserGuid,
                group.VersionNumber
            );
        }

    }
}