using System;
using UnityEngine;
using UnityEngine.UI;
using SortType = SchedulingUtilities.TimeOffRequestReportSorter.SortType;
using Column = SchedulingUtilities.TestGUI.Column;

namespace SchedulingUtilities
{
    public class SortIndicator : MonoBehaviour
    {
        public Image indicatorImage;

        private Column _column;
        private RectTransform _indicatorRT;

        private void Awake()
        {
            _indicatorRT = indicatorImage.GetComponent<RectTransform>();
        }

        public void Setup(Column column)
        {
            _column = column;
        }

        public void SetIndicator(SortType sortType)
        {
            switch (_column)
            {
                case Column.Name:
                    HandleIndicatorSet(sortType, SortType.Name, SortType.NameReverse);
                    break;
                case Column.Title:
                    HandleIndicatorSet(sortType, SortType.Title, SortType.TitleReverse);
                    break;
                case Column.Start:
                    HandleIndicatorSet(sortType, SortType.TimeOffStart, SortType.TimeOffStartReverse);
                    break;
                case Column.Hours:
                    HandleIndicatorSet(sortType, SortType.Hours, SortType.HoursReverse);
                    break;
                case Column.Requested:
                    HandleIndicatorSet(sortType, SortType.DateTimeRequested, SortType.DateTimeRequestedReverse);
                    break;
                case Column.Status:
                    HandleIndicatorSet(sortType, SortType.Status, SortType.StatusReverse);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleIndicatorSet(SortType sortType, SortType forwards, SortType reverse)
        {
            if (sortType == forwards)
                SetIndicatorTopToBottom();
            else if (sortType == reverse)
                SetIndicatorBottomToTop();
            else 
                SetIndicatorOff();
        }

        private void SetIndicatorTopToBottom()
        {
            _indicatorRT.gameObject.SetActive(true);
            _indicatorRT.localRotation = Quaternion.Euler(0, 0, 180);
        }

        private void SetIndicatorBottomToTop()
        {
            _indicatorRT.gameObject.SetActive(true);
            _indicatorRT.localRotation = Quaternion.identity;
        }

        private void SetIndicatorOff()
        {
            _indicatorRT.gameObject.SetActive(false);
        }
    }
}
