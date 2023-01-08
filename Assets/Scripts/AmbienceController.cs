using UnityEngine;

public class AmbienceController : MonoBehaviour
{
    public AudioSource wavesAudio;
    public AudioSource windAudio;
    public float maxWavesHeight = 50;
    public float minWindHeight = 25;
    public float maxWindHeight = 100;

    private Transform _playerTransform;
    private float _targetWavesVolume;
    private float _targetWindVolume;

    private void Awake()
    {
        _playerTransform = FindObjectOfType<Camera>().transform;
        _targetWindVolume = windAudio.volume;
        _targetWavesVolume = wavesAudio.volume;
    }

    private void Update()
    {
        float wavesT = Mathf.Clamp01(_playerTransform.position.y / maxWavesHeight);
        float windT = Mathf.Clamp01((_playerTransform.position.y - minWindHeight) / maxWindHeight);

        var volume = PlayerPrefs.GetFloat("sfxVolume");
        wavesAudio.volume = ((1 - wavesT) * _targetWavesVolume) * volume;
        windAudio.volume = windT * _targetWindVolume * volume;
    }
}