using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AWA.Util.Paging;
namespace AWA.Util.Extensions
{
    public static class IQueryableExtends
    {
        /// <summary>
        /// 对其进行分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public static PagingModel<T> GetPageList<T>(this IQueryable<T> query,int pageSize, int pageIndex)
        {
            
            int count = query.Count();
            var list = query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            var paginModel = new PagingModel<T>()
            {
                Count = count,
                Items = list,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return paginModel;
        }
    }
}
