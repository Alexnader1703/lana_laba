using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_Lana
{
    /// <summary>
    /// Класс для поиска минимального остовного дерева (алгоритм Прима)
    /// </summary>
    public class MinimumSpanningTree : OptimizationAlgorithm
    {
        public MinimumSpanningTree(int[,] matrix) : base(matrix) { }

        public override (List<int> path, int distance) Solve()
        {
            if (!ValidateInput())
                throw new InvalidOperationException("Некорректные входные данные");

            var parent = new int[verticesCount];
            var key = new int[verticesCount];
            var mstSet = new bool[verticesCount];
            var resultPath = new List<int>();
            int totalWeight = 0;

            for (int i = 0; i < verticesCount; i++)
            {
                key[i] = int.MaxValue;
                mstSet[i] = false;
            }

            key[0] = 0;
            parent[0] = -1;

            for (int count = 0; count < verticesCount - 1; count++)
            {
                int u = MinKey(key, mstSet);
                mstSet[u] = true;
                resultPath.Add(u);

                for (int v = 0; v < verticesCount; v++)
                {
                    if (adjacencyMatrix[u, v] != 0 && !mstSet[v] && adjacencyMatrix[u, v] < key[v])
                    {
                        parent[v] = u;
                        key[v] = adjacencyMatrix[u, v];
                    }
                }
            }

           
            for (int i = 1; i < verticesCount; i++)
            {
                totalWeight += adjacencyMatrix[i, parent[i]];
            }

            return (resultPath, totalWeight);
        }

        private int MinKey(int[] key, bool[] mstSet)
        {
            int min = int.MaxValue;
            int minIndex = -1;

            for (int v = 0; v < verticesCount; v++)
            {
                if (!mstSet[v] && key[v] < min)
                {
                    min = key[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }

        protected override bool ValidateInput()
        {
           
            for (int i = 0; i < verticesCount; i++)
            {
                for (int j = 0; j < verticesCount; j++)
                {
                    if (adjacencyMatrix[i, j] < 0 || adjacencyMatrix[i, j] != adjacencyMatrix[j, i])
                        return false;
                }
            }
            return true;
        }
    }
}
