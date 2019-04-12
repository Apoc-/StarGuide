using System.Collections.Generic;
using Assets.Scripts;
using CreativeSpore.SuperTilemapEditor;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DoorBehaviour : MonoBehaviour
{
    private Collider2D _doorTrigger;
    private bool _oldState = true;

    private readonly HashSet<Collider2D> CollidersTouchingMe = new HashSet<Collider2D>();
    public Vector2 DoorLocation;
    public STETilemap DoorTilemap;

    private bool IsOpen => CollidersTouchingMe.Count > 0;

    private void Start()
    {
        _doorTrigger = GetComponent<Collider2D>();
    }

    // Destroy everything that enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        var canOpenDoors = other.gameObject.GetComponent<ICanOpenDoors>() != null;
        if (canOpenDoors && !CollidersTouchingMe.Contains(other)) CollidersTouchingMe.Add(other);
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (CollidersTouchingMe.Contains(other)) CollidersTouchingMe.Remove(other);
    }

    private void Update()
    {
        if (!Application.isPlaying) return;


        SetDoorState(IsOpen);
    }

    private void SetDoorState(bool state)
    {
        if (state == _oldState)
            return;

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

        _oldState = state;
        DoorTilemap.UpdateMeshImmediate();
    }
}