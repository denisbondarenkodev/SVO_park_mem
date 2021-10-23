using System;
using System.Collections.Generic;
using System.Linq;

namespace SVO.Console
{
    class Program
    {
        static string GetAirClass(int seats, IReadOnlyList<(int, int, string)> classes) {
            foreach (var (min, max, @class) in classes) {
                if (seats >= min && seats <= max)
                    return @class;
            }

            throw new InvalidOperationException("unknown class");
        }

        static void Main(string[] args)
        {
            // Определение размера фюзеляжа
            var inputAirClasses = new CsvReader("../data/Aircraft_Classes_Private.csv")
                .Select(x => (airClass: x["Aircraft_Class"], seats: int.Parse(x["Max_Seats"])))
                .OrderBy(x => x.seats)
                .ToArray();

            var airClasses = new (int, int, string)[inputAirClasses.Length];
            for (int i = 0; i < airClasses.Length; i++)
                airClasses[i] = (
                    i == 0 ? 0 : inputAirClasses[i - 1].seats, 
                    inputAirClasses[i].seats, 
                    inputAirClasses[i].airClass);

            var inputTimetables = new CsvReader("../data/Timetable_private.csv")
                .ToArray();

            for (int i = 0; i < inputTimetables.Length; i++)
            {
                var seats = int.Parse(inputTimetables[i]["flight_PAX"]);
                inputTimetables[i]["Aircraft_Stand"] = GetAirClass(seats, airClasses);
            }

            foreach (var air in inputTimetables) {
                System.Console.WriteLine($"{air[""]} {air["flight_AC_PAX_capacity_total"]} = {air["Aircraft_Stand"]}");
            }
        }
    }
}
