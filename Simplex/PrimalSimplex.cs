using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LPR381Project.Common;

namespace LPR381Project.Simplex.Primal
{
    enum PivotKind
    {
        Optimal,
        Infeasable,
        SubOptimal,
    }

    internal class PrimalSimplex
    {
        private Tableau table;
        private int iteration = 0;

        public PrimalSimplex(Tableau table)
        {
            this.table = table;
        }

        // TODO: return void since be are editing the table passe and not creating a new one
        public Tableau Solve()
        {
            PivotKind status = PivotKind.SubOptimal;

            // this.table.PrintTable(iteration);
            this.table.WriteTable(iteration);

            iteration++;

            while (status != PivotKind.Optimal)
            {
                status = this.Pivot();

                if (status != PivotKind.Optimal)
                {
                    //this.table.PrintTable(iteration);
                    this.table.WriteTable(iteration);

                    iteration++;
                }
            }

            Console.WriteLine();

            return this.table;
        }

        public PivotKind Pivot()
        {
            double[][] tmpTable = this
                .table.table.Select(innerList => innerList.ToArray())
                .ToArray();

            int pivotRow;
            int pivotCol;

            pivotCol = this.table.GetPivotCol();

            if (pivotCol == -1)
            {
                return PivotKind.Optimal;
            }

            pivotRow = this.table.GetPivotRow(pivotCol);

            if (pivotRow == -1)
            {
                // TODO: return PivotKind.Infeasable instead of ending process here
                Console.WriteLine($"Problem is infeasable at iteration {iteration}");
                Environment.Exit(1);
            }

            // calculate the pivotRow
            for (int i = 0; i < tmpTable[0].Length; i++)
            {
                var a = tmpTable[pivotRow][i];
                var b = tmpTable[pivotRow][pivotCol];

                this.table.table[pivotRow][i] = a / b;
            }

            for (int row = 0; row < tmpTable.Length; row++)
            {
                if (row == pivotRow)
                {
                    continue;
                }

                int rowSize = tmpTable[row].Length;
                for (int col = 0; col < rowSize; col++)
                {
                    var a = tmpTable[row][col];
                    var b = tmpTable[row][pivotCol];
                    var c = this.table.table[pivotRow][col];

                    this.table.table[row][col] = a - (b * c);
                }
            }

            return PivotKind.SubOptimal;
        }
    }
}
