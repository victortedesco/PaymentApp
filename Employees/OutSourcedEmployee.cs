namespace PaymentApp.Employees
{
    public class OutSourcedEmployee : Employee
    {
        public double AdditionalCharge { get; }

        public OutSourcedEmployee(string name, uint hours, double valuePerHour, double additionalCharge) : base(name, hours, valuePerHour)
        {
            AdditionalCharge = additionalCharge;
        }

        public override double Payment()
        {
            return (base.Payment() + AdditionalCharge) * 1.16;
        }
    }
}
