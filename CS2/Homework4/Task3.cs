using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework4
{
    /// <summary>
    /// Дан фрагмент программы. Надо:
    /// а. Свернуть обращение к OrderBy с использованием лямбда-выражения =>.
    /// b. * Развернуть обращение к OrderBy с использованием делегата .
    /// </summary>
    static class Task3
    {
        // фрагмент программы
        public static void Do()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>()
            {
                {"four", 4 },
                {"two", 2 },
                {"one", 1 },
                {"three", 3 }
            };

            // фрагмент программы
            var d = dict.OrderBy(delegate (KeyValuePair<string, int> pair) { return pair.Value; } );

            // вариант a. Свернуть обращение к OrderBy с использованием лямбда-выражения =>.
            var d1 = dict.OrderBy((pair) => pair.Value);

            // вариант b. * Развернуть обращение к OrderBy с использованием делегата .
            MyDelegate del = new MyDelegate(func);
            var d2 = dict.OrderBy(del.Invoke);


            foreach (var pair in d)
            {
                Console.WriteLine("{0} - {1}", pair.Key, pair.Value);
            }

            foreach (var pair in d1)
            {
                Console.WriteLine("{0} - {1}", pair.Key, pair.Value);
            }

            foreach (var pair in d2)
            {
                Console.WriteLine("{0} - {1}", pair.Key, pair.Value);
            }

            Console.ReadLine();
        }

        private delegate int MyDelegate(KeyValuePair<string, int> pair);

        private static int func(KeyValuePair<string, int> pair)
        {
            return pair.Value;
        }
    }
}
