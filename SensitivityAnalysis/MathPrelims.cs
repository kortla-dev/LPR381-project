using LPR381Project.Common;
using MathNet.Numerics.LinearAlgebra;

namespace LPR381Project.SensitivityAnalysis.Preliminaries
{
    internal class Prelims
    {
        public Tableau optimal;
        public double[][] initial;

        public List<int> Xbv;
        public List<int> Xnbv;
        public Vector<double> Cbv;
        public Vector<double> Cnbv;
        public Vector<double> b;
        public Matrix<double> B;
        public Matrix<double> BInv;
        public Vector<double> CbvBInv;
        public Matrix<double> N;

        public Matrix<double> Nt;

        public Prelims(Tableau optimal, double[][] initial)
        {
            this.optimal = optimal;
            this.initial = initial;

            int numVars = this.optimal.CountVars();
            int numCons = this.optimal.CountCons();

            if (numVars != numCons)
            {
                Console.WriteLine("Cannot inverse a non-square matrix");
                Environment.Exit(0);
            }

            this.Xbv = new List<int>();
            this.Xnbv = new List<int>();
            this.Cbv = Vector<double>.Build.Dense(1);
            this.Cnbv = Vector<double>.Build.Dense(1);
            this.b = Vector<double>.Build.Dense(1);
            this.B = Matrix<double>.Build.Dense(1, 1);
            this.BInv = Matrix<double>.Build.Dense(1, 1);
            this.CbvBInv = Vector<double>.Build.Dense(1);
            this.N = Matrix<double>.Build.Dense(1, 1);
            this.Nt = Matrix<double>.Build.Dense(1, 1);
        }

        public void Calculate()
        {
            this.Xbv = this.GetXbv();
            this.Xnbv = this.GetXnbv();
            this.Cbv = this.GetCbv();
            // Console.WriteLine("Cbv\n" + $"{this.Cbv}");
            this.Cnbv = this.GetCnbv();
            // Console.WriteLine("Cnbv\n" + $"{this.Cnbv}");
            this.b = this.Getb();
            // Console.WriteLine("b\n" + $"{this.b}");
            this.B = this.GetB();
            // Console.WriteLine("B\n" + $"{this.B}");
            this.BInv = this.B.Inverse();
            // Console.WriteLine("B^-1\n" + $"{this.BInv}");
            this.CbvBInv = this.Cbv * this.BInv;
            // Console.WriteLine("CbvB^-1\n" + $"{this.CbvBInv}");
            this.N = this.GetN();
            // Console.WriteLine("N\n" + $"{this.N}");
            this.Nt = this.NewTable();
        }

        private List<int> GetXbv()
        {
            List<int> indexes = new();
            int numCons = this.optimal.CountCons();
            int lenRow = this.optimal.LenRows();

            for (int row = 1; row < numCons + 1; row++)
            {
                for (int col = 0; col < lenRow; col++)
                {
                    var val = this.optimal.table[row][col];
                    if (val != 1.0)
                    {
                        continue;
                    }

                    if (this.optimal.IsBasic(col))
                    {
                        indexes.Add(col);
                    }
                }
            }

            return indexes;
        }

        private List<int> GetXnbv()
        {
            List<int> indexes = new();
            int lenRow = this.optimal.LenRows() - 1;

            for (int col = 0; col < lenRow; col++)
            {
                if (!this.optimal.IsBasic(col))
                {
                    indexes.Add(col);
                }
            }

            return indexes;
        }

        private Vector<double> GetCbv()
        {
            int numBasics = this.Xbv.Count;
            var Cbv = Vector<double>.Build.Dense(numBasics);

            for (int i = 0; i < numBasics; i++)
            {
                Cbv[i] = this.initial[0][this.Xbv[i]] * -1;
            }

            return Cbv;
        }

        private Vector<double> GetCnbv()
        {
            int numNonBasics = this.Xnbv.Count;
            var Cnbv = Vector<double>.Build.Dense(numNonBasics);

            for (int i = 0; i < numNonBasics; i++)
            {
                Cnbv[i] = this.initial[0][this.Xnbv[i]] * -1;
            }

            return Cnbv;
        }

        private Vector<double> Getb()
        {
            int numCons = this.optimal.CountCons();
            var b = Vector<double>.Build.Dense(numCons);

            for (int i = 0; i < numCons; i++)
            {
                b[i] = this.initial[i + 1][^1];
            }

            return b;
        }

        private Matrix<double> GetB()
        {
            int numXbv = this.Xbv.Count;
            int numCons = this.optimal.CountCons();

            var B = Matrix<double>.Build.Dense(numXbv, numCons);

            for (int row = 0; row < numCons; row++)
            {
                for (int col = 0; col < numXbv; col++)
                {
                    B[row, col] = this.initial[row + 1][this.Xbv[col]];
                }
            }

            return B;
        }

        private Matrix<double> GetN()
        {
            int numXnbv = this.Xnbv.Count;
            int numCons = this.optimal.CountCons();

            var N = Matrix<double>.Build.Dense(numXnbv, numCons);

            for (int row = 0; row < numCons; row++)
            {
                for (int col = 0; col < numXnbv; col++)
                {
                    N[row, col] = this.initial[row + 1][this.Xnbv[col]];
                }
            }

            return N;
        }

        public double GetOriginalZ()
        {
            return this.optimal.table[0][^1];
        }

        public List<Vector<double>> GetAVectors()
        {
            List<Vector<double>> vec = new();

            int numVars = this.optimal.CountVars();
            int numCons = this.optimal.CountCons();

            int numVarCon = numVars + numCons;

            // vars + cons
            for (int col = 0; col < numVarCon; col++)
            {
                var tmpVec = Vector<double>.Build.Dense(numCons);

                for (int row = 0; row < numCons; row++)
                {
                    tmpVec[row] = this.initial[row + 1][col];
                }

                vec.Add(tmpVec);
            }

            return vec;
        }

        public Matrix<double> NewTable()
        {
            (int rows, int cols) = this.optimal.GetSize();
            var newTable = Matrix<double>.Build.Dense(rows, cols);

            var AVec = this.GetAVectors();

            int numVars = this.optimal.CountVars();
            int numCons = this.optimal.CountCons();
            int numVarCon = numVars + numCons;

            // z row
            for (int col = 0; col < numVarCon; col++)
            {
                var a = this.CbvBInv;
                var b = AVec[col];
                var c = this.initial[0][col] * -1;

                newTable[0, col] = (a * b) - c;
            }

            // z value
            var zRhs = this.optimal.LenRows() - 1;
            newTable[0, zRhs] = this.CbvBInv * this.b;

            // constraints
            for (int col = 0; col < numVarCon; col++)
            {
                var vecTmp = this.BInv * AVec[col];

                for (int row = 0; row < numCons; row++)
                {
                    newTable[row + 1, col] = vecTmp[row];
                }
            }

            // b
            var bNew = this.BInv * this.b;
            for (int row = 0; row < numCons; row++)
            {
                newTable[row + 1, zRhs] = bNew[row];
            }

            // Apply thresholding to replace very small numbers with 0
            double threshold = 1e-10; // 1*10^-10
            newTable = newTable.Map(value => Math.Abs(value) < threshold ? 0 : value);

            return newTable;
        }
    }
}
