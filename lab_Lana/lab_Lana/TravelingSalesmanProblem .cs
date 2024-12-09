using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_Lana
{
    /// <summary>
    /// Класс для решения задачи коммивояжера (метод ближайшего соседа)
    /// </summary>
    public class TravelingSalesmanProblem : OptimizationAlgorithm
    {
        public TravelingSalesmanProblem(int[,] matrix) : base(matrix) { }

        public override (List<int> path, int distance) Solve()
        {
            if (!ValidateInput())
                throw new InvalidOperationException("Некорректные входные данные");

            var visited = new bool[verticesCount];
            var path = new List<int> { 0 }; 
            visited[0] = true;
            int currentVertex = 0;
            int totalDistance = 0;

            while (path.Count < verticesCount)
            {
                int nextVertex = FindNearestNeighbor(currentVertex, visited);
                path.Add(nextVertex);
                totalDistance += adjacencyMatrix[currentVertex, nextVertex];
                visited[nextVertex] = true;
                currentVertex = nextVertex;
            }

       
            totalDistance += adjacencyMatrix[currentVertex, 0];
            path.Add(0);

            return (path, totalDistance);
        }

        private int FindNearestNeighbor(int current, bool[] visited)
        {
            int nearest = -1;
            int minDistance = int.MaxValue;

            for (int i = 0; i < verticesCount; i++)
            {
                if (!visited[i] && adjacencyMatrix[current, i] < minDistance && adjacencyMatrix[current, i] != 0)
                {
                    minDistance = adjacencyMatrix[current, i];
                    nearest = i;
                }
            }

            return nearest;
        }

        protected override bool ValidateInput()
        {
            // Проверка на связность графа и неотрицательность весов
            for (int i = 0; i < verticesCount; i++)
            {
                for (int j = 0; j < verticesCount; j++)
                {
                    if (i != j && adjacencyMatrix[i, j] <= 0)
                        return false;
                }
            }
            return true;
        }
    }

}
