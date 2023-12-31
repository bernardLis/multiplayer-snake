using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class WonScreen : VisualElement
{
    GameManager _gameManager;
    VisualElement _root;

    public WonScreen(Snake snake)
    {
        _gameManager = GameManager.Instance;
        _root = _gameManager.Root;

        _root.Add(this);
        AddToClassList("wonScreen");

        DOTween.To(() => style.opacity.value, x => style.opacity = x, 1, 0.5f)
                .SetUpdate(true);

        Label label = new($"{snake.Name} won!");
        label.AddToClassList("title");
        label.style.color = snake.Color;
        Add(label);

        Label size = new($"Size: {snake.Size}");
        size.AddToClassList("title");
        size.style.color = snake.Color;
        Add(size);

        VisualElement buttons = new();
        buttons.style.flexDirection = FlexDirection.Row;
        Add(buttons);

        MyButton restartButton = new("Restart", callback: () => _gameManager.RestartGame());
        buttons.Add(restartButton);

        MyButton menuButton = new("Menu", callback: () => _gameManager.GoToMenu());
        buttons.Add(menuButton);
    }
}
