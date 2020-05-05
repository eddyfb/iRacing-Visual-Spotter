using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using iRacingSdkWrapper;
using iRSDKSharp;

namespace Overlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        public string _carLR { get; set; }

        Brush singleColor = Brushes.Yellow;
        Brush doubleColor = Brushes.Red;
        Brush middleColor = Brushes.Yellow;

        private SdkWrapper wrapper;

        public MainWindow()
        {


            InitializeComponent();

            // Create instance
            wrapper = new SdkWrapper();
            // Listen to events
            wrapper.TelemetryUpdated += OnTelemetryUpdated;
            wrapper.SessionInfoUpdated += OnSessionInfoUpdated;

            // Start it!
            wrapper.Start();
        }


        public void Settings(Color one, Color two, Color middle, Color nothing, String locationY, String locationX, String Height, String OverallWidth, String IndicatorWidth, String SpaceBetweenIndicators, String SpaceToEdge)
        {
            CarLeftIndicator.Fill = new SolidColorBrush(nothing);
            CarRightIndicator.Fill = new SolidColorBrush(nothing);

            singleColor = new SolidColorBrush(one);
            doubleColor = new SolidColorBrush(two);
            middleColor = new SolidColorBrush(middle);

            this.Left = int.Parse(locationX);
            this.Top = int.Parse(locationY);
            this.Height = int.Parse(Height);
            this.Width = int.Parse(OverallWidth);

            LeftIndicatorWidth.Width = new GridLength(int.Parse(IndicatorWidth), GridUnitType.Star);
            RightIndicatorWidth.Width = new GridLength(int.Parse(IndicatorWidth), GridUnitType.Star);

            CenterWidth.Width = new GridLength(int.Parse(SpaceBetweenIndicators) * 2, GridUnitType.Star);
            LeftMargin.Width = new GridLength(int.Parse(SpaceToEdge), GridUnitType.Star);
            RightMargin.Width = new GridLength(int.Parse(SpaceToEdge), GridUnitType.Star);
        }



        private void OnSessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            // Use session info...
        }
        private void OnTelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            if (wrapper.GetData("CarLeftRight") != null)
            {
                _carLR = wrapper.GetData("CarLeftRight").ToString();
                if (_carLR == null)
                {
                    //do nothing... shouldn't get here
                }
                else if (_carLR == "0")
                {
                    //do nothing... spotter disabled
                }
                else if (_carLR == "1")
                {
                    //All clear
                    CarLeftIndicator.Fill = Brushes.Transparent;
                    CarRightIndicator.Fill = Brushes.Transparent;
                }
                else if (_carLR == "2")
                {
                    //1 car left
                    CarLeftIndicator.Fill = singleColor;
                }
                else if (_carLR == "3")
                {
                    //1 car right
                    CarRightIndicator.Fill = singleColor;
                }
                else if (_carLR == "4")
                {
                    //cars on each side
                    CarLeftIndicator.Fill = middleColor;
                    CarRightIndicator.Fill = middleColor;
                }
                else if (_carLR == "5")
                {
                    //2(+?) cars left
                    CarLeftIndicator.Fill = doubleColor;

                }
                else if (_carLR == "6")
                {
                    //2(+?) cars right
                    CarRightIndicator.Fill = doubleColor;
                }
            }
        }

    }
}
