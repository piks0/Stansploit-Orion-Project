using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StansploitOrionProject;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ShowMainShell();
    }

    public void ShowWelcome()
    {
        MainShellGrid.Visibility = Visibility.Collapsed;
        WelcomeHost.Visibility = Visibility.Visible;
        WelcomeHost.Content = new Views.WelcomeView();
    }

    public void ShowMainShell()
    {
        WelcomeHost.Visibility = Visibility.Collapsed;
        MainShellGrid.Visibility = Visibility.Visible;
        LoadView("Dashboard");
    }

    public void NavigateTo(string section)
    {
        LoadView(section);
    }

    private void Nav_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string section)
        {
            LoadView(section);
        }
    }

    private void CheckUpdates_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("You are running the latest Orion build.", "Updates", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void LoadView(string section)
    {
        string title = section switch
        {
            "Dashboard" => "Dashboard",
            "Tweaks" => "System Tweaks & Optimization",
            "Installer" => "Gaming Utilities Installer",
            _ => section
        };

        PageTitleText.Text = title;
        SetActiveNavigation(section);

        switch (section)
        {
            case "Dashboard":
                MainContent.Content = new Views.DashboardView();
                break;
            case "Tweaks":
                MainContent.Content = new Views.TweaksView();
                break;
            case "Installer":
                MainContent.Content = new Views.InstallerView();
                break;
            case "Settings":
                MainContent.Content = new Views.SettingsView();
                break;
            default:
                MainContent.Content = new TextBlock
                {
                    Text = $"{section} Section Implementation Coming Soon...",
                    FontSize = 20,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = Brushes.White
                };
                break;
        }
    }

    private void SetActiveNavigation(string section)
    {
        var activeBackground = new SolidColorBrush(Color.FromRgb(0x22, 0x15, 0x3B));
        var inactiveBackground = Brushes.Transparent;
        var activeBorderBrush = (Brush)Application.Current.Resources["AccentColor"]!;
        var inactiveBorderBrush = Brushes.Transparent;
        var activeForeground = (Brush)Application.Current.Resources["TextColor"]!;
        var inactiveForeground = (Brush)Application.Current.Resources["MutedTextColor"]!;

        DashboardNavBorder.Background = section == "Dashboard" ? activeBackground : inactiveBackground;
        DashboardNavBorder.BorderBrush = section == "Dashboard" ? activeBorderBrush : inactiveBorderBrush;
        DashboardNavLabel.Foreground = section == "Dashboard" ? activeForeground : inactiveForeground;
        DashboardNavIcon.Fill = section == "Dashboard" ? (Brush)Application.Current.Resources["AccentColor"]! : (Brush)Application.Current.Resources["AccentColorBright"]!;

        TweaksNavBorder.Background = section == "Tweaks" ? activeBackground : inactiveBackground;
        TweaksNavBorder.BorderBrush = section == "Tweaks" ? activeBorderBrush : inactiveBorderBrush;
        TweaksNavLabel.Foreground = section == "Tweaks" ? activeForeground : inactiveForeground;
        TweaksNavIcon.Stroke = section == "Tweaks" ? (Brush)Application.Current.Resources["AccentColor"]! : (Brush)Application.Current.Resources["AccentColorBright"]!;

        InstallerNavBorder.Background = section == "Installer" ? activeBackground : inactiveBackground;
        InstallerNavBorder.BorderBrush = section == "Installer" ? activeBorderBrush : inactiveBorderBrush;
        InstallerNavLabel.Foreground = section == "Installer" ? activeForeground : inactiveForeground;
        InstallerNavIcon.Stroke = section == "Installer" ? (Brush)Application.Current.Resources["AccentColor"]! : (Brush)Application.Current.Resources["AccentColorBright"]!;

        SettingsNavBorder.Background = section == "Settings" ? activeBackground : inactiveBackground;
        SettingsNavBorder.BorderBrush = section == "Settings" ? activeBorderBrush : inactiveBorderBrush;
        SettingsNavLabel.Foreground = section == "Settings" ? activeForeground : inactiveForeground;
        SettingsNavIcon.Stroke = section == "Settings" ? (Brush)Application.Current.Resources["AccentColor"]! : (Brush)Application.Current.Resources["AccentColorBright"]!;
    }
}
