using PaymentApp.Employees;
using System.Text;

namespace PaymentApp.App
{
    public class FileAPI(InputReader inputReader)
    {
        private readonly InputReader _inputReader = inputReader;

        public void ImportFromFile()
        {
            string path = "funcionarios.txt";
            // Formato correto para o arquivo: Nome; Horas; ValorPorHora; TaxaAdicional.
            // Se taxa adicional for menor ou igual a 0, o funcionário não é terceirizado.
            if (File.Exists(path))
            {
                using StreamReader streamReader = new(path, true);
                string line;
                int lineIndex = 0;
                while ((line = streamReader.ReadLine()) != null)
                {
                    lineIndex++;
                    if (line.StartsWith('#') || line.StartsWith("//") || string.IsNullOrWhiteSpace(line)) continue;

                    var data = line.Split(';');
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = data[i].Trim();
                    }
                    ProcessEmployeeData(lineIndex, data);
                }
                ReportImportResults();
                AppState.IsValidEntries = true;
                bool registerMoreEntries = _inputReader.ReadString("Deseja cadastrar outro usuário? (Y/N):").ToLower() == "y";
                if (registerMoreEntries)
                {
                    AppState.IsRegisteringEmployees = true;
                    while (AppState.IsRegisteringEmployees)
                    {
                        RegisterEmployee();
                    }
                }
            }
            else
            {
                Console.WriteLine($"Arquivo \"{path}\" não encontrado.");
                Console.WriteLine("Iniciando o processo manual...\n");
                AppState.IsRegisteringEmployees = true;
                AppState.IsValidEntries = false;
            }
        }

        private static void ReportImportResults()
        {
            var employeesCount = Employee.Employees.Count;
            if (employeesCount == 0)
            {
                Console.WriteLine("O arquivo existe, mas ele não possui nenhum funcionário.");
            }
            else
            {
                Console.WriteLine($"{employeesCount} funcionário(s) importado(s) com sucesso.");
            }
        }

        private static void ProcessEmployeeData(int lineIndex, string[] data)
        {
            try
            {
                if (data.Length > 4)
                {
                    Console.WriteLine($"A linha {lineIndex} possui mais de 4 valores, os valores depois de {data[3]} serão ignorados.");
                }
                string name = data[0];
                uint hours = uint.Parse(data[1]);
                double valuePerHour = double.Parse(data[2].Replace(",", "."));
                double additionalCharge = double.Parse(data[3].Replace(",", "."));
                if (additionalCharge <= 0)
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
                Console.WriteLine($"Erro ao importar funcionário da linha {lineIndex}: {e.Message}");
            }
        }

        public void WriteToFile()
        {
            string path = "relatorio.csv";

            using StreamWriter streamWriter = new(path, false, new UTF8Encoding(true));

            streamWriter.WriteLine("Id,Nome,Pagamento,Terceirizado");
            foreach (var employee in Employee.Employees)
            {
                var isOutsourced = employee is OutSourcedEmployee ? "Sim" : "Não";
                streamWriter.WriteLine($"{employee.Id},{employee.Name},${employee.Payment():0.00},{isOutsourced}");
            }
            Console.WriteLine($"\nRelatório escrito em {Path.GetFullPath(path)}");
        }

        public void RegisterEmployee()
        {
            Console.WriteLine($"Funcionário #{Employee.CurrentId + 1}");
            bool isOutsourced = _inputReader.ReadString("O funcionário é terceirizado? (Y/N):").ToLower() == "y";

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
            AppState.IsRegisteringEmployees = _inputReader.ReadString("Deseja cadastrar outro usuário? (Y/N):").ToLower() == "y";
            AppState.IsValidEntries = true;
        }
    }
}
