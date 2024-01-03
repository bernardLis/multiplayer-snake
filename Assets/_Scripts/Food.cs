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
        if (other.TryGetComponent(out SnakeController snakeController))
        {
            PickUp(snakeController);
        }
    }

    protected virtual void PickUp(SnakeController snakeController)
    {
        AudioManager.Instance.PlaySFX("Eat Food");
        DOTween.Kill(transform);
        snakeController.Grow();
        OnCollected?.Invoke();
        Destroy(gameObject);
    }

}
