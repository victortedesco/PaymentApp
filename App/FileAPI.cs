using PaymentApp.Employees;

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
                int i = 0;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.StartsWith('#') || line.StartsWith("//")) continue;

                    i++;
                    Employee.CurrentId++;
                    var data = line.Split(';');

                    try
                    {
                        if (data.Length > 4)
                        {
                            Console.WriteLine($"A linha {i} possui mais de 4 entradas, elas serão ignoradas.");
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
                        Console.WriteLine($"Erro ao importar funcionário da linha {i}: {e.Message}");
                    }
                }
                AppState.IsValidEntries = true;
                bool registerMoreEntries = _inputReader.ReadString("Deseja cadastrar outro usuário? (Y/N):").ToLower()[0] == 'y';
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

        public void WriteToFile()
        {
            string path = "relatorio.csv";

            using StreamWriter streamWriter = new(path, false);

            streamWriter.WriteLine("Id,Nome,Pagamento");
            foreach (var employee in Employee.Employees)
            {
                streamWriter.WriteLine($"{employee.Id},{employee.Name},${employee.GetPayment():0.00}");
            }
            Console.WriteLine($"\nRelatório escrito em {Path.GetFullPath(path)}");
        }

        public void RegisterEmployee()
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
            AppState.IsRegisteringEmployees = _inputReader.ReadString("Deseja cadastrar outro usuário? (Y/N):").ToLower()[0] == 'y';
            AppState.IsValidEntries = true;
        }
    }
}
