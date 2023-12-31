using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class MyButton : Button
{
    const string _ussCommonButtonBasic = "button";

    AudioManager _audioManager;

    protected Label _text;

    Action _currentCallback;
    public MyButton(string buttonText = null, string className = null, Action callback = null)
    {
        _audioManager = AudioManager.Instance;

        _text = new Label(buttonText);
        _text.style.whiteSpace = WhiteSpace.Normal;
        Add(_text);
        if (buttonText == "")
            _text.style.display = DisplayStyle.None;

        AddToClassList(_ussCommonButtonBasic);

        if (callback != null)
        {
            _currentCallback = callback;
            clicked += callback;
        }

        RegisterCallback<PointerEnterEvent>(OnPointerEnter);
        RegisterCallback<PointerLeaveEvent>(OnPointerLeave);

        RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    void OnPointerUp(PointerUpEvent evt)
    {
        this.Blur();
    }

    public void ChangeCallback(Action newCallback)
    {
        clickable = new Clickable(() => { });
        clicked += newCallback;
    }

    public void ClearCallbacks() { clickable = new Clickable(() => { }); }

    public void SetText(string newText)
    {
        _text.text = newText;
        _text.style.display = DisplayStyle.Flex;
    }

    void OnPointerEnter(PointerEnterEvent evt)
    {
        if (!enabledSelf)
            return;
        if (_audioManager != null)
            _audioManager.PlaySFX("Change Color");
    }

    void OnPointerLeave(PointerLeaveEvent evt)
    {
    }

    void PreventInteraction(MouseEnterEvent evt)
    {
        evt.PreventDefault();
        evt.StopImmediatePropagation();
    }

    void OnDisable()
    {
        UnregisterCallback<PointerEnterEvent>(OnPointerEnter);
        UnregisterCallback<PointerLeaveEvent>(OnPointerLeave);

        // https://forum.unity.com/threads/hover-state-control-from-code.914504/
        RegisterCallback<MouseEnterEvent>(PreventInteraction);
    }

    void OnEnable()
    {
        RegisterCallback<PointerEnterEvent>(OnPointerEnter);
        RegisterCallback<PointerLeaveEvent>(OnPointerLeave);

        UnregisterCallback<MouseEnterEvent>(PreventInteraction);
    }

}
