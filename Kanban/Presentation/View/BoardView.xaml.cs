using IntroSE.Kanban.Backend.BusinessLayer;
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
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        private BoardViewModel viewModel;

        public BoardView(BackendController controller, UserModel user)
        {
            InitializeComponent();
            this.DataContext = new BoardViewModel(controller, user);
            viewModel = (BoardViewModel)DataContext;

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Logout();
            new MainWindow().Show();
            this.Close();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            new AddTaskView(viewModel.Controller, viewModel.UserEmail, viewModel.Board.Columns[0].Tasks).ShowDialog();
        }

        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {
            new AddColumnView(viewModel.Controller, viewModel.UserEmail, viewModel.Board.Columns).ShowDialog();
        }

        public void OpenTask_DoubleClick(object sender, RoutedEventArgs e)
        {
            new InfoTaskView(viewModel.Controller, viewModel.SelectedTask).ShowDialog();
            viewModel.OnTaskViewClose();
        }

        public void EditColumn_Click(object sender, RoutedEventArgs e)
        {
            new EditColumnView(viewModel.Controller, viewModel.UserEmail, viewModel.SelectedColumn).ShowDialog();
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            viewModel.DeleteTask();
        }

        private void DeleteColumn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.DeleteColumn();
        }

        private void DueDateSort_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SortByDueDate();
        }

        private void CeationTimeSort_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SortByCreationTime();
        }

        private void TitleSort_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SortByTitle();
        }

        private void MoveLeftColumn_click(object sender, RoutedEventArgs e)
        {
            viewModel.MoveLeftColumn();
        }

        private void MoveRightColumn_click(object sender, RoutedEventArgs e)
        {
            viewModel.MoveRightColumn();
        }

        private void AdvanceTask_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AdvanceTask();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.FilterOn();
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            viewModel.FilterOn();
        }

    }
}
