using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Setting : ScriptableObject
{
    public List<SettingMap> MapOptions = new();
    public SettingMap Map;

    public List<SettingSnake> SnakeOptions = new();
    public SettingSnake Snake;

    public List<SettingFood> FoodOptions = new();
    public SettingFood Food;

    public bool IsPowerUpActive = true;
}
