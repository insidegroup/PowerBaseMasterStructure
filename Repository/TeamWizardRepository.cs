using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
    public class TeamWizardRepository
    {

        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //List of All Teams that an Admin can Edit
        public List<spDDAWizard_SelectTeamsFiltered_v1Result> GetTeams(string filter)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.spDDAWizard_SelectTeamsFiltered_v1(adminUserGuid, filter,"TeamName").ToList();
        }

        //Get all ClietnSubUnits of a Team
        public List<spDDAWizard_SelectTeamClientSubUnits_v1Result> GetTeamClientSubUnits(int teamId)
        {
            return db.spDDAWizard_SelectTeamClientSubUnits_v1(teamId).ToList();
        }

        //Get all SystemUsers of a Team
        public List<spDDAWizard_SelectTeamSystemUsers_v1Result> GetTeamSystemUsers(int teamId)
        {
            return db.spDDAWizard_SelectTeamSystemUsers_v1(teamId).ToList();
        }

        //Get Filtered List Of SystemUsers
        public List<spDDAWizard_SelectTeamAvailableSystemUsersFiltered_v1Result> GetTeamAvailableSystemUsers(string filter, string filterField, int teamId)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.spDDAWizard_SelectTeamAvailableSystemUsersFiltered_v1(filter, filterField,teamId, adminUserGuid).ToList();
        }

        //Get Filtered List Of SystemUsers
        public List<spDDAWizard_SelectClientSubUnitsFiltered_v1Result> GetTeamAvailableClientSubUnits(string filter, string filterField)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.spDDAWizard_SelectClientSubUnitsFiltered_v1(filter, filterField, adminUserGuid).ToList();
        }

        //Get List of Items attached to a Location (to populate LocationLinkedItemsVM)
        public void AddLinkedItems(TeamLinkedItemsVM teamLinkedItemsScreenViewModel)
        {
            int teamid = teamLinkedItemsScreenViewModel.Team.TeamId;

            HierarchyDC hierarchyDC = new HierarchyDC(Settings.getConnectionString());
            teamLinkedItemsScreenViewModel.Addresses =
                (from n in hierarchyDC.TeamAddresses where n.TeamId == teamid select n.Address).ToList();

            teamLinkedItemsScreenViewModel.Contacts =
                (from n in hierarchyDC.TeamContacts where n.TeamId == teamid select n.Contact).ToList();

            
            teamLinkedItemsScreenViewModel.CreditCards =
                (from n in hierarchyDC.CreditCardTeams where n.TeamId == teamid select n.CreditCard).ToList();

            ExternalSystemParameterDC externalSystemParameterDC = new ExternalSystemParameterDC(Settings.getConnectionString());
            teamLinkedItemsScreenViewModel.ExternalSystemParameters =
                (from n in externalSystemParameterDC.ExternalSystemParameterTeams where n.TeamId == teamid select n.ExternalSystemParameter).ToList();

            teamLinkedItemsScreenViewModel.ExternalSystemLogins =
                (from n in hierarchyDC.ExternalSystemLoginTeams where n.TeamId == teamid select n.ExternalSystemLogin).ToList();

            GDSAdditionalEntryDC gdsAdditionalEntryDC = new GDSAdditionalEntryDC(Settings.getConnectionString());
            teamLinkedItemsScreenViewModel.GDSAdditionalEntries =
                (from n in gdsAdditionalEntryDC.GDSAdditionalEntryTeams where n.TeamId == teamid select n.GDSAdditionalEntry).ToList();
            
            teamLinkedItemsScreenViewModel.LocalOperatingHoursGroups =
                (from n in hierarchyDC.LocalOperatingHoursGroupTeams where n.TeamId == teamid select n.LocalOperatingHoursGroup).ToList();

            teamLinkedItemsScreenViewModel.PNROutputGroups =
                (from n in hierarchyDC.PNROutputGroupTeams where n.TeamId == teamid select n.PNROutputGroup).ToList();
            
            PolicyGroupDC policyGroupDC = new PolicyGroupDC(Settings.getConnectionString());
            teamLinkedItemsScreenViewModel.PolicyGroups =
                (from n in policyGroupDC.PolicyGroupTeams where n.TeamId == teamid select n.PolicyGroup).ToList();
            
            PublicHolidayGroupDC publicHolidayGroupDC = new PublicHolidayGroupDC(Settings.getConnectionString());
            teamLinkedItemsScreenViewModel.PublicHolidayGroups =
                (from n in publicHolidayGroupDC.PublicHolidayGroupTeams where n.TeamId == teamid select n.PublicHolidayGroup).ToList();

            teamLinkedItemsScreenViewModel.QueueMinderGroups =
                (from n in hierarchyDC.QueueMinderGroupTeams where n.TeamId == teamid select n.QueueMinderGroup).ToList();

            TicketQueueGroupDC ticketQueueGroupDC = new TicketQueueGroupDC(Settings.getConnectionString());
            teamLinkedItemsScreenViewModel.TicketQueueGroups =
                (from n in ticketQueueGroupDC.TicketQueueGroupTeams where n.TeamId == teamid select n.TicketQueueGroup).ToList();

            teamLinkedItemsScreenViewModel.ValidPseudoCityOrOfficeIds =
                (from n in hierarchyDC.TeamDefaultPseudoCityOrOfficeIds where n.TeamId == teamid select n.ValidPseudoCityOrOfficeId).ToList();

            ServicingOptionGroupDC servicingOptionGroupDC = new ServicingOptionGroupDC(Settings.getConnectionString());
            teamLinkedItemsScreenViewModel.ServicingOptionGroups =
                (from n in servicingOptionGroupDC.ServicingOptionGroupTeams where n.TeamId == teamid select n.ServicingOptionGroup).ToList();
           
        }

        //Compare two Teams and return a list of messages about changes
        public WizardMessages BuildTeamChangeMessages(WizardMessages wizardMessages, Team originalTeam, TeamWizardVM teamChanges)
        {
            TeamRepository teamRepository = new TeamRepository();
            ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
            SystemUserRepository systemUserRepository = new SystemUserRepository();

            Team updatedTeam  = new Team();
            updatedTeam = teamChanges.Team;
            //teamRepository.EditGroupForDisplay(updatedTeam); removed- gets info from original Team

            if (originalTeam == null)
            {
                wizardMessages.AddMessage("A new Team \"" + updatedTeam.TeamName + "\"has been added.", true);
            }
            else
            {


                if (originalTeam.TeamName != updatedTeam.TeamName)
                {
                    wizardMessages.AddMessage("Team Name will be updated to \"" + updatedTeam.TeamName + "\".", true);
                }

                if (originalTeam.TeamEmail != updatedTeam.TeamEmail)
                {
                    wizardMessages.AddMessage("Team Email will be updated to \"" + updatedTeam.TeamEmail + "\".", true);
                }

                if (originalTeam.TeamPhoneNumber != updatedTeam.TeamPhoneNumber)
                {
                    wizardMessages.AddMessage("Team phone Number will be updated to \"" + updatedTeam.TeamPhoneNumber + "\".", true);
                }
                if (originalTeam.CityCode != updatedTeam.CityCode)
                {
                    wizardMessages.AddMessage("Team City Code will be updated to \"" + updatedTeam.CityCode + "\".", true);
                }
                if (originalTeam.TeamQueue != updatedTeam.TeamQueue)
                {
                    wizardMessages.AddMessage("Team Queue will be updated to \"" + updatedTeam.TeamQueue + "\".", true);
                }
                

                if (originalTeam.TeamTypeCode != updatedTeam.TeamTypeCode)
                {
                    TeamType teamType = new TeamType();
                    TeamTypeRepository teamTypeRepository = new TeamTypeRepository();
                    teamType = teamTypeRepository.GetTeamType(updatedTeam.TeamTypeCode);
                    wizardMessages.AddMessage("Team Type will be updated to \"" + teamType.TeamTypeDescription + "\".", true);
                }

                if (originalTeam.HierarchyType != updatedTeam.HierarchyType)
                {
                    wizardMessages.AddMessage("Hierarchy will be updated to \"" + updatedTeam.HierarchyType + "\".", true);
                }

                if (originalTeam.HierarchyItem != updatedTeam.HierarchyItem)
                {
                    wizardMessages.AddMessage(updatedTeam.HierarchyType + " value will be updated to \"" + updatedTeam.HierarchyItem + "\".", true);
                }
                if (originalTeam.TravelerTypeGuid != updatedTeam.TravelerTypeGuid)
                {
                    wizardMessages.AddMessage("TravelerType will be updated to \"" + updatedTeam.TravelerTypeName + "\".", true);
                }
            }

            if (teamChanges.SystemUsersAdded != null)
            {
                if (teamChanges.SystemUsersAdded.Count > 0)
                {
                    foreach (SystemUser item in teamChanges.SystemUsersAdded)
                    {
                        SystemUser systemUser = new SystemUser();
                        systemUser = systemUserRepository.GetUserBySystemUserGuid(item.SystemUserGuid);
                        if (systemUser != null)
                        {
                            wizardMessages.AddMessage("You will add User \"" + systemUser.LastName + "," + (systemUser.MiddleName != "" ? systemUser.MiddleName + " " : "") + systemUser.FirstName + "\".", true);
                        }
                    }
                }
            }
            if (teamChanges.SystemUsersRemoved != null)
            {
                if (teamChanges.SystemUsersRemoved.Count > 0)
                {
                    foreach (SystemUser item in teamChanges.SystemUsersRemoved)
                    {
                        SystemUser systemUser = new SystemUser();
                        systemUser = systemUserRepository.GetUserBySystemUserGuid(item.SystemUserGuid);
                        if (systemUser != null)
                        {
                            wizardMessages.AddMessage("You will remove User \"" + systemUser.LastName + "," + (systemUser.MiddleName != "" ? systemUser.MiddleName + " " : "") + systemUser.FirstName + "\".", true);
                        }
                    }
                }
            }
            if (teamChanges.ClientSubUnitsAdded != null)
            {
                if (teamChanges.ClientSubUnitsAdded.Count > 0)
                {
                    foreach (ClientSubUnitTeam item in teamChanges.ClientSubUnitsAdded)
                    {
                        ClientSubUnit clientSubUnit = new ClientSubUnit();
                        clientSubUnit = clientSubUnitRepository.GetClientSubUnit(item.ClientSubUnitGuid);
                        if (clientSubUnit != null)
                        {
                            wizardMessages.AddMessage("You will add ClientSubUnit \"" + clientSubUnit.ClientSubUnitName + "\".", true);
                        }
                    }
                }
            }
            if (teamChanges.ClientSubUnitsRemoved != null)
            {
                if (teamChanges.ClientSubUnitsRemoved.Count > 0)
                {
                    foreach (ClientSubUnitTeam item in teamChanges.ClientSubUnitsRemoved)
                    {
                        ClientSubUnit clientSubUnit = new ClientSubUnit();
                        clientSubUnit = clientSubUnitRepository.GetClientSubUnit(item.ClientSubUnitGuid);
                        if (clientSubUnit != null)
                        {
                            wizardMessages.AddMessage("You will remove ClientSubUnit \"" + clientSubUnit.ClientSubUnitName + "\".", true);
                        }
                    }
                }
            }

            if (teamChanges.ClientSubUnitsAltered != null)
            {
                if (teamChanges.ClientSubUnitsAltered.Count > 0)
                {
                    foreach (ClientSubUnitTeam item in teamChanges.ClientSubUnitsAltered)
                    {
                        ClientSubUnit clientSubUnit = new ClientSubUnit();
                        clientSubUnit = clientSubUnitRepository.GetClientSubUnit(item.ClientSubUnitGuid);
                        if (clientSubUnit != null)
                        {
                            wizardMessages.AddMessage("You will alter ClientSubUnit \"" + clientSubUnit.ClientSubUnitName + "\".", true);
                        }
                    }
                }
            }
            return wizardMessages;
        }

        //Add to DB
        public int AddTeam(Team team)
        {
             string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
             int? teamId = -1;

             HierarchyDC teamDC = new HierarchyDC(Settings.getConnectionString());
                teamDC.spDesktopDataAdmin_InsertTeam_v1(
                ref teamId, 
                team.TeamName,
                team.TeamEmail,
                team.TeamPhoneNumber,
                team.TeamQueue,
                team.TeamTypeCode,
                team.CityCode,
                team.HierarchyType,
                team.HierarchyCode,
                adminUserGuid
            );

                return (int)teamId;

        }

        //Update Team
        public void UpdateTeam(Team team)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            HierarchyDC teamDC = new HierarchyDC(Settings.getConnectionString());
            teamDC.spDesktopDataAdmin_UpdateTeam_v1(
                team.TeamId,
                team.TeamName,
                team.TeamEmail,
                team.TeamPhoneNumber,
                team.TeamQueue,
                team.TeamTypeCode,
                team.CityCode, 
                team.HierarchyType,
                team.HierarchyCode,
                adminUserGuid,
                team.VersionNumber
            );

        }

        //Update Team SystemUsers
        public WizardMessages UpdateTeamSystemUsers(TeamWizardVM teamChanges, WizardMessages wizardMessages)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            Team team = new Team();
            team = teamChanges.Team;
            bool changesExist = false;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("TeamSystemUsers");
            doc.AppendChild(root);

            SystemUserRepository systemUserRepository = new SystemUserRepository();
            if (teamChanges.SystemUsersAdded != null)
            {
                if (teamChanges.SystemUsersAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlSystemUsersAdded = doc.CreateElement("SystemUsersAdded");

                    foreach (SystemUser item in teamChanges.SystemUsersAdded)
                    {
                        SystemUser systemUser = new SystemUser();
                        systemUser = systemUserRepository.GetUserBySystemUserGuid(item.SystemUserGuid);
                        string systemUserName = systemUser.FirstName + (item.MiddleName != "" ? item.MiddleName + " " : "") + systemUser.LastName;

                        XmlElement xmlSystemUser = doc.CreateElement("SystemUser");
                        xmlSystemUsersAdded.AppendChild(xmlSystemUser);

                        XmlElement xmlSystemUserName = doc.CreateElement("SystemUserName");
                        xmlSystemUserName.InnerText = systemUserName;
                        xmlSystemUser.AppendChild(xmlSystemUserName);

                        XmlElement xmlSystemUserGuid  = doc.CreateElement("SystemUserGuid");
                        xmlSystemUserGuid.InnerText = item.SystemUserGuid;
                        xmlSystemUser.AppendChild(xmlSystemUserGuid);


                    }
                    root.AppendChild(xmlSystemUsersAdded);
                }
            }
            if (teamChanges.SystemUsersRemoved != null)
            {
                if (teamChanges.SystemUsersRemoved.Count > 0)
                {
                    changesExist = true;
                   // xml = xml + "<SystemUsersRemoved>";
                    XmlElement xmlSystemUsersRemoved = doc.CreateElement("SystemUsersRemoved");
                    foreach (SystemUser item in teamChanges.SystemUsersRemoved)
                    {
                        SystemUser systemUser = new SystemUser();
                        systemUser = systemUserRepository.GetUserBySystemUserGuid(item.SystemUserGuid);
                        string systemUserName = systemUser.FirstName + (item.MiddleName != "" ? item.MiddleName + " " : "") + systemUser.LastName;

                        XmlElement xmlSystemUser = doc.CreateElement("SystemUser");
                        xmlSystemUsersRemoved.AppendChild(xmlSystemUser);

                        XmlElement xmlSystemUserName = doc.CreateElement("SystemUserName");
                        xmlSystemUserName.InnerText = systemUserName;
                        xmlSystemUser.AppendChild(xmlSystemUserName);

                        XmlElement xmlSystemUserGuid = doc.CreateElement("SystemUserGuid");
                        xmlSystemUserGuid.InnerText = item.SystemUserGuid;
                        xmlSystemUser.AppendChild(xmlSystemUserGuid);
                    }
                    root.AppendChild(xmlSystemUsersRemoved);
                }
            }

            if (changesExist)
            {
                var output = (from n in db.spDDAWizard_UpdateTeamSystemUsers_v1(
                    team.TeamId,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateTeamSystemUsers_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }
            }
            return wizardMessages;
        }

        //Update Team ClientSubUnits
        public WizardMessages UpdateTeamClientSubUnits(TeamWizardVM teamChanges, WizardMessages wizardMessages)
        {
            Team team = new Team();
            team = teamChanges.Team;
            bool changesExist = false;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);// Create the root element
            XmlElement root = doc.CreateElement("TeamClientSubUnits");
            doc.AppendChild(root);

            ClientSubUnitTeamRepository clientSubUnitTeamRepository = new ClientSubUnitTeamRepository();
            ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();

            if (teamChanges.ClientSubUnitsAdded != null)
            {
                if (teamChanges.ClientSubUnitsAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlClientSubUnitsAdded = doc.CreateElement("ClientSubUnitsAdded");


                    foreach (ClientSubUnitTeam item in teamChanges.ClientSubUnitsAdded)
                    {
                        ClientSubUnit clientSubUnit = new ClientSubUnit();
                        clientSubUnit = clientSubUnitRepository.GetClientSubUnit(item.ClientSubUnitGuid);

                        XmlElement xmlClientSubUnit = doc.CreateElement("ClientSubUnit");
                        xmlClientSubUnitsAdded.AppendChild(xmlClientSubUnit);

                        XmlElement xmlClientSubUnitName = doc.CreateElement("ClientSubUnitName");
                        xmlClientSubUnitName.InnerText = clientSubUnit.ClientSubUnitName;
                        xmlClientSubUnit.AppendChild(xmlClientSubUnitName);

                        XmlElement xmlClientSubUnitGuid = doc.CreateElement("ClientSubUnitGuid");
                        xmlClientSubUnitGuid.InnerText = item.ClientSubUnitGuid;
                        xmlClientSubUnit.AppendChild(xmlClientSubUnitGuid);

                        XmlElement xmlIncludeInClientDroplistFlag = doc.CreateElement("IncludeInClientDroplistFlag");
                        xmlIncludeInClientDroplistFlag.InnerText = item.IncludeInClientDroplistFlag == true ? "1" : "0";
                        xmlClientSubUnit.AppendChild(xmlIncludeInClientDroplistFlag);


                        
                    }
                    root.AppendChild(xmlClientSubUnitsAdded);
                }
            }
            if (teamChanges.ClientSubUnitsRemoved != null)
            {
                if (teamChanges.ClientSubUnitsRemoved.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlClientSubUnitsRemoved = doc.CreateElement("ClientSubUnitsRemoved");

                    foreach (ClientSubUnitTeam item in teamChanges.ClientSubUnitsRemoved)
                    {
                        ClientSubUnit clientSubUnit = new ClientSubUnit();
                        clientSubUnit = clientSubUnitRepository.GetClientSubUnit(item.ClientSubUnitGuid);

                        XmlElement xmlClientSubUnit = doc.CreateElement("ClientSubUnit");
                        xmlClientSubUnitsRemoved.AppendChild(xmlClientSubUnit);

                        XmlElement xmlClientSubUnitName = doc.CreateElement("ClientSubUnitName");
                        xmlClientSubUnitName.InnerText = clientSubUnit.ClientSubUnitName;
                        xmlClientSubUnit.AppendChild(xmlClientSubUnitName);

                        XmlElement xmlClientSubUnitGuid = doc.CreateElement("ClientSubUnitGuid");
                        xmlClientSubUnitGuid.InnerText = item.ClientSubUnitGuid;
                        xmlClientSubUnit.AppendChild(xmlClientSubUnitGuid);



                    }
                    root.AppendChild(xmlClientSubUnitsRemoved);
                }
            }

            if (teamChanges.ClientSubUnitsAltered != null)
            {
                if (teamChanges.ClientSubUnitsAltered.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlClientSubUnitsAltered = doc.CreateElement("ClientSubUnitsAltered");

                    foreach (ClientSubUnitTeam item in teamChanges.ClientSubUnitsAltered)
                    {
                        ClientSubUnit clientSubUnit = new ClientSubUnit();
                        clientSubUnit = clientSubUnitRepository.GetClientSubUnit(item.ClientSubUnitGuid);

                        XmlElement xmlClientSubUnit = doc.CreateElement("ClientSubUnit");
                        xmlClientSubUnitsAltered.AppendChild(xmlClientSubUnit);

                        XmlElement xmlClientSubUnitName = doc.CreateElement("ClientSubUnitName");
                        xmlClientSubUnitName.InnerText = clientSubUnit.ClientSubUnitName;
                        xmlClientSubUnit.AppendChild(xmlClientSubUnitName);

                        XmlElement xmlClientSubUnitGuid = doc.CreateElement("ClientSubUnitGuid");
                        xmlClientSubUnitGuid.InnerText = item.ClientSubUnitGuid;
                        xmlClientSubUnit.AppendChild(xmlClientSubUnitGuid);

                        XmlElement xmlIncludeInClientDroplistFlag = doc.CreateElement("IncludeInClientDroplistFlag");
                        xmlIncludeInClientDroplistFlag.InnerText = item.IncludeInClientDroplistFlag == true ? "1" : "0";
                        xmlClientSubUnit.AppendChild(xmlIncludeInClientDroplistFlag);
                    }
                    root.AppendChild(xmlClientSubUnitsAltered);
                }
            }
            if (changesExist)
            {
                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

                var output = (from n in db.spDDAWizard_UpdateTeamClientSubUnits_v1(
                    team.TeamId,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateTeamClientSubUnits_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }
            }
            return wizardMessages;
        }

       
        //Delete From DB
        public void Delete(Team team)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            HierarchyDC teamDC = new HierarchyDC(Settings.getConnectionString());
            teamDC.spDesktopDataAdmin_DeleteTeam_v1(
                team.TeamId,
                adminUserGuid,
                team.VersionNumber
            );
        }

    }
}