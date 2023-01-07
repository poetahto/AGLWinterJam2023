using poetools.Tools;
using UnityEngine;
using UnityEngine.Assertions;

namespace poetools.Abstraction.Unity
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rigidbody2DWrapper : PhysicsComponent
    {
        private Rigidbody2D _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        
        public override Vector3 Velocity
        {
            get => _rigidbody.velocity;
            set => _rigidbody.velocity = value;
        }
    }
}