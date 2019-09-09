using System;

namespace FirstCharGetter
{
    class Program
    {
        static void Main(string[] args)
        {
            bool flagContinue = true;
            while (flagContinue)
            {
                Console.WriteLine("Input string: ");
                string inputString = Console.ReadLine();

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
                    flagContinue = false;
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
            if (key.Key.ToString().Equals("n", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }

    }
}
