using System;
using System.ComponentModel;
using System.IO;

namespace AdventOfCode
{
    public static class Inputs
    {
        public static string[] inputs;
        
        public static void Init() 
        {
            var days = Directory.GetFiles("Input");
            inputs = new string[days.Length];
            for (var i = 0; i < inputs.Length; i++) inputs[i] = ReadFile(days[i]); 
        }

        public static string ReadFile(string file)
        {
            using StreamReader f = new(file);
            return f.ReadToEnd();
        }
    }
}