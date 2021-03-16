using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
    public class SystemUserWizardRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get Filtered List Of SystemUsers
        public List<spDDAWizard_SelectSystemUsersFiltered_v1Result> GetAvailableSystemUsers(string filter, string filterField)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.spDDAWizard_SelectSystemUsersFiltered_v1(filter, filterField, adminUserGuid).ToList();
        }

        //Get All Teams Of a SystemUser
        public List<spDDAWizard_SelectSystemUserTeams_v1Result> GetSystemUserTeams(string systemUserGuid)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.spDDAWizard_SelectSystemUserTeams_v1(systemUserGuid, adminUserGuid).ToList();
        }

        //List of All Teams that an Admin can Edit
        public List<spDDAWizard_SelectTeamsFiltered_v1Result> GetTeams(string filter, string filterField)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.spDDAWizard_SelectTeamsFiltered_v1(adminUserGuid, filter, filterField).ToList();
        }
        
        //List of All LOcations with their Country
        public List<spDDAWizard_SelectTeamsFiltered_v1Result> GetLocations(string filter, string filterField)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.spDDAWizard_SelectTeamsFiltered_v1(adminUserGuid, filter, filterField).ToList();
        }

        //Compare two SystemUsers and return a list of messages about changes
        public WizardMessages BuildSystemUserChangeMessages(WizardMessages wizardMessages, SystemUserWizardVM originalSystemUser, SystemUserWizardVM systemUserChanges)
        {
            TeamRepository teamRepository = new TeamRepository();
            LocationRepository locationRepository = new LocationRepository();
            SystemUserRepository systemUserRepository = new SystemUserRepository();
            ExternalSystemLoginRepository externalSystemLoginRepository = new ExternalSystemLoginRepository();

			//Messages for DefaultProfile changes
			if (originalSystemUser.SystemUser.DefaultProfileIdentifier == null)
			{
				originalSystemUser.SystemUser.DefaultProfileIdentifier = false;
			}

			if (originalSystemUser.SystemUser.DefaultProfileIdentifier != systemUserChanges.SystemUser.DefaultProfileIdentifier)
			{
				wizardMessages.AddMessage("Default Profile will be updated to \"" + systemUserChanges.SystemUser.DefaultProfileIdentifier + "\".", true);
			}

			//Messages for CubaBookingAllowed changes
			if (originalSystemUser.SystemUser.CubaBookingAllowanceIndicator == null)
			{
				originalSystemUser.SystemUser.CubaBookingAllowanceIndicator = false;
			}

			if (originalSystemUser.SystemUser.CubaBookingAllowanceIndicator != systemUserChanges.SystemUser.CubaBookingAllowanceIndicator)
			{
				wizardMessages.AddMessage("Cuba Booking Allowed will be updated to \"" + systemUserChanges.SystemUser.CubaBookingAllowanceIndicator + "\".", true);
			}

			//Messages for RestrictedFlag changes
			if (originalSystemUser.SystemUser.RestrictedFlag == null)
			{
				originalSystemUser.SystemUser.RestrictedFlag = false;
			}

			if (originalSystemUser.SystemUser.RestrictedFlag != systemUserChanges.SystemUser.RestrictedFlag)
			{
				wizardMessages.AddMessage("Restricted will be updated to \"" + systemUserChanges.SystemUser.RestrictedFlag + "\".", true);
			}

			//Messages for Location
			if (originalSystemUser.SystemUserLocation == null)
            {
                if (systemUserChanges.SystemUserLocation != null){
                    
                    Location location = new Location();
                    location = locationRepository.GetLocation(systemUserChanges.SystemUserLocation.LocationId);
                     wizardMessages.AddMessage("Location will be updated to \"" + location.LocationName + "\".", true);
                }
            
            }
            else
            {
                if (systemUserChanges.SystemUserLocation != null){
                    if (systemUserChanges.SystemUserLocation.LocationId != originalSystemUser.SystemUserLocation.LocationId)
                    {
                        Location location = new Location();
                        location = locationRepository.GetLocation(systemUserChanges.SystemUserLocation.LocationId);
                        wizardMessages.AddMessage("Location will be updated to \"" + location.LocationName + "\".", true);
                    }
                }
            }

			//Messages for BackOfficeAgentIdentifier
			if (originalSystemUser.ExternalSystemLoginSystemUserCountries != systemUserChanges.ExternalSystemLoginSystemUserCountries)
			{
				wizardMessages.AddMessage("User's Back Office Identifiers will be updated.", true);
			}
                    
            //sort List<systemUserChanges> for compare
            if (systemUserChanges.GDSChanged)
            {
                wizardMessages.AddMessage("SystemUserGDSs will be updated.", true);

            }

            //originalSystemUser.SystemUserGDSs.Sort((x, y) => string.Compare(x.GDSCode, y.GDSCode));
            //systemUserChanges.SystemUserGDSs.Sort((x, y) => string.Compare(x.GDSCode, y.GDSCode));
            //compare GDSs
            //if (!originalSystemUser.SystemUserGDSs.SequenceEqual(systemUserChanges.SystemUserGDSs))
            //{
               // wizardMessages.AddMessage("SystemUserGDSs will be updated.", true);
            //}

            //Systemuser teams
            if (systemUserChanges.TeamsAdded != null)
            {
                if (systemUserChanges.TeamsAdded.Count > 0)
                {
                    foreach (Team item in systemUserChanges.TeamsAdded)
                    {
                        Team team = new Team();
                        team = teamRepository.GetTeam(item.TeamId);
                        if (team != null)
                        {
                            wizardMessages.AddMessage("You will add user to Team \"" + team.TeamName + "\".", true);
                        }
                    }
                }
            }
            if (systemUserChanges.TeamsRemoved != null)
            {
                if (systemUserChanges.TeamsRemoved.Count > 0)
                {
                    foreach (Team item in systemUserChanges.TeamsRemoved)
                    {
                        Team team = new Team();
                        team = teamRepository.GetTeam(item.TeamId);
                        if (team != null)
                        {
                            wizardMessages.AddMessage("You will remove user from Team \"" + team.TeamName + "\".", true);
                        }
                    }
                }
            }
            return wizardMessages;
        }

        //Update SystemUser Teams
        public WizardMessages UpdateSystemUserTeams(SystemUserWizardVM systemuserChanges, WizardMessages wizardMessages)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            bool changesExist = false;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("SystemUserTeams");
            doc.AppendChild(root);

            //string xml = "<SystemUserTeams>";

            TeamRepository teamRepository = new TeamRepository();
            if (systemuserChanges.TeamsAdded != null)
            {
                if (systemuserChanges.TeamsAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlTeamsAdded = doc.CreateElement("TeamsAdded");

                    foreach (Team item in systemuserChanges.TeamsAdded)
                    {
                        Team team = new Team();
                        team = teamRepository.GetTeam(item.TeamId);
                        if (team != null)
                        {
                            //xml = xml + "<Team>";
                            //xml = xml + "<TeamName>" + team.TeamName + "</TeamName>";
                            //xml = xml + "<TeamId>" + item.TeamId + "</TeamId>";
                            //xml = xml + "</Team>";

                            XmlElement xmlTeam = doc.CreateElement("Team");
                            xmlTeamsAdded.AppendChild(xmlTeam);

                            XmlElement xmlTeamName = doc.CreateElement("TeamName");
                            xmlTeamName.InnerText = team.TeamName;
                            xmlTeam.AppendChild(xmlTeamName);

                            XmlElement xmlTeamId = doc.CreateElement("TeamId");
                            xmlTeamId.InnerText = item.TeamId.ToString();
                            xmlTeam.AppendChild(xmlTeamId);
                        }
                    }
                    root.AppendChild(xmlTeamsAdded);
                    //xml = xml + "</TeamsAdded>";
                }
            }
            if (systemuserChanges.TeamsRemoved != null)
            {
                if (systemuserChanges.TeamsRemoved.Count > 0)
                {
                    changesExist = true;
                   // xml = xml + "<TeamsRemoved>";
                    XmlElement xmlTeamsRemoved = doc.CreateElement("TeamsRemoved");

                    foreach (Team item in systemuserChanges.TeamsRemoved)
                    {
                        Team team = new Team();
                        team = teamRepository.GetTeam(item.TeamId);
                        if (team != null)
                        {
                           // xml = xml + "<Team>";
                            //xml = xml + "<TeamName>" + team.TeamName + "</TeamName>";
                            //xml = xml + "<TeamId>" + item.TeamId + "</TeamId>";
                            //xml = xml + "</Team>";
                            XmlElement xmlTeam = doc.CreateElement("Team");
                            xmlTeamsRemoved.AppendChild(xmlTeam);

                            XmlElement xmlTeamName = doc.CreateElement("TeamName");
                            xmlTeamName.InnerText = team.TeamName;
                            xmlTeam.AppendChild(xmlTeamName);

                            XmlElement xmlTeamId = doc.CreateElement("TeamId");
                            xmlTeamId.InnerText = item.TeamId.ToString();
                            xmlTeam.AppendChild(xmlTeamId);
                        }
                    }
                    root.AppendChild(xmlTeamsRemoved);
                    //xml = xml + "</TeamsRemoved>";
                }
            }
            //xml = xml + "</SystemUserTeams>";

            //only run if changes made to SystemUser's Teams
            if (changesExist)
            {
                var output = (from n in db.spDDAWizard_UpdateSystemUserTeams_v1(
                    systemuserChanges.SystemUser.SystemUserGuid,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateSystemUserTeams_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }
            }
            return wizardMessages;
        }

        //Update SystemUser GDSss
        public WizardMessages UpdateSystemUserGDSs(SystemUserWizardVM systemuserChanges, WizardMessages wizardMessages)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("SystemUserGDSs");
            doc.AppendChild(root);


            GDSRepository gdsRepository = new GDSRepository();
            if (systemuserChanges.SystemUserGDSs != null)
            {
                if (systemuserChanges.SystemUserGDSs.Count > 0)
                {

                    foreach (fnDesktopDataAdmin_SelectSystemUserGDSs_v1Result item in systemuserChanges.SystemUserGDSs)
                    {
                        GDS gds = new GDS();
                        gds = gdsRepository.GetGDS(item.GDSCode);
                        if (gds != null)
                        {

                            XmlElement xmlGDS = doc.CreateElement("GDS");
                            root.AppendChild(xmlGDS);

                            XmlElement xmlGDSName = doc.CreateElement("GDSName");
                            xmlGDSName.InnerText = gds.GDSName;
                            xmlGDS.AppendChild(xmlGDSName);

                            XmlElement xmlGDSCode = doc.CreateElement("GDSCode");
                            xmlGDSCode.InnerText = gds.GDSCode;
                            xmlGDS.AppendChild(xmlGDSCode);

                            XmlElement xmlPseudoCityOrOfficeId = doc.CreateElement("PseudoCityOrOfficeId");
                            xmlPseudoCityOrOfficeId.InnerText = item.PseudoCityOrOfficeId;
                            xmlGDS.AppendChild(xmlPseudoCityOrOfficeId);

                            XmlElement xmlGDSSignOn = doc.CreateElement("GDSSignOn");
                            xmlGDSSignOn.InnerText = item.GDSSignOn;
                            xmlGDS.AppendChild(xmlGDSSignOn);

                            XmlElement xmlDefaultGDS = doc.CreateElement("DefaultGDS");
                            xmlDefaultGDS.InnerText = item.DefaultGDS == true ? "1" : "0";
                            xmlGDS.AppendChild(xmlDefaultGDS);

                        }
                    }
                }
            }

            var output = (from n in db.spDDAWizard_UpdateSystemUserGDSs_v1(
                systemuserChanges.SystemUser.SystemUserGuid,
                System.Xml.Linq.XElement.Parse(doc.OuterXml),
                adminUserGuid)
                            select n).ToList();

			if (output != null)
			{
				foreach (spDDAWizard_UpdateSystemUserGDSs_v1Result message in output)
				{
					if (message != null)
					{
						if (message.MessageText != null && message.Success != null)
						{
							wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
						}
					}
				}
			}
            return wizardMessages;
        }



    }
}