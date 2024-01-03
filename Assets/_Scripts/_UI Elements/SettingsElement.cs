using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsElement : VisualElement
{
    Setting _setting;

    public SettingsElement(Setting setting)
    {
        _setting = setting;
        AddGridSizeDropdown();
        AddSnakeDropdown();
        AddFoodDropdown();
        AddPowerUpBox();
    }

    void AddGridSizeDropdown()
    {
        DropdownField mapDropdown = new("Map Size");
        foreach (SettingMap m in _setting.MapOptions)
            mapDropdown.choices.Add(m.Name);
        mapDropdown.index = 1;
        Add(mapDropdown);

        mapDropdown.RegisterValueChangedCallback((e) =>
        {
            _setting.Map = _setting.MapOptions[mapDropdown.index];
        });
    }

    void AddSnakeDropdown()
    {
        DropdownField snakeDropdown = new("Snake Speed");
        foreach (SettingSnake m in _setting.SnakeOptions)
            snakeDropdown.choices.Add(m.Name);
        snakeDropdown.index = 1;
        Add(snakeDropdown);

        snakeDropdown.RegisterValueChangedCallback((e) =>
        {
            _setting.Snake = _setting.SnakeOptions[snakeDropdown.index];
        });
    }

    void AddFoodDropdown()
    {
        DropdownField foodDropdown = new("Food");
        foreach (SettingFood m in _setting.FoodOptions)
            foodDropdown.choices.Add(m.Name);
        foodDropdown.index = 1;
        Add(foodDropdown);

        foodDropdown.RegisterValueChangedCallback((e) =>
        {
            _setting.Food = _setting.FoodOptions[foodDropdown.index];
        });
    }

    void AddPowerUpBox()
    {
        Toggle powerUpToggle = new("Power Ups");
        powerUpToggle.value = true;
        Add(powerUpToggle);

        powerUpToggle.RegisterValueChangedCallback((e) =>
        {
            _setting.IsPowerUpActive = powerUpToggle.value;
        });

    }
}
