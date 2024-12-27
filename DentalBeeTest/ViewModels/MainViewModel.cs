using CommunityToolkit.Mvvm.Input;
using DentalBeeTest.Services.Interfaces;
using DentalBeeTest.ViewModels.Interfaces;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DentalBeeTest.ViewModels
{
    public class MainViewModel : PropertyChangedBase, IMainViewModel
    {
        private readonly ILogger<MainViewModel> _logger;
        private readonly ICalculatorService _calculatorService;

        private string _firstNumber = string.Empty;
        public string FirstNumber
        {
            get => _firstNumber;
            set
            {
                ProcessEnteredNumber(_firstNumber, value);
                SetProperty(ref _firstNumber, value, nameof(FirstNumber));
            }
        }

        private string _secondNumber = string.Empty;
        public string SecondNumber
        {
            get => _secondNumber;
            set
            {
                ProcessEnteredNumber(_secondNumber, value);
                SetProperty(ref _secondNumber, value, nameof(SecondNumber));
            }
        }

        private string _resultText = string.Empty;
        public string ResultText
        {
            get => _resultText;
            set
            {
                SetProperty(ref _resultText, value, nameof(ResultText));
            }
        }

        private ICommand _calculateCommand;
        public ICommand CalculateCommand =>
            _calculateCommand ??= new AsyncRelayCommand(ExecuteCalculateAsync);

        private ICommand _addCommand;

        public event Func<string, Task>? DisplayErrorEvent;

        public ICommand AddCommand =>
            _addCommand ??= new AsyncRelayCommand(ExecuteAddAsync);

        public MainViewModel(ILogger<MainViewModel> logger, ICalculatorService calculatorService)
        {
            _logger = logger;
            _calculatorService = calculatorService;
        }

        public void Initialize()
        {
            var res = _calculatorService.Initialize();
            if (!res)
            {
                DisplayErrorEvent?.Invoke("Error while initializing the calculator");
            }
        }

        private async Task ExecuteCalculateAsync()
        {
            try
            {
                await _calculatorService.PressEqual();

                var result = await _calculatorService.GetResult();

                ResultText = result;
                FirstNumber = string.Empty;
                SecondNumber = string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during the operation");
                DisplayErrorEvent?.Invoke("Error while executing the operation. Please restart the application");
            }
        }
        
        private async Task ExecuteAddAsync()
        {
            try
            {
                await _calculatorService.PressAdd();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during the operation");
                DisplayErrorEvent?.Invoke("Error while executing the operation. Please restart the application");
            }
        }

        private async void ProcessEnteredNumber(string oldVal, string newVal)
        {
            try
            {
                if (oldVal.Count() < newVal.Count())
                {
                    await _calculatorService.PressNumber(newVal.Last());
                }
                else if (oldVal.Count() > newVal.Count())
                {
                    await _calculatorService.PressBackspace();
                }
                else
                {
                    //Do nothing since we didn't change the number
                }
            }
            catch
            {
                DisplayErrorEvent?.Invoke("Error while sending the event. Please restart the application");
            }
        }
    }

    public abstract class PropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaiseAllPropertiesChanged()
        {
            OnPropertyChanged(null);
        }

        public void OnPropertyChanged(string? propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
                    [CallerMemberName] string propertyName = "",
                    Action? onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
