using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UIElements;
using DG.Tweening;
using System.Linq;

public static class Helpers
{
    const string _ussCommonTextPrimary = "common__text-primary";

    static List<ArcMovementElement> _arcMovementElements = new();

    public static void SetUpHelpers(VisualElement root)
    {
        _arcMovementElements = new();
        for (int i = 0; i < 50; i++)
        {
            ArcMovementElement el = new(null, Vector3.zero, Vector3.zero);
            el.AddToClassList(_ussCommonTextPrimary);
            _arcMovementElements.Add(el);
            root.Add(el);
        }
    }

    /* UI toolkit */
    public static void DisplayTextOnElement(VisualElement root, VisualElement element, string text, Color color)
    {
        Label l = new Label(text);
        l.style.color = color;

        Vector3 start = new Vector3(element.worldBound.xMin, element.worldBound.yMin, 0);
        Vector3 end = new Vector3(element.worldBound.xMin + Random.Range(-100, 100),
                element.worldBound.yMin - 100, 0);

        ArcMovementElement arcMovementElement = _arcMovementElements.FirstOrDefault(x => !x.IsMoving);
        arcMovementElement.InitializeMovement(l, start, end);
        arcMovementElement.OnArcMovementFinished += ()
                => DOTween.To(x => arcMovementElement.style.opacity = x, 1, 0, 1).SetUpdate(true);
    }
}