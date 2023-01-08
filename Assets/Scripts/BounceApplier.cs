using poetools;
using poetools.Abstraction;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class BounceApplier : MonoBehaviour
    {
        [SerializeField] private GroundCheck groundCheck;

        private PhysicsComponent _physics;
        
        private void Awake()
        {
            _physics = GetComponent<PhysicsComponent>();
            groundCheck.OnTouchGround += GroundCheckOnOnTouchGround;
        }

        private void OnDestroy()
        {
            groundCheck.OnTouchGround -= GroundCheckOnOnTouchGround;
        }

        private void GroundCheckOnOnTouchGround()
        {
            if (groundCheck.ConnectedCollider.TryGetComponent(out BouncePlatform bounce))
            {
                _physics.Velocity = new Vector3(_physics.Velocity.x, 0, _physics.Velocity.z);
                _physics.Velocity += groundCheck.ContactNormal * bounce.launchSpeed;
            }
        }
    }
}