using System;
using System.Collections.Generic;
using DataModel;

namespace Behaviour.Console
{
    public class ConsoleDataBindings
    {
        private Dictionary<string, Func<string>> _dataBindings;

        public ConsoleDataBindings()
        {
            _dataBindings = new Dictionary<string, Func<string>>();
            RegisterDataBindings();
        }

        void RegisterDataBindings()
        {
            var gs = GameManager.Instance.GameState;
            
            _dataBindings.Add("navData", () => gs.PlayerData.SpaceShipData.GetNavigationDataString());
            _dataBindings.Add("engine", () => gs.PlayerData.SpaceShipData.Engine.ToConsoleString());
            _dataBindings.Add("heading", () => gs.PlayerData.SpaceShipData.Heading.ToString());
        }

        public string GetDatum(string datum)
        {
            if (!_dataBindings.ContainsKey(datum)) return null;

            return _dataBindings[datum].Invoke();
        }

        public bool DatumIsRegistered(string datum)
        {
            return _dataBindings.ContainsKey(datum);
        }
    }
}