using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class Day5
    {
        public enum ItemType
        {
            Seed,
            Soil,
            Fertilizer,
            Water,
            Light,
            Temperature,
            Humidity,
            Location
        }
        public static ItemType NextItemType(ItemType type)
        {
            return (ItemType)(((double)type + 1) % 8);
        }
        public class Item
        {
            public double ItemID { get; set; }
            public ItemType Type { get; set; }
            public Item(double itemID, ItemType type)
            {
                ItemID = itemID;
                Type = type;
            }
            public override string ToString()
            {
                return $"ItemId: {ItemID}\tType: {Type}";
            }
        }

        public class Almanac
        {
            private readonly static string[] MapNames = new string[]
                {
                    "seed-to-soil",
                    "soil-to-fertilizer",
                    "fertilizer-to-water",
                    "water-to-light",
                    "light-to-temperature",
                    "temperature-to-humidity",
                    "humidity-to-location",
                 };
            public Dictionary<ItemType, AlmanacMap> Maps { get; set; }
            public List<Item> Items { get; set; }

            public Almanac(string[] fileContent)
            {
                Maps = AlmanacMap.GetMaps(fileContent);
                Items = GetItems(fileContent[0]);
            }
            public List<Item> GetItems(string firstLine)
            {
                List<Item> items = new List<Item>();
                foreach (var item in firstLine.Split(' '))
                {
                    if (item.Contains("seeds:") || !double.TryParse(item, out double itemID))
                        continue;
                    items.Add(new Item(itemID, ItemType.Seed));

                    foreach (var map in Maps)
                    {
                        double convertedID = map.Value.Convert(itemID);
                        items.Add(new Item(convertedID, NextItemType(map.Key)));
                        itemID = convertedID;
                    }
                }
                return items;
            }

            public double GetLowestLocationPart2(string firstLine)
            {
                var firstLineSplit = firstLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                double lowestLocation = double.MaxValue;
                double allNumbersToProcess = 0;
                long numbersProcessed = 0;

                // Pre-calculate the total numbers to process
                for (int i = 1; i < firstLineSplit.Length; i += 2)
                {
                    if (double.TryParse(firstLineSplit[i + 1], out double rangeLength))
                        allNumbersToProcess += rangeLength;
                }

                object lockObject = new object(); // For thread synchronization

                Parallel.ForEach(firstLineSplit.Where((_, index) => index % 2 != 0), firstLinePart =>
                {
                    if (!double.TryParse(firstLinePart, out double startRange) ||
                        !double.TryParse(firstLineSplit[Array.IndexOf(firstLineSplit, firstLinePart) + 1], out double rangeLength))
                        return;

                    for (double j = startRange; j < startRange + rangeLength; j++)
                    {
                        double currentItemID = j;
                        foreach (var map in Maps)
                        {
                            currentItemID = map.Value.Convert(currentItemID);
                        }

                        lock (lockObject)
                        {
                            if (currentItemID < lowestLocation)
                                lowestLocation = currentItemID;
                        }

                        // Batched progress update
                        if (Interlocked.Add(ref numbersProcessed, 1) % 1000 == 0)
                        {
                            lock (lockObject)
                            {
                                Console.Clear();
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.WriteLine("Numbers to process:\t\t" + allNumbersToProcess);
                                Console.WriteLine("Numbers processed:\t\t" + numbersProcessed);
                                Console.WriteLine("Progress:\t\t\t" + Convert.ToInt32(numbersProcessed * 100 / allNumbersToProcess) + " %");
                            }
                        }
                    }
                });

                return lowestLocation;
            }

            public class AlmanacMap
            {
                public string Name { get; set; }
                public List<Converter> Converters { get; set; }
                public AlmanacMap(string name, string[] fileContent)
                {
                    Name = name;
                    Converters = GetConverters(fileContent);
                }
                public double Convert(double source)
                {
                    foreach (var converter in Converters)
                    {
                        if (source >= converter.From && source < converter.From + converter.Length)
                        {
                            double offset = source - converter.From;
                            return converter.To + offset;
                        }
                    }
                    return source;
                }
                public double GetNextBreakpoint(double source)
                {
                    foreach (var converter in Converters)
                    {
                        if (source >= converter.From && source < converter.From + converter.Length)
                        {
                            return converter.From + converter.Length;
                        }
                    }
                    return source;
                }
                public List<Converter> GetConverters(string[] fileContent)
                {
                    List<Converter> converters = new List<Converter>();
                    bool isConverterSection = false;
                    foreach (var line in fileContent)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        if (line.Contains(Name + " map"))
                        {
                            isConverterSection = true;
                            continue;
                        }

                        if (isConverterSection && !line.Contains("map"))
                        {
                            var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length == 3 && double.TryParse(parts[0], out double to) && double.TryParse(parts[1], out double from) && double.TryParse(parts[2], out double length))
                            {
                                converters.Add(new Converter { From = from, To = to, Length = length });
                            }
                        }
                        if (!line.Contains(Name) && line.Contains("map"))
                        {
                            isConverterSection = false;
                        }
                    }

                    return converters;
                }
                public static Dictionary<ItemType, AlmanacMap> GetMaps(string[] fileContent)
                {
                    Dictionary<ItemType, AlmanacMap> maps = new Dictionary<ItemType, AlmanacMap>();
                    for (int i = 0; i < MapNames.Length; i++)
                    {
                        maps.Add((ItemType)i, new AlmanacMap(MapNames[i], fileContent));
                    }
                    return maps;
                }
                public class Converter
                {
                    public double From { get; set; }
                    public double To { get; set; }
                    public double Length { get; set; }
                }
            }
        }


        static void FirstPart(string fileContent)
        {
            Console.WriteLine("First part:");
            Almanac almanac = new Almanac(fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
            double lowestLocation = almanac.Items.Where(item => item.Type == ItemType.Location).Min(item => item.ItemID);
            for (int i = 0; i < almanac.Items.Count; i++)
            {
                if (i % 8 == 0) Console.Write(Environment.NewLine);
                Console.WriteLine(almanac.Items[i]);
            }
            Console.WriteLine();
            foreach (var item in almanac.Items.Where(item => item.Type == ItemType.Location).ToList())
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("\nLowest location:\t" + lowestLocation);
        }

        static void SecondPart(string fileContent)
        {
            Console.WriteLine("Are you ready to display second part? (no - to abort)");
            if (Console.ReadLine() == "no") return;
            Console.WriteLine("Second part:");
            Console.WriteLine();
            Almanac almanac = new Almanac(fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
            double lowestLocation = almanac.GetLowestLocationPart2(fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None)[0]);
            Console.WriteLine("\nLowest location:\t" + lowestLocation);
        }
        public static void ShowResults()
        {
            var fileContent = File.ReadAllText("../../Day5/day5input.txt");
            Console.WriteLine("Day 5:");
            FirstPart(fileContent);
            SecondPart(fileContent);
        }
    }
}