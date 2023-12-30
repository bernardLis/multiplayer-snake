using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Food : MonoBehaviour
{

    public event Action OnCollected;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out SnakeController snake))
        {
            DOTween.Kill(transform);
            snake.Grow();
            OnCollected?.Invoke();
            Destroy(gameObject);
        }
    }

}
