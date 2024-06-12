using PaymentApp.App;
using PaymentApp.Employees;

namespace Employment
{
    internal class Program
    {
        private static InputReader InputReader { get; } = new();
        private static FileAPI FileAPI { get; } = new(InputReader);

        static void Main()
        {
            Console.Clear();
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var importFromFile = InputReader.ReadString("Deseja importar funcionários de um arquivo? (Y/N):").ToLower()[0] == 'y';

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
                while (AppState.IsRegisteringEmployees)
                {
                    FileAPI.RegisterEmployee();
                }
            }

            Console.WriteLine("\nRelatório:");
            Employee.DisplayEmployees();
            Console.WriteLine();

            var writeToFile = InputReader.ReadString("Deseja escrever esse relátório em um arquivo .csv? (Y/N):").ToLower()[0] == 'y';
            if (writeToFile)
            {
                FileAPI.WriteToFile();
            }


            Console.ReadLine();
        }
    }
}
