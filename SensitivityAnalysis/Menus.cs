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

                conIdx -= 1;

                Console.Write("Enter new RHS value: ");

                // TODO: validate input
                var inputVal = Console.ReadLine();

                prelims.b[conIdx] = double.Parse(inputVal);

                running = false;
            }
        }
    }
}
