using UnityEngine;

public class AmbienceController : MonoBehaviour
{
    public AudioSource wavesAudio;
    public AudioSource windAudio;
    public float minWavesHeight = 50;
    public float maxWavesHeight = 50;
    public float minWindHeight = 25;
    public float maxWindHeight = 100;

    private Transform _playerTransform;

    private void Awake()
    {
        _playerTransform = FindObjectOfType<Camera>().transform;
    }

    private void Update()
    {
        float wavesT = Mathf.Clamp01(1 - ((_playerTransform.position.y - minWavesHeight) / maxWavesHeight));
        float windT = Mathf.Clamp01((_playerTransform.position.y - minWindHeight) / maxWindHeight);

        wavesAudio.volume = wavesT;
        windAudio.volume = windT;
    }
}