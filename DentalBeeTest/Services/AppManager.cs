using DentalBeeTest.Services.Interfaces;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Microsoft.Extensions.Logging;
namespace DentalBeeTest.Services
{
    public class AppManager : IAppManager
    {
        private readonly ILogger<AppManager> _logger;

        private AutomationElement? _mainWindow;
        private Application? _app;

        public AppManager(ILogger<AppManager> logger)
        {
            _logger = logger;
        }

        public void Initialize(string appName)
        {
            try
            {
                _logger.LogInformation("AppManager launching the app");
                _app = Application.LaunchStoreApp(appName);

                _logger.LogInformation("AppManager app launched");
                _app.WaitWhileMainHandleIsMissing();
                _app.WaitWhileBusy();
                var handle = _app.MainWindowHandle;

                _logger.LogInformation("AppManager handle obtained");

                var mainWindowHandle = handle;
                if (mainWindowHandle != IntPtr.Zero)
                {
                    var automation = new UIA3Automation();
                    var mainWindow = automation.FromHandle(mainWindowHandle).AsWindow();
                    if (mainWindow != null)
                    {
                        _logger.LogInformation("AppManager main window set");
                        _mainWindow = mainWindow;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void FinishCurrentApp()
        {
            _app.Close();
        }

        public Task<bool> PressButton(string buttonAutomationId)
        {
            try
            {
                var button = FindElement(buttonAutomationId).AsButton();

                button.Invoke();

                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public Task<string?> GetElementName(string elementAutomationId)
        {
            var resultElement = FindElement(elementAutomationId);

            if (resultElement != null)
            {
                return Task.FromResult<string?>(resultElement.Properties.Name.Value);
            }
            else
            {
                return Task.FromResult<string?>(null);
            }
        }

        private AutomationElement FindElement(string text)
        {
            if (_app.HasExited)
            {
                throw new Exception("The app is closed");
            }

            if (_mainWindow != null)
            {
                var element = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId(text));
                return element;
            }
            else
            {
                throw new Exception("Main window is null in FindElement");
            }
        }
    }
}
