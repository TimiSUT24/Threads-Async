using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Threads_Async
{
    public class Car
    {
        public string ?Name { get; set; }
        public double Distance { get; set; }
        public double Speed { get; set; } 
        public bool HasHalfWay { get; set; } = false;     
        public bool Starts { get; set; } = false;

        public async Task Start(CancellationToken token) 
        {          
            while (Distance <= 5000)
            {
                double speedms = Speed / 3.6;
                Distance += speedms;                            

                if (Distance > 2500 && !HasHalfWay)
                {
                    Console.WriteLine($"{Name} Has reached half way.\n");
                    HasHalfWay = true;
                }

                await Task.Delay(1000,token);               

            }
            Console.WriteLine($"{Name} Has finished the race");
        }   
            
        public async Task NoGas()
        {
            Console.WriteLine($"\n{Name} No gas!!\n");    //speed goes to 0 and after 15 sec comes up again 
            Speed = 0;

            await Task.Run(async () => 
                  {
                    await Task.Delay(15000); 
                    Speed = 120;
                    Console.WriteLine($"\n{Name}'s Has gas\n");
                  });
        }

        public async Task ChangeTires()
        {
            Console.WriteLine($"\n{Name} Tires is broken\n");
            Speed = 0;
            await Task.Run(async () =>
                  {
                    await Task.Delay(10000);
                    Speed = 120;
                    Console.WriteLine($"\n{Name}'s Has changed tires\n");
                  });

        }

        public async Task DirtyWindow()
        {
            Console.WriteLine($"\n{Name} Need to wash windows\n");
            Speed = 0;
           await Task.Run(async () =>
                 {
                    await Task.Delay(5000);
                    Speed = 120;
                    Console.WriteLine($"\n{Name}'s windows are clean\n");
                 });

        }

        public async Task EngineProblem()
        {
            Console.WriteLine($"\n{Name} Engine Problem\n");
            Speed -= 20;
            if(Speed < 0)
            {
                Speed = 0;
            }
            Console.WriteLine($"\n{Name} {Speed} km/h\n");
            await Task.Delay(100);
        }
    }
}
