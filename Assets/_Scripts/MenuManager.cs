using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    AudioManager _audioManager;
    [SerializeField] Sound _menuMusic;
    [SerializeField] Setting _setting;

    VisualElement _root;

    public Snake[] Snakes;

    MyButton _playButton;

    public event Action OnGameStarted;
    void Start()
    {
        _audioManager = AudioManager.Instance;
        _audioManager.PlayMusic(_menuMusic);

        // get the root visual element
        _root = GetComponent<UIDocument>().rootVisualElement;
        VisualElement snakeContainer = _root.Q<VisualElement>("snakes");
        Helpers.SetUpHelpers(_root);

        for (int i = 0; i < 4; i++)
        {
            Snakes[i].Initialize(Names[Random.Range(0, Names.Length)]);
            Snakes[i].IsActive = false;
            MenuSnakeContainer snake = new(Snakes[i]);
            snakeContainer.Add(snake);
        }

        // play button
        VisualElement playButtonContainer = _root.Q<VisualElement>("playButtonContainer");
        _playButton = new MyButton("Play", callback: StartGame);
        playButtonContainer.Add(_playButton);

        // game settings
        VisualElement settingsContainer = _root.Q<VisualElement>("settingsContainer");
        settingsContainer.Add(new SettingsElement(_setting));


        // options optionsContainer
        VisualElement optionsContainer = _root.Q<VisualElement>("optionsContainer");
        optionsContainer.Add(new OptionsElement());
    }

    void StartGame()
    {
        bool isAnySnakeActive = false;
        foreach (Snake snake in Snakes)
        {
            if (snake.IsActive)
            {
                isAnySnakeActive = true;
                break;
            }
        }

        if (!isAnySnakeActive)
        {
            _audioManager.PlaySFX("Death");
            Helpers.DisplayTextOnElement(_root, _playButton, "No snakes selected", Color.red);
            return;
        }

        _audioManager.PlaySFX("Play Click");
        OnGameStarted?.Invoke();

        VisualElement fade = new();
        fade.AddToClassList("fade");
        _root.Add(fade);

        DOTween.To(x => fade.style.opacity = x, fade.style.opacity.value, 1, 0.5f)
                .OnComplete(() =>
                {
                    SceneManager.LoadScene("Main");
                });
    }


    public string[] Names = new string[] {
"Sugar",
"Tangerine",
"Nectar",
"Jelly",
"Lolly",
"Trixie",
"Butters",
"Hopper",
"Stripe",
"Checkers",
"Barnum",
"Brownie",
"Poochie",
"Flinch",
"Pez",
"Truffle",
"Spud",
"Duck",
"Chunk",
"Nappo",
"Mentos",
"Raisin",
"Dove",
"Cadbury",
"Good and Plenty",
"Jaffa",
"Kit Kat",
"Mister Pink",
"Crunch",
"Pistachio",
"Praline",
"Jell-O",
"Cool Whip",
"Babka",
"Ladyfinger",
"Marble",
"Red Velvet",
"Chiffon",
"Marzipan",
"Mustard",
"Avocado",
"Coriander",
"Tango",
"Pepper",
"Jabba",
"Wiggles",
"Dude",
"Baloo",
"Scrappy",
"Santa Paws",
"Juniper",
"Clever",
"Monkey",
"Blinker",
"Sinatra",
"Cootie",
"Cranberry",
"007 and a Half",
"Dirty Mary",
"Lodi",
"Head Over Heels",
"Skittles",
"Chardonnay",
"Old Vine",
"Wild Wild West",
"Blue Lagoon",
"Ready Red",
"Madea",
"Malibu",
"Popsicle",
"Crusher",
"Snapper",
"Hurricane",
"Sanoma",
"Daddario",
"Rudolph the Red",
"Copacabana",
"Little Bear",
"Diva",
"Southside",
"Arizona Sunrise",
"Yum Yum",
"Buffalo",
"Doctor Funk",
"Durango",
"King Kong",
"Wobbler",
"Mandarin",
"Cosmopolitan",
"Bacardi Orange",
"Bam Bam",
"Rider",
"Rock Star",
"Jamaica",
"Hawaiian Punch",
"Lady Killer",
"Lemon Flip",
"Zorro",
"Kermit",
"Beetlejuice",
"One Eye",
"High Class",
"Swoop",
"First Born",
"Fair Weather",
"Rusher",
"Evil",
"Bombshell",
"Big Red",
"Regal",
"Fraidy",
"Munch",
"Chalupa",
"Cheesy"
     };
}

