using System;
using System.IO;

namespace AdventOfCode
{
    class DayX
    {
        static void FirstPart(string fileContent)
        {
            Console.WriteLine("First part:");
            Console.WriteLine();
        }
        static void SecondPart(string fileContent)
        {
            Console.WriteLine("Second part:");
            Console.WriteLine();
        }
        public static void ShowResults()
        {
            var fileContent = File.ReadAllText("../../dayxinput.txt");
            Console.WriteLine("Day X:");
            FirstPart(fileContent);
            SecondPart(fileContent);
        }
    }
}