using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleBlog.Infrastructure
{
    public class PagedData<T>:IEnumerable<T>
    {
        private IEnumerable<T> _currentItems;
        public int TotalCount { get; private set; }
        public int Page { get; private set; }
        public int PerPage { get; private set; }
        public int TotalPage { get; private set; }

        public bool HasNextpage { get; private set; }
        public bool HasPreviouspage { get; private set; }

        public int NextPage
        {
            get
            {
                if (!HasNextpage)
                    throw new InvalidOperationException();
                return Page + 1;
            }
        }

        public int PreviousPage
        {
            get
            {
                if (!HasPreviouspage)
                    throw new InvalidOperationException();
                return Page - 1;
            }
        }

        public PagedData(IEnumerable<T> currentItems, int totalCount, int page, int perPage)
        {
            _currentItems = currentItems;
            TotalCount = totalCount;
            Page = page;
            PerPage = perPage;
            TotalPage=(int) Math.Ceiling((float) TotalCount/PerPage);
            HasNextpage=Page<TotalPage;
            HasPreviouspage=Page>1;

            }


        public IEnumerator<T> GetEnumerator()
        {
            return _currentItems.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}