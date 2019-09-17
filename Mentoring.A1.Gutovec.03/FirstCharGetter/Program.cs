using System;

namespace FirstCharGetter
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Input string: ");
                var inputString = Console.ReadLine();

                try
                {
                    Console.WriteLine($"First char is: {inputString.FirstChar()}");
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine($"Incorrect data: {e.Message} \n");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong...");
                    break;
                }

                if (!IsContinueChosen())
                {
                    break;
                }
            }

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }

        private static bool IsContinueChosen()
        {
            Console.WriteLine("Would you like to continue?(y/n):");
            var key = Console.ReadKey();
            Console.WriteLine();
            return !key.Key.ToString().Equals("n", StringComparison.OrdinalIgnoreCase);
        }

    }
}
