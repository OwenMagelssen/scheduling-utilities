using System;

namespace SchedulingUtilities
{
    public class TimeOffRequestReportSorter
    {
        private readonly TimeOffRequestReport _report;
        
        public TimeOffRequestReportSorter(TimeOffRequestReport report)
        {
            this._report = report;
        }
        
        public enum SortType
        {
            Name,
            NameReverse,
            Title,
            TitleReverse,
            TimeOffStart,
            TimeOffStartReverse,
            Hours,
            HoursReverse,
            DateTimeRequested,
            DateTimeRequestedReverse,
            Status,
            StatusReverse
        }

        public event Action<SortType> OnSorted;
        
        public SortType CurrentSortType => _currentSortType;
        private SortType _currentSortType;

        public void SortByName()
        {
            if (_currentSortType == SortType.Name)
            {
                _report.SortByNameReverse();
                _currentSortType = SortType.NameReverse;
            }
            else
            {
                _report.SortByName();
                _currentSortType = SortType.Name;
            }
            
            OnSorted?.Invoke(_currentSortType);
        }

        public void SortByTitle()
        {
            if (_currentSortType == SortType.Title)
            {
                _report.SortByTitleReverse();
                _currentSortType = SortType.TitleReverse;
            }
            else
            {
                _report.SortByTitle();
                _currentSortType = SortType.Title;
            }
            
            OnSorted?.Invoke(_currentSortType);
        }

        public void SortByTimeOffStart()
        {
            if (_currentSortType == SortType.TimeOffStart)
            {
                _report.SortByTimeOffStartReverse();
                _currentSortType = SortType.TimeOffStartReverse;
            }
            else
            {
                _report.SortByTimeOffStart();
                _currentSortType = SortType.TimeOffStart;
            }
            
            OnSorted?.Invoke(_currentSortType);
        }

        public void SortByHours()
        {
            if (_currentSortType == SortType.Hours)
            {
                _report.SortByHoursReverse();
                _currentSortType = SortType.HoursReverse;
            }
            else
            {
                _report.SortByHours();
                _currentSortType = SortType.Hours;
            }
            
            OnSorted?.Invoke(_currentSortType);
        }

        public void SortByDateTimeRequested()
        {
            if (_currentSortType == SortType.DateTimeRequested)
            {
                _report.SortByDateTimeRequestedReverse();
                _currentSortType = SortType.DateTimeRequestedReverse;
            }
            else
            {
                _report.SortByDateTimeRequested();
                _currentSortType = SortType.DateTimeRequested;
            }
            
            OnSorted?.Invoke(_currentSortType);
        }
        
        public void SortByStatus()
        {
            if (_currentSortType == SortType.Status)
            {
                _report.SortByStatusReverse();
                _currentSortType = SortType.StatusReverse;
            }
            else
            {
                _report.SortByStatus();
                _currentSortType = SortType.Status;
            }
            
            OnSorted?.Invoke(_currentSortType);
        }
    }
}
