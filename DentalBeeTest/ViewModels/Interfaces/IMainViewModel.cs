using System.Windows.Input;

namespace DentalBeeTest.ViewModels.Interfaces
{
    public interface IMainViewModel
    {
        void Initialize();

        string FirstNumber { get; set; }

        string SecondNumber { get; set; }

        string ResultText { get; set; }

        ICommand CalculateCommand { get; }

        ICommand AddCommand { get; }

        event Func<string, Task>? DisplayErrorEvent;
    }
}
