

namespace Stage0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome7875();
            Welcome8620();
            Console.ReadKey();
        }

        static partial void Welcome8620();

        private static void Welcome7875()
        {
            Console.WriteLine("Enter you name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}