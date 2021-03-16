using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class AddressRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public Address GetAddress(int addressId)
        {
            return db.Addresses.SingleOrDefault(c => c.AddressId == addressId);
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(Address address)
        {
            if (address.CountryCode != null)
            {

                Country country = new Country();
                country = db.Countries.SingleOrDefault(c => c.CountryCode == address.CountryCode);
                address.CountryName = country.CountryName;
            }

            if (address.MappingQualityCode != null)
            {

                MappingQuality mappingQuality = new MappingQuality();
                mappingQuality = db.MappingQualities.SingleOrDefault(c => c.MappingQualityCode == address.MappingQualityCode);
                address.MappingQualityDescription = mappingQuality.MappingQualityDescription;
            }

            //StateProvince Code is saved as StateProvinceName in the database
            StateProvinceRepository stateProvinceRepository = new StateProvinceRepository();
            StateProvince stateProvince = new StateProvince();

            if (address.StateProvinceName != null)
            {
                stateProvince = stateProvinceRepository.GetStateProvinceByCountry(address.CountryCode, address.StateProvinceName);
            }
            else if (address.StateProvinceCode != null)
            {
                stateProvince = stateProvinceRepository.GetStateProvinceByCountry(address.CountryCode, address.StateProvinceCode);
            }

            if (stateProvince != null)
            {
                address.StateProvinceCode = stateProvince.StateProvinceCode;
                address.StateProvinceName = stateProvince.Name;
            }
        }

        //Edit Address
        public void Edit(Address address)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateAddress_v1(
                address.AddressId,
                address.FirstAddressLine,
                address.SecondAddressLine,
                address.CityName,
                address.CountyName,
                address.StateProvinceName,
                address.LatitudeDecimal,
                address.LongitudeDecimal,
                address.MappingQualityCode,
                address.PostalCode,
                address.ReplicatedFromClientMaintenanceFlag,
                address.CountryCode,
                adminUserGuid,
                address.VersionNumber
            );

        }

        //Delete Address
        public void Delete(Address address)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteAddress_v1(
                address.AddressId,
                adminUserGuid,
                address.VersionNumber 
            );

        }
    }
}