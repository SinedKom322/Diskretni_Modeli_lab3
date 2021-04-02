using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP1
{
    class BBA
    {
        private Data Data;  //Інформація про даний екземпляр: матриця витрат, кількість міст, початкове обмеження
        private int UpperBound; //Верхня межа
        public Dictionary<int, Vertex> Visited;
        public SortedSet<Vertex> NotVisited;    //Відсортовані за зростанням відповідно до нижньої межі, вершини, які ми опустили
        private Vertex BestRoad;    // Вершина, яка є кінцем найкращого рішення
        private Stopwatch Watch;    // Годинник, який підраховує час так, що коли минула хвилина, він перерве алгоритм і поверне найкращий шлях на сьогодні
        private int CalculateCost(Dictionary<int, Vertex> visited, Vertex vertex)   // Обчислимо шлях на основі відвіданих вершин
        {
            var cost = 0;
            var keys = visited.Keys.ToArray();  //Присвоєння ключів словника новій таблиці. Ключі в цьому словнику - це цифри вершин, відвіданих на даний момент
            for (var i = 0; i < visited.Keys.Count - 1; i++)    // Огляд усіх ключів зі словника, переданих у параметрі
                cost += Data.TspArray[keys[i]][keys[i + 1]];    // Додавання відповідної ваги краю до вартості
            cost += Data.TspArray[keys[keys.Length - 1]][vertex.Id];    // Додавання передостаннього краю
            cost += Data.TspArray[vertex.Id][0];    
            return cost;     
        }
        private void FindBetterSolution(SortedSet<Vertex> notVisited) // Функція здійснює пошук у всіх раніше пропущених перспективних вершинах для кращого рішення
        {
            var notVisit = new SortedSet<Vertex>(new VertexComparer()); // Новий SortedSet, щоб мати можливість додавати до нього перспективних нащадків 
                                                                        // який ми опустимо заради більш сприятливого зв'язку, ніж з ним.
                                                                        // Пізніше цей SortedSet буде передано як параметр цій функції в рекурсивному виклику
            foreach (var vertex in notVisited)  // Для будь-якої вершини, яка раніше була опущена
            {
                if (vertex.LowerBound > UpperBound) // Якщо нижня межа більше верхньої, немає сенсу шукати далі
                                                    // Оскільки ця колекція сортується за зростанням за нижчим обмеженням, кожна наступна вершина має все більше і більше обмежень
                                                    // що ще більше перевищить верхню межу
                    break;
                var level = vertex.Level;
                var nextVertex = vertex;
                while (level < Data.pointsCount - 1) // Спускаючись "вниз", щоб знайти рішення
                {
                    nextVertex.SetPossibleConnections(Data, nextVertex.Parents);    // Розрахунок можливих зв’язків для даної вершини
                    AddToNotVisited(nextVertex, notVisit);  // Додавання всіх нащадків, крім першого (що є найбільш вигідним і буде з ним підключено)
                                                            // до колекції, щоб мати можливість їх рекурсивно шукати
                    nextVertex = nextVertex.PossibleConnections.First();    // Призначте найкраще з’єднання як наступну вершину для перевірки
                    if (nextVertex.LowerBound > UpperBound) break;  // Якщо нижня межа наступної найбільш перспективної дитини перевищує верхню, припиніть пошук
                    level = nextVertex.Level;  
                }
                if (nextVertex.Parents.Count == Data.pointsCount - 1)    // Якщо алгоритм досяг листа, розрахуйте шлях на основі відвіданих вершин
                {
                    var up = CalculateCost(nextVertex.Parents, nextVertex); // Виклик функції, яка обчислює шлях
                    if (up < UpperBound)    // Якщо дорога краща за верхнє обмеження, призначте його як нове верхнє обмеження
                    {
                        UpperBound = up;
                        BestRoad = nextVertex;  // І додайте останню вершину як останню вершину найкращого шляху
                    }
                }
               
            }
            
            if (notVisit.Count != 0)    // Якщо є принаймні 1 пропущена вершина, викличте той самий метод із колекцією, що містить ці вершини
                FindBetterSolution(notVisit);
        }   
        private void AddToNotVisited(Vertex vertex, SortedSet<Vertex> notVisited)   // Додаючи пропущені вершини до колекції, вершина передається в параметрах
        {
            foreach (var ver in vertex.PossibleConnections.Skip(1)) // Будь-яке можливе з'єднання, крім першого, що є найкращим, додайте до колекції опущених вузлів
            {
                notVisited.Add(ver);
            }
        }
        private void ShowBestRoad() // Переглянути рішення
        {
            Console.WriteLine(" ");
            Console.Write($"\nWaga: {UpperBound}\n");
            Console.WriteLine(" ");
            Console.Write($"Shlyah: ");
            foreach (var parentsKey in BestRoad.Parents.Keys)
            {
                Console.Write($"{parentsKey} ");
            }
            Console.Write($"{BestRoad.Id}\n");

        }
        public void Solve() // Функція, яка знаходить перше рішення
        {
            Watch = new Stopwatch();
            Watch.Start();
            Data.SetLowerBoundTable();  // Розрахунок мінімумів для кожного рядка з матриці витрат
            var root = new Vertex(Data.LowerBound);     // Кореневе створення тут завжди буде першою вершиною
            Visited = new Dictionary<int, Vertex>();
            NotVisited = new SortedSet<Vertex>(new VertexComparer());   // Ініціалізація колекції, яка зберігає пропущені на даний момент вершини завдяки існуванню кращих
            Visited.Add(root.Id, root);
            root.SetPossibleConnections(Data, root.Parents);   // Розрахунок можливих зв’язків для кореня
            AddToNotVisited(root, NotVisited);  // Додайте всі з’єднання до колекції, виключаючи перше, яке є найкращим
            var nextVertex = root.PossibleConnections.First();  // Призначте найкращий зв’язок як наступну вершину для відвідування
            var level = nextVertex.Level;   // Спускаючись по дереву
            Visited.Add(nextVertex.Id,nextVertex);
            while (level<Data.pointsCount - 1) // Поки ми не спустимось вниз, продовжуйте повторювати
            {
                nextVertex.SetPossibleConnections(Data, nextVertex.Parents);
                AddToNotVisited(nextVertex, NotVisited);
                nextVertex = nextVertex.PossibleConnections.First();
                level = nextVertex.Level;
                Visited.Add(nextVertex.Id, nextVertex);
            }
            UpperBound = CalculateCost(nextVertex.Parents, nextVertex);     // Призначення як верхня межа першого розчину
            BestRoad = nextVertex;
            FindBetterSolution(NotVisited);   // Виклик функції, яка шукає кращого рішення
            ShowBestRoad(); // Покажіть найкраще рішення
            Watch.Stop();
        }
        public BBA(Data data)
        {
            Data = data;
        }
    }
}
    