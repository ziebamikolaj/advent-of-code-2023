using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    class Day10
    {
        class PipeMaze
        {
            private Tile[,] grid;
            private Tile startTile;

            public PipeMaze(string[] fileContent)
            {
                int height = fileContent.Length;
                int width = fileContent[0].Length;
                grid = new Tile[width, height];

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        grid[x, y] = new Tile(x, y, fileContent[y][x]);
                        if (grid[x, y].Value == 'S')
                        {
                            startTile = grid[x, y];
                        }
                    }
                }

                foreach (Tile tile in grid)
                {
                    tile.SetConnectedNeighbors(GetConnectedNeighbors(tile));
                }
            }

            public override string ToString()
            {
                var builder = new StringBuilder();
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    for (int x = 0; x < grid.GetLength(0); x++)
                    {
                        builder.Append(grid[x, y]);
                    }
                    builder.AppendLine();
                }
                return builder.ToString();
            }

            public int GetDistanceToFurthestPoint()
            {
                Queue<Tile> queue = new Queue<Tile>();

                startTile.Visited = true;
                queue.Enqueue(startTile);
                while (queue.Count > 0)
                {
                    Tile currentTile = queue.Dequeue();
                    foreach (Tile neighbor in currentTile.ConnectedNeighbors)
                    {
                        if (!neighbor.Visited)
                        {
                            neighbor.Visited = true;
                            queue.Enqueue(neighbor);
                        }
                    }
                }
                return grid.Cast<Tile>().Where(t => t.Visited).Count() / 2;
            }

            private List<Tile> GetConnectedNeighbors(Tile tile)
            {
                var neighbors = new List<Tile>();
                var tilesToCheck = tile.TilesToCheck();
                for (int i = 0; i < tilesToCheck.GetLength(0); i++)
                {
                    var x = tile.X + tilesToCheck[i, 0];
                    var y = tile.Y + tilesToCheck[i, 1];
                    if (AreCoordinatesValid(x, y))
                    {
                        var neighbor = grid[x, y];
                        if (tile.IsConnectedTo(neighbor))
                        {
                            neighbors.Add(neighbor);
                        }
                    }
                }
                return neighbors;
            }
            public bool AreCoordinatesValid(int x, int y)
            {
                return x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1);
            }
            public class Tile
            {
                public int X { get; private set; }
                public int Y { get; private set; }
                public char Value { get; private set; }
                public bool Visited { get; set; }
                public int Distance { get; set; }
                public List<Tile> ConnectedNeighbors { get; set; }

                public Tile(int x, int y, char value)
                {
                    X = x;
                    Y = y;
                    Value = ConvertToBoxDrawingChar(value);
                    ConnectedNeighbors = new List<Tile>();
                    Visited = false;
                    Distance = 0;
                }
                public int[,] TilesToCheck()
                {
                    return Value switch
                    {
                        '│' => new int[,] { { 0, -1 }, { 0, 1 } },
                        '─' => new int[,] { { -1, 0 }, { 1, 0 } },
                        '└' => new int[,] { { 0, -1 }, { 1, 0 } },
                        '┘' => new int[,] { { -1, 0 }, { 0, -1 } },
                        '┐' => new int[,] { { -1, 0 }, { 0, 1 } },
                        '┌' => new int[,] { { 0, 1 }, { 1, 0 } },
                        'S' => new int[,] { { 0, -1 }, { 0, 1 }, { -1, 0 }, { 1, 0 } },
                        _ => new int[,] { }
                    };
                }

                public bool IsConnectedTo(Tile neighbor)
                {
                    if (neighbor == null) return false;

                    int dx = neighbor.X - X;
                    int dy = neighbor.Y - Y;

                    return Value switch
                    {
                        '│' => dx == 0 && dy != 0,
                        '─' => dx != 0 && dy == 0,
                        '└' => (dx == 1 && dy == 0) || (dx == 0 && dy == -1),
                        '┘' => (dx == -1 && dy == 0) || (dx == 0 && dy == -1),
                        '┐' => (dx == -1 && dy == 0) || (dx == 0 && dy == 1),
                        '┌' => (dx == 1 && dy == 0) || (dx == 0 && dy == 1),
                        'S' => IsConnectedFromStart(neighbor, dx, dy),
                        _ => false
                    };
                }

                private bool IsConnectedFromStart(Tile neighbor, int dx, int dy)
                {
                    return neighbor.Value switch
                    {
                        '│' => dx == 0,
                        '─' => dy == 0,
                        '└' => (dx == 1 && dy == 0) || (dx == 0 && dy == -1),
                        '┘' => (dx == -1 && dy == 0) || (dx == 0 && dy == -1),
                        '┐' => (dx == -1 && dy == 0) || (dx == 0 && dy == 1),
                        '┌' => (dx == 1 && dy == 0) || (dx == 0 && dy == 1),
                        _ => false
                    };
                }

                public static char ConvertToBoxDrawingChar(char c)
                {
                    return c switch
                    {
                        '|' => '│',
                        '-' => '─',
                        'L' => '└',
                        'J' => '┘',
                        '7' => '┐',
                        'F' => '┌',
                        _ => c
                    };
                }
                public bool IsPipe()
                {
                    return "│─└┘┐┌".Contains(Value);
                }
                public override string ToString()
                {
                    return Value.ToString();
                }

                internal void SetConnectedNeighbors(List<Tile> tiles)
                {
                    ConnectedNeighbors = tiles;
                }
            }
        }
        static void FirstPart(string fileContent)
        {
            Console.WriteLine("First part:");
            var maze = new PipeMaze(fileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            Console.WriteLine(maze);
            Console.WriteLine(maze.GetDistanceToFurthestPoint());
        }
        static void SecondPart(string fileContent)
        {
            Console.WriteLine("Second part:");
            Console.WriteLine();
        }
        public static void ShowResults()
        {
            var fileContent = File.ReadAllText("../../Day10/day10input.txt");
            Console.WriteLine("Day X:");
            FirstPart(fileContent);
            SecondPart(fileContent);
        }
    }
}