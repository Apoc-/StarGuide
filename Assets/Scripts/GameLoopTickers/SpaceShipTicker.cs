using System;
using Behaviour;
using DataModel;
using UnityEngine;
using Util;

namespace SpaceShip
{
    public class SpaceShipTicker : ITickable
    {
        private SpaceShipData _shipData => GameManager.Instance.GameState.PlayerData.SpaceShipData;

        public void Tick()
        {
            HandleEngineTick();

            /*var pos = _shipData.Position;
            Debug.Log($"Pos: ({pos.x:F3},{pos.y:F3},{pos.z:F3})");
            Debug.Log("GridPos: " + _shipData.GridPosition);
            Debug.Log("Hdng: " + _shipData.Heading);
            Debug.Log("Engine: " + _shipData.Engine.ToConsoleString());*/
        }

        private void HandleEngineTick()
        {
            var engine = _shipData.Engine;

            if (!engine.IsRunning()) return;

            if (engine.CurrentSpeed < engine.MaxSpeed)
            {
                UpdateSpeed(engine);
            }

            UpdatePosition();

            HandleArrival();
        }

        private void HandleArrival()
        {
            if (IsAtHeading())
            {
                _shipData.Position = _shipData.Heading;

                StopEngine();
            }
        }

        private bool IsAtHeading()
        {
            return Vector3.Distance(_shipData.Position, _shipData.Heading) < 0.001f;
        }

        private void StopEngine()
        {
            _shipData.Engine.Stop();
        }

        private void UpdatePosition()
        {
            var dt = Time.fixedDeltaTime / 60f;

            var s0 = _shipData.Position;
            var ds = _shipData.Engine.CurrentSpeed * dt;
            var dsVec = ds * (_shipData.Heading - _shipData.Position).normalized;

            _shipData.Position = s0 + dsVec;

            _shipData.GridPosition = new Vector3Int(
                Mathf.RoundToInt(_shipData.Position.x),
                Mathf.RoundToInt(_shipData.Position.y),
                Mathf.RoundToInt(_shipData.Position.z));
        }

        private void UpdateSpeed(ShipEngine engine)
        {
            var dt = Time.fixedDeltaTime;
            var dv = dt * engine.Acceleration;
            var dvAuH = UnitHelper.KmsToAuh(dv);

            engine.CurrentSpeed += dvAuH;
            if (engine.CurrentSpeed > engine.MaxSpeed) engine.CurrentSpeed = engine.MaxSpeed;
        }
    }
}