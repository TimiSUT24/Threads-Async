using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Threads_Async
{
    public class Race
    {
        public List<Car> cars = new List<Car>();
        public bool raceActive = true;
        private readonly object raceLock = new object();
        public void Racing()
        {
            
            Car car1 = new Car { Name = "Petter", Distance = 0, Speed = 120 };
            Car car2 = new Car { Name = "Tim", Distance = 0, Speed = 120 };

            cars.Add(car1);
            cars.Add(car2);

            using CancellationTokenSource cts = new(); // CancellationToken gives a task a signal that it should stop when it is safe
            CancellationToken token = cts.Token;

            Counter();

            List<Thread> threads = new List<Thread>();
            foreach (var car in cars)
            {
                Thread t1 = new Thread(() => car.Start(() => raceActive)); // each car get there own thread 
                t1.Start();
                t1.Name = car.Name;
                threads.Add(t1);                                                                     
                Thread randomEvent = new Thread(() => RandomEvent(car)); // and there own random event thread
                randomEvent.Start();
                randomEvent.IsBackground = true;
                threads.Add(randomEvent);
            }
    
            Task.Run(() => UserInput(token));
      
            while (raceActive)  
            {
                foreach(var car in cars)
                {
                    if(car.Distance >= 5000) //raceActive will be false if hit 5000 meters
                    {
                        lock (raceLock)
                        {
                            raceActive = false;
                            cts.Cancel();  // Tasks are cancelled 
                            Console.WriteLine($"{car.Name} has finished the race!");
                        }                                          
                    }
                }

                Thread.Sleep(100);
            }

        }

        public async Task UserInput(CancellationToken token)
        {
            
            while (raceActive)
            { 
                var userInput = Console.ReadKey();
                    
                if (userInput.Key == ConsoleKey.Enter)
                {
                    foreach (var car in cars)
                    {
                       var distance = Math.Round(car.Distance, 2);
                       Console.WriteLine($"\n{car.Name} speed is {car.Speed}km/h and have reached {distance} meters.\n");
                    }
                      
                }             
                await Task.Delay(1000,token);              
            }
            
  
        }

        public void Counter()
        {
            int count = 0;
            Console.Write($"Race starts in"); // Race starts after counter method is finished 
            while (count < 3)
            {               
                count++;
                Console.Write($" \n{count}");
                Thread.Sleep(1000);
               
            }          
            Console.Clear();
            Console.WriteLine("GOOOOOO!!!");
            Console.WriteLine("Race has started");
        }

        public void RandomEvent(Car car)
        {
            Random random = new Random();
            while (true)
            {
                bool active;
                lock (raceLock) // threads exit on there own when false 
                {
                    active = raceActive;
                }

                if (!active)
                {
                    break;
                }
                    
                Thread.Sleep(10000);  //random event every 10 sec 
                int method = random.Next(1, 51);               
                
                switch (method)
                {
                    case <= 1: car.NoGas(); break; 
                    case <= 2: car.ChangeTires(); break;
                    case <= 5: car.DirtyWindow(); break;
                    case <= 25:car.EngineProblem(); break;

                }
                
            }
        }

             
    }
}
