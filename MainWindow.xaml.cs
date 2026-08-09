using System.Text;
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
        LoadView("Dashboard");
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
            "PowerPlan" => "Power Plan Optimization",
            "Installer" => "Gaming Utilities Installer",
            "Debloater" => "System Debloater",
            "Experimental" => "Experimental Tweaks",
            _ => section
        };

        PageTitleText.Text = title;

        switch (section)
        {
            case "Dashboard":
                MainContent.Content = new Views.DashboardView();
                break;
            case "PowerPlan":
                MainContent.Content = new Views.PowerPlanView();
                break;
            case "Installer":
                MainContent.Content = new Views.InstallerView();
                break;
            case "Debloater":
                MainContent.Content = new Views.DebloaterView();
                break;
            case "Experimental":
                MainContent.Content = new Views.ExperimentalView();
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