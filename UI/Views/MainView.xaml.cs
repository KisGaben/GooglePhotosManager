using Core.Services;
using Microsoft.UI.Xaml;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using UI.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace UI.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : Window
    {
        MainViewModel ViewModel;

        public MainView(MainViewModel vm)
        {
            InitializeComponent();
            Root.DataContext = ViewModel = vm;
        }

    }

    
}
