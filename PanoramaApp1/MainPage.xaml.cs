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
using PanoramaApp1.Resources;
using System.Text;

namespace PanoramaApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const string _xmlBMIFileName = "bmiHistory.xml";

        public MainPage()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
            DataContext = App.BMIValuesViewModel;
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            var historyOfBMIButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/bmi.history.png", UriKind.RelativeOrAbsolute));
            historyOfBMIButton.Click += BMIHistory_Click;
            historyOfBMIButton.Text = AppResources.ApBar_HistoryButtonText;
            ApplicationBar.Buttons.Add(historyOfBMIButton);

            var aboutAppButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/info.png", UriKind.RelativeOrAbsolute));
            aboutAppButton.Click += BMIAbout_Click;
            aboutAppButton.Text = AppResources.ApBar_AboutButtonText;
            ApplicationBar.Buttons.Add(aboutAppButton);
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.BMIValuesViewModel.IsDataLoaded)
            {
                App.BMIValuesViewModel.LoadData();
            }
        }

        private async void CalcBMIButton_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = false;

            BMIValidationOutput validationOutput = BMICalculator.ValidateInputText(TxtBoxBMIHeight.Text, TxtBoxBMIWeight.Text);

            if (validationOutput.Error == null)
            {
                CalcBMIClickAnimationStart.Begin();

                BMICalculator bmiCalc = new BMICalculator(validationOutput, App.BMIValuesViewModel.Items);
                BMILimit bmiLimit = bmiCalc.ShowLimitList.FirstOrDefault(x => x.Equals(bmiCalc.ShowLimit));

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

                await IsolatedStorageOps.Append(
                    new BMIHistoryModel {
                        Date = DateTime.Now,
                        Value = bmiCalc.Value },
                    _xmlBMIFileName);
            }
            else
            {
                MessageBox.Show(validationOutput.Error);
            }

            
            ((Button)sender).IsEnabled = true;
            (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
        }

        private async void BMIHistory_Click(object sender, EventArgs e)
        {
            var list = await IsolatedStorageOps.Load<List<BMIHistoryModel>>(_xmlBMIFileName);
            var output = new StringBuilder();

            if (list.Count == 0)
            {
                output.Append(AppResources.BMIHistoryIsEmptyPlaceholder);
            }
            else
            {
                int i = 0;

                list.Sort((x, y) => y.Date.CompareTo(x.Date));
                list.ForEach(x => output.Append(String.Format(
                    "{0,2}. {1}\t{2}\n",
                    ++i,
                    x.Date.ToShortDateString(),
                    x.Value)));
            }
            
            MessageBox.Show(output.ToString(), AppResources.BMIHistoryWindowTitle, MessageBoxButton.OK);
        }

        private void BMIAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                String.Format("{0}\n{1}\n\n{2}",
                    AppResources.DesignedAndCreatedBySentence,
                    AppResources.CreatorNameAndSurname,
                    AppResources.CreatorEmail),
                String.Format("{0} {1}",
                    AppResources.AboutSentence,
                    AppResources.ApplicationTitle),
                MessageBoxButton.OK);
        }
    }
}