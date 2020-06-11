using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework4
{
    /// <summary>
    /// Дана коллекция List<T>. Требуется подсчитать, сколько раз каждый элемент встречается в данной коллекции:
    ///для целых чисел;
    ///* для обобщенной коллекции;
    ///** используя Linq.
    /// </summary>
    static class Task2
    {
        static List<int> collection = new List<int>();

        /// <summary>
        /// Демонстрация задания
        /// </summary>
        public static void Do()
        {
            // создадим запрос Linq на поиск номера 3
            var count3 = from n in collection
                         where n == 3
                         select n;
            collection.AddRange(new int[] { 3, 5, 6, 8, 3, 3, 5, 2, 7, 5, 2, 3 });
            Predicate<int> predicate5 = new Predicate<int>((i) => { return i == 5; }); // ищем цифру 5
            Console.WriteLine("Количество цифр 5 в коллекции: " + collection.FindAll(predicate5).Count);
            Console.WriteLine("Количество цифр 3 в коллекции: " + count3.Count<int>() + " (посчитано с помощью Linq запроса)");
            Console.ReadLine();
        }
    }
}
