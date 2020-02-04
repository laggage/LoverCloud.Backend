namespace LoverCloud.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class PaginatedList<T>: List<T>
    {
        public PaginatedList()
        {
        }

        public PaginatedList(int pageIndex, int pageSize, int totalItemsCount, IEnumerable<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItemsCount = totalItemsCount;
            AddRange(data);
        }

        public int TotalItemsCount { get; set; }
        /// <summary>
        /// 当前页码, 从 1 开始
        /// </summary>
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int PageCount
        {
            get => (TotalItemsCount / PageSize) + 
                   (TotalItemsCount % PageSize > 0 ? 1 : 0);
        }

        public bool HasNext => PageIndex < PageCount;
        public bool HasPrevious => PageIndex > 1;
    }
}
