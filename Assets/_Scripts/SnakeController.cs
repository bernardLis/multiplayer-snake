using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Shapes;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class SnakeController : MonoBehaviour
{
    GameManager _gameManager;
    PlayerInput _playerInput;
    Setting _setting;

    GameTextDisplayer _gameTextDisplayer;

    Snake _snake;

    IEnumerator _moveCoroutine;
    float _speed;
    Vector2 _lastMovementDirection;
    Vector2 _movementDirection;

    [SerializeField] GameObject _snakeSegmentPrefab;
    List<Vector2> _positions = new();
    List<GameObject> _snakeSegments = new();

    [SerializeField] GameObject _deathEffectPrefab;

    public event Action<Snake> OnDeath;
    public void Initialize(Snake snake)
    {
        if (_gameManager == null) _gameManager = GameManager.Instance;
        _playerInput = _gameManager.GetComponent<PlayerInput>();
        _setting = _gameManager.Setting;
        _gameTextDisplayer = GameTextDisplayer.Instance;

        _snake = snake;
        GetComponent<Triangle>().Color = _snake.Color;

        _movementDirection = Vector2.up;
        transform.rotation = Quaternion.Euler(0, 0, 270);
        _positions.Add(transform.position);

        SubscribeInputActions();

        _speed = _setting.Snake.SnakeSpeed;
        _moveCoroutine = MoveCoroutine();
        StartCoroutine(_moveCoroutine);
    }

    /* INPUT */

    void OnDisable()
    {
        if (_playerInput == null) return;
        UnsubscribeInputActions();
    }

    void OnDestroy()
    {
        if (_playerInput == null) return;
        UnsubscribeInputActions();
    }

    void SubscribeInputActions()
    {
        _playerInput.actions[_snake.MovementSchema.ToString()].performed += GetMovementVector;
        _playerInput.actions["Space"].performed += (ctx) => Grow();
    }

    void UnsubscribeInputActions()
    {
        _playerInput.actions[_snake.MovementSchema.ToString()].performed -= GetMovementVector;
        _playerInput.actions["Space"].performed -= (ctx) => Grow();
    }

    void GetMovementVector(InputAction.CallbackContext context)
    {
        Vector3 inputValue = context.ReadValue<Vector2>();
        inputValue = inputValue.normalized;
        if (inputValue == Vector3.zero) return;
        if (inputValue.x != 0 && inputValue.y != 0) return;        // no diagonal movement

        // block turning 180 degrees
        if (inputValue.x == -_movementDirection.x || inputValue.y == -_movementDirection.y) return;
        if (inputValue.x == -_lastMovementDirection.x || inputValue.y == -_lastMovementDirection.y) return;

        _movementDirection = new(inputValue.x, inputValue.y);
        Vector3 rotation = transform.rotation.eulerAngles;
        if (inputValue.y == 1) rotation = new Vector3(0, 0, 270);
        if (inputValue.y == -1) rotation = new Vector3(0, 0, 90);
        if (inputValue.x == 1) rotation = new Vector3(0, 0, 180);
        if (inputValue.x == -1) rotation = new Vector3(0, 0, 0);
        transform.DORotate(rotation, 0.2f);
    }

    IEnumerator MoveCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_speed);
            Move();
        }
    }

    void Move()
    {
        _lastMovementDirection = _movementDirection;
        transform.DOMove(transform.position + (Vector3)_movementDirection, 0.1f)
                   .OnComplete(() => _positions.Add(transform.position));

        for (int i = 0; i < _snakeSegments.Count; i++)
            _snakeSegments[i].transform.DOMove(_positions[_positions.Count - 1 - i], 0.1f);
    }

    public void Grow()
    {
        int index = _positions.Count - 1 - _snakeSegments.Count;
        Vector3 pos = _positions[index];
        pos.z = -0.5f;

        GameObject segment = Instantiate(_snakeSegmentPrefab, pos, Quaternion.identity);
        segment.GetComponent<Rectangle>().Color = _snake.Color;
        segment.transform.DOScale(0.5f, 0.5f)
                .SetEase(Ease.OutBounce)
                .OnComplete(() => segment.GetComponent<Collider2D>().enabled = true);
        _snakeSegments.Add(segment);

        _snake.Size++;
        if (_snake.Size % 5 == 0)
            _gameTextDisplayer.ShowText($"{_snake.Name} Size: {_snake.Size}");
    }

    public int GetSize()
    {
        return _snakeSegments.Count;
    }

    public void Die()
    {
        _gameTextDisplayer.ShowText($"{_snake.Name} Dies!");

        GameObject de = Instantiate(_deathEffectPrefab, transform.position, Quaternion.identity);
        ParticleSystem.MainModule settings = de.GetComponent<ParticleSystem>().main;
        settings.startColor = _snake.Color;

        AudioManager.Instance.PlaySFX("Death");
        if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
        DOTween.Kill(transform);

        OnDeath?.Invoke(_snake);
        foreach (GameObject segment in _snakeSegments)
        {
            DOTween.Kill(segment);
            Destroy(segment);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out SnakeController snake))
        {
            if (snake.GetSize() == GetSize())
                Die();
            if (snake.GetSize() > GetSize())
                Die();
        }
    }

    /* power ups */

    public void Disco(float duration)
    {
        _gameTextDisplayer.ShowText($"{_snake.Name} Disco!");
        StartCoroutine(DiscoCoroutine(duration));
    }

    IEnumerator DiscoCoroutine(float duration)
    {
        float endTime = Time.time + duration;
        while (true)
        {
            GetComponent<Triangle>().Color = Random.ColorHSV();

            foreach (GameObject segment in _snakeSegments)
                segment.GetComponent<Rectangle>().Color = Random.ColorHSV();

            yield return new WaitForSeconds(0.2f);
            if (Time.time > endTime) break;
        }

        GetComponent<Triangle>().Color = _snake.Color;
        foreach (GameObject segment in _snakeSegments)
            segment.GetComponent<Rectangle>().Color = _snake.Color;
    }

    public void SpeedPickedUp(float duration)
    {
        _gameTextDisplayer.ShowText($"{_snake.Name} Speed!");
        _speed = _setting.Snake.SnakeSpeed * 0.5f;
        StartCoroutine(EndSpeedCoroutine(duration));
    }

    IEnumerator EndSpeedCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        _speed = _setting.Snake.SnakeSpeed;
    }

}
