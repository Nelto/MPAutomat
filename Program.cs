using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPautomat
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string rule;
                string recogstr;
                Console.WriteLine("Введите начальный символ грамматики");
                Automat automat = new Automat(Console.ReadLine());
                Console.WriteLine("Введите правила грамматики. Каждое правило - на отдельной строке. Левая часть не должна повторяться\n" +
                    "Пример строки: S->aB\n" +
                    "Для символа пустой строки (эпсилон) зарезервирован символ 'e'.\n" +
                    "Для завершения ввода введите пустую строку.");
                for (; ; )
                {
                    rule = Console.ReadLine();
                    if (rule == string.Empty) break;
                    automat.AddRule(rule);
                }
                for (; ; )
                {
                    Console.WriteLine("Введите строку для распознования (оставьте строку пустой для выхода из программы)");
                    recogstr = Console.ReadLine();
                    if (recogstr == string.Empty) break;
                    automat.RecognitionString(recogstr);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.Read();
            }
        }
    }
}
