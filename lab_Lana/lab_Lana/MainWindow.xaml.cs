using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace lab_Lana
{
    public partial class MainWindow : Window
    {
        private int[,] adjacencyMatrix;
        private int verticesCount;
        private const int VertexRadius = 20;
        private const int CanvasMargin = 50;

        public MainWindow()
        {
            InitializeComponent();
        }

        
        private void OnCreateMatrixClick(object sender, RoutedEventArgs e)
        {
            MatrixPanel.Children.Clear();  

            if (!int.TryParse(VerticesCountTextBox.Text, out verticesCount) || verticesCount <= 0)
            {
                MessageBox.Show("Введите корректное количество вершин.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

          
            for (int i = 0; i < verticesCount; i++)
            {
                StackPanel rowPanel = new StackPanel { Orientation = Orientation.Horizontal };

                for (int j = 0; j < verticesCount; j++)
                {
                   
                    TextBox textBox = new TextBox { Width = 50, Margin = new Thickness(5) };
                    rowPanel.Children.Add(textBox);
                }

               
                MatrixPanel.Children.Add(rowPanel);
            }
        }

        private void OnSolveClick(object sender, RoutedEventArgs e)
        {
            GetMatrixFromDynamicInputs();

            OptimizationAlgorithm algorithm = CreateAlgorithm();
            if (algorithm == null) return;

            
            if (algorithm is ShortestPath shortestPathAlgorithm)
            {
             
                var (allPaths, distances) = shortestPathAlgorithm.SolveAllPaths();
                DisplayAllResults(allPaths, distances);
            }
            else
            {
              
                var (path, distance) = algorithm.Solve();
                DisplaySingleResult(path, distance);
            }
        }
        private void DisplaySingleResult(List<int> path, int distance)
        {
         
            GraphCanvas.Children.Clear();

        
            ResultTextBlock.Text = "Найденный путь:\n";

          
            ResultTextBlock.Text += $"Путь: {string.Join(" -> ", path)}\n";
            ResultTextBlock.Text += $"Общее расстояние: {distance}\n";

         
            DrawGraph(path);
        }

        private OptimizationAlgorithm CreateAlgorithm()
        {
            switch (AlgorithmComboBox.SelectedIndex)
            {
                case 0:
                    return new TravelingSalesmanProblem(adjacencyMatrix);
                case 1:
                    return new ShortestPath(adjacencyMatrix); 
                case 2:
                    return new MinimumSpanningTree(adjacencyMatrix);
                default:
                    MessageBox.Show("Выберите алгоритм решения", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return null;
            }
        }


        private void GetMatrixFromDynamicInputs()
        {
         
            if (verticesCount <= 0)
            {
                MessageBox.Show("Количество вершин не задано корректно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

    
            adjacencyMatrix = new int[verticesCount, verticesCount];

            int rowIndex = 0;
            foreach (StackPanel rowPanel in MatrixPanel.Children)
            {
                int colIndex = 0;
                foreach (TextBox textBox in rowPanel.Children)
                {
                 
                    adjacencyMatrix[rowIndex, colIndex] = int.TryParse(textBox.Text, out int value) ? value : 0;
                    colIndex++;
                }
                rowIndex++;
            }
        }

        private void DisplayAllResults(List<List<int>> allPaths, List<int> distances)
        {
            
            GraphCanvas.Children.Clear();

            ResultTextBlock.Text = "Найденные пути от вершины 0 до всех остальных вершин:\n";

            
            for (int i = 0; i < allPaths.Count; i++)
            {
                var path = allPaths[i];
                int distance = distances[i];
                ResultTextBlock.Text += $"Вершина {i}: путь {string.Join(" -> ", path)}, длина = {distance}\n";

                
                DrawGraph(path);
            }
        }


        private void DrawGraph(List<int> path)
        {
            var vertices = new Dictionary<int, Point>();
            double centerX = GraphCanvas.ActualWidth / 2;
            double centerY = GraphCanvas.ActualHeight / 2;
            double radius = Math.Min(centerX, centerY) - CanvasMargin;

           
            for (int i = 0; i < verticesCount; i++)
            {
                double angle = 2 * Math.PI * i / verticesCount;
                double x = centerX + radius * Math.Cos(angle);
                double y = centerY + radius * Math.Sin(angle);
                vertices[i] = new Point(x, y);

                DrawVertex(i, x, y);
            }

           
            for (int i = 0; i < path.Count - 1; i++)
            {
                DrawEdge(vertices[path[i]], vertices[path[i + 1]], adjacencyMatrix[path[i], path[i + 1]].ToString());
            }
        }

        private void DrawVertex(int index, double x, double y)
        {
            var vertex = new Ellipse
            {
                Width = VertexRadius * 2,
                Height = VertexRadius * 2,
                Fill = Brushes.LightBlue,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            var label = new TextBlock
            {
                Text = index.ToString(),
                TextAlignment = TextAlignment.Center
            };

            Canvas.SetLeft(vertex, x - VertexRadius);
            Canvas.SetTop(vertex, y - VertexRadius);
            Canvas.SetLeft(label, x - VertexRadius / 2);
            Canvas.SetTop(label, y - VertexRadius / 2);

            GraphCanvas.Children.Add(vertex);
            GraphCanvas.Children.Add(label);
        }

        private void DrawEdge(Point start, Point end, string weight)
        {
            var line = new Line
            {
                X1 = start.X,
                Y1 = start.Y,
                X2 = end.X,
                Y2 = end.Y,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            GraphCanvas.Children.Add(line);

            var weightLabel = new TextBlock
            {
                Text = weight,
                Foreground = Brushes.Black,
                Background = Brushes.White,
                Padding = new Thickness(2),
                Margin = new Thickness(0, 0, 0, 0)
            };

            var midPoint = new Point((start.X + end.X) / 2, (start.Y + end.Y) / 2);
            Canvas.SetLeft(weightLabel, midPoint.X);
            Canvas.SetTop(weightLabel, midPoint.Y);
            GraphCanvas.Children.Add(weightLabel);
        }
        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

           
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;


                
                LoadFile(filePath);
            }
        }

        private void LoadFile(string filePath)
        {
            try
            {
                
                var lines = System.IO.File.ReadAllLines(filePath);

                if (lines.Length == 0)
                {
                    MessageBox.Show("Файл пустой.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

               
                verticesCount = lines.Length;
                adjacencyMatrix = new int[verticesCount, verticesCount];

               
                for (int i = 0; i < verticesCount; i++)
                {
                    
                    var row = lines[i].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                   
                    if (row.Length != verticesCount)
                    {
                        MessageBox.Show("Некорректное количество столбцов в строке.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    for (int j = 0; j < verticesCount; j++)
                    {
                       
                        adjacencyMatrix[i, j] = int.TryParse(row[j], out int value) ? value : 0;
                    }
                }

               
                UpdateMatrixPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateMatrixPanel()
        {
         
            MatrixPanel.Children.Clear();

        
            for (int i = 0; i < verticesCount; i++)
            {
                StackPanel rowPanel = new StackPanel { Orientation = Orientation.Horizontal };

                for (int j = 0; j < verticesCount; j++)
                {
                    
                    TextBox textBox = new TextBox
                    {
                        Width = 50,
                        Margin = new Thickness(5),
                        Text = adjacencyMatrix[i, j].ToString()
                    };
                    rowPanel.Children.Add(textBox);
                }

                
                MatrixPanel.Children.Add(rowPanel);
            }
        }


    }
}
