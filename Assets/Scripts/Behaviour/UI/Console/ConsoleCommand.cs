using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Behaviour.Console
{
    public class ConsoleCommand
    {
        public string CommandName;
        public int MinParameterCount;
        public Func<List<string>, ConsoleBehaviour, string> CommandFunction;

        public string Execute(string[] parameters, ConsoleBehaviour console)
        {
            var args = parameters.ToList();
            args.RemoveRange(0, 1);

            if (args.Count < MinParameterCount) return FailNotEnoughArguments(args.Count);
            
            return CommandFunction.Invoke(args, console);
        }

        private string FailNotEnoughArguments(int args)
        {
            return "ERROR: " + CommandName + " needs " + MinParameterCount + " args, given: " + args;
        }
    }
}