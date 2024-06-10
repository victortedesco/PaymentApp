using Employment.App;
using Employment.Employees;

namespace Employment
{
    internal class Program
    {
        private readonly static InputReader _inputReader = new();

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var quantity = _inputReader.ReadUint("Digite a quantidade:");
            Console.Clear();

            for (int i = 0; i < quantity; i++)
            {
                RegisterEmployee();
            }

            DisplayEmployees();
            Console.ReadLine();
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
            Console.WriteLine();
        }

        private static void DisplayEmployees()
        {
            Console.WriteLine("Relatório:");
            foreach (Employee employee in Employee.Employees)
            {
                Console.WriteLine($"{employee.Name} | ${employee.GetPayment():0.00}");
            }
        }
    }
}
