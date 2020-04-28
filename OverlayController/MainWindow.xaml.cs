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


        Overlay.MainWindow overlayWindow = new Overlay.MainWindow();
        System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();

        public MainWindow()
        {
            InitializeComponent();

            ni.Icon = new System.Drawing.Icon("spotter.ico");
            ni.Visible = true;
            ni.Text = "Visual Spotter Controller \n Overlay is NOT running.";
            ni.ContextMenu = new System.Windows.Forms.ContextMenu();
            ni.ContextMenu.MenuItems.Add("Exit");
            ni.ContextMenu.MenuItems[0].Click +=
                                delegate (object sender, EventArgs args)
                                {
                                    ExitApplication();
                                };
            ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };


            MouseDown += Window_MouseDown;

            OverlayWidth.Text = Screen.PrimaryScreen.WorkingArea.Width.ToString();
            IndicatorHeight.Text = ((Screen.PrimaryScreen.Bounds.Height)/12).ToString();
        }


        #region UI interaction methods

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartButton.Content.ToString() == "Start Overlay")
            {
                StartButton.Content = "Close Overlay";
                ni.Text = "Visual Spotter Controller \n Overlay IS running.";
                overlayWindow.Settings(ColorPicker1.SelectedColor.Value, ColorPicker2.SelectedColor.Value, ColorPicker3.SelectedColor.Value, Color.FromArgb(0, 0, 0, 0), YLocation.Text, XLocation.Text, IndicatorHeight.Text, OverlayWidth.Text, IndicatorWidth.Text, SpaceBetweenIndicators.Text, SpaceToEdge.Text);
                overlayWindow.Show();
                overlayWindow.Activate();
                if (TestBox.IsChecked == true)
                {
                    TestBox.IsChecked = false;
                }
            }
            else
            {
                CloseOverlay();
            }
        }

        private void TestBox_Checked(object sender, RoutedEventArgs e)
        {
            ni.Text = "Visual Spotter Controller \n Overlay IS running.";
            overlayWindow.Settings(ColorPicker1.SelectedColor.Value, ColorPicker2.SelectedColor.Value, ColorPicker3.SelectedColor.Value, ColorPicker1.SelectedColor.Value, YLocation.Text, XLocation.Text, IndicatorHeight.Text, OverlayWidth.Text, IndicatorWidth.Text, SpaceBetweenIndicators.Text, SpaceToEdge.Text);
            overlayWindow.Show();
            overlayWindow.Activate();
        }

        private void TestBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (StartButton.Content.ToString() == "Close Overlay")
            {
                overlayWindow.Settings(ColorPicker1.SelectedColor.Value, ColorPicker2.SelectedColor.Value, ColorPicker3.SelectedColor.Value, Color.FromArgb(0, 0, 0, 0), YLocation.Text, XLocation.Text, IndicatorHeight.Text, OverlayWidth.Text, IndicatorWidth.Text, SpaceBetweenIndicators.Text, SpaceToEdge.Text);
            }
            else
            {
                ni.Text = "Visual Spotter Controller \n Overlay is NOT running.";
                overlayWindow.Hide();
            }
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
            StartButton.Content = "Start Overlay";
            ni.Text = "Visual Spotter Controller \n Overlay is NOT running.";
            overlayWindow.Hide();
            if (TestBox.IsChecked == true)
            {
                TestBox.IsChecked = false;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ni.Dispose();
            overlayWindow.Close();
        }

        private void ExitApplication()
        {
            this.Close();
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

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }

        private void ApplyChanges()
        {
            overlayWindow.Settings(ColorPicker1.SelectedColor.Value, ColorPicker2.SelectedColor.Value, ColorPicker3.SelectedColor.Value, Color.FromArgb(0, 0, 0, 0), YLocation.Text, XLocation.Text, IndicatorHeight.Text, OverlayWidth.Text, IndicatorWidth.Text, SpaceBetweenIndicators.Text, SpaceToEdge.Text);
            if (TestBox.IsChecked == true)
            {
                overlayWindow.Settings(ColorPicker1.SelectedColor.Value, ColorPicker2.SelectedColor.Value, ColorPicker3.SelectedColor.Value, ColorPicker1.SelectedColor.Value, YLocation.Text, XLocation.Text, IndicatorHeight.Text, OverlayWidth.Text, IndicatorWidth.Text, SpaceBetweenIndicators.Text, SpaceToEdge.Text);
            }
        }

        #endregion //General Methods
    }
}