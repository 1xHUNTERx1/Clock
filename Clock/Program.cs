using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EventsDemo
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello World!");

            var clock_1 = new Clock("Seico", 1952);
            var clock_2 = new Clock("Rolex", 1968);
            var clock_3 = new Clock("Casio", 1977);

            clock_1.AlarmTriggeredEvent += Alarm_1;
            clock_1.AlarmTriggeredEvent += Alarm_2;
            clock_1.AlarmTriggeredEvent += Alarm_3;
            clock_1.AlarmTriggeredEvent += Alarm_3;
            clock_1.AlarmTriggeredEvent -= Alarm_3;


            clock_1.RegisterAlarm(9, 30, 15);
            clock_1.RegisterAlarm(11, 10, 20);
            while (true)
            {
                Thread.Sleep(1);
                Console.Clear();
                clock_1.IncreaseSeconds();
                Console.WriteLine(clock_1);
            }
        }

        private static string[] japaneseClocks = { "seico", "casio" };

        private static void Alarm_1(object clock, EventArgs evs)
        {
            var currentClock = clock as Clock;

            bool isJapanese = japaneseClocks.Contains(currentClock.Brand.ToLower());

            if (isJapanese)
            {
                Console.WriteLine($"Japan Alarm of {currentClock.Brand}. Since {currentClock.YearOfMake}.");
            }
            else
            {
                Console.WriteLine($"Alarm of {currentClock.Brand}. Since {currentClock.YearOfMake}.");
            }
        }

        private static void Alarm_2(object clock, EventArgs evs)
        {
            Console.WriteLine("Alarm II - was triggered");
        }

        private static void Alarm_3(object clock, EventArgs evs)
        {
            Console.WriteLine($"Alarm III - was triggered SN: {((MyEventArguments)evs).SerialNumber}");
        }
    }
    public class Clock
    {
        private List<int> alarmTimes;
        public event EventHandler AlarmTriggeredEvent;
        private int seconds = 34000;
        public Clock(string brand, int yearOfMake)
        {
            Brand = brand;
            YearOfMake = yearOfMake;
            alarmTimes = new List<int>();
        }

        public string Brand { get; }
        public int YearOfMake { get; }
        public int Seconds => seconds % 60; //0-59 
        public int Minutes => (seconds / 60) % 60;//0-59 
        public int Hours => (seconds / 3600);//0-23
        public void IncreaseSeconds(int scnds = 1)
        {
            seconds += scnds;

            foreach (int alarmTime in alarmTimes)
            {
                if (seconds == alarmTime)
                {
                    var arguments = new MyEventArguments();
                    arguments.SerialNumber = "SHU-23-456-21";
                    AlarmTriggeredEvent?.Invoke(this, arguments);
                }
            }

            if (seconds > (60 * 60 * 24))
            {
                seconds -= 60 * 60 * 24;
            }
        }
        public void RegisterAlarm(int hours, int minutes, int seconds)
        {
            alarmTimes.Add(seconds + minutes * 60 + hours * 60 * 60);
        }
        public override string ToString() =>
                                $"[{Hours:D2} : {Minutes:D2} : {Seconds:D2}]";
    }
    public class MyEventArguments : EventArgs
    {
        public string SerialNumber { get; set; }
    }
