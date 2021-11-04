using System.Windows;

namespace ContratosMVVM.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(object dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }
    }
}
