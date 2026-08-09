using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StansploitOrionProject.Views;

public partial class TweaksView : UserControl
{
    public TweaksView()
    {
        InitializeComponent();
        LoadSubView("PowerPlan");
    }

    private void Tab_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string section)
        {
            LoadSubView(section);
        }
    }

    private void LoadSubView(string section)
    {
        // Highlight active tab
        foreach (var child in ((StackPanel)TabPowerPlan.Parent).Children)
        {
            if (child is Button b)
            {
                b.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#151515"));
                b.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#888888"));
            }
        }

        Button? activeTab = section switch { "PowerPlan" => TabPowerPlan, "Debloater" => TabDebloater, "Experimental" => TabExperimental, _ => null };
        if (activeTab != null)
        {
            activeTab.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#300030"));
            activeTab.Foreground = (Brush)Application.Current.Resources["AccentColor"];
        }

        switch (section)
        {
            case "PowerPlan": TweaksContent.Content = new PowerPlanView(); break;
            case "Debloater": TweaksContent.Content = new DebloaterView(); break;
            case "Experimental": TweaksContent.Content = new ExperimentalView(); break;
        }
    }
}
