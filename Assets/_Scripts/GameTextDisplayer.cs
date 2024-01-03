using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class GameTextDisplayer : Singleton<GameTextDisplayer>
{
    VisualElement _textContainer;

    Queue<string> _text = new();

    IEnumerator _showTextCoroutine;
    void Start()
    {
        _textContainer = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("textContainer");
    }

    public void ShowText(string text)
    {
        _text.Enqueue(text);
        if (_showTextCoroutine == null)
        {
            _showTextCoroutine = ShowTextCoroutine();
            StartCoroutine(_showTextCoroutine);
        }
    }

    IEnumerator ShowTextCoroutine()
    {
        while (_text.Count > 0)
        {
            Label label = new(_text.Dequeue());
            _textContainer.Add(label);
            DOTween.To(x => label.style.opacity = x, label.style.opacity.value, 1, 0.5f);
            DOTween.To(x => label.style.opacity = x, label.style.opacity.value, 0, 0.5f).SetDelay(1.5f);
            yield return new WaitForSeconds(2f);
            _textContainer.Clear();
        }

        _textContainer.Clear();
        _showTextCoroutine = null;
    }
}
