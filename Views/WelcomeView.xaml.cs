using System.Windows;
using System.Windows.Controls;

namespace StansploitOrionProject.Views;

public partial class WelcomeView : UserControl
{
    public event RoutedEventHandler? LetsGoClicked;

    public WelcomeView()
    {
        InitializeComponent();
    }

    private void LetsGo_Click(object sender, RoutedEventArgs e)
    {
        LetsGoClicked?.Invoke(this, e);
        if (Window.GetWindow(this) is MainWindow mw)
        {
            mw.ShowMainShell();
        }
    }
}
