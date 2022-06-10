// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Basics.Viewmodels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Basics.UserControls
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public SettingsViewModel SettingsViewModel
        {
            get { return (SettingsViewModel)GetValue(SettingsViewModelProperty); }
            set { SetValue(SettingsViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SettingsViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SettingsViewModelProperty =
            DependencyProperty.Register("SettingsViewModel", typeof(SettingsViewModel), typeof(Settings), new PropertyMetadata(null));

        public Settings()
        {
            InitializeComponent();
        }

        //private void Changetheme()
        //{
        //    switch (Properties.Settings.Default.Theme)
        //    {
        //        case "Blue":
        //            this.Resources["Backgroundcolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("AliceBlue"));
        //            this.Resources["Bordercolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
        //            this.Resources["Menucolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("WhiteSmoke"));
        //            Properties.Settings.Default.BackgroundColor = "AliceBlue";
        //            Properties.Settings.Default.BorderColor = "Black";
        //            Properties.Settings.Default.MenuColor = "WhiteSmoke";
        //            Properties.Settings.Default.Save();
        //            break;
        //        case "Black":
        //            this.Resources["Backgroundcolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
        //            this.Resources["Bordercolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Yellow"));
        //            this.Resources["Menucolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Gray"));
        //            Properties.Settings.Default.BackgroundColor = "Black";
        //            Properties.Settings.Default.BorderColor = "Yellow";
        //            Properties.Settings.Default.MenuColor = "Gray";
        //            Properties.Settings.Default.Save();
        //            break;
        //        case "Yellow":
        //            this.Resources["Backgroundcolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Beige"));
        //            this.Resources["Bordercolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
        //            this.Resources["Menucolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Beige"));
        //            Properties.Settings.Default.BackgroundColor = "Beige";
        //            Properties.Settings.Default.BorderColor = "Black";
        //            Properties.Settings.Default.MenuColor = "Beige";
        //            Properties.Settings.Default.Save();
        //            break;
        //        case "Green":
        //            this.Resources["Backgroundcolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("AliceBlue"));
        //            this.Resources["Bordercolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
        //            this.Resources["Menucolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("WhiteSmoke"));
        //            Properties.Settings.Default.BackgroundColor = "AliceBlue";
        //            Properties.Settings.Default.BorderColor = "Black";
        //            Properties.Settings.Default.MenuColor = "WhiteSmoke";
        //            Properties.Settings.Default.Save();
        //            break;

        //    }
        //}
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    }
}