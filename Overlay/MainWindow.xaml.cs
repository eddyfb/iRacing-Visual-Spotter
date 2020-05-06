using iRacingSimulator.Drivers;
using iRacingSimulator.Events;
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
using System.Collections.ObjectModel;
using iRacingSimulator;

namespace Overlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        private ObservableCollection<Driver> _drivers = new ObservableCollection<Driver>();

                public string _carLR { get; set; }

        double _trackDistPctFromKM = 0.05;

        double warnDistance = 0.1;//.1 KM

        Brush singleColor = Brushes.Yellow;
        Brush doubleColor = Brushes.Red;
        Brush middleColor = Brushes.Yellow;
        Brush classWarningColor = Brushes.Blue;

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
            Sim.Instance.Start();
            wrapper.Start();
        }



        public void Settings(Color one, Color two, Color middle, Color nothing, String locationY, String locationX, String Height, String OverallWidth, String IndicatorWidth, String SpaceBetweenIndicators, String SpaceToEdge, double FasterClassWarningDistance, Color FasterClassWarningColor)
        {
            warnDistance = FasterClassWarningDistance;

            CarLeftIndicator.Fill = new SolidColorBrush(nothing);
            CarRightIndicator.Fill = new SolidColorBrush(nothing);

            singleColor = new SolidColorBrush(one);
            doubleColor = new SolidColorBrush(two);
            middleColor = new SolidColorBrush(middle);

            classWarningColor = new SolidColorBrush(FasterClassWarningColor);

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
            _drivers.Clear();
            foreach (Driver driver in Sim.Instance.Drivers)
            {
                _drivers.Add(driver);
            }
            if (Sim.Instance.SessionData.Track != null)
            {
                double trackLength = Sim.Instance.SessionData.Track.Length;

                _trackDistPctFromKM = warnDistance / trackLength;
            }
            // Use session info...
        }
        private void OnTelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            CarLeftRightCheck(e);
            CarBehindRelativeSpeedCheck(e);
        }




        private void CarBehindRelativeSpeedCheck(SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            if (_drivers.Count > 2 && Sim.Instance.Driver != null)
            {
                Dictionary<Driver, float> driversAndPositons = new Dictionary<Driver, float>();
                List<Driver> activeDrivers = _drivers.ToList();
                if (e.TelemetryInfo.CarIdxLapDistPct.Value != null)
                {
                    List<float> posList = e.TelemetryInfo.CarIdxLapDistPct.Value.ToList();

                    //This info should be in Driver.Live.TrackSurface but it isn't showing up most of the time
                    TrackSurfaces[] driversTrackSurfaces = (TrackSurfaces[])wrapper.GetData("CarIdxTrackSurface");

                    foreach (Driver driver in activeDrivers)
                    {
                        if (((driversTrackSurfaces[driver.Id] != TrackSurfaces.InPitStall) && (driversTrackSurfaces[driver.Id] != TrackSurfaces.NotInWorld) && (driversTrackSurfaces[driver.Id] != TrackSurfaces.AproachingPits) | driver == Sim.Instance.Driver)) //Was getting duplicate entries for some reason.
                        {
                            driversAndPositons.Add(driver, posList[driver.Id]);
                        }
                    }

                    if (driversAndPositons != null && driversAndPositons.Count > 2)
                    {

                        var driversAndPositonsOrdered = (from pair in driversAndPositons orderby pair.Value descending select pair).ToList(); //Need to not include people on pit road.

                        int driverPos = driversAndPositonsOrdered.FindIndex(u => u.Key == Sim.Instance.Driver);
                        int yourClassSpeed = Sim.Instance.Driver.Car.CarClassRelSpeed;

                        if (driversAndPositonsOrdered.ElementAt(driversAndPositonsOrdered.Count - 1).Key == Sim.Instance.Driver) // If you are the most recent one to cross the S/F line get the next person who will cross the S/F.
                        {
                            Driver driverBehind = driversAndPositonsOrdered.ElementAt(0).Key;
                            int driverBehindClassSpeed = driverBehind.Car.CarClassRelSpeed;

                            if (yourClassSpeed < driverBehindClassSpeed && (1 + Sim.Instance.Driver.Live.LapDistance) - driverBehind.Live.LapDistance <= _trackDistPctFromKM)
                            {
                                FastClassApproachingIndicator1.Fill = classWarningColor;
                            }
                            else
                            {
                                FastClassApproachingIndicator1.Fill = Brushes.Transparent;
                            }
                        }
                        else if (driversAndPositonsOrdered.Count > driverPos + 1) //Check if there is a driver behind you.
                        {

                            Driver driverBehind = driversAndPositonsOrdered.ElementAt(driverPos + 1).Key;

                            int driverBehindClassSpeed = driverBehind.Car.CarClassRelSpeed;

                            if (yourClassSpeed < driverBehindClassSpeed && (Sim.Instance.Driver.Live.LapDistance - driverBehind.Live.LapDistance <= _trackDistPctFromKM && Sim.Instance.Driver.Live.LapDistance - driverBehind.Live.LapDistance > 0))
                            {
                                FastClassApproachingIndicator1.Fill = classWarningColor;
                            }
                            else
                            {
                                FastClassApproachingIndicator1.Fill = Brushes.Transparent;
                            }
                        }
                        else
                        {
                            FastClassApproachingIndicator1.Fill = Brushes.Transparent;
                        }

                    }
                    driversAndPositons = null;

                }
            }
        }

        private void CarLeftRightCheck(SdkWrapper.TelemetryUpdatedEventArgs e)
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
