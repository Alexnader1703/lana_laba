using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_Lana
{
    /// <summary>
    /// Класс для поиска кратчайшего пути (алгоритм Дейкстры)
    /// </summary>
    public class ShortestPath : OptimizationAlgorithm
    {
        public ShortestPath(int[,] matrix) : base(matrix) { }

        public override (List<int> path, int distance) Solve()
        {
            if (!ValidateInput())
                throw new InvalidOperationException("Некорректные входные данные");

            var distances = new int[verticesCount];
            var visited = new bool[verticesCount];
            var previous = new int[verticesCount];

            for (int i = 0; i < verticesCount; i++)
            {
                distances[i] = int.MaxValue;
                previous[i] = -1;
            }

            distances[0] = 0; 

            for (int i = 0; i < verticesCount - 1; i++)
            {
                int minVertex = FindMinDistance(distances, visited);
                visited[minVertex] = true;

                for (int j = 0; j < verticesCount; j++)
                {
                    if (!visited[j] &&
                        adjacencyMatrix[minVertex, j] != 0 &&
                        distances[minVertex] != int.MaxValue &&
                        distances[minVertex] + adjacencyMatrix[minVertex, j] < distances[j])
                    {
                        distances[j] = distances[minVertex] + adjacencyMatrix[minVertex, j];
                        previous[j] = minVertex;
                    }
                }
            }

         
            var path = RestorePath(previous, verticesCount - 1);
            return (path, distances[verticesCount - 1]);
        }

        private int FindMinDistance(int[] distances, bool[] visited)
        {
            int min = int.MaxValue;
            int minIndex = -1;

            for (int i = 0; i < verticesCount; i++)
            {
                if (!visited[i] && distances[i] <= min)
                {
                    min = distances[i];
                    minIndex = i;
                }
            }

            return minIndex;
        }

        private List<int> RestorePath(int[] previous, int endVertex)
        {
            var path = new List<int>();
            for (int vertex = endVertex; vertex != -1; vertex = previous[vertex])
            {
                path.Insert(0, vertex);
            }
            return path;
        }

        protected override bool ValidateInput()
        {
           
            for (int i = 0; i < verticesCount; i++)
            {
                for (int j = 0; j < verticesCount; j++)
                {
                    if (adjacencyMatrix[i, j] < 0)
                        return false;
                }
            }
            return true;
        }
    }
}
