using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OptionsElement : VisualElement
{
    AudioManager _audioManager;
    const string _ussVolumeSlider = "volumeSlider";

    public OptionsElement()
    {
        _audioManager = AudioManager.Instance;
        AddVolumeSliders();
    }

    void AddVolumeSliders()
    {
        VisualElement parent = this;
        Label title = new("Audio Options");
        title.AddToClassList("title");
        parent.Add(title);

        Slider master = AddVolumeSlider("Master", parent);
        master.AddToClassList(_ussVolumeSlider);
        master.value = PlayerPrefs.GetFloat("MasterVolume", 1);
        master.RegisterValueChangedCallback(MasterVolumeChange);

        Slider music = AddVolumeSlider("Music", parent);
        music.AddToClassList(_ussVolumeSlider);
        music.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        music.RegisterValueChangedCallback(MusicVolumeChange);

        Slider SFX = AddVolumeSlider("SFX", parent);
        SFX.AddToClassList(_ussVolumeSlider);
        SFX.value = PlayerPrefs.GetFloat("SFXVolume", 1);
        SFX.RegisterValueChangedCallback(SFXVolumeChange);
    }

    Slider AddVolumeSlider(string name, VisualElement parent)
    {
        //https://forum.unity.com/threads/changing-audio-mixer-group-volume-with-ui-slider.297884/
        VisualElement container = CreateContainer(name);
        Slider volumeSlider = new()
        {
            lowValue = 0.001f,
            highValue = 1f
        };
        volumeSlider.style.width = 200;
        volumeSlider.value = PlayerPrefs.GetFloat(name, 1);

        container.Add(volumeSlider);
        parent.Add(container);

        return volumeSlider;
    }

    void MasterVolumeChange(ChangeEvent<float> evt)
    {
        PlayerPrefs.SetFloat("MasterVolume", evt.newValue);
        PlayerPrefs.Save();
        _audioManager.SetMasterVolume(evt.newValue);
    }

    void MusicVolumeChange(ChangeEvent<float> evt)
    {
        PlayerPrefs.SetFloat("MusicVolume", evt.newValue);
        PlayerPrefs.Save();
        _audioManager.SetMusicVolume(evt.newValue);
    }

    void SFXVolumeChange(ChangeEvent<float> evt)
    {
        PlayerPrefs.SetFloat("SFXVolume", evt.newValue);
        PlayerPrefs.Save();
        _audioManager.SetSFXVolume(evt.newValue);
    }


    VisualElement CreateContainer(string labelText)
    {
        VisualElement container = new();
        container.style.flexDirection = FlexDirection.Row;
        Label label = new(labelText);
        container.Add(label);
        return container;
    }


}
