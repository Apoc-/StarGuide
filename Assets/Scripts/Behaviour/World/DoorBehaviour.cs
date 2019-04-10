using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
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

    private bool IsOpen => CollidersTouchingMe.Count > 0;

    private void Start()
    {
        _doorTrigger = GetComponent<Collider2D>();
    }
    
    
    private HashSet<Collider2D> CollidersTouchingMe = new HashSet<Collider2D>();
    
    // Destroy everything that enters the trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        var canOpenDoors = other.gameObject.GetComponent<ICanOpenDoors>() != null;
        if (canOpenDoors && !CollidersTouchingMe.Contains(other))
        {
            CollidersTouchingMe.Add(other);
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (CollidersTouchingMe.Contains(other))
        {
            CollidersTouchingMe.Remove(other);
        }
    }

    void Update()
    {
        SetDoorState(IsOpen);
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
