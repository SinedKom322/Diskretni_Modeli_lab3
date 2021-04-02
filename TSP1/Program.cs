using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP1
{
    class Program
    {
        static void Main(string[] args)
        {       
            var loader = new Loader();
          
                    Console.WriteLine($"Nazva failu :");
                    var name = Console.ReadLine();
                    var data = loader.ReadFromFile($"{name}.txt");
                    if (data.pointsCount == 0)
                    {
                        Console.ReadKey();
                        return;
                    }
                    var solver = new BBA(data);
                    var sw = new Stopwatch();
                    sw.Start();
                    solver.Solve();
                    sw.Stop();
                  
            
            Console.ReadKey();

        }
    }
}
