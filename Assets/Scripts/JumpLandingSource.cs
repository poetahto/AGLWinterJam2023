using System;
using poetools;
using UnityEngine;

[Serializable]
public class JumpPlayer : IDisposable
{
    [SerializeField] private AudioBank audioBank;
    [SerializeField] private AudioSource player;

    private IDisposable _listener;

    public void Initialize()
    {
        _listener = Services.EventBus.AddListener<JumpLandEvent>(OnStep, "Footstep Player");
    }

    public void Dispose()
    {
        _listener.Dispose();
    }
    
    public void OnStep(JumpLandEvent eventData)
    {
        AudioBank.TagData data;

        data = eventData.Surface.TryGetComponent(out TagHolder tagHolder) ? audioBank.Lookup(tagHolder.tags) : audioBank.defaultData;
        
        player.PlayOneShot(data.audioClip, data.volume);
    }
}

public class JumpLandingSource : MonoBehaviour
{
    public float minSpeed = 0.5f;
    private GroundCheck _groundCheck;
    private Vector3 _previousPosition;
    private float _speed;

    private void Awake()
    {
        _groundCheck = GetComponent<GroundCheck>();
        _groundCheck.OnTouchGround += GroundCheckOnOnTouchGround;
    }

    private void OnDestroy()
    {
        _groundCheck.OnTouchGround -= GroundCheckOnOnTouchGround;
    }

    private void GroundCheckOnOnTouchGround()
    {
        if (_speed > minSpeed)
            Services.EventBus.Invoke(new JumpLandEvent{Surface = _groundCheck.ConnectedCollider}, "Jump Landing");
    }

    private void Update()
    {
        _speed = (transform.position - _previousPosition).magnitude / Time.deltaTime;

        // if (_groundCheck.JustEntered && _groundCheck.TimeSpentFalling > minSpeed)
            // Services.EventBus.Invoke(new JumpLandEvent{Surface = _groundCheck.ConnectedCollider}, "Jump Landing");

        _previousPosition = transform.position;
    }
}