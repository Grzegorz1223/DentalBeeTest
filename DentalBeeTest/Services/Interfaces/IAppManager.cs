namespace DentalBeeTest.Services.Interfaces
{
    public interface IAppManager
    {
        void Initialize(string appName);

        void FinishCurrentApp();
        
        Task<bool> PressButton(string buttonAutomationId);

        Task<string?> GetElementName(string elementAutomationId);
    }
}
