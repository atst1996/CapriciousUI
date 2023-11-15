using System.ComponentModel;

namespace CapriciousUI.Wpf.Sample;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public int SampleIntValue { get; set; }
}
