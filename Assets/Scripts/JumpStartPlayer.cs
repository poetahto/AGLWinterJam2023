using poetools;
using UnityEngine;

namespace DefaultNamespace
{
    public class JumpStartPlayer : MonoBehaviour
    {
        [SerializeField] private GroundCheck groundCheck;
        [SerializeField] private AudioBank audioBank;
        [SerializeField] private AudioSource audioSource;

        public void Play()
        {
            if (groundCheck.ConnectedCollider != null)
            {
                AudioBank.TagData data;

                data = groundCheck.ConnectedCollider.TryGetComponent(out TagHolder tagHolder) ? audioBank.Lookup(tagHolder.tags) : audioBank.defaultData;
        
                audioSource.PlayOneShot(data.audioClip, data.volume);
            }
        }
    }
}