using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Chapbook;

public class Poem
{
    private List<int> _timings;
    private List<string> _words;
    private string _originalPoem;
    public string Title;

    public Poem(string title, string originalPoem)
    {
        _originalPoem = originalPoem;
        Title = title;
    }

    public Poem(string title, string originalPoem, List<int> timings, List<string> words)
    {
        _originalPoem = originalPoem;
        _timings = timings;
        _words = words;
        Title = title;
    }

    public void SetPoemWords()
    {
        _words = new List<string>();
        bool end = false;
        string tempPoem = _originalPoem;
        int highlighted =  tempPoem.Substring(0, tempPoem.IndexOf(' ')).Length;;
        while (!end)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            highlighted = Math.Clamp(highlighted, 0, tempPoem.Length);
            Console.Write(tempPoem.Substring(0,highlighted));
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(tempPoem.Substring(highlighted));
            var input = Console.ReadKey().Key;
            if (input == ConsoleKey.RightArrow)
            {
                highlighted++;
            }
            else if (input == ConsoleKey.LeftArrow)
            {
                highlighted--;
            }
            else if (input == ConsoleKey.Spacebar || input == ConsoleKey.Enter)
            {
                _words.Add(tempPoem.Substring(0,highlighted).Trim());
                tempPoem = tempPoem.Substring(highlighted);
                if (tempPoem.Length <= 0)
                {
                    end = true;
                }
                else if(tempPoem[0] == ' ')
                {
                    tempPoem = tempPoem.Substring(1);
                }

                try
                {
                    highlighted = tempPoem.Substring(0, tempPoem.IndexOf(' ')).Length;
                }
                catch
                {
                    highlighted = tempPoem.Length;
                }
                
            }
            
        }
        
    }

    public void ResetPoemWordsTimings()
    {
        bool end = false;
        int wordToEdit = 0;
        while (!end)
        {
            Console.Clear();
            for (int i = 0; i < _words.Count; i++)
            {
                if (wordToEdit == i)
                    Console.BackgroundColor = ConsoleColor.Blue;

                Console.Write("|" + _words[i] + "|");
                
                if (wordToEdit == i)
                    Console.BackgroundColor = ConsoleColor.Black;
                
            }
            Console.WriteLine();
            Console.WriteLine();
            for (int i = 0; i < _words.Count; i++)
            {
                if (wordToEdit == i)
                    Console.BackgroundColor = ConsoleColor.Blue;

                Console.Write("|" + _timings[i] + "|");
              
                if (wordToEdit == i)
                    Console.BackgroundColor = ConsoleColor.Black;
            }
            
            var input = Console.ReadKey().Key;
        
            if (input == ConsoleKey.LeftArrow)
            {
                wordToEdit--;
                wordToEdit = Math.Clamp(wordToEdit, 0, _words.Count);
            }
            else if (input == ConsoleKey.RightArrow)
            {
                wordToEdit++;
                wordToEdit = Math.Clamp(wordToEdit, 0, _words.Count);
            }
            else if (input is ConsoleKey.Spacebar or ConsoleKey.Enter)
            {
                Console.Clear();
                Console.WriteLine(_words[wordToEdit]);
                Console.WriteLine(_timings[wordToEdit]);
                Console.WriteLine("1) Add word(s) after\n2) Add word(s) before\n3) Edit Word(s)\n4) Edit Timing\n5) Preview\n6) End");
                input = Console.ReadKey().Key;
                if (input == ConsoleKey.D1)
                {
                    Console.WriteLine("Words:");
                    string words = Console.ReadLine() ?? "";
                    _words.Insert(wordToEdit + 1, words);
                    int timing;
                    Console.WriteLine("Enter timing: (integer in milliseconds)");
                    while (!int.TryParse(Console.ReadLine(), out timing))
                    {
                        Console.WriteLine("Must be an integer in milliseconds.");
                    }
                    _timings.Insert(wordToEdit + 1, timing);
                }
                else if (input == ConsoleKey.D2)
                {
                    Console.WriteLine("Words:");
                    string words = Console.ReadLine() ?? "";
                    _words.Insert(wordToEdit + 1, words);
                    int timing;
                    Console.WriteLine("Enter timing: (integer in milliseconds)");
                    while (!int.TryParse(Console.ReadLine(), out timing))
                    {
                        Console.WriteLine("Must be an integer in milliseconds.");
                    }
                    _timings.Insert(wordToEdit, timing);
                }
                else if (input == ConsoleKey.D3)
                {
                    Console.WriteLine("Words:");
                    string words = Console.ReadLine() ?? "";
                    _words[wordToEdit] =  words;
                }
                else if (input == ConsoleKey.D4)
                {
                    int timing;
                    Console.WriteLine("Enter timing: (integer in milliseconds)");
                    while (!int.TryParse(Console.ReadLine(), out timing))
                    {
                        Console.WriteLine("Must be an integer in milliseconds.");
                    }
                    _timings[wordToEdit] = timing;
                }
                else if(input == ConsoleKey.D5)
                {
                    for (int i = wordToEdit > 2 ? wordToEdit - 2 : 0; i < (wordToEdit + 3 < _words.Count ? wordToEdit + 3 : _words.Count - 1); i++)
                    {
                        Console.Clear();
                        for (int j = 0; j < Console.WindowHeight / 2; j++)
                        {
                            Console.WriteLine();
                        }
            
                        Console.WriteLine((new string (' ', Console.WindowWidth/2 - (_words[i].Length/2))) + _words[i]);
                        Thread.Sleep(_timings[i]);
                    }
                    
                }
                else if (input == ConsoleKey.D6)
                {
                    end = true;
                }
            }
        }

        


    }

    public void Save()
    {
        string timings = "{";
        string words = "{\"";

        for (int i = 0; i < _words.Count; i++)
        {
            timings += _timings[i].ToString();
            words += _words[i].Replace("\"", "\\\"") + "\"";
            if (i + 1 < _words.Count)
            {
                timings += ", ";
                words += ", \"";
            }

        }

        timings += "}";
        words += "}";

        Console.WriteLine("Poem " + Title.Replace(" ", "_") + " = new Poem(\"" + Title + "\", \"" +
                          _originalPoem.Replace("\"", 
    "\\\"") + "\", new List<int>() " + timings +  ", new List<string>() " + words +");");
        Console.WriteLine("Enter to continue...");
        Console.ReadLine();
    }

    public void SetPoemTiming()
    {
        _timings = new List<int>();
        Stopwatch stopwatch = new Stopwatch();
        Console.Clear();
        Console.WriteLine("3...");
        Thread.Sleep(TimeSpan.FromSeconds(1));
        Console.WriteLine("2...");
        Thread.Sleep(TimeSpan.FromSeconds(1));
        Console.WriteLine("1...");
        Thread.Sleep(TimeSpan.FromSeconds(1));
        for(int i = 0; i < _words.Count; i++)
        {
            Console.Clear();
            Console.WriteLine("Next words" + new string (' ', 40 - "Next words".Length) + "Real Display");
            Console.WriteLine("-------------------------------------------------------");
            if(_words[i].Length < 20)
                Console.WriteLine(_words[i] + (new string (' ', 40 - _words[i].Length)) + _words[i]);
            else
                Console.WriteLine(_words[i] + (new string (' ', 5)) + _words[i]);
            if(i+1 < _words.Count)
                Console.WriteLine(_words[i+1]);
            if(i+2 < _words.Count)
                Console.WriteLine(_words[i+2]);
            stopwatch.Start();
            Console.ReadLine();
            _timings.Add((int)stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
        }
    }

    public void Perform()
    {
        for(int i = 0; i < _words.Count; i++)
        {
            Console.Clear();
            for (int j = 0; j < Console.WindowHeight / 2; j++)
            {
                Console.WriteLine();
            }
            
            Console.WriteLine((new string (' ', Console.WindowWidth/2 - (_words[i].Length/2))) + _words[i]);
            Thread.Sleep(_timings[i]);
            
        }

        Console.WriteLine();
        Console.WriteLine("Enter to continue");
        Console.ReadLine();
    }


}