using PaymentApp.App;
using PaymentApp.Employees;

namespace PaymentApp
{
    internal class Program
    {
        private static InputReader InputReader { get; } = new();
        private static FileAPI FileAPI { get; } = new(InputReader);

        static void Main()
        {
            SetupConsoleEncoding();
            ProcessEmployeeData();
            DisplayEmployeeReport();
            PromptForReportFileWrite();
            Console.ReadLine();
        }

        private static void DisplayEmployeeReport()
        {
            Console.WriteLine("\nRelatório:");
            Employee.DisplayEmployees();
            Console.WriteLine();
        }

        private static void PromptForReportFileWrite()
        {
            var writeToFile = InputReader.ReadString("Deseja escrever esse relátório em um arquivo .csv? (Y/N):").ToLower() == "y";
            if (writeToFile)
            {
                FileAPI.WriteToFile();
            }
        }

        private static void ProcessEmployeeData()
        {
            var importFromFile = InputReader.ReadString("Deseja importar funcionários de um arquivo? (Y/N):").ToLower() == "y";

            if (importFromFile)
            {
                FileAPI.ImportFromFile();
                if (!AppState.IsValidEntries)
                {
                    Employee.Employees.Clear();
                    Employee.CurrentId = 0;
                    while (AppState.IsRegisteringEmployees)
                    {
                        FileAPI.RegisterEmployee();
                    }
                }
            }
            else
            {
                AppState.IsRegisteringEmployees = true;
                while (AppState.IsRegisteringEmployees)
                {
                    FileAPI.RegisterEmployee();
                }
            }
        }

        private static void SetupConsoleEncoding()
        {
            Console.Clear();
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }
    }
}
