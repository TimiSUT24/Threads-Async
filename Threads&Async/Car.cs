using System;
using System.Threading;

namespace Threads_Async
{
    public class Car
    {
        public string ?Name { get; set; }
        public double Distance { get; set; }
        public double Speed { get; set; }
        public bool HasHalfWay { get; set; } = false;

        public void Start(Func<bool> raceActive)
        {
            while (Distance < 5000 && raceActive())
            {
                double speedms = Speed / 3.6;
                Distance += speedms;

                if (Distance >= 2500 && !HasHalfWay)
                {
                    Console.WriteLine($"{Name} has reached halfway!");
                    HasHalfWay = true;
                }

                Thread.Sleep(1000);
            }        
        }

        public void NoGas()
        {
            Console.WriteLine($"{Name} has no gas!");
            double originalSpeed = Speed;
            Speed = 0;
            Thread.Sleep(15000);
            Speed = originalSpeed;
            Console.WriteLine($"{Name} has gas!");
        }

        public void ChangeTires()
        {
            Console.WriteLine($"{Name} is changing tires!");
            double originalSpeed = Speed;
            Speed = 0;
            Thread.Sleep(10000);
            Speed = originalSpeed;
            Console.WriteLine($"{Name} has new tires!");
        }

        public void DirtyWindow()
        {
            Console.WriteLine($"{Name} needs to clean windows!");
            double originalSpeed = Speed;
            Speed = 0;
            Thread.Sleep(5000);
            Speed = originalSpeed;
            Console.WriteLine($"{Name} windows are clean!");
        }

        public void EngineProblem()
        {
            Console.WriteLine($"{Name} has engine problem speed reduced!");
            Speed -= 20;
            if (Speed < 0)
            {
                Speed = 0;
            }             
            Console.WriteLine($"{Name} speed reduced to {Speed} km/h.");
        }
    }
}