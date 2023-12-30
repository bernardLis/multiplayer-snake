using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Setting : ScriptableObject
{
    public Vector2Int GridSize = new(31, 21);
    public float SnakeSpeed = 1;
    public float FoodSpawnRate = 1;
    public Vector3 CameraPosition = new(15, 10, -10);
    public float CameraSize = 12;
}
