using System;
using System.Linq;
using System.Reflection;

namespace AdventOfCode
{
    class AdventOfCode
    {
        static void Main()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Get all 'Day' classes in the 'AdventOfCode' namespace
            var dayClasses = assembly.GetTypes()
                                     .Where(t => t.IsClass &&
                                                 t.Namespace == "AdventOfCode" &&
                                                 t.Name.Contains("Day"))
                                     .ToList();

            while (true)
            {
                Console.WriteLine("Enter the day number to show results (or 'exit' to quit):");
                string input = Console.ReadLine();
                Console.Clear();
                if (input.ToLower() == "exit")
                    break;

                // Find the specific class
                string className = $"Day{input}";
                var dayClass = dayClasses.FirstOrDefault(c => c.Name == className);

                if (dayClass != null)
                {
                    MethodInfo showResultsMethod = dayClass.GetMethod("ShowResults");

                    if (showResultsMethod != null)
                    {
                        // Create an instance of the class and invoke the method
                        var instance = Activator.CreateInstance(dayClass);
                        showResultsMethod.Invoke(instance, null);
                    }
                    else
                    {
                        Console.WriteLine($"'ShowResults' method not found in {className}.");
                    }
                }
                else
                {
                    Console.WriteLine($"Class for Day {input} not found.");
                }
            }
        }
    }
}
