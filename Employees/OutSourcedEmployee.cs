namespace Employment.Employees
{
    public class OutSourcedEmployee : Employee
    {
        public double AdditionalCharge { get; }

        public OutSourcedEmployee(int id, string name, uint hours, double valuePerHour, double additionalCharge) : base(id, name, hours, valuePerHour)
        {
            AdditionalCharge = additionalCharge;
        }

        public override double GetPayment()
        {
            return (base.GetPayment() + AdditionalCharge) * 1.16;
        }
    }
}
