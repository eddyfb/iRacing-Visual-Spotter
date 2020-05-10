using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Overlay;

namespace OverlayController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        Overlay.MainWindow _overlayWindow = null;
        System.Windows.Forms.NotifyIcon _ni = new System.Windows.Forms.NotifyIcon();

        bool _testOverlay = false;

        public MainWindow()
        {


            InitializeComponent();

            _ni.Icon = new System.Drawing.Icon("spotter.ico");
            _ni.Visible = true;
            _ni.Text = "Visual Spotter Controller \n Overlay is NOT running.";
            _ni.ContextMenu = new System.Windows.Forms.ContextMenu();
            _ni.ContextMenu.MenuItems.Add("Exit");
            _ni.ContextMenu.MenuItems[0].Click +=
                                delegate (object sender, EventArgs args)
                                {
                                    ExitApplication();
                                };
            _ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                    this.Activate(); //Attempts to make this the topmost window 
                };


            MouseDown += Window_MouseDown;

            OverlayWidth.Text = Screen.PrimaryScreen.WorkingArea.Width.ToString();
            IndicatorHeight.Text = ((Screen.PrimaryScreen.Bounds.Height)/12).ToString();

            CheckIfRunning();

            LoadProgramSettings();

            DispatcherTimer mediaTimer = new DispatcherTimer();
            mediaTimer.Interval = TimeSpan.FromSeconds(5);
            mediaTimer.Tick += new EventHandler(mediaTimer_Tick);
            mediaTimer.Start();

        }


        #region UI interaction methods

        private void TestBox_Checked(object sender, RoutedEventArgs e)
        {
            if (_overlayWindow != null)
            {
                CloseOverlay();
            }
                _ni.Text = "Visual Spotter Controller \n Overlay IS running.";
                _overlayWindow = new Overlay.MainWindow();
                _overlayWindow.Settings(ColorPicker1.SelectedColor.Value, ColorPicker2.SelectedColor.Value, ColorPicker3.SelectedColor.Value, ColorPicker1.SelectedColor.Value, YLocation.Text, XLocation.Text, IndicatorHeight.Text, OverlayWidth.Text, IndicatorWidth.Text, SpaceBetweenIndicators.Text, SpaceToEdge.Text, double.Parse(WarningDistance.Text), ColorPicker4.SelectedColor.Value);
                _overlayWindow.Show();
                _overlayWindow.Activate();
                _testOverlay = true;
        }

        private void TestBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _ni.Text = "Visual Spotter Controller \n Overlay is NOT running.";
            CloseOverlay();
            _testOverlay = false;

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }


        private void ApplyChangesButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyChanges();
        }

        #endregion //UI interaction methods


        #region General methods

        private void CloseOverlay()
        {
            _ni.Text = "Visual Spotter Controller \n Overlay is NOT running.";
            if (_overlayWindow != null)
            {
                _overlayWindow.Close();
                _overlayWindow = null;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _ni.Dispose();
            CloseOverlay();
            ExitApplication();
        }

        private void CheckIfRunning()
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                if (System.Windows.MessageBox.Show("Visual Spotter already running. Only one instance of this application is allowed.", "Visual Spotter Already Running", MessageBoxButton.OK, MessageBoxImage.Stop) == MessageBoxResult.OK)
                {
                    ExitApplication();
                }
            }
        }

        private void ExitApplication()
        {
            System.Environment.Exit(0);
        }



        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void NumberValidationTextBoxWithNeg(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+-");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberValidationTextBoxWithDec(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+.");
            e.Handled = regex.IsMatch(e.Text);
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }

        private void ApplyChanges()
        {
            if (TestBox.IsChecked == true)
            {
                _testOverlay = true;
                _overlayWindow.Settings(ColorPicker1.SelectedColor.Value, ColorPicker2.SelectedColor.Value, ColorPicker3.SelectedColor.Value, ColorPicker1.SelectedColor.Value, YLocation.Text, XLocation.Text, IndicatorHeight.Text, OverlayWidth.Text, IndicatorWidth.Text, SpaceBetweenIndicators.Text, SpaceToEdge.Text, double.Parse(WarningDistance.Text), ColorPicker4.SelectedColor.Value);
            }
            SaveProgramSettings();
        }

        private void StartOverlay()
        {
            if (TestBox.IsChecked == true)
            {
                _testOverlay = true;
                _overlayWindow.Settings(ColorPicker1.SelectedColor.Value, ColorPicker2.SelectedColor.Value, ColorPicker3.SelectedColor.Value, ColorPicker1.SelectedColor.Value, YLocation.Text, XLocation.Text, IndicatorHeight.Text, OverlayWidth.Text, IndicatorWidth.Text, SpaceBetweenIndicators.Text, SpaceToEdge.Text, double.Parse(WarningDistance.Text), ColorPicker4.SelectedColor.Value);
            }
            else
            {
                _overlayWindow = new Overlay.MainWindow();
                _overlayWindow.Settings(ColorPicker1.SelectedColor.Value, ColorPicker2.SelectedColor.Value, ColorPicker3.SelectedColor.Value, Color.FromArgb(0, 0, 0, 0), YLocation.Text, XLocation.Text, IndicatorHeight.Text, OverlayWidth.Text, IndicatorWidth.Text, SpaceBetweenIndicators.Text, SpaceToEdge.Text, double.Parse(WarningDistance.Text), ColorPicker4.SelectedColor.Value);
                _overlayWindow.Show();
                _overlayWindow.Activate();
            }
            _ni.Text = "Visual Spotter Controller \n Overlay IS running.";
        }


        private void mediaTimer_Tick(object sender, EventArgs e)
        {
            if (_testOverlay == false)
            {
                if (Process.GetProcessesByName("iRacingSim64DX11").Length == 1 && _overlayWindow == null)
                {

                    StartOverlay();

                    iRacingDetecionText.Text = "iRacing IS RUNNING.";
                }
                else if (Process.GetProcessesByName("iRacingSim64DX11").Length == 0 && _overlayWindow != null)
                {
                    iRacingDetecionText.Text = "iRacing NOT detected running.";
                    CloseOverlay();
                }
            }
        }

        private void SaveProgramSettings()
        {
            Properties.Settings.Default.OneCarColor = System.Drawing.Color.FromArgb(ColorPicker1.SelectedColor.Value.A, ColorPicker1.SelectedColor.Value.R, ColorPicker1.SelectedColor.Value.G, ColorPicker1.SelectedColor.Value.B);
            Properties.Settings.Default.TwoCarColor = System.Drawing.Color.FromArgb(ColorPicker2.SelectedColor.Value.A, ColorPicker2.SelectedColor.Value.R, ColorPicker2.SelectedColor.Value.G, ColorPicker2.SelectedColor.Value.B);
            Properties.Settings.Default.InTheMiddleColor = System.Drawing.Color.FromArgb(ColorPicker3.SelectedColor.Value.A, ColorPicker3.SelectedColor.Value.R, ColorPicker3.SelectedColor.Value.G, ColorPicker3.SelectedColor.Value.B);
            Properties.Settings.Default.FasterClassColor = System.Drawing.Color.FromArgb(ColorPicker4.SelectedColor.Value.A, ColorPicker4.SelectedColor.Value.R, ColorPicker4.SelectedColor.Value.G, ColorPicker4.SelectedColor.Value.B);
            Properties.Settings.Default.FasterClassDistance = WarningDistance.Text;
            Properties.Settings.Default.XLocation = XLocation.Text;
            Properties.Settings.Default.YLocation = YLocation.Text;
            Properties.Settings.Default.OverlayWidth = OverlayWidth.Text;
            Properties.Settings.Default.IndicatorHeight = IndicatorHeight.Text;
            Properties.Settings.Default.IndicatorWidth = IndicatorWidth.Text;
            Properties.Settings.Default.SpaceBetween = SpaceBetweenIndicators.Text;
            Properties.Settings.Default.SpaceToEdge = SpaceToEdge.Text;
            Properties.Settings.Default.Save();
        }
        private void LoadProgramSettings()
        {
            if (Properties.Settings.Default.OneCarColor != null)
            {
                if (Properties.Settings.Default.OneCarColor.IsEmpty == false)
                {
                    ColorPicker1.SelectedColor = Color.FromArgb(Properties.Settings.Default.OneCarColor.A, Properties.Settings.Default.OneCarColor.R, Properties.Settings.Default.OneCarColor.G, Properties.Settings.Default.OneCarColor.B);
                    ColorPicker2.SelectedColor = Color.FromArgb(Properties.Settings.Default.TwoCarColor.A, Properties.Settings.Default.TwoCarColor.R, Properties.Settings.Default.TwoCarColor.G, Properties.Settings.Default.TwoCarColor.B);
                    ColorPicker3.SelectedColor = Color.FromArgb(Properties.Settings.Default.InTheMiddleColor.A, Properties.Settings.Default.InTheMiddleColor.R, Properties.Settings.Default.InTheMiddleColor.G, Properties.Settings.Default.InTheMiddleColor.B);
                    ColorPicker4.SelectedColor = Color.FromArgb(Properties.Settings.Default.FasterClassColor.A, Properties.Settings.Default.FasterClassColor.R, Properties.Settings.Default.FasterClassColor.G, Properties.Settings.Default.FasterClassColor.B);
                    WarningDistance.Text = Properties.Settings.Default.FasterClassDistance;
                    XLocation.Text = Properties.Settings.Default.XLocation;
                    YLocation.Text = Properties.Settings.Default.YLocation;
                    OverlayWidth.Text = Properties.Settings.Default.OverlayWidth;
                    IndicatorHeight.Text = Properties.Settings.Default.IndicatorHeight;
                    IndicatorWidth.Text = Properties.Settings.Default.IndicatorWidth;
                    SpaceBetweenIndicators.Text = Properties.Settings.Default.SpaceBetween;
                    SpaceToEdge.Text = Properties.Settings.Default.SpaceToEdge;
                }
            }
        }

        #endregion //General Methods
    }
}