namespace LoverCloud.Core.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public abstract class QueryParameters : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private const int DefaultPageSize = 10;
        private const int DefaultMaxPageSize = 100;

        private int _pageIndex = 1;
        public virtual int PageIndex
        {
            get => _pageIndex;
            set => SetField(ref _pageIndex, value);
        }

        private int _pageSize = DefaultPageSize;
        public virtual int PageSize
        {
            get => _pageSize;
            set => SetField(ref _pageSize, value);
        }

        private int _maxPageSize = DefaultMaxPageSize;
        public virtual int MaxPageSize
        {
            get => _maxPageSize;
            set => SetField(ref _maxPageSize, value);
        }

        public string OrderBy { get; set; }
        public string Fields { get; set; }

        protected void SetField<TField>(
            ref TField field, in TField newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TField>.Default.Equals(field, newValue))
                return;
            field = newValue;
            if (propertyName == nameof(PageSize) || 
                propertyName == nameof(MaxPageSize)) SetPageSize();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetPageSize()
        {
            if (_maxPageSize <= 0) _maxPageSize = DefaultMaxPageSize;
            if (_pageSize <= 0) _pageSize = DefaultPageSize;
            _pageSize = _pageSize > _maxPageSize ? _maxPageSize : _pageSize;
        }
    }
}
