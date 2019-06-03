using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        Visibility dateRangeCriteriaVisibility;
        public Visibility DateRangeCriteriaVisibility
        {
            get { return dateRangeCriteriaVisibility; }
            set
            {
                dateRangeCriteriaVisibility = value;
                datePeriodCriteriaVisibility = Visibility.Collapsed;
                specificDateCriteriaVisibility = Visibility.Collapsed;
                poNumberCriteriaVisibility = Visibility.Collapsed;
                NotifyVisibilityChanges();
            }
        }

        Visibility datePeriodCriteriaVisibility;
        public Visibility DatePeriodCriteriaVisibility
        {
            get { return datePeriodCriteriaVisibility; }
            set
            {
                datePeriodCriteriaVisibility = value;
                dateRangeCriteriaVisibility = Visibility.Collapsed;
                specificDateCriteriaVisibility = Visibility.Collapsed;
                poNumberCriteriaVisibility = Visibility.Collapsed;
                NotifyVisibilityChanges();
            }
        }

        Visibility specificDateCriteriaVisibility;
        public Visibility SpecificDateCriteriaVisibility
        {
            get { return specificDateCriteriaVisibility; }
            set
            {
                specificDateCriteriaVisibility = value;
                dateRangeCriteriaVisibility = Visibility.Collapsed;
                datePeriodCriteriaVisibility = Visibility.Collapsed;
                poNumberCriteriaVisibility = Visibility.Collapsed;
                NotifyVisibilityChanges();
            }
        }

        Visibility poNumberCriteriaVisibility;
        public Visibility PONumberCriteriaVisibility
        {
            get { return poNumberCriteriaVisibility; }
            set
            {
                poNumberCriteriaVisibility = value;
                dateRangeCriteriaVisibility = Visibility.Collapsed;
                datePeriodCriteriaVisibility = Visibility.Collapsed;
                specificDateCriteriaVisibility = Visibility.Collapsed;
                NotifyVisibilityChanges();
            }
        }

        void NotifyVisibilityChanges()
        {
            NotifyPropertyChanged("DateRangeCriteraVisibility");
            NotifyPropertyChanged("DatePeriodCriteraVisibility");
            NotifyPropertyChanged("SpecificDateCriteraVisibility");
            NotifyPropertyChanged("PONumberCriteraVisibility");
        }

        public void SetVisibility(ActiveCriteriaPanel activePanel)
        {
            if (activePanel == ActiveCriteriaPanel.DateRange) DateRangeCriteriaVisibility = Visibility.Visible;
            if (activePanel == ActiveCriteriaPanel.DatePeriod) DatePeriodCriteriaVisibility = Visibility.Visible;
            if (activePanel == ActiveCriteriaPanel.SpecificDate) SpecificDateCriteriaVisibility = Visibility.Visible;
            if (activePanel == ActiveCriteriaPanel.PONumber) PONumberCriteriaVisibility = Visibility.Visible;
        }
    }
}
