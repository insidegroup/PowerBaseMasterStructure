using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;


namespace CWTDesktopDatabase.Repository
{
    public class WorkFlowItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of Workflow Items - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectWorkFlowItems_v1Result> PageWorkFlowItems(int workFlowGroupId, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectWorkFlowItems_v1(workFlowGroupId, filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectWorkFlowItems_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

      

        //Get one Item
        public WorkFlowItem GetItem(int workFlowGroupId, int formId)
        {
            return db.WorkFlowItems.SingleOrDefault(c => (c.WorkFlowGroupId == workFlowGroupId)
                    && (c.FormId == formId));
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(WorkFlowItem workFlowItem)
        {
            //Add Form INfo
            FormRepository formRepository = new FormRepository();
            Form form = new Form();
            form = formRepository.GetForm(workFlowItem.FormId);
            if (form != null)
            {
                workFlowItem.FormName = form.FormName;
            }

            //Add PolicyGroupName
            WorkFlowGroupRepository workFlowGroupRepository = new WorkFlowGroupRepository();
            WorkFlowGroup workFlowGroup = new WorkFlowGroup();
            workFlowGroup = workFlowGroupRepository.GetGroup(workFlowItem.WorkFlowGroupId);
            if (workFlowGroup != null)
            {
                workFlowItem.WorkFlowGroupName = workFlowGroup.WorkFlowGroupName;
            }

        }
    }
}
