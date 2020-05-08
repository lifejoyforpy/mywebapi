using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace MyWebApi.Core.Model
{
    public interface IPagedListModel< T>
    {
        IList<T> Data { get; set; }

        /// <summary>
        /// page size
        /// </summary>
        int PageSize { get;}
        /// <summary>
        /// page index
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// total count
        /// </summary>
        int TotalCount { get; set; }
        /// <summary>
        /// page count
        /// </summary>
        int PageCount { get; set; }
        /// <summary>
        /// data count 
        /// </summary>
        int Count { get; set; }

    }
    public class PagedListModel<T> : IPagedListModel<T>
    {
        private IList<T> _data = new List<T>();
        public IList<T> Data { get => _data;
            set
            {
                if (value != null) _data = value;
            } }

        private int _pageSize = 10;
        public int PageSize  { 
            get => _pageSize;
            set {
                if (value > 0)
                    _pageSize = value;
            }
        }
        private int _pageIndex = 1;
        public int PageIndex {
            get => _pageIndex;
            set {
                if (value > 0) _pageIndex = value;
            }
         }
        private int _totalCount ;
        public int TotalCount {
            get => _totalCount;
            set {
                if (value > 0)
                    _totalCount = value;
            }
        }
        public int PageCount => _totalCount / _pageSize + 1;

        public int Count => _data.Count;

        int IPagedListModel<T>.PageCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IPagedListModel<T>.Count { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public T this[int index]=> _data[index];
    }
}
