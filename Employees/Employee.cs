namespace Employment.Employees
{
    public class Employee
    {
        public static List<Employee> Employees { get; } = [];

        public int Id { get; }
        public string Name { get; }
        public uint Hours { get; }
        public double ValuePerHour { get; }

        public Employee(int id, string name, uint hours, double valuePerHour)
        {
            Id = id;
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
