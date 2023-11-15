using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CapriciousUI.Wpf.Sample;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnThemeSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Theme? theme = ((ComboBox)sender).SelectedIndex switch
        {
            1 => Theme.Light,
            // 2 => Theme.Dark,
            _ => null,
        };

        ThemeManager.SetTheme(App.Current, theme);
    }
}