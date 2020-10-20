using IntroSE.Kanban.Backend.BusinessLayer;
using Presentation.Model;
using Presentation.View;
using Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;
        //private Registration registrationView;
        //private BoardView boardView;


        public MainWindow()
        {
            InitializeComponent();

            this.viewModel = new MainWindowViewModel(new BackendController());
            this.DataContext = viewModel;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }
        public MainWindow(BackendController controller)
        {
            InitializeComponent();
            this.viewModel = new MainWindowViewModel(controller);
            this.DataContext = viewModel;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = viewModel.Login();
            if (user != null) { 
                 BoardView boardView = new BoardView(viewModel.Controller, user);
                boardView.Show();
                this.Close();
            }
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Registration registrationView = new Registration(viewModel.Controller);
            registrationView.Show();
            this.Close();
        }
    }
}
