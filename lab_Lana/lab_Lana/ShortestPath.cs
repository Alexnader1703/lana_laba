using System;
using System.Collections.Generic;

namespace lab_Lana
{
    /// <summary>
    /// Класс для поиска кратчайшего пути (алгоритм Дейкстры)
    /// </summary>
    public class ShortestPath : OptimizationAlgorithm
    {
        public ShortestPath(int[,] matrix) : base(matrix) { }

        /// <summary>
        /// Поиск кратчайшего пути от вершины 0 до всех остальных вершин
        /// </summary>
        public override (List<int> path, int totalCost) Solve()
        {
            var (paths, distances) = SolveAllPaths();
            var path = paths[verticesCount - 1];
            var distance = distances[verticesCount - 1];
            return (path, distance);
        }

        public (List<List<int>> paths, List<int> distances) SolveAllPaths()
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
                if (minVertex == -1) break;
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

            var allPaths = new List<List<int>>();
            for (int i = 0; i < verticesCount; i++)
            {
                var path = RestorePath(previous, i);
                allPaths.Add(path);
            }

            return (allPaths, new List<int>(distances));
        }

        /// <summary>
        /// Находит вершину с минимальным расстоянием, которая ещё не посещена
        /// </summary>
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

        /// <summary>
        /// Восстанавливает путь от начальной вершины до указанной вершины
        /// </summary>
        private List<int> RestorePath(int[] previous, int endVertex)
        {
            var path = new List<int>();
            for (int vertex = endVertex; vertex != -1; vertex = previous[vertex])
            {
                path.Insert(0, vertex); // добавляем в начало списка
            }
            return path;
        }

        /// <summary>
        /// Проверка корректности матрицы смежности (отсутствие отрицательных весов)
        /// </summary>
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
