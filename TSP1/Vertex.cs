using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace TSP1
{
    class Vertex 
    {
        public int Id;  // Номер вершини
        public int Level;   // Рівень, на якому знаходиться вершина. Починаючи з кореня дерева станів. Відлік від 0.
        public int LowerBound { get; private set; } // Нижня межа заданої вершини
        public SortedSet<Vertex> PossibleConnections { get; private set; }  // Можливі зв’язки з даної вершини відсортовані за зростанням відповідно до нижньої межі.
                                                                            // Перший елемент завжди буде найкращим поєднанням
        public Dictionary<int,Vertex> Parents { get; private set; } // Словник, що містить вершини, відвідані раніше, ніж ми дійшли до поточного.
        private int SetLowerBound(int[] lowerBoundTable, int cost)  // Обчисліть нижню межу для даної вершини
        {
            var diff = cost - lowerBoundTable[Level];   // Обчисліть різницю між вагою ребра та мінімумом із відповідного рядка в матриці витрат
            return diff;    // Повернення різниці
        }
        public void SetPossibleConnections(Data data, Dictionary<int,Vertex> visited)   // Розрахунок можливих зв’язків з цієї вершини
        {
            for (var i = 0; i < data.pointsCount; i++)
            {
                if (Id == i || visited.ContainsKey(i)) continue;    // Якщо id == i означає, що ми хотіли б перейти до вершини, в якій ми перебуваємо в даний момент, тому ми опускаємо
                                                                    // І якщо потенційна вершина вже є у раніше відвіданій, ми її опускаємо
                var lowerBound = LowerBound + SetLowerBound(data.LowerBoundTable, data.TspArray[Id][i]);    // Обчисліть нижню межу для наступної вершини
                                                                                                            // на основі обмежень у поточній вершині
                PossibleConnections.Add(new Vertex(i, Level + 1, lowerBound, this));  // Додавання нової вершини до SortedSet
                                                                                      // Повторюємо, поки не додамо всі можливі зв’язки
            }
        }
        public Vertex(int lowerBound)
        {
            Id = 0;
            Level = 0;
            LowerBound = lowerBound;
            PossibleConnections = new SortedSet<Vertex>(new VertexComparer());
            Parents=new Dictionary<int, Vertex>();
        }
        public Vertex(int id, int level, int lowerBound, Vertex vertex)
        {
            Id = id;
            Level = level;
            LowerBound = lowerBound;
            PossibleConnections = new SortedSet<Vertex>(new VertexComparer());
            Parents = new Dictionary<int, Vertex>();
            if (vertex.Parents != null)
            {
                foreach (var vertexParent in vertex.Parents)
                {
                    Parents.Add(vertexParent.Key,vertexParent.Value);
                }
            }  
            Parents.Add(vertex.Id,vertex);
        }
    }
}
