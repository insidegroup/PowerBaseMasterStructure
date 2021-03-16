using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Helpers
{
    /*
   * This is for paging, it lets us know if there should be Next/Previous links on the page
   * It also lets us know the PageCount
   * Page size is passed in
   * used when list page is populated by SQL function which returns ALL records
   *
   * 
   * some pages use CWTPaginatedList.cs,  This is used in conjuction with SQL Stored procedures which have paging
   * 
   * TO DO: remove passed in pagesize and set here, all pages hae one size so better to set in one place
   */
    public class PaginatedList<T> : List<T>
    {

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            
              
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            this.AddRange(source.Skip(PageIndex * PageSize).Take(PageSize));
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex + 1 < TotalPages);
            }
        }
    }
}
