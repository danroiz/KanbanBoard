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
using Presentation.Model;
using Presentation.ViewModel;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for InfoTaskView.xaml
    /// </summary>
    
    public partial class InfoTaskView : Window
    {
        private InfoTaskViewModel viewModel;

        public InfoTaskView(BackendController controller,TaskModel task)
        {
            InitializeComponent();
            this.DataContext = new InfoTaskViewModel(controller, task);
            viewModel = (InfoTaskViewModel)DataContext;

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void BackToBoard_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveChanges_click(object sender, RoutedEventArgs e)
        {
            viewModel.Save();
        }

    }
}
