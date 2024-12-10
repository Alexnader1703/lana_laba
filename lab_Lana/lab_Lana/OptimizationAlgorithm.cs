using System;
using System.Collections.Generic;

namespace lab_Lana
{
    /// <summary>
    /// Базовый абстрактный класс для всех алгоритмов
    /// </summary>
    public abstract class OptimizationAlgorithm
    {
        protected int[,] adjacencyMatrix;
        protected int verticesCount;

        public OptimizationAlgorithm(int[,] matrix)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1))
                throw new ArgumentException("Матрица должна быть квадратной");

            adjacencyMatrix = matrix;
            verticesCount = matrix.GetLength(0);
        }

      
        public abstract (List<int> path, int totalCost) Solve();

        protected abstract bool ValidateInput();
    }
}
