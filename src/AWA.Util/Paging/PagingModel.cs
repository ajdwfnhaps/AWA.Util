using System;
using System.Collections.Generic;
using System.Text;

namespace AWA.Util.Paging
{
    public class PagingModel<T>
    {
        private int _pageIndex = 1;
        public PagingModel()
        {
            PageSize = 10;
            PageIndex = 1;
        }
        public int PageSize { get; set; }

        public int PageIndex
        {
            get
            {
                if (_pageIndex <= 1) return 1;
                return _pageIndex;
            }
            set
            {
                _pageIndex = value;
            }
        }

        public int Count { get; set; }

        public int PageTotal
        {
            get
            {
                if (Count <= 0) return 0;
                var mod = Count % PageSize;
                if (mod > 0)
                {
                    return (Count / PageSize) + 1;
                }
                return Count / PageSize;
            }
        }

        public IEnumerable<T> Items { get; set; }

    }
}
