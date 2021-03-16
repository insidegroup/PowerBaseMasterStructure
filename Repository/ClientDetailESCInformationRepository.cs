using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System.Linq.Dynamic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ClientDetailESCInformationRepository
    {
        private ClientDetailDC db = new ClientDetailDC(Settings.getConnectionString());

        //Add ClientDetail ESCInformation
        public void Add(ClientDetail clientDetail, ClientDetailESCInformation clientDetailESCInformation)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientDetailESCInformation_v1(
                clientDetail.ClientDetailId,
                clientDetailESCInformation.ESCInformation,
                adminUserGuid
            );
        }

        //Edit ClientDetail ESCInformation
        public void Edit(ClientDetailESCInformation clientDetailESCInformation)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientDetailESCInformation_v1(
                clientDetailESCInformation.ClientDetailId,
                clientDetailESCInformation.ESCInformation,
                adminUserGuid,
                clientDetailESCInformation.VersionNumber
            );
        }

        //Add ClientDetail ESCInformation
        public void Delete(ClientDetailESCInformation clientDetailESCInformation)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteClientDetailESCInformation_v1(
                clientDetailESCInformation.ClientDetailId,
                adminUserGuid,
                clientDetailESCInformation.VersionNumber
            );
        }
        //Get one Item from Contact
       // public ClientDetailESCInformation GetESCInformationClientDetail(int escInformationId)
        //{
        //    return db.ClientDetailESCInformations.SingleOrDefault(c => c.ESCInformation == escInformationId);
        //}

        //Get one Item from ClientDetail
        public ClientDetailESCInformation GetClientDetailESCInformation(int clientDetailId)
        {
            return db.ClientDetailESCInformations.SingleOrDefault(c => c.ClientDetailId == clientDetailId);
        }

    }
}