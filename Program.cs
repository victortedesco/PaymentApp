﻿using Employment.App;
using Employment.Employees;

namespace Employment
{
    internal class Program
    {
        private readonly static InputReader _inputReader = new();
        private static bool isRegisteringEmployees = true;

        static void Main()
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (isRegisteringEmployees)
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
    }
}
