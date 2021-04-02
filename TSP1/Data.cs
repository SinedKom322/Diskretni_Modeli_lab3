using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP1
{
    class Data
    {
        public int pointsCount;  // Кількість міст
        public int[][] TspArray;    // Двовимірний масив, що містить матрицю витрат
        public int[] LowerBoundTable;   // Масив, що містить мінімуми з кожного рядка матриці витрат
        public int LowerBound;  // Початкова нижня межа для даного екземпляра
        public void SetLowerBoundTable()    // Метод, який обчислює мінімум кожного рядка матриці витрат
        {
            var points = pointsCount;
            LowerBoundTable = new int[points];
            for (int i = 0; i < points; i++)
            {
                var min = i==0 ? TspArray[i][1] : TspArray[i][0];   // Якщо i = 0, ми призначаємо min [i] [1], а не [i] [0], оскільки в [i] [0] є -1, що означає діагональ
                for (int j = 1; j < points; j++)
                {
                    var cost = TspArray[i][j];
                    if (cost!=-1 && cost < min) // Якщо вага не по діагоналі і менший за поточний хв, призначте хв
                        min = cost;
                }
                LowerBoundTable[i] = min;   // Введення мінімуму в таблицю
            }
            var sum = 0;
            for (var i = 0; i < points; i++)    //Підсумовуючи всі мінімуми для обчислення початкової нижньої межі
            {
                sum += LowerBoundTable[i];
            }
            LowerBound = sum;
        }
        public Data(int points, int [][]tspArray)
        {
            pointsCount = points;
            TspArray = tspArray;
        }
    }
}
