using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientTopUnitMatrixDPCodesVM : CWTBaseViewModel
    {
        public ClientTopUnit ClientTopUnit { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitMatrixDPCodes_v1Result> MatrixDPCodes { get; set; }
        
        public ClientTopUnitMatrixDPCodesVM()
        {
        }
        public ClientTopUnitMatrixDPCodesVM(ClientTopUnit clientTopUnit, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitMatrixDPCodes_v1Result> matrixDPCodes)
        {
            ClientTopUnit = clientTopUnit;
            MatrixDPCodes = matrixDPCodes;
        }
    }
}