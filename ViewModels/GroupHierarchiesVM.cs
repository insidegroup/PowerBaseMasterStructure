using System.Collections.Generic;


namespace CWTDesktopDatabase.ViewModels
{
    public class GroupHierarchiesVM : CWTBaseViewModel
    {
        public List<GroupHierarchyVM> Hierarchies { get; set; }      

        public GroupHierarchiesVM()
        {
        }
        public GroupHierarchiesVM(List<GroupHierarchyVM> hierarchies)
        {
            Hierarchies = hierarchies;
        }
    }
}