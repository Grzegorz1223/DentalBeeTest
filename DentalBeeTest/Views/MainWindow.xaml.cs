using DentalBeeTest.ViewModels.Interfaces;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DentalBeeTest.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow(IMainViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            viewModel.DisplayErrorEvent += DisplayError;

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as IMainViewModel;

            vm.Initialize();

            FirstNumberTextBox.IsEnabled = true;
            SecondNumberTextBox.IsEnabled = false;
            CalculateButton.IsEnabled = false;
            AddButton.IsEnabled = true;

            this.Deactivated += (s, e) => this.Activate();

            FirstNumberTextBox.Focus();
        }

        private Task DisplayError(string error)
        {
            string messageBoxText = error;
            string caption = "Critical error";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);

            return Task.CompletedTask;
        }

        private void FirstNumber_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var vm = DataContext as IMainViewModel;

            var textBox = e.OriginalSource as TextBox;

            vm.FirstNumber = textBox.Text;

            FirstNumberTextBox.Focus();
        }

        private void SecondNumber_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var vm = DataContext as IMainViewModel;

            var textBox = e.OriginalSource as TextBox;

            vm.SecondNumber = textBox.Text;

            SecondNumberTextBox.Focus();
        }

        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var vm = DataContext as IMainViewModel;

            //Probably this validation should be in the VM, but no time to setup the infrastructre for it
            if (!string.IsNullOrEmpty(vm.FirstNumber))
            {
                FirstNumberTextBox.IsEnabled = false;
                SecondNumberTextBox.IsEnabled = true;
                CalculateButton.IsEnabled = true;
                AddButton.IsEnabled = false;

                vm.AddCommand.Execute(null);

                this.Activate();
                SecondNumberTextBox.Focus();
            }
            else
            {
                string messageBoxText = "You need to specify the first number";
                string caption = "Incorrect operation";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);

                this.Activate();
                FirstNumberTextBox.Focus();
            }
        }

        private void CalculateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var vm = DataContext as IMainViewModel;

            //Probably this validation should be in the VM, but no time to setup the infrastructre for it
            if (!string.IsNullOrEmpty(vm.SecondNumber))
            {
                FirstNumberTextBox.IsEnabled = true;
                SecondNumberTextBox.IsEnabled = false;
                CalculateButton.IsEnabled = false;
                AddButton.IsEnabled = true;

                vm.CalculateCommand.Execute(null);

                this.Activate();
                FirstNumberTextBox.Focus();
            }
            else
            {
                string messageBoxText = "You need to specify the second number";
                string caption = "Incorrect operation";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);

                this.Activate();
                SecondNumberTextBox.Focus();
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}