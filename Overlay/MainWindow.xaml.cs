using System;
using System.Collections.Generic;
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

        iRacingSDK sdk = new iRacingSDK();

        public MainWindow()
        {


            InitializeComponent();

            DispatcherTimer mediaTimer = new DispatcherTimer();
            mediaTimer.Interval = TimeSpan.FromMilliseconds(5);
            mediaTimer.Tick += new EventHandler(mediaTimer_Tick);
            mediaTimer.Start();
        }

        void mediaTimer_Tick(object sender, EventArgs e)
        {
            FunctionThatDoesStuff();
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

            LeftIndicatorWidth.Width = new GridLength(int.Parse(IndicatorWidth),GridUnitType.Star);
            RightIndicatorWidth.Width = new GridLength(int.Parse(IndicatorWidth), GridUnitType.Star);

            CenterWidth.Width = new GridLength(int.Parse(SpaceBetweenIndicators)*2, GridUnitType.Star);
            LeftMargin.Width = new GridLength(int.Parse(SpaceToEdge), GridUnitType.Star);
            RightMargin.Width = new GridLength(int.Parse(SpaceToEdge), GridUnitType.Star);
        }

            public void FunctionThatDoesStuff()
        {
            if (sdk.IsConnected() == true)
            {
                if (sdk.GetData("CarLeftRight") != null)
                {
                    _carLR = sdk.GetData("CarLeftRight").ToString();
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
            else if (sdk.IsInitialized == true) sdk.Shutdown();
            else sdk.Startup();
        }

        

    }
}
