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
            
            _dataBindings.Add("position", () => gs.PlayerData.SpaceShip.Position.ToString());
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