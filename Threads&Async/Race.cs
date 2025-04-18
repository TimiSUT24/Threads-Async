using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Threads_Async
{
    public class Race
    {
       
        public async Task Racing()
        {
            
            Car car1 = new Car { Name = "Petter", Distance = 0, Speed = 120 };
            Car car2 = new Car { Name = "Tim", Distance = 0, Speed = 120 };

            List<Car> cars = new List<Car>
            {
               car1,
               car2
            };
            using CancellationTokenSource cts = new(); // CancellationToken gives a task a signal that it should stop when it is safe
            CancellationToken token = cts.Token;
            await Counter();
            Task t1 = car1.Start(token);
            Task t2 = car2.Start(token);
            
            Task events = RandomEvent(car1, token);
            Task events2 = RandomEvent(car2, token);
            
            Task Input = UserInput(cars, token);

            await Task.WhenAny(t1, t2); // ends when one task are completed
            cts.Cancel(); // Cancel the tasks after they are completed         
        }

        public async Task UserInput(List<Car> cars, CancellationToken token)
        {

            while (true)
            {
                var userInput = Console.ReadKey();
                
                if(userInput.Key == ConsoleKey.Enter)
                {
                   foreach(var car in cars)
                   {
                        var distance = Math.Round(car.Distance, 2);
                        Console.WriteLine($"\n{car.Name} speed is {car.Speed}km/h and have reached {distance} meters.\n");
                   }
                }
               
                await Task.Delay(1000, token);
            }
  
        }

        public async Task Counter()
        {
            int count = 0;
            Console.Write($"Race starts in"); // Race starts after counter method is finished 
            while (count < 3)
            {               
                count++;
                Console.Write($" \n{count}");
                await Task.Delay(1000);
               
            }          
            Console.Clear();
            Console.WriteLine("GOOOOOO!!!");
            Console.WriteLine("Race has started");
        }

        public async Task RandomEvent(Car car, CancellationToken token)
        {

            Random random = new Random();
            while (!token.IsCancellationRequested)
            {
                int delay = 10000;
                await Task.Delay(delay, token);
                

                int method = random.Next(1, 51);
              
                switch (method)
                {
                    case <= 1: await car.NoGas(); break; // await task so it doesnt randomize until it finishes
                    case <= 2: await car.ChangeTires(); break;
                    case <= 5: await car.DirtyWindow(); break;
                    case <= 10: await car.EngineProblem(); break;

                }
            }
        }

             
    }
}
