using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioMenu : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider, _soundSlider;

    private string _musicKey = "MusicVolume", _soundKey = "SoundVolume";

    public delegate void Audio(int volume);
    public event Audio OnChangeMusic;
    public event Audio OnChangeSound;


    private void Start()
    {
        if (!PlayerPrefs.HasKey(_musicKey))
            PlayerPrefs.SetInt(_musicKey, 8);
        if (!PlayerPrefs.HasKey(_soundKey))
            PlayerPrefs.SetInt(_soundKey, 16);

        _musicSlider.value = PlayerPrefs.GetInt(_musicKey);
        _soundSlider.value = PlayerPrefs.GetInt(_soundKey);

        // ������������� �� ������� ��������� �������� ���������
        _musicSlider.onValueChanged.AddListener(OnMusicVolumeChange);
        _soundSlider.onValueChanged.AddListener(OnSoundVolumeChange);
    }

    // ����� ��� ��������� ��������� ��������� ������
    private void OnMusicVolumeChange(float value)
    {
        PlayerPrefs.SetInt(_musicKey, (int)value);
        PlayerPrefs.Save();
        OnChangeMusic?.Invoke((int)value);
        Debug.Log("Music Volume changed to: " + value);
    }

    // ����� ��� ��������� ��������� ��������� �������� ��������
    private void OnSoundVolumeChange(float value)
    {
        PlayerPrefs.SetInt(_soundKey, (int)value);
        PlayerPrefs.Save(); 
        OnChangeSound?.Invoke((int)value);
        Debug.Log("Sound Volume changed to: " + value);
    }

    private void OnDestroy()
    {
        // ������������ �� �������, ����� �������� ������ ������
        _musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChange);
        _soundSlider.onValueChanged.RemoveListener(OnSoundVolumeChange);
    }
}
