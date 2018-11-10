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
        private int step;
        private bool isRecognition;


        public Automat(string start)
        {
            score.Add(start);
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
            while (inputstring[step] != string.Empty && score[step] != string.Empty && inputstring[step][0] == score[step][0])
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
                inputstring.RemoveAt(inputstring.Count - 1);
                score.RemoveAt(score.Count - 1);
                step--;
            }
            inputstring.RemoveAt(inputstring.Count - 1);
            score.RemoveAt(score.Count - 1);
            step--;
        }

        private bool TermCount()
        {
            int termCount = 0;
            for (int i = 0; i < score[step].Length; i++)
            {
                if (score[step][i] < 'A' || score[step][i] > 'Z') termCount++;
            }
            return termCount > inputstring[step].Length;
        }

        private void Recognition(string notTerm)
        {
            for (int i = 0; i < functions[notTerm].Length; i++)
            {
                if (isRecognition) break;
                if (Regex.IsMatch(score[step], "^[A-Z]") || functions[notTerm][i] == "e")
                {
                    score.Add((functions[notTerm][i] + score[step].Substring(1, score[step].Length - 1)));
                    inputstring.Add(inputstring[step]);
                    step++;
                    Shift();
                    if (!isRecognition)
                    {
                        if (Regex.IsMatch(score[step], "^[A-Z]") && !TermCount()) Recognition(Convert.ToString(score[step][0]));
                        else ReverseShift();
                    }
                }
            }
            if (!isRecognition && notTerm != score[0])
            {
                ReverseShift();
            }
        }

        private void Print()
        {
            Console.WriteLine(" {0} | {1} | {2}", "Шаг", "Входная строка", "Содержимое магазина");
            for (int i = 0; i < step + 1; i++)
            {
                Console.WriteLine("{0,4} |{1,15} | {2,19}", i, inputstring[i], score[i]);
            }
        }

        public void RecognitionString(string inputstr)
        {
            isRecognition = false;
            step = 0;
            inputstring.Add(inputstr);
            Recognition(score[0]);
            if (isRecognition)
            {
                Console.WriteLine("\nСтрока успешно распознана:\n");
                Print();
            }
            else Console.WriteLine("\nНевозможно распознать строку с помощью данной граматики\n");
            inputstring.Clear();
            score.RemoveRange(1, score.Count-1);
        }
    }
}