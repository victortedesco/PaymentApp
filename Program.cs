using Employment.App;
using Employment.Employees;

namespace Employment
{
    internal class Program
    {
        private readonly static InputReader _inputReader = new();
        private static bool isValidEntries = true;
        private static bool isRegisteringEmployees = true;

        static void Main()
        {
            Console.Clear();
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var importFromFile = _inputReader.ReadString("Deseja importar funcionários de um arquivo? (Y/N):").ToLower()[0] == 'y';

            if (importFromFile)
            {
                ImportFromFile();
            }
            else
            {
                while (isRegisteringEmployees)
                {
                    RegisterEmployee();
                }
            }

            if (isValidEntries)
            {
                DisplayEmployees();

                var writeToFile = _inputReader.ReadString("Deseja escrever esse relátório em um arquivo .csv? (Y/N):").ToLower()[0] == 'y';
                if (writeToFile)
                {
                    WriteToFile();
                }
            }

            Console.ReadLine();
        }

        private static void ImportFromFile()
        {
            string path = "funcionarios.txt";
            // Formato correto para o arquivo: Nome; Horas; ValorPorHora; TaxaAdicional.
            // Se taxa adicional for 0, o funcionário não é terceirizado.
            if (File.Exists(path))
            {
                using StreamReader streamReader = new(path);
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    Employee.CurrentId++;
                    var data = line.Split(';');
                    
                    try
                    {
                        string name = data[0];
                        uint hours = uint.Parse(data[1]);
                        double valuePerHour = double.Parse(data[2].Replace(",", "."));
                        double additionalCharge = double.Parse(data[3].Replace(",", "."));
                        if (additionalCharge == 0)
                        {
                            Employee.Employees.Add(new Employee(name, hours, valuePerHour));
                        }
                        else
                        {
                            Employee.Employees.Add(new OutSourcedEmployee(name, hours, valuePerHour, additionalCharge));
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Erro ao importar funcionário: {e.Message}");
                    }
                }
            } 
            else
            {
                Console.WriteLine("Arquivo \"funcionarios.txt\" não encontrado.");
                isValidEntries = false;
            }
        }

        private static void RegisterEmployee()
        {
            Employee.CurrentId++;
            Console.WriteLine($"Funcionário #{Employee.CurrentId}");
            bool isOutsourced = _inputReader.ReadString("O funcionário é terceirizado? (Y/N):").ToLower()[0] == 'y';

            string name = _inputReader.ReadString("Digite o nome do funcionário:");
            uint hours = _inputReader.ReadUint("Digite a quantidade de horas trabalhadas:");
            double valuePerHour = _inputReader.ReadDouble("Digite o valor por hora:");

            if (isOutsourced)
            {
                double additionalCharge = _inputReader.ReadDouble("Digite a taxa adicional:");
                Employee.Employees.Add(new OutSourcedEmployee(name, hours, valuePerHour, additionalCharge));
            }
            else
            {
                Employee.Employees.Add(new Employee(name, hours, valuePerHour));
            }
            isRegisteringEmployees = _inputReader.ReadString("Deseja cadastrar outro usuário? (Y/N):").ToLower()[0] == 'y';
        }

        private static void DisplayEmployees()
        {
            Console.WriteLine("\nRelatório:");
            foreach (var employee in Employee.Employees)
            {
                Console.WriteLine($"{employee.Name} | ${employee.GetPayment():0.00}");
            }
        }

        private static void WriteToFile()
        {
            string path = "relatorio.csv";

            using StreamWriter streamWriter = new(path, false);

            streamWriter.WriteLine("Nome, Pagamento");
            foreach (var employee in Employee.Employees)
            {
                streamWriter.WriteLine($"{employee.Name}, {employee.GetPayment():0.00}");
            }
            Console.WriteLine($"\nRelatório escrito em {Path.GetFullPath(path)}");
        }
    }
}
