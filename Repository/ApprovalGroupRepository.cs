using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Repository
{
    public class ApprovalGroupRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of Approval Groups - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectApprovalGroups_v1Result> PageApprovalGroups(bool deleted, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectApprovalGroups_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectApprovalGroups_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Page of Approval Groups (Orphaned) - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectApprovalGroupsOrphaned_v1Result> PageOrphanedApprovalGroups(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectApprovalGroupsOrphaned_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectApprovalGroupsOrphaned_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Approval Group
        public ApprovalGroup GetGroup(int id)
        {
            return db.ApprovalGroups.SingleOrDefault(c => c.ApprovalGroupId == id);
        }

        //Change the deleted status on an item
        public void UpdateGroupDeletedStatus(ApprovalGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateApprovalGroupDeletedStatus_v1(
                    group.ApprovalGroupId,
                    group.DeletedFlag,
                    adminUserGuid,
                    group.VersionNumber
                    );

        }

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(ApprovalGroup group)
        {

            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            TripType tripType = new TripType();
            tripType = tripTypeRepository.GetTripType(group.TripTypeId);
            if (tripType != null)
            {
                group.TripType = tripType;
            }

            group.ApprovalGroupName = Regex.Replace(group.ApprovalGroupName, @"[^\w\-()*]", "-");

            List<fnDesktopDataAdmin_SelectApprovalGroupHierarchy_v1Result> hierarchy = new List<fnDesktopDataAdmin_SelectApprovalGroupHierarchy_v1Result>();
            hierarchy = GetGroupHierarchy(group.ApprovalGroupId);

            if (hierarchy.Count > 0)
            {
                HierarchyRepository hierarchyRepository = new HierarchyRepository();
                HierarchyGroup hierarchyGroup = hierarchyRepository.GetHierarchyGroup(
                    hierarchy[0].HierarchyType ?? "",
                    hierarchy[0].HierarchyCode ?? "",
                    hierarchy[0].HierarchyName ?? "",
                    hierarchy[0].TravelerTypeGuid ?? "",
                    hierarchy[0].TravelerTypeName ?? "",
                    hierarchy[0].SourceSystemCode ?? ""
                );

                if (hierarchyGroup != null)
                {
                    group.HierarchyType = hierarchyGroup.HierarchyType;
                    group.HierarchyCode = hierarchyGroup.HierarchyCode;
                    group.HierarchyItem = hierarchyGroup.HierarchyItem;
                    group.ClientSubUnitGuid = hierarchyGroup.ClientSubUnitGuid;
                    group.ClientSubUnitName = hierarchyGroup.ClientSubUnitName;
                    group.TravelerTypeGuid = hierarchyGroup.TravelerTypeGuid;
                    group.TravelerTypeName = hierarchyGroup.TravelerTypeName;
                    group.ClientTopUnitName = hierarchyGroup.ClientTopUnitName;
                    group.SourceSystemCode = hierarchyGroup.SourceSystemCode;
                }
            }

            if (hierarchy.Count > 1)
            {
                group.IsMultipleHierarchy = true;
                group.HierarchyType = "Multiple";
                group.HierarchyItem = "Multiple";
                group.HierarchyCode = "Multiple";
            }
            else
            {
                group.IsMultipleHierarchy = false;
            }

            //ApprovalGroupApprovalTypeItems
            ApprovalGroupApprovalTypeRepository approvalGroupApprovalTypeRepository = new ApprovalGroupApprovalTypeRepository();
            if (group.ApprovalGroupApprovalTypeItems != null && group.ApprovalGroupApprovalTypeItems.Count > 0)
            {
                foreach (ApprovalGroupApprovalTypeItem approvalGroupApprovalTypeItems in group.ApprovalGroupApprovalTypeItems)
                {
                    approvalGroupApprovalTypeItems.ApprovalGroupApprovalTypes = new SelectList(approvalGroupApprovalTypeRepository.GetAllApprovalGroupApprovalTypes().ToList(), "ApprovalGroupApprovalTypeId", "ApprovalGroupApprovalTypeDescription", approvalGroupApprovalTypeItems.ApprovalGroupApprovalTypeId);
                }
            }
        }

        //Get Hierarchy Details
        public List<fnDesktopDataAdmin_SelectApprovalGroupHierarchy_v1Result> GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectApprovalGroupHierarchy_v1(id);
            return result.ToList();
        }

        //Edit Group
        public void Edit(ApprovalGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            XmlDocument approvalGroupApprovalTypeItems = GetApprovalGroupApprovalTypeItemsToXML(group);

            db.spDesktopDataAdmin_UpdateApprovalGroup_v1(
                adminUserGuid,
                group.ApprovalGroupId,
                group.ApprovalGroupName,
                group.EnabledFlag,
                group.EnabledDate,
                group.ExpiryDate,
                group.InheritFromParentFlag,
                group.HierarchyType,
                group.HierarchyCode,
                group.TravelerTypeGuid,
                group.ClientSubUnitGuid,
                group.SourceSystemCode,
                group.IsMultipleHierarchy,
                System.Xml.Linq.XElement.Parse(approvalGroupApprovalTypeItems.OuterXml),
                adminUserGuid,
                group.VersionNumber
            );
        }

        //Add Group
        public void Add(ApprovalGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            XmlDocument approvalGroupApprovalTypeItems = GetApprovalGroupApprovalTypeItemsToXML(group);

            db.spDesktopDataAdmin_InsertApprovalGroup_v1(
                adminUserGuid,
                group.ApprovalGroupName,
                group.EnabledFlag,
                group.EnabledDate,
                group.ExpiryDate,
                group.InheritFromParentFlag,
                group.HierarchyType,
                group.HierarchyCode,
                group.TravelerTypeGuid,
                group.ClientSubUnitGuid,
                group.SourceSystemCode,
                System.Xml.Linq.XElement.Parse(approvalGroupApprovalTypeItems.OuterXml),
                adminUserGuid
            );
        }

        private XmlDocument GetApprovalGroupApprovalTypeItemsToXML(ApprovalGroup group)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ApprovalGroupApprovalTypeItems");
            doc.AppendChild(root);

            if (group.ApprovalGroupApprovalTypeItems != null)
            {
                foreach (ApprovalGroupApprovalTypeItem approvalGroupApprovalTypeItem in group.ApprovalGroupApprovalTypeItems)
                {
                    if (approvalGroupApprovalTypeItem != null)
                    {
                        if (approvalGroupApprovalTypeItem.ApprovalGroupApprovalTypeId != 0 && approvalGroupApprovalTypeItem.ApprovalGroupApprovalTypeItemValue != null)
                        {
                            XmlElement xmlApprovalGroupApprovalType = doc.CreateElement("ApprovalGroupApprovalTypeItem");

                            XmlElement xmlApprovalGroupApprovalTypeId = doc.CreateElement("ApprovalGroupApprovalTypeId");
                            xmlApprovalGroupApprovalTypeId.InnerText = approvalGroupApprovalTypeItem.ApprovalGroupApprovalTypeId.ToString();
                            xmlApprovalGroupApprovalType.AppendChild(xmlApprovalGroupApprovalTypeId);

                            XmlElement xmlApprovalGroupApprovalTypeItemValue = doc.CreateElement("ApprovalGroupApprovalTypeItemValue");
                            xmlApprovalGroupApprovalTypeItemValue.InnerText = approvalGroupApprovalTypeItem.ApprovalGroupApprovalTypeItemValue;
                            xmlApprovalGroupApprovalType.AppendChild(xmlApprovalGroupApprovalTypeItemValue);

                            root.AppendChild(xmlApprovalGroupApprovalType);
                        }
                    }
                }
            }

            return doc;
        }
    }
}
