using Presentation.Model;
using Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddColumnView.xaml
    /// </summary>
    public partial class AddColumnView : Window
    {
        private AddColumnViewModel viewModel;

        public AddColumnView(BackendController controller, string userEmail, ObservableCollection<ColumnModel> columns)
        {
            InitializeComponent();
            this.viewModel = new AddColumnViewModel(controller, userEmail, columns);
            this.DataContext = viewModel;

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void BackToBoard_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddColumn_click(object sender, RoutedEventArgs e)
        {
            viewModel.AddColumn(); 
        }

    }
}
