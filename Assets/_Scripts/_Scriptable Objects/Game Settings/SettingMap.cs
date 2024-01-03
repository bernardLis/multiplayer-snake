using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SettingMap : ScriptableObject
{
    public string Name;
    public Vector2Int GridSize;
    public Vector3 CameraPosition;
    public float CameraSize;
}
