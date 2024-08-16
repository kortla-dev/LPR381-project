/**using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LPR381Project.Common;


namespace LPR381Project.Branching.Knapsack
{
    internal class Knapsack
    {
        private Tableau table;
        public int RHS = 0; // remeber to do something with this
        private int iteration = 0;
        public int lengt = new Common.Tableau.LenRows(); 

        public array Knapsack(Tableau table)
        {
            this.table = table;
            //find size of inner list
            //public int lengt = new Common.Tableau.LenRows(); 
            //check only 1 constraint
            //gwt tablue into arr so we can manipulate easier
            Array current = new Array[5;lengt];
            for (int i = 0; i<lengt; i++)
            {
                current[0,i]= i+1;
                current[1,i]=this.table[0][i] ;
                current[2,i]=this.table[1][i] ;
                current[3,i]= 0;
                current[4,i]= 0;
                current[5,i]= 0;
                
                //list 1; integerindex representing int
            
            //list 2: z function coeficcients
            //list 3; constraint coeficcients
            //list 4 flag includ4ed or not
            //list 5 : PINNED OR NOT
            //list 6 ratio
            //var RHS*******
            return current;
            }


        
        public int Iteration(Array current) 
        {
            for (int i = 0; i<lengt; i++){ 
                //ratio test, rank
                current[5;i] = current[1,i]/current[2,i];

                //sort according to ratio

                //RHS

                //RHS-L3 if>0 l4=1 ; if ans<0 branch  L5=1 , move positions and branch. 
            // candidate value = sum flag(l4)*Z(l2)



            }





        }
        

        

            
           
        
        
       
        //some data structure to keep further itterations
        //itterate: 
            //RHS-L3 if>0 l4=1 ; if ans<0 branch  L5=1 , move positions and branch. 
            // candidate value = sum flag(l4)*Z(l2)
        }
        
        

    }
}
**/
using System;
using System.Collections.Generic;
using System.Transactions;
using LPR381Project.Common;

namespace LPR381Project.Branching.Knapsack
{
    internal class Knapsack
    {
        private Tableau table;
        private int RHS = 0; // This should be set or used later in the code
        private int iteration = 0;
        private int length; // To store the number of rows in the tableau

        public Knapsack(Tableau table)
        {
            this.table = table;
            this.length = table.LenRows(); // Initialize length with the number of rows in the tableau
        }

        public double[,] InitializeKnapsack()
        {
            double[,] current = new double[6, length]; // 6 rows for the structure you've described

            for (int i = 0; i < length; i++)
            {
                current[0, i] = i + 1;               // Index
                current[1, i] = table.table[0][i];         // Z function coefficients
                current[2, i] = table.table[1][i];         // Constraint coefficients
                current[3, i] = 0;                   // Flag for included or not
                current[4, i] = 0;                   // Pinned or not
                current[5, i] = 0;                   // Ratio (Z/Constraint)
            }

            return current;
        }

        public void Iteration(double[,] current)
        {
            for (int i = 0; i < length; i++)
            {
                // Ratio test
                current[5, i] = current[2, i] != 0 ? current[1, i] / current[2, i] : 0;

                // Sorting according to the ratio could be done here (not implemented in this example)

                // RHS - L3 if > 0, L4 = 1; if < 0, branch, L5 = 1, move positions and branch.
                double result = RHS - current[2, i];
                if (result > 0)
                {
                    double value = current[2, i];
                    current[3, i] = 1; // Flag it as included
                    
                }
                else
                {
                    current[4, i] = 1; // Pin it (//add this instance into a libary to deal with in a future itteration branching would involve more logic)
                }
                DisplayKnapsack(current);
                
                


                // Calculate the candidate value as the sum of the flagged Z coefficients
                // This could be done outside of the loop or with more logic
            }

            // Additional logic for branching and further iterations would be added here
           

        }
        public void DisplayKnapsack(double[,] knapsackTable)
        {
            int rows = knapsackTable.GetLength(0);
            int columns = knapsackTable.GetLength(1);

            // Display the headers
            Console.WriteLine("Index\tZ Coeff\tConstraint\tIncluded\tPinned\tRatio");

            // Display the data
            for (int col = 0; col < columns; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    Console.Write($"{knapsackTable[row, col]:F2}\t");
                }
                Console.WriteLine();
            }
        }

    }
    
}