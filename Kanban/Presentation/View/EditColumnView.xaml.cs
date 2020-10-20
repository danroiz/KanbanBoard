using Presentation.Model;
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
    /// Interaction logic for EditColumnView.xaml
    /// </summary>
    public partial class EditColumnView : Window
    {
        private EditColumnViewModel viewModel;

        public EditColumnView(BackendController controller, string userEmail, ColumnModel column)
        {
            InitializeComponent();
            this.DataContext = new EditColumnViewModel(controller,userEmail,column);
            this.viewModel = (EditColumnViewModel)DataContext;

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void BackToBoard_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EditColumn_click(object sender, RoutedEventArgs e)
        {
            viewModel.EditColumn();
           
        }


    }
}
