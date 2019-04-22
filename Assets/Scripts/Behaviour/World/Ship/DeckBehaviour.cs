using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Behaviour.World;
using CreativeSpore.SuperTilemapEditor;
using SpaceShip;
using UnityEngine;

public class DeckBehaviour : MonoBehaviour, IInitializable
{
    public string DeckName;
    public DeckCategory DeckCategory;
    public int DeckNumber;
    public TilemapGroup TilemapGroup;
    public bool EnableDirt;

    public ShipBehaviour Ship { get; private set; }

    private List<ITickable> _tickables = new List<ITickable>();
    public List<LiftBehaviour> Lifts { get; } = new List<LiftBehaviour>();

    // Update is called once per frame
    void Update()
    {
        _tickables.ForEach(tickable => tickable.Tick());
    }

    public void Initialize()
    {
        if (EnableDirt)
            _tickables.Add(new SpaceShipEnvironmentTicker(TilemapGroup));
        
        Ship = GetComponentInParent<ShipBehaviour>();
        
        var lifts = GetComponentsInChildren<LiftBehaviour>();
        
        foreach (var liftBehaviour in lifts)
        {
            liftBehaviour.Initialize();
            Lifts.Add(liftBehaviour);   
        }
    }
}