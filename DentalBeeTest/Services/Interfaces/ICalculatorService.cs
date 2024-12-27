namespace DentalBeeTest.Services.Interfaces
{
    public interface ICalculatorService
    {
        bool Initialize();

        void Finish();

        Task PressNumber(char number);

        Task PressAdd();

        Task PressEqual();

        Task PressBackspace();

        Task<string> GetResult();
    }
}
