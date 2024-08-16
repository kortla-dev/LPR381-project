using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using LPR381Project.Common;
using System.Diagnostics;

namespace LPR381Project.Simplex.Dual
{
     enum DualPivotKind
    {
        Optimal,
        Infeasible,
        SubOptimal,
    }

    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    internal class DualSimplex
    {private Tableau table;
        private int iteration = 0;

        public DualSimplex(Tableau table)
        {
            this.table = table;
        }

        // Main solving method
        public Tableau Solve()
        {
            DualPivotKind status = DualPivotKind.SubOptimal;

            this.table.WriteTable(iteration);
            iteration++;

            while (status != DualPivotKind.Optimal)
            {
                status = this.Pivot();

                if (status != DualPivotKind.Optimal)
                {
                    this.table.WriteTable(iteration);
                    iteration++;
                }
            }

            Console.WriteLine();
            return this.table;
        }
  // Perform a pivot operation
        public DualPivotKind Pivot()
        {
            int pivotRow, pivotCol;

           
            pivotCol = this.table.GetPivotCol();  // Implement this method to select the pivot column
            pivotRow = this.table.GetPivotRow(pivotCol);  // Implement this method to select the pivot row
            if (pivotRow == -1 || pivotCol == -1)
            {
                return DualPivotKind.Optimal;
            }

            if (pivotRow == -1)
            {
                // Handle infeasibility case if needed
                Console.WriteLine($"Problem is infeasible at iteration {iteration}");
                Environment.Exit(1);
            }

            this.ManualPivot(pivotRow, pivotCol);

            return DualPivotKind.SubOptimal;
        }

        // Perform the pivot operation manually
        public void ManualPivot(int pivotRow, int pivotCol)
        {
            double[][] tmpTable = this
                           .table.table.Select(innerList => innerList.ToArray())
                           .ToArray();

            // Normalize pivot row
            for (int i = 0; i < tmpTable[0].Length; i++)
            {
                var a = tmpTable[pivotRow][i];
                var b = tmpTable[pivotRow][pivotCol];

                this.table.table[pivotRow][i] = a / b;
            }

            // Update other rows
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
        }

        // Method to get the pivot column
        public int GetPivotCol()
        {
            int numCols = 1; // Assuming table is a List<List<double>>
            //use get row lengt
            int pivotCol = -1;
            double mostNegativeCost = 0;

            for (int col = 0; col < numCols - 1; col++) // Last column is RHS
            {
                if (this.table.table[0][col] < mostNegativeCost)
                {
                    mostNegativeCost = this.table.table[0][col];
                    pivotCol = col;
                }
            }

            return pivotCol;
        }

        // Method to get the pivot row
        public int GetPivotRow(int pivotCol)
        {
            int numRows = this.table.LenRows();
            int pivotRow = -1;
            double minRatio = double.MaxValue;

            for (int row = 1; row < numRows; row++) // Skip the objective row
            {
                if (this.table.table[row][pivotCol] > 0)
                {
                    double ratio = this.table.table[row][^1] / this.table.table[row][pivotCol]; // RHS / pivot column value

                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        pivotRow = row;
                    }
                }
            }

            return pivotRow;
        
    }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
