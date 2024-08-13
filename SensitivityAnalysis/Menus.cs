using LPR381Project.SensitivityAnalysis.Preliminaries;

namespace LPR381Project.SensitivityAnalysis.Menus
{
    internal class Menus
    {
        public static void ShadowPrices(Prelims prelims)
        {
            List<double> shadowPrices = new();
            Console.WriteLine("Shadow Prices");

            for (int i = 0; i < prelims.b.Count; i++)
            {
                double bTmp = prelims.b[i];
                double zTmp = prelims.GetOriginalZ();

                prelims.b[i] += 1;

                double zNew = prelims.CbvBInv * prelims.b;

                prelims.b[i] = bTmp;

                Console.WriteLine($"x{i + 1}: R{Math.Round(zNew - zTmp, 2)}");
            }
        }

        public static void ChangeRhs(Prelims prelims)
        {
            var numCons = prelims.optimal.CountCons();

            bool running = true;

            while (running)
            {
                for (int con = 0; con < numCons; con++)
                {
                    Console.WriteLine($"{con + 1}. con{con + 1}");
                }

                Console.Write("\nSelect a constraint: ");

                var inputIdx = Console.ReadLine();

                int conIdx;

                if (int.TryParse(inputIdx, out conIdx))
                {
                    if (conIdx < 0 || conIdx > numCons)
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid option please try again");
                        continue;
                    }
                }

                // conIdx -= 1;

                Console.Write("Enter new RHS value: ");

                // TODO: validate input
                var inputVal = Console.ReadLine();

                prelims.initial[conIdx][^1] = double.Parse(inputVal);

                running = false;
            }

            prelims.Calculate();
        }

        public static void RangeOfBasic(Prelims prelims)
        {
            for (int idx = 0; idx < prelims.Xbv.Count; idx++)
            {
                string var = prelims.optimal.colToVar[prelims.Xbv[idx]];
                Console.WriteLine($"{var}: {(int)prelims.Nt[0, prelims.Xbv[idx]]}");
            }
        }

        public static void RangeOfNonBasic(Prelims prelims)
        {
            for (int idx = 0; idx < prelims.Xnbv.Count; idx++)
            {
                string var = prelims.optimal.colToVar[prelims.Xnbv[idx]];
                Console.WriteLine($"{var}: {(int)prelims.Nt[0, prelims.Xnbv[idx]]}");
            }
        }

        public static void ChangeBasicCoef(Prelims prelims)
        {
            int numBasic = prelims.Xbv.Count;

            bool running = true;

            while (running)
            {
                for (int basics = 0; basics < numBasic; basics++)
                {
                    string varName = prelims.optimal.colToVar[prelims.Xbv[basics]];
                    Console.WriteLine($"{basics + 1}. {varName}");
                }

                Console.Write("Enter a number: ");

                var inputIdx = Console.ReadLine();

                int conIdx;

                if (int.TryParse(inputIdx, out conIdx))
                {
                    if (conIdx < 0 || conIdx > prelims.Xbv.Count)
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid option please try again");
                        continue;
                    }
                }

                conIdx--;

                Console.Write("Enter new value: ");

                // TODO: validate input
                var inputVal = Console.ReadLine();

                prelims.initial[0][prelims.Xbv[conIdx]] = double.Parse(inputVal);

                running = false;
            }

            prelims.Calculate();
        }

        public static void ChangeNonBasicCoef(Prelims prelims)
        {
            int numNonBasic = prelims.Xnbv.Count;

            bool running = true;

            while (running)
            {
                for (int basics = 0; basics < numNonBasic; basics++)
                {
                    string varName = prelims.optimal.colToVar[prelims.Xnbv[basics]];
                    Console.WriteLine($"{basics + 1}. {varName}");
                }

                Console.Write("Enter a number: ");

                var inputIdx = Console.ReadLine();

                int conIdx;

                if (int.TryParse(inputIdx, out conIdx))
                {
                    if (conIdx < 0 || conIdx > prelims.Xbv.Count)
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid option please try again");
                        continue;
                    }
                }

                conIdx--;

                Console.Write("Enter new value: ");

                // TODO: validate input
                var inputVal = Console.ReadLine();

                prelims.initial[0][prelims.Xnbv[conIdx]] = double.Parse(inputVal);

                running = false;
            }

            prelims.Calculate();
        }
    }
}
