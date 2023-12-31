using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FoodManager : MonoBehaviour
{
    GameManager _gameManager;

    [SerializeField] Transform _foodHolder;

    [SerializeField] Food _foodPrefab;
    List<Food> _foods = new();

    void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.OnRestart += Restart;
        StartCoroutine(SpawnFood());
    }

    IEnumerator SpawnFood()
    {
        while (true)
        {
            yield return new WaitForSeconds(_gameManager.Setting.Food.FoodSpawnRate);
            Vector3 pos = _gameManager.GetRandomFreeTile().transform.position;
            pos.z = -0.5f;
            Food food = Instantiate(_foodPrefab, pos, Quaternion.identity);
            food.transform.parent = _foodHolder;
            _foods.Add(food);
            food.OnCollected += () => _foods.Remove(food);

            food.transform.DOScale(0.5f, 0.5f).SetEase(Ease.OutBounce);
        }
    }

    void Restart()
    {
        for (int i = _foods.Count - 1; i >= 0; i--)
        {
            Destroy(_foods[i].gameObject);
        }
        _foods.Clear();
    }

}
