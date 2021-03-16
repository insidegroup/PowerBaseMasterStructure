using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Helpers
{
    /*
    * This is for paging, it lets us know if there should be Next/Previous links on the page
    * It also lets us know the PageCount
    * Page size is 15 records
    * This is used in conjuction with SQL Stored procedures which have paging too and proc will return 16 records (lets us know if next page)
    * 
    * some pages use PaginatedList.cs, used when list page is populated by SQL function which returns ALL records
    */
    public class CWTPaginatedList<T> : List<T>
    {

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        //used for sequencing pages (page size is normally 50)
        public CWTPaginatedList(List<T> source, int pageIndex, int totalRecords, int pageSize)
        {
           
            PageSize = (int)pageSize;
            PageIndex = pageIndex;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);
            if (PageIndex > TotalPages)
            {
                PageIndex = TotalPages;
            }
            this.AddRange(source.Take(PageSize));

        }

        //Used for most pages, page size is 15
        public CWTPaginatedList(List<T> source, int pageIndex, int totalRecords)
        {
            PageSize = 15; //ALSO SPECIFIED IN STORED PROCS, MUST MATCH
            PageIndex = pageIndex;
            TotalCount = source.Count(); //16 OR LESS
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);
            if (PageIndex > TotalPages)
            {
                PageIndex = TotalPages;
            }
            this.AddRange(source.Take(PageSize));

        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (TotalCount > PageSize);
            }
        }
    }
}
