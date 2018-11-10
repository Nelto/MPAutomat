using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MPautomat
{
    class Automat
    {
        private Dictionary<string, string[]> functions = new Dictionary<string, string[]>();
        private List<string> inputstring = new List<string>();
        private List<string> score = new List<string>();
        private int step = 0;
        private bool isRecognition = false;


        public Automat()
        {
            string rule;
 
            Console.WriteLine("Введите правила грамматики. Каждое правило - на отдельной строке. Левая часть не должна повторяться\n" +
                "Пример строки: S->aB\n" +
                "Для символа пустой строки (эпсилон) зарезервирован символ 'e'.\n" +
                "Для завершения ввода введите пустую строку.");
            for (; ; )
            {
                rule = Console.ReadLine();
                if (rule == string.Empty || rule == "") break;
                
            }
            Console.WriteLine("Введите начальный символ грамматики");
            score.Add(Console.ReadLine());
            Console.WriteLine("Введите строку для распознования");
            inputstring.Add(Console.ReadLine());
            recognitionString(score[0]);
            for(int i =0; i < score.Count; i++)
            {
                Console.WriteLine( $"{i}  {inputstring[i]}  {score[i]}");
            }
            Console.Read();
        }

        public void AddRule(string rule)
        {
            string[] splt;
            splt = rule.Replace(" ", "").Split(new char[] { '-', '>' }, System.StringSplitOptions.RemoveEmptyEntries);
            functions.Add(splt[0], splt[1].Split(new char[] { '|' }));
        }

        private void Shift()
        {
            score[step] = score[step].Replace("e", string.Empty);
                while (inputstring[step] != string.Empty && score[step]!=string.Empty && inputstring[step][0] == score[step][0])
                {
                    inputstring.Add(inputstring[step].Remove(0, 1));
                    score.Add(score[step].Remove(0, 1));
                    step++;
                }
            if (score[step] == string.Empty && inputstring[step] == string.Empty) isRecognition = true;
        }

        private void ReverseShift()
        {
            while (!inputstring[step].Equals(inputstring[step - 1]))
            {
                inputstring.RemoveAt(inputstring.Count-1);
                score.RemoveAt(score.Count-1);
                step--;
            }
            inputstring.RemoveAt(inputstring.Count - 1);
            score.RemoveAt(score.Count - 1);
            step--;
        }

        private bool TermCount()
        {
            int termCount = 0;
            for (int i =0; i < score[step].Length; i++)
            {
                if (score[step][i] < 'A' || score[step][i] > 'Z') termCount++;
            }
            return termCount > inputstring[step].Length;
        }

        private void recognitionString(string notTerm)
        {
            for (int i = 0; i < functions[notTerm].Length; i++)
            {
                if (isRecognition) break;
                if (Regex.IsMatch(score[step], "^[A-Z]") || functions[notTerm][i] == "e")
                {
                    score.Add((functions[notTerm][i]+score[step].Substring(1, score[step].Length-1)));
                    inputstring.Add(inputstring[step]);
                    step++;
                    Shift();
                    if (!isRecognition)
                    {
                        if (Regex.IsMatch(score[step], "^[A-Z]")&&!TermCount()) recognitionString(Convert.ToString(score[step][0]));
                        else ReverseShift();
                    }
                }
            }
            if (!isRecognition && notTerm!=score[0])
            {
                ReverseShift();
            }
        }

        public void RecognitionResult()
        {

        }
    }
}