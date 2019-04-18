using System.Collections;
using System.Collections.Generic;
using CreativeSpore.SuperTilemapEditor;
using SpaceShip;
using UnityEngine;

public class DeckBehaviour : MonoBehaviour
{
    public TilemapGroup TilemapGroup;

    private List<ITickable> _tickables = new List<ITickable>();
    public bool EnableDirt;

    // Start is called before the first frame update
    void Start()
    {
        if (EnableDirt)
            _tickables.Add(new SpaceShipEnvironmentTicker(TilemapGroup));
    }

    // Update is called once per frame
    void Update()
    {
        _tickables.ForEach(tickable => tickable.Tick());
    }
}