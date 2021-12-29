using System;

namespace IBMPonderThisDec2021
{
    class Program
    {
        public static int cellsn;
        public static int ballsm;
        public static string combination;
        public static string permutation;
        public static int pinCount;
        static void Main(string[] args)
        {
            Console.WriteLine("IBM Ponder This december 2021");
            Console.WriteLine("This tool is just a simulator for the drop of the balls");
            Console.WriteLine("Type the number of cells: ");
            while (true)
            {
                int n;
                string testString = Console.ReadLine();
                bool isNumeric = int.TryParse(testString, out n);

                if (isNumeric && n > 1)
                {
                    cellsn = n;
                    break;
                }
                else
                    Console.WriteLine("invalid number, please try again");
            }
            Console.WriteLine("Type the number of balls: ");
            while (true)
            {
                int n;
                string testString = Console.ReadLine();
                bool isNumeric = int.TryParse(testString, out n);

                if (isNumeric && n > 0)
                {
                    ballsm = n;
                    break;
                }
                else
                    Console.WriteLine("invalid number, please try again");
            }

            for (int pin = cellsn - 1; pin > 0; pin--) // Gets the ammount of pins
            {
                pinCount = pinCount + pin;
            }

            Console.WriteLine("Type the combination of the pins: ");
            Console.WriteLine("use this format, i.e:     RRRRRLLRLR");
            while (true)
            {
                string testString = Console.ReadLine();
                bool validString = true;
                foreach(char charInTest in testString)
                {
                    if(charInTest != '<' && charInTest != '>' && charInTest != 'R' && charInTest != 'L')
                        validString = false;
                }

                if (validString && testString.Length == pinCount)
                {
                    combination = testString;
                    break;
                }
                else
                    Console.WriteLine("invalid combination, please try again");
            }

            Console.WriteLine("Type the permutation of the cells: ");
            Console.WriteLine("use this format, i.e:     1,2,5,4,3");
            while (true)
            {
                string testString = Console.ReadLine();
                if (!testString.Contains(','))
                {
                    Console.WriteLine("invalid combination, please try again");
                    continue;
                }
                string[] testStringArray = testString.Split(',');

                if(!(testStringArray.Length == cellsn))
                {
                    Console.WriteLine("invalid combination, please try again");
                    continue;
                }

                bool validNumbers = true;
                foreach(string permutation in testStringArray)
                {
                    int n;
                    bool isNumeric = int.TryParse(permutation, out n);
                    if (!isNumeric || n <= 0 || n > cellsn)
                        validNumbers = false;
                }
                if (!validNumbers)
                {
                    Console.WriteLine("invalid combination, please try again");
                    continue;
                }
                if (!CheckDuplicates(testStringArray))
                {
                    Console.WriteLine("invalid permutation, duplicated numbers");
                    continue;
                }
                permutation = testString;
                break;
            }

            int[] cellResult = new int[cellsn + 1];
            pinCount = 0;

            // Build pins and permutation arrays based on inputs
            string[,] pins = new string[cellsn, cellsn];
            string[] permut = permutation.Split(",");
            int stepCount = 0;
            char[] combArray = combination.ToCharArray();

            for (int stepRow = 1; stepRow < cellsn; stepRow++)
            {
                for (int stepColumn = 1; stepColumn <= stepRow; stepColumn++)
                {
                    pins[stepRow, stepColumn] = combArray[stepCount].ToString();
                    stepCount++;
                }
            }
            
            // Looking for the answer!




            cellResult = CellSimulation(combination, pins);
            double answer = UtilityFunction(cellResult, permut);

            Console.WriteLine("The cell result is: ");
            foreach (int cellBalls in cellResult)
            {
                if (cellBalls == 0)
                    continue;
                Console.Write("|" + cellBalls.ToString() + "| ");
            }
            Console.WriteLine();
            Console.WriteLine("The utility is: " + answer);

        }

        static int[] CellSimulation(string combin, string[,] pins) //Simulates all the event of all balls falling and returns the amout of balls in each cell
        {
            char[] combArray = combin.ToCharArray();
            int position = 1;
            int[] cellResult = new int[cellsn + 1];

            for (int ball = 1; ball <= ballsm; ball++)
            {
                position = 1;
                for (int step = 1; step < cellsn; step++)
                {
                    while (true)
                    {
                        if (pins[step, position] == "R")
                        {
                            pins[step, position] = "L";
                            position++;
                            break;
                        }
                        else if (pins[step, position] == ">")
                        {
                            position++;
                            break;
                        }
                        else if (pins[step, position] == "L")
                        {
                            pins[step, position] = "R";
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                cellResult[position]++;
            }

            return cellResult;
        }

        static double UtilityFunction(int[] cellResults, string[] permutation)
        {
            double utility = 0;

            for(int cell = 1; cell <= cellsn; cell++)
            {
                if (cell == 1)
                    utility = Math.Pow(((double)(cellsn * cellResults[Convert.ToInt32(permutation[cell - 1])]) / ballsm), cell);
                else
                    utility = utility * Math.Pow(((double)(cellsn * cellResults[Convert.ToInt32(permutation[cell - 1])]) / ballsm), cell);
            }
            return utility;
        }

        static bool CheckDuplicates(string[] sampleStringArray)
        {
            bool noDuplicates = true;

            for(int i = 0; i < sampleStringArray.Length; i++)
            {
                for(int j = i + 1; j < sampleStringArray.Length; j++)
                {
                    if (sampleStringArray[i] == sampleStringArray[j])
                    {
                        noDuplicates = false;
                        break;
                    }
                }
            }

            return noDuplicates;
        }

    }
}
