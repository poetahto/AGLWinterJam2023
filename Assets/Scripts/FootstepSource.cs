using System;
using poetools;
using poetools.Abstraction;
using UnityEngine;

public struct FootstepEvent
{
    public Collider Surface;
}

[Serializable]
public class FootstepPlayer : IDisposable
{
    [SerializeField] private AudioBank audioBank;
    [SerializeField] private AudioSource player;

    private IDisposable _listener;

    public void Initialize()
    {
        _listener = Services.EventBus.AddListener<FootstepEvent>(OnStep, "Footstep Player");
    }

    public void Dispose()
    {
        _listener.Dispose();
    }
    
    public void OnStep(FootstepEvent eventData)
    {
        AudioBank.TagData data;

        data = eventData.Surface.TryGetComponent(out TagHolder tagHolder) ? audioBank.Lookup(tagHolder.tags) : audioBank.defaultData;
        
        player.PlayOneShot(data.audioClip, data.volume);
    }
}

public struct JumpLandEvent
{
    public Collider Surface;
}

public class FootstepSource : MonoBehaviour
{
    [SerializeField] private float stepDistance;
    [SerializeField] private GroundCheck groundCheck;

    public Vector2 InputDirection { get; set; }
    private float _elapsed;

    private void Awake()
    {
        Services.EventBus.AddListener<JumpLandEvent>(e => { _elapsed = 0; }, "Resetting footstep source.");
    }

    private void Update()
    {
        if (groundCheck.IsGrounded && InputDirection != Vector2.zero)
        {
            _elapsed += (transform.position - _previousPosition).magnitude;

            if (_elapsed >= stepDistance)
            {
                _elapsed = 0;
                Services.EventBus.Invoke(new FootstepEvent { Surface = groundCheck.ConnectedCollider },
                    "Player Footstep Source");
            }
        }

        _previousPosition = transform.position;
    }

    private Vector3 _previousPosition;
}