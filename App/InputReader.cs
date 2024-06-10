namespace Employment.App
{
    public class InputReader
    {
        public string ReadString(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine().Trim();
        }

        public uint ReadUint(string message)
        {
            uint value;
            do
            {
                Console.WriteLine(message);
            } while (!uint.TryParse(Console.ReadLine(), out value) || value == 0);
            return value;
        }

        public double ReadDouble(string message)
        {
            double value;
            do
            {
                Console.WriteLine(message);
            } while (!double.TryParse(Console.ReadLine(), out value) || value <= 0);
            return value;
        }
    }
}
