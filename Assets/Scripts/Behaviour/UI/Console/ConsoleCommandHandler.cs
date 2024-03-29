using System;
using System.Collections.Generic;
using System.Linq;
using DataModel;
using TMPro;
using UnityEngine;

namespace Behaviour.Console
{
    public class ConsoleCommandHandler : MonoBehaviour
    {
        private Dictionary<string, ConsoleCommand> _commandData;
        private ConsoleDataBindings _dataBindings;

        private void Start()
        {
            _dataBindings = new ConsoleDataBindings();
            RegisterCommands();
        }

        private void RegisterCommands()
        {
            _commandData = new Dictionary<string, ConsoleCommand>();

            RegisterCommand(
                new ConsoleCommand
                {
                    CommandName = "echo",
                    MinParameterCount = 1,
                    CommandFunction = CmdEcho
                });

            RegisterCommand(
                new ConsoleCommand
                {
                    CommandName = "clear",
                    MinParameterCount = 0,
                    CommandFunction = CmdCls
                });

            RegisterCommand(
                new ConsoleCommand
                {
                    CommandName = "data",
                    MinParameterCount = 1,
                    CommandFunction = CmdDta
                });

            RegisterCommand(
                new ConsoleCommand
                {
                    CommandName = "scan",
                    MinParameterCount = 1,
                    CommandFunction = CmdScan
                });

            RegisterCommand(
                new ConsoleCommand
                {
                    CommandName = "plot",
                    MinParameterCount = 3,
                    CommandFunction = CmdPlot
                });

            RegisterCommand(
                new ConsoleCommand
                {
                    CommandName = "start",
                    MinParameterCount = 1,
                    CommandFunction = CmdStart
                });

            RegisterCommand(
                new ConsoleCommand
                {
                    CommandName = "stop",
                    MinParameterCount = 1,
                    CommandFunction = CmdStop
                });
        }

        private string CmdStart(List<string> arg1, ConsoleBehaviour arg2)
        {
            var startables = GameManager.Instance.Startables;
            var name = arg1[0];

            if (!startables.ContainsKey(name)) return $"ERROR: Did not find {name}.";
            
            var startable = startables[name];
            if (startable.IsRunning()) return $"WARN: {name} already running.";
            
            startable.Start();
            
            return $"Started {name}." ;
        }

        private string CmdStop(List<string> arg1, ConsoleBehaviour arg2)
        {
            var startables = GameManager.Instance.Startables;
            var name = arg1[0];

            if (!startables.ContainsKey(name)) return $"ERROR: Did not find {name}.";

            var startable = startables[name];

            if (!startable.IsRunning()) return $"WARN: {name} not running.";
            startables[name].Stop();

            return $"Stopped {name}." ;
        }


        private void RegisterCommand(ConsoleCommand cmd)
        {
            _commandData.Add(cmd.CommandName, cmd);
        }


        public string ExecuteCommand(string commandBuffer, ConsoleBehaviour console)
        {
            var parameters = commandBuffer.Split(' ');

            if (parameters[0].Equals(String.Empty)) return FailNoCommandFound();

            var cmd = parameters[0].ToLower();
            if (!_commandData.ContainsKey(cmd)) return NoSuchCommand(cmd);

            return _commandData[cmd].Execute(parameters, console);
        }

        private string NoSuchCommand(string com)
        {
            return "ERROR: Command " + com + " not found.";
        }

        private string FailNoCommandFound()
        {
            return "ERROR: No command found.";
        }

        private string DatumNotFound(string datum)
        {
            return "ERROR: Datum " + datum + " not found.";
        }

        private string CmdEcho(List<string> args, ConsoleBehaviour console)
        {
            return String.Join(" ", args);
        }

        private string CmdCls(List<string> args, ConsoleBehaviour console)
        {
            console.ClearScreen();
            return "0";
        }

        private string CmdDta(List<string> args, ConsoleBehaviour console)
        {
            var datumName = args[0];

            if (!_dataBindings.DatumIsRegistered(datumName)) return DatumNotFound(datumName);

            return _dataBindings.GetDatum(datumName);
        }

        private string CmdScan(List<string> arg1, ConsoleBehaviour arg2)
        {
            var target = arg1[0];

            return "WIP";
        }

        private string CmdPlot(List<string> arg1, ConsoleBehaviour arg2)
        {
            var x = int.Parse(arg1[0]);
            var y = int.Parse(arg1[1]);
            var z = int.Parse(arg1[2]);
            var target = new Vector3Int(x, y, z);

            GameManager.Instance.GameState.PlayerData.SpaceShipData.Heading = target;

            return "Plotted course to " + target;
        }
    }
}