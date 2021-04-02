using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP1
{
    class Loader
    {
        public Data ReadFromFile(string fileName)
        {
            try
            {
                using (var sr = new StreamReader(fileName))
                {
                    var points = Convert.ToInt32(sr.ReadLine());
                    var array = new int[points][];
                    for (var i = 0; i < points; i++)
                    {
                        array[i] = new int[points];
                    }
                    for (var i = 0; i < points; i++)
                    {
                        var line = sr.ReadLine();
                        if (line == null) continue;
                        var numbers = line.Split(' ');
                        for (var j = 0; j < points; j++)
                            array[i][j] = Convert.ToInt32(numbers[j]);
                    }

                    ///////////////////////////////////////////////////////////////////////////////////////
                    // Відображення матриці витрат
                    for (var i = 0; i < points; i++)
                    {
                        Console.WriteLine($"");
                        for (var j = 0; j < points; j++)
                            Console.Write($"{array[i][j]} ");
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////
                    return new Data(points, array);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Data(0, null);
            }
        }
       
    }
}
