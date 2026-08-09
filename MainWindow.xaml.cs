using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StansploitOrionProject;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ShowWelcome();
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
            default:
                MainContent.Content = new TextBlock 
                { 
                    Text = $"{section} Section Implementation Coming Soon...", 
                    FontSize = 20, 
                    VerticalAlignment = VerticalAlignment.Center, 
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = System.Windows.Media.Brushes.Black
                };
                break;
        }
    }
}
