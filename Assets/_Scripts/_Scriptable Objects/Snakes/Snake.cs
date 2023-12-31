using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Snake : ScriptableObject
{
    public string Name;

    public SnakeMovementSchemas MovementSchema;
    public Sound JoinSound;
    public Color Color;
    public bool IsActive;
    public List<Sprite> Buttons;

    public int Size = 0;

    public void Initialize(string newName)
    {
        IsActive = false;
        Name = newName;
        Size = 0;
    }
}


public enum SnakeMovementSchemas
{
    MoveWASD,
    MoveYGHJ,
    MovePL,
    MoveArrows
}

