using UnityEngine.Audio;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

//https://www.youtube.com/watch?v=6OT43pvUyfY
public class AudioManager : SingletonPersistent<AudioManager>
{
    public List<Sound> Sounds = new();

    [SerializeField] AudioMixer _mixer;

    AudioSource _musicAudioSource;
    List<AudioSource> _sfxAudioSources = new();

    Sound _currentMusicSound;
    int _currentMusicClipIndex;

    bool _isPopulated;

    protected override void Awake()
    {
        base.Awake();

        PopulateAudioSources();
        SetPlayerPrefVolume();
    }

    void PopulateAudioSources()
    {
        if (_isPopulated) return;
        _isPopulated = true;
        Debug.Log($"pop audio sources");
        GameObject musicGameObject = new("Music");
        musicGameObject.transform.parent = transform;
        _musicAudioSource = musicGameObject.AddComponent<AudioSource>();
        _musicAudioSource.loop = true;
        _musicAudioSource.outputAudioMixerGroup = _mixer.FindMatchingGroups("Music")[0];

        _sfxAudioSources = new();
        for (int i = 0; i < 25; i++)
        {
            GameObject sfxGameObject = new("SFX" + i);
            sfxGameObject.transform.parent = transform;
            AudioSource a = sfxGameObject.AddComponent<AudioSource>();
            a.spatialBlend = 1;
            a.rolloffMode = AudioRolloffMode.Custom;
            a.maxDistance = 50;
            a.outputAudioMixerGroup = _mixer.FindMatchingGroups("SFX")[0];

            _sfxAudioSources.Add(a);
        }
    }

    public void PlayMusic(Sound sound)
    {
        if (sound == null)
        {
            Debug.LogError("No music to play");
            return;
        }
        _currentMusicSound = sound;
        _currentMusicClipIndex = 0;
        _musicAudioSource.pitch = sound.Pitch;
        StartCoroutine(PlayMusicCoroutine());
    }

    IEnumerator PlayMusicCoroutine()
    {
        if (_musicAudioSource.isPlaying)
        {
            yield return _musicAudioSource.DOFade(0, 5)
                    .SetUpdate(true)
                    .WaitForCompletion();
        }

        _musicAudioSource.volume = 0;
        _musicAudioSource.clip = _currentMusicSound.Clips[_currentMusicClipIndex];
        _musicAudioSource.Play();

        yield return _musicAudioSource.DOFade(_currentMusicSound.Volume, 5)
                .SetUpdate(true)
                .WaitForCompletion();

        yield return new WaitForSecondsRealtime(_musicAudioSource.clip.length - 10);

        _currentMusicClipIndex++;
        if (_currentMusicClipIndex >= _currentMusicSound.Clips.Length)
            _currentMusicClipIndex = 0;

        StartCoroutine(PlayMusicCoroutine());
    }

    public void PlaySFXDelayed(string soundName, float delay)
    {
        StartCoroutine(PlaySFXDelayedCoroutine(soundName, delay));
    }
    IEnumerator PlaySFXDelayedCoroutine(string soundName, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySFX(soundName);
    }

    public AudioSource PlaySFX(string soundName)
    {
        Sound sound = Sounds.First(s => s.name == soundName);
        if (sound == null)
        {
            Debug.LogError($"No sound {soundName} in library");
            return null;
        }

        return PlaySFX(sound);
    }

    public AudioSource PlaySFX(Sound sound, bool isLooping = false)
    {
        AudioSource a = _sfxAudioSources.FirstOrDefault(s => s.isPlaying == false);

        if (a == null) return null;

        a.loop = isLooping;
        Sound instance = Instantiate(sound);
        instance.Play(a);

        return a;
    }

    public Sound GetSound(string name)
    {
        Sound s = Sounds.First(s => s.name == name);
        if (s == null)
            Debug.LogError($"No sound with name {name} in library");
        return s;
    }

    /* volume setters */
    void SetPlayerPrefVolume()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 1));
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 1));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 1));
    }

    // https://forum.unity.com/threads/changing-audio-mixer-group-volume-with-ui-slider.297884/ 
    public void SetMasterVolume(float volume)
    {
        _mixer.SetFloat("MasterVolume", Mathf.Log(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        _mixer.SetFloat("MusicVolume", Mathf.Log(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        _mixer.SetFloat("SFXVolume", Mathf.Log(volume) * 20);
    }
}
