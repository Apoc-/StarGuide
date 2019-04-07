using System.Collections;
using System.Collections.Generic;
using Behaviour;
using Behaviour.World;
using CreativeSpore.SuperTilemapEditor;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DoorBehaviour : MonoBehaviour
{
    public Vector2 DoorLocation;
    private Collider2D _doorTrigger;
    public STETilemap DoorTilemap;

    private bool _isOpen;

    private void Start()
    {
        _doorTrigger = GetComponent<Collider2D>();
    }

    void Update()
    {
        var playerCollider = GameManager.Instance.Player.GetComponent<Collider2D>();
        
        if(_doorTrigger.IsTouching(playerCollider))
        {
            _isOpen = true;
        }
        else
        {
            _isOpen = false;
        }

        SetDoorState(_isOpen);
    }

    void SetDoorState(bool state)
    {
        
        var bot = DoorLocation;
        var top = new Vector2(bot.x, bot.y + 1);

        if (!state)
        {
            DoorTilemap.SetTile(bot, 199);
            DoorTilemap.SetTile(top, 167);
        }
        else
        {
            DoorTilemap.Erase(bot);
            DoorTilemap.Erase(top);
        }
        
        DoorTilemap.UpdateMeshImmediate();
    }
}
