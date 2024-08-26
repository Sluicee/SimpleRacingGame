using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> musicTracks; // Список треков фоновой музыки
    private AudioSource musicSource; // AudioSource для фоновой музыки
    [SerializeField] private AudioMixerGroup gameMixerGroup; // AudioMixerGroup для управления громкостью в игре
    [SerializeField] private AudioMixerGroup menuMixerGroup; // AudioMixerGroup для управления громкостью в меню и паузе
    [SerializeField] private float gameVolume = 0.5f; // Громкость в игре
    [SerializeField] private float menuVolume = 1.0f; // Громкость в меню и паузе
    [SerializeField] private bool isDefaultGameMode = true; // Флаг для определения режима по умолчанию

    private void Start()
    {
        musicSource = GetComponent<AudioSource>();

        // Выберите случайный трек
        if (musicTracks.Count > 0)
        {
            AudioClip selectedTrack = musicTracks[Random.Range(0, musicTracks.Count)];
            musicSource.clip = selectedTrack;
            musicSource.loop = true; // Включите зацикливание, чтобы музыка не прекращалась
            musicSource.Play(); // Запустите воспроизведение
        }

        // Установите громкость в зависимости от флага
        if (isDefaultGameMode)
        {
            SetVolumeForGame();
        }
        else
        {
            SetVolumeForMenu();
        }
    }

    public void SetVolumeForGame()
    {
        musicSource.outputAudioMixerGroup = gameMixerGroup;
        musicSource.volume = gameVolume;
    }

    public void SetVolumeForMenu()
    {
        musicSource.outputAudioMixerGroup = menuMixerGroup;
        musicSource.volume = menuVolume;
    }

    public void EnterGameMode()
    {
        SetVolumeForGame();
    }

    public void EnterMenuMode()
    {
        SetVolumeForMenu();
    }
}
