using System;
using System.Collections.Generic;
using System.Text;

namespace AWA.Util.Paging
{
    public static class IEnumerableExtend
    {
        public static PagingModel<T> ToPager<T>(this IEnumerable<T> sourece,int pageSize,int pageIndex,int count)
        {
            var model = new PagingModel<T>();
            model.PageIndex = pageIndex;
            model.PageSize = pageSize;
            model.Count = count;
            model.Items = sourece;
            return model;
        }
    }
}
