using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PLINQ
{

    public class AveradgePerDepartment
    {
        public string GroupName { get; set; }
        public float Value { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var stReader = new StreamReader(@"company.csv");
            var re = new CsvHelper.CsvReader(stReader);
            var st = new Stopwatch();
            st.Start();

            var listWithResult = re.GetRecords<Person>()
                .AsParallel()
                .GroupBy(p => p.Department)
                .Select(p =>
                    new AveradgePerDepartment
                    {
                        GroupName = p.Key,
                        Value = p.Average(s => s.Salary)
                    })
                    .AsSequential()
                    .ToList();

            st.Stop();
            listWithResult.ForEach(l =>
            {
                Console.WriteLine($"{l.GroupName} - {l.Value}");
            });
            Console.WriteLine(st.Elapsed.TotalMilliseconds);
            Console.ReadKey();


            stReader = new StreamReader(@"company.csv");
            re = new CsvHelper.CsvReader(stReader);        
            var st2 = new Stopwatch();
            st2.Start();

            var listWithResult2 = re.GetRecords<Person>()
               .AsParallel()
               .Select(s => 
               new
               {
                   group = s.Age > 16 && s.Age < 30 ? "17-29" : s.Age >= 30 && s.Age < 46 ? "30-45" : s.Age > 45 ? "45+" : "to young",
                   person = s
               })
               .GroupBy(p => p.group)
               .Select(p =>
                   new AveradgePerDepartment
                   {
                       GroupName = p.Key,
                       Value = p.Max(s => s.person.Salary)
                   })
                   .AsSequential()
                   .ToList();

            st2.Stop();
            listWithResult2.ForEach(l =>
            {
                Console.WriteLine($"{l.GroupName} - {l.Value}");
            });
            Console.WriteLine(st2.Elapsed.TotalMilliseconds);
            Console.ReadKey();
        }
    }
}
