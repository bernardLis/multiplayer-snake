using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.InputSystem;

public class MenuSnakeContainer : VisualElement
{
    MenuManager _menuManager;
    PlayerInput _playerInput;

    Label _joinLabel;
    VisualElement _colorContainer;

    Snake _snake;
    public MenuSnakeContainer(Snake snake)
    {
        _snake = snake;

        AddToClassList("snakeContainer");

        Label title = new(snake.Name);
        title.AddToClassList("title");
        Add(title);

        AddKeyContainer();

        _joinLabel = new("Press a key to join");
        _joinLabel.style.opacity = 0;
        Add(_joinLabel);

        DOTween.To(x => _joinLabel.style.opacity = x, _joinLabel.style.opacity.value, 1, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetId("joinLabel");

        if (_menuManager == null) _menuManager = MenuManager.Instance;
        _menuManager.OnGameStarted += OnGameStarted;
        _playerInput = _menuManager.GetComponent<PlayerInput>();
        _playerInput.actions[snake.MovementSchema.ToString()].performed += Join;

        _colorContainer = new();
        _colorContainer.AddToClassList("colorContainer");
        _colorContainer.style.backgroundColor = Color.white;
        Add(_colorContainer);
    }

    void AddKeyContainer()
    {
        VisualElement keyContainer = new();
        keyContainer.AddToClassList("keyContainer");
        Add(keyContainer);

        VisualElement topContainer = new();
        VisualElement bottomContainer = new();
        bottomContainer.style.flexDirection = FlexDirection.Row;
        keyContainer.Add(topContainer);
        keyContainer.Add(bottomContainer);
        for (int i = 0; i < _snake.Buttons.Count; i++)
        {
            VisualElement key = new();
            key.AddToClassList("key");
            key.style.backgroundImage = new StyleBackground(_snake.Buttons[i].texture);
            if (i == 0)
                topContainer.Add(key);
            else
                bottomContainer.Add(key);
        }
    }

    void Join(InputAction.CallbackContext context)
    {
        _playerInput.actions[_snake.MovementSchema.ToString()].performed -= Join;
        _joinLabel.style.opacity = 1;
        DOTween.Kill("joinLabel");
        _joinLabel.text = "Press key to change color";
        _snake.IsActive = true;

        SetColor();
    }

    void SetColor()
    {
        RollColor(default);

        _playerInput.actions[_snake.MovementSchema.ToString()].performed += RollColor;
    }

    void RollColor(InputAction.CallbackContext context)
    {
        Color random = new(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );

        _snake.Color = random;
        _colorContainer.style.backgroundColor = random;
    }

    void OnGameStarted()
    {
        _playerInput.actions[_snake.MovementSchema.ToString()].performed -= RollColor;
        _playerInput.actions[_snake.MovementSchema.ToString()].performed -= Join;
        _playerInput = null;
        _menuManager.OnGameStarted -= OnGameStarted;
    }
}
