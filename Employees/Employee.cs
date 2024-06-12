namespace PaymentApp.Employees
{
    public class Employee
    {
        public static List<Employee> Employees { get; } = [];
        public static uint CurrentId { get; set; } = 0;

        public uint Id { get; } = CurrentId;
        public string Name { get; }
        public uint Hours { get; }
        public double ValuePerHour { get; }

        public Employee(string name, uint hours, double valuePerHour)
        {
            Name = name;
            Hours = hours;
            ValuePerHour = valuePerHour;
        }

        public virtual double GetPayment()
        {
            return Hours * ValuePerHour;
        }

        public static void DisplayEmployees()
        {
            foreach (var employee in Employees)
            {
                Console.WriteLine(employee.ToString());
            }
        }

        public override string ToString()
        {
            return $"{this.Name} | ${this.GetPayment():0.00}";
        }
    }
}
