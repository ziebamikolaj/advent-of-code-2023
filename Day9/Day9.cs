using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    class Day9
    {
        class ValueHistory
        {
            public List<long> HistoryValues { get; private set; } = new List<long>();
            public ValueHistory SubHistory { get; private set; }

            public ValueHistory(List<long> historyValues)
            {
                if (historyValues.All(n => n == 0))
                {
                    HistoryValues = null;
                    return;
                }
                HistoryValues = historyValues;
                List<long> subHistoryValues = new List<long>();
                for (int i = 1; i < historyValues.Count; i++)
                {
                    subHistoryValues.Add(historyValues[i] - historyValues[i - 1]);
                }
                SubHistory = new ValueHistory(subHistoryValues);
            }
            private long PredictNextValue()
            {
                if (HistoryValues == null) return 0;
                return HistoryValues.Last() + SubHistory.PredictNextValue();
            }
            private long PredictPreviousValue()
            {
                if (HistoryValues == null) return 0;
                return HistoryValues.First() - SubHistory.PredictPreviousValue();
            }
            public static long SumAllFuturePredictions(List<ValueHistory> historyValues)
            {
                long sum = 0;
                foreach (var value in historyValues)
                {
                    sum += value.PredictNextValue();
                }
                return sum;
            }
            public static long SumAllPastPredictions(List<ValueHistory> historyValues)
            {
                long sum = 0;
                foreach (var value in historyValues)
                {
                    sum += value.PredictPreviousValue();
                }
                return sum;
            }

            public static List<ValueHistory> GetHistoriesFromFile(string[] fileContent)
            {
                List<ValueHistory> histories = new List<ValueHistory>();
                foreach (string line in fileContent)
                {
                    List<long> parts = line.Split(new string[] { " " }, StringSplitOptions.None).Select(long.Parse).ToList();
                    histories.Add(new ValueHistory(parts));
                }

                return histories;
            }
            public override string ToString()
            {
                string result = $"Original history: {string.Join(", ", HistoryValues)}";
                //traverse through all sub histories and print them
                ValueHistory currentHistory = SubHistory;
                while (currentHistory.HistoryValues != null)
                {
                    result += $"{Environment.NewLine}\tSub history: {string.Join(", ", currentHistory.HistoryValues)}";
                    currentHistory = currentHistory.SubHistory;
                }
                return result;
            }
        }
        static void FirstPart(string fileContent)
        {
            Console.WriteLine("First part:");
            List<ValueHistory> valueHistories = ValueHistory.GetHistoriesFromFile(fileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            Console.WriteLine($"Sum of future predictions: {ValueHistory.SumAllFuturePredictions(valueHistories)}");

        }
        static void SecondPart(string fileContent)
        {
            Console.WriteLine("Second part:");
            List<ValueHistory> valueHistories = ValueHistory.GetHistoriesFromFile(fileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            Console.WriteLine($"Sum of past predictions: {ValueHistory.SumAllPastPredictions(valueHistories)}");
        }
        public static void ShowResults()
        {
            var fileContent = File.ReadAllText("../../Day9/day9input.txt");
            Console.WriteLine("Day 9:");
            FirstPart(fileContent);
            SecondPart(fileContent);
        }
    }
}