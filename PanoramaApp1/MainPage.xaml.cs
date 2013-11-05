using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PanoramaApp1.Utilities;
using System.Threading;

namespace PanoramaApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.BMIValuesViewModel;
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.BMIValuesViewModel.IsDataLoaded)
            {
                App.BMIValuesViewModel.LoadData();
            }
        }

        private void CalcBMIButton_Click(object sender, RoutedEventArgs e)
        {
            BMIValidationOutput validationOutput = BMICalculator.ValidateInputText(TxtBoxBMIHeight.Text, TxtBoxBMIWeight.Text);

            if (validationOutput.Error == null)
            {
                CalcBMIClickAnimationStart.Begin();

                BMICalculator bmiCalc = new BMICalculator(validationOutput, App.BMIValuesViewModel.Items);
                BMILimit bmiLimit = bmiCalc.ShowLimitList.FirstOrDefault(s => s.Equals(bmiCalc.ShowLimit));

                if (App.BMIValuesViewModel.Prop != null)
                    App.BMIValuesViewModel.Prop.CurrentColor = null;
                App.BMIValuesViewModel.Prop = bmiLimit;

                bmiLimit.IsSelected = true;
                bmiLimit.CurrentColor = bmiLimit.Color;

                txtBMIResult.Text = bmiCalc.Value.ToString();

                CalcBMIClickAnimationEnd.Begin();
                Deployment.Current.Dispatcher.BeginInvoke(() => {
                    BMIList.ScrollTo(App.BMIValuesViewModel.Prop);
                });
            }
            else
            {
                MessageBox.Show(validationOutput.Error);
            }
        }
    }
}