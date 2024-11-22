using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource, _soundSource;

    [SerializeField] private int _maxVolumeLivel = 10;

    private string _musicKey = "MusicVolume", _soundKey = "SoundVolume";

    private void Start()
    {
        AudioMenu menu = GetComponent<AudioMenu>();
        if (menu != null)
        {
            menu.OnChangeMusic += ChangeVolumeMusic;
            menu.OnChangeSound += ChangeVolumeSound;
        }


        _musicSource.volume = CalculateVolume(PlayerPrefs.GetInt(_musicKey));
        _soundSource.volume = CalculateVolume(PlayerPrefs.GetInt(_soundKey));
    }

    private void ChangeVolumeMusic(int volume) =>
        _musicSource.volume = CalculateVolume(PlayerPrefs.GetInt(_musicKey));

    private void ChangeVolumeSound(int volume) =>
        _soundSource.volume = CalculateVolume(PlayerPrefs.GetInt(_soundKey));

    private float CalculateVolume(int volume) 
        => (float)volume / _maxVolumeLivel;

    public float GetVolumeSound() => _soundSource.volume;
}
