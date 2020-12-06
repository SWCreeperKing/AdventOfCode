using System.IO;

namespace AdventOfCode.Better_Run
{
    public static class Inputs
    {
        public static string[] inputs;
        
        public static void Init() 
        {
            var days = Directory.GetFiles("Input");
            inputs = new string[days.Length];
            for (var i = 0; i < inputs.Length; i++) inputs[i] = ReadFile(days[i]).Remove("\r");
        }

        public static string ReadFile(string file)
        {
            using StreamReader f = new(file);
            return f.ReadToEnd();
        }
    }
}