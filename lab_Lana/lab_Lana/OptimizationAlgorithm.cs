using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public abstract (List<int> path, int distance) Solve();
        protected abstract bool ValidateInput();
    }

}
