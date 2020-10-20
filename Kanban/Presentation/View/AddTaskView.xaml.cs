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
    /// Interaction logic for AddTaskView.xaml
    /// </summary>
    public partial class AddTaskView : Window
    {
        private AddTaskViewModel viewModel;
        
        public AddTaskView(BackendController controller, string userEmail, ObservableCollection<TaskModel> tasks)
        {
            InitializeComponent();

            this.DataContext = new AddTaskViewModel(controller, userEmail, tasks);//tasks);
            this.viewModel = (AddTaskViewModel)DataContext;


            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void BackToBoard_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddTask_click(object sender, RoutedEventArgs e)
        {
            if(viewModel.AddTask()) this.Close();
        }

       
    }
}
