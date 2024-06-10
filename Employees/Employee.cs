namespace Employment.Employees
{
    public class Employee
    {
        public static List<Employee> Employees { get; } = [];
        public static int CurrentId { get; set; } = 0;

        public int Id { get; } = CurrentId;
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
    }
}
