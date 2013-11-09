using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PanoramaApp1.Resources;
using PanoramaApp1.Utilities;

namespace PanoramaApp1.ViewModels
{
    public class BMIBoundariesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<BMILimit> Items { get; private set; }
        public bool IsDataLoaded { get; private set; }

        public BMIBoundariesViewModel()
        {
            Items = new ObservableCollection<BMILimit>();
            Prop = null;
        }

        private BMILimit _prop;

        public BMILimit Prop
        {
            get { return _prop; }
            set
            {
                if (value != _prop)
                {
                    _prop = value;
                    NotifyPropertyChanged("Prop");
                }
                
            }
        }
        
        public void LoadData()
        {
            Items.Add(new BMILimit() { Description = AppResources.BMILimitLvl_0, LowBoundary = 0, UpBoundary = 14.99M, Color = "#99FF0000", Info = AppResources.BMILimitLvl_0_Info });
            Items.Add(new BMILimit() { Description = AppResources.BMILimitLvl_1, LowBoundary = 15, UpBoundary = 15.99M, Color = "#ccFA9A50", Info = AppResources.BMILimitLvl_1_Info });
            Items.Add(new BMILimit() { Description = AppResources.BMILimitLvl_2, LowBoundary = 16, UpBoundary = 18.49M, Color = "#bbFFE600", Info = AppResources.BMILimitLvl_2_Info });
            Items.Add(new BMILimit() { Description = AppResources.BMILimitLvl_3, LowBoundary = 18.5M, UpBoundary = 24.99M, Color = "#777FFF00", Info = AppResources.BMILimitLvl_3_Info });
            Items.Add(new BMILimit() { Description = AppResources.BMILimitLvl_4, LowBoundary = 25, UpBoundary = 29.99M, Color = "#bbFFE600", Info = AppResources.BMILimitLvl_4_Info });
            Items.Add(new BMILimit() { Description = AppResources.BMILimitLvl_5, LowBoundary = 30, UpBoundary = 34.99M, Color = "#ccFA9A50", Info = AppResources.BMILimitLvl_5_Info });
            Items.Add(new BMILimit() { Description = AppResources.BMILimitLvl_6, LowBoundary = 35, UpBoundary = 39.99M, Color = "#99FF0000", Info = AppResources.BMILimitLvl_6_Info });
            Items.Add(new BMILimit() { Description = AppResources.BMILimitLvl_7, LowBoundary = 40, UpBoundary = Decimal.MaxValue, Color = "#99FF0000", Info = AppResources.BMILimitLvl_7_Info });
            
            IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
