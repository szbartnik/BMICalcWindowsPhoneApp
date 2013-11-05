using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PanoramaApp1.Utilities
{
    public class BMILimit : INotifyPropertyChanged
    {
        public string Description { get; set; }
        public decimal LowBoundary { get; set; }
        public decimal UpBoundary { get; set; }
        public string Color { get; set; }

        private string _currentColor;

        public string CurrentColor
        {
            get { return _currentColor; }
            set
            {
                if (value != _currentColor)
                {
                    _currentColor = value;
                    NotifyPropertyChanged("CurrentColor");
                }
            }
        }
        

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    NotifyPropertyChanged("IsSelected");
                }
            }
        }

        public string FullRangeForView
        {
            get
            {
                if (LowBoundary == 0)
                {
                    return "\u2264 " + UpBoundary;
                }
                else if (UpBoundary == Decimal.MaxValue)
                {
                    return "\u2265 " + LowBoundary;
                }
                else
                {
                    return String.Format("{0} - {1}", LowBoundary, UpBoundary);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
