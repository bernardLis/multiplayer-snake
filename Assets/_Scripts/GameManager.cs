using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public Setting Setting;

    [Header("Grid")]
    [SerializeField] Transform _tileHolder;
    [SerializeField] GameObject _tilePrefab;
    [SerializeField] GameObject _borderPrefab;
    List<GameObject> _tiles = new();

    Vector2Int _gridSize = new(31, 21);

    [Header("Snakes")]
    [SerializeField] Snake[] _snakeSOs;
    [SerializeField] GameObject _snakePrefab;
    List<GameObject> _snakes = new();
    int _activeSnakeCount;

    [HideInInspector] public VisualElement Root;
    int _time;
    Label _timerLabel;

    WonScreen _wonScreen;

    public event Action OnRestart;
    void Start()
    {
        Root = GetComponent<UIDocument>().rootVisualElement;

        VisualElement fade = new();
        fade.AddToClassList("fade");
        fade.style.opacity = 1;
        Root.Add(fade);

        DOTween.To(x => fade.style.opacity = x, fade.style.opacity.value, 0, 0.5f)
                .OnComplete(() =>
                {
                    GenerateGrid();
                    GenerateBorders();
                    SetUpCamera();
                    GenerateSnakes();
                    StartGame();
                    StartTimer();
                });

    }

    void GenerateGrid()
    {
        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                GameObject tile = Instantiate(_tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                tile.transform.parent = _tileHolder;
                if (y % 2 == 0)
                    tile.GetComponent<Rectangle>().Color = Color.gray;
                _tiles.Add(tile);
            }
        }
    }

    void GenerateBorders()
    {
        for (int x = -1; x < _gridSize.x + 1; x++)
        {
            for (int y = -1; y < _gridSize.y + 1; y++)
            {
                if (x == -1 || x == _gridSize.x || y == -1 || y == _gridSize.y)
                {
                    GameObject border = Instantiate(_borderPrefab, new Vector3(x, y, 0), Quaternion.identity);
                    border.transform.parent = _tileHolder;
                }
            }
        }
    }

    void SetUpCamera()
    {
        Camera camera = Camera.main;
        camera.transform.position = Setting.CameraPosition;
        camera.orthographicSize = Setting.CameraSize;
    }

    public GameObject GetRandomFreeTile()
    {
        GameObject tile = _tiles[Random.Range(0, _tiles.Count)];
        // check if there is a collider in position of tile
        if (Physics2D.OverlapCircleNonAlloc(tile.transform.position, 0.5f, new Collider2D[1]) > 0)
            return GetRandomFreeTile();

        return tile;
    }

    void GenerateSnakes()
    {
        int x = 4;
        foreach (Snake s in _snakeSOs)
        {
            if (!s.IsActive) continue;

            GameObject snake = Instantiate(_snakePrefab, new Vector3(x, 1, -0.5f), Quaternion.identity);
            _snakes.Add(snake);
            snake.name = s.name;
            snake.GetComponent<SnakeController>().Initialize(s);
            x += 4;

            snake.GetComponent<SnakeController>().OnDeath += OnSnakeDeath;
            _activeSnakeCount++;
        }
    }

    void OnSnakeDeath(Snake snake)
    {
        Debug.Log($"Snake died");
        _activeSnakeCount--;
        if (_activeSnakeCount == 0)
            ShowWonScreen(snake);
    }

    void ShowWonScreen(Snake snake)
    {
        Time.timeScale = 0;
        _wonScreen = new(snake);
    }

    void StartGame()
    {
    }

    void StartTimer()
    {
        _timerLabel = Root.Q<Label>("timer");
        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        while (true)
        {
            _time++;
            int minutes = Mathf.FloorToInt(_time / 60f);
            int seconds = Mathf.FloorToInt(_time - minutes * 60);

            _timerLabel.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            yield return new WaitForSeconds(1f);
        }
    }


    public void RestartGame()
    {
        foreach (Snake s in _snakeSOs)
            s.Size = 0;
        _time = 0;
        OnRestart?.Invoke();

        DOTween.To(x => _wonScreen.style.opacity = x, _wonScreen.style.opacity.value, 0, 0.5f)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    _wonScreen.RemoveFromHierarchy();
                    Time.timeScale = 1;
                    GenerateSnakes();
                });
    }


    public void GoToMenu()
    {
        Time.timeScale = 1;
        // TODO: music - fade out
        SceneManager.LoadScene("Menu");
    }

}
