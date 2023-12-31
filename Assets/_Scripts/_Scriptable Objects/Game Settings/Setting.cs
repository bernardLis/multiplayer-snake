using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Setting : ScriptableObject
{
    public List<SettingGridSize> GridSizeOptions = new();
    public SettingGridSize GridSize;

    public List<SettingSnake> SnakeOptions = new();
    public SettingSnake Snake;

    public List<SettingFood> FoodOptions = new();
    public SettingFood Food;

}
