using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Note.DependencyInjection;
using Note.ViewModels;

namespace Note.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            ViewModel = ViewModelBase.Create<MainWindowViewModel>();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}