using System.Windows;

namespace CSI.BatchTracker.ViewModels
{
    public class SearchCriteriaVisibilityManager : ViewModelBase
    {
        public enum ActiveCriteriaPanel
        {
            DateRange,
            DatePeriod,
            SpecificDate,
            PONumber
        }

        public SearchCriteriaVisibilityManager()
        {
            DateRangeCriteriaVisibility = Visibility.Visible;
            DatePeriodCriteriaVisibility = Visibility.Collapsed;
            SpecificDateCriteriaVisibility = Visibility.Collapsed;
            PONumberCriteriaVisibility = Visibility.Collapsed;
        }

        Visibility dateRangeCriteriaVisibility;
        public Visibility DateRangeCriteriaVisibility
        {
            get { return dateRangeCriteriaVisibility; }
            set
            {
                dateRangeCriteriaVisibility = value;
                NotifyPropertyChanged("DateRangeCriteriaVisibility");
            }
        }

        Visibility datePeriodCriteriaVisibility;
        public Visibility DatePeriodCriteriaVisibility
        {
            get { return datePeriodCriteriaVisibility; }
            set
            {
                datePeriodCriteriaVisibility = value;
                NotifyPropertyChanged("DatePeriodCriteriaVisibility");
            }
        }

        Visibility specificDateCriteriaVisibility;
        public Visibility SpecificDateCriteriaVisibility
        {
            get { return specificDateCriteriaVisibility; }
            set
            {
                specificDateCriteriaVisibility = value;
                NotifyPropertyChanged("SpecificDateCriteriaVisibility");
            }
        }

        Visibility poNumberCriteriaVisibility;
        public Visibility PONumberCriteriaVisibility
        {
            get { return poNumberCriteriaVisibility; }
            set
            {
                poNumberCriteriaVisibility = value;
                NotifyPropertyChanged("PONumberCriteriaVisibility");
            }
        }

        public void SetVisibility(ActiveCriteriaPanel activePanel)
        {
            if (activePanel == ActiveCriteriaPanel.DateRange)
            {
                DateRangeCriteriaVisibility = Visibility.Visible;
                DatePeriodCriteriaVisibility = Visibility.Collapsed;
                SpecificDateCriteriaVisibility = Visibility.Collapsed;
                PONumberCriteriaVisibility = Visibility.Collapsed;
            }

            if (activePanel == ActiveCriteriaPanel.DatePeriod)
            {
                DateRangeCriteriaVisibility = Visibility.Collapsed;
                DatePeriodCriteriaVisibility = Visibility.Visible;
                SpecificDateCriteriaVisibility = Visibility.Collapsed;
                PONumberCriteriaVisibility = Visibility.Collapsed;
            }

            if (activePanel == ActiveCriteriaPanel.SpecificDate)
            {
                DateRangeCriteriaVisibility = Visibility.Collapsed;
                DatePeriodCriteriaVisibility = Visibility.Collapsed;
                SpecificDateCriteriaVisibility = Visibility.Visible;
                PONumberCriteriaVisibility = Visibility.Collapsed;
            }

            if (activePanel == ActiveCriteriaPanel.PONumber)
            {
                DateRangeCriteriaVisibility = Visibility.Collapsed;
                DatePeriodCriteriaVisibility = Visibility.Collapsed;
                SpecificDateCriteriaVisibility = Visibility.Collapsed;
                PONumberCriteriaVisibility = Visibility.Visible;
            }
        }
    }
}
