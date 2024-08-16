using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381Project.Branching.CutPlane
{
    internal class CuttingPlane
    {
        public Solution Solve(LPModel model)
    {
        Console.WriteLine("We can call");
        var solution = new Solution();
        var cuts = new List<double[]>();
 
        int iteration = 0;
        bool isInteger = false;
        double epsilon = 1e-6;
 
        // Extract coefficients and constraints from the LPModel
        double[] coefficients = model.Coefficients.Select(c => c.Value).ToArray();
        double[] constraints = model.Constraints.Select(c => c.Coefficients.Sum(co => co.Value)).ToArray();
        double rhs = model.Constraints.Sum(c => c.RightHandSide);
 
        while (!isInteger)
        {
            iteration++;
            Console.WriteLine($"Iteration {iteration}");
 
            
            double[] currentSolution = SolveLinearRelaxation(model, coefficients, rhs, cuts);
            solution.OptimalValues = currentSolution.ToList();
            solution.ObjectiveValue = CalculateObjectiveValue(coefficients, currentSolution);
 
            Console.WriteLine("Current Solution:");
            for (int i = 0; i < currentSolution.Length; i++)
            {
                Console.WriteLine($"x{i + 1} = {currentSolution[i]}");
            }
 
            
            isInteger = currentSolution.All(x => Math.Abs(x - Math.Round(x)) < epsilon);
 
            if (isInteger)
            {
                Console.WriteLine("Integer solution found.");
                break;
            }
 
            
            double[] newCut = GenerateCut(currentSolution);
            cuts.Add(newCut);
 
            Console.WriteLine("Added a new cut.");
        }
 
        
        Console.WriteLine("Final Integer Solution:");
        for (int i = 0; i < solution.OptimalValues.Count; i++)
        {
            Console.WriteLine($"x{i + 1} = {Math.Round(solution.OptimalValues[i])}");
        }
        Console.WriteLine($"Optimal objective value: {solution.ObjectiveValue}");
 
        return solution;
    }
 
    private double[] SolveLinearRelaxation(LPModel model, double[] coefficients, double rhs, List<double[]> cuts)
    {
        int numVariables = coefficients.Length;
 
       
        double[] constraints = new double[numVariables];
 
        
        if (model.Constraints.Count == 1)
        {
            var constraintCoefficients = model.Constraints[0].Coefficients;
            if (constraintCoefficients.Count != numVariables)
            {
                throw new ArgumentException("Constraints array length does not match the number of variables.");
            }
 
            for (int i = 0; i < numVariables; i++)
            {
                constraints[i] = constraintCoefficients[i].Value;
            }
        }
        else
        {
            throw new InvalidOperationException("The model should contain exactly one constraint.");
        }
 
        double[] solution = new double[numVariables];
        double maxObjectiveValue = double.NegativeInfinity;
 
        
        for (int i = 0; i < (1 << numVariables); i++)
        {
            double[] candidate = new double[numVariables];
            double constraintValue = 0;
            double objectiveValue = 0;
 
            for (int j = 0; j < numVariables; j++)
            {
                candidate[j] = (i & (1 << j)) != 0 ? 1 : 0;
 
                constraintValue += candidate[j] * constraints[j];
                objectiveValue += candidate[j] * coefficients[j];
            }
 
            
            if (constraintValue <= rhs && cuts.All(cut => CheckCut(cut, candidate)))
            {
                if (objectiveValue > maxObjectiveValue)
                {
                    maxObjectiveValue = objectiveValue;
                    Array.Copy(candidate, solution, numVariables);
                }
            }
        }
 
        return solution;
    }
 
    private bool CheckCut(double[] cut, double[] candidate)
    {
        double cutValue = 0;
        for (int i = 0; i < candidate.Length; i++)
        {
            cutValue += cut[i] * candidate[i];
        }
        return cutValue >= cut.Length - 1;
    }
 
    private double[] GenerateCut(double[] solution)
    {
        int numVariables = solution.Length;
        double[] cut = new double[numVariables];
        for (int i = 0; i < numVariables; i++)
        {
            cut[i] = solution[i] > 0.5 ? 1 : -1;
        }
        return cut;
    }
 
    private double CalculateObjectiveValue(double[] coefficients, double[] solution)
    {
        double objectiveValue = 0;
        for (int i = 0; i < solution.Length; i++)
        {
            objectiveValue += solution[i] * coefficients[i];
        }
        return objectiveValue;
    }
}
    }
}
