using DentalBeeTest.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace DentalBeeTest.Services
{
    public class CalculatorService : ICalculatorService
    {
        private readonly ILogger<CalculatorService> _logger;
        private readonly IAppManager _appManager;


        public CalculatorService(IAppManager appManager, ILogger<CalculatorService> logger)
        {
            _logger = logger;
            _appManager = appManager;
        }

        public bool Initialize()
        {
            try
            {
                _logger.LogInformation("CalculatorService opening the app");
                _appManager.Initialize("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");

                _logger.LogInformation("CalculatorService app opened");

                return true;
            }
            catch
            {
                _logger.LogInformation("CalculatorService app opening failed");
                return false;
            }
        }

        public void Finish()
        {
            _logger.LogInformation("CalculatorService finishing app");
            _appManager.FinishCurrentApp();
        }

        public async Task<string> GetResult()
        {
            var name = await _appManager.GetElementName("CalculatorResults");

            if (name != null)
            {
                return Regex.Replace(name, "[^0-9]", string.Empty);
            }
            else
            {
                throw new Exception("Result is null in GetResult");
            }
        }

        public async Task PressAdd()
        {
            var res = await _appManager.PressButton("plusButton");

            if (!res)
            {
                throw new Exception("Button couldn't be pressed");
            }
        }

        public async Task PressEqual()
        {
            var res = await _appManager.PressButton("equalButton");

            if (!res)
            {
                throw new Exception("Button couldn't be pressed");
            }
        }
        
        public async Task PressBackspace()
        {
            var res = await _appManager.PressButton("backSpaceButton");

            if (!res)
            {
                throw new Exception("Button couldn't be pressed");
            }
        }

        public async Task PressNumber(char number)
        {
            var buttonAutomationId = string.Empty;

            switch (number)
            {
                case '1':
                    buttonAutomationId = "num1Button";
                    break;
                case '2':
                    buttonAutomationId = "num2Button";
                    break;
                case '3':
                    buttonAutomationId = "num3Button";
                    break;
                case '4':
                    buttonAutomationId = "num4Button";
                    break;
                case '5':
                    buttonAutomationId = "num5Button";
                    break;
                case '6':
                    buttonAutomationId = "num6Button";
                    break;
                case '7':
                    buttonAutomationId = "num7Button";
                    break;
                case '8':
                    buttonAutomationId = "num8Button";
                    break;
                case '9':
                    buttonAutomationId = "num9Button";
                    break;
                case '0':
                    buttonAutomationId = "num0Button";
                    break;
                default:
                    break;

            }

            var res = await _appManager.PressButton(buttonAutomationId);

            if (!res)
            {
                throw new Exception("Button couldn't be pressed");
            }
        }
    }
}
