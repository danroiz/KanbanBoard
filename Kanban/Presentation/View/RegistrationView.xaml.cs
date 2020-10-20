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
using System.Windows.Shapes;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {

        private RegisterViewModel viewModel; 

        public Registration(BackendController controller)
        {
            InitializeComponent();
            this.DataContext = new RegisterViewModel(controller);
            this.viewModel = (RegisterViewModel)DataContext;

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }


        private void BackToLogin_click(object sender, RoutedEventArgs e)
        {
            new MainWindow(viewModel.Controller).Show();
            this.Close();

        }

        private void ContinueRegister_Click(object sender, RoutedEventArgs e)
        {

            if (viewModel.register())
            {
                new MainWindow(viewModel.Controller).Show();
                this.Close();
            }
        }

         
    }
}
