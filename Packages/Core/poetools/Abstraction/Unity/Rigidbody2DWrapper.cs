using poetools.Tools;
using UnityEngine;
using UnityEngine.Assertions;

namespace poetools.Abstraction.Unity
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rigidbody2DWrapper : PhysicsComponent
    {
        [SerializeField]
        [Tooltip("The downwards direction used to check if we are grounded.")]
        private Vector3 gravityDirection = Vector3.down;
        
        [SerializeField]
        [Tooltip("How steep a slope we can climb without slipping.")]
        private float slopeLimitDegrees = 45f;
        
        [SerializeField]
        [Tooltip("Draws debug information to the screen.")]
        private bool showDebug;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        
        private void Update()
        {
            WasGroundedLastFrame = IsGrounded;
            CheckIsGrounded();
    
            if (JustEntered)
                TimeSpentFalling = 0;
    
            if (JustExited)
                TimeSpentGrounded = 0;
            
            if (IsGrounded)
                TimeSpentGrounded += Time.deltaTime;
            
            else TimeSpentFalling += Time.deltaTime;
        }

        public override Vector3 Velocity
        {
            get => _rigidbody.velocity;
            set => _rigidbody.velocity = value;
        }

        public override bool IsGrounded => _isGrounded;
        public override float AirTime => TimeSpentFalling;
        public override float GroundTime => TimeSpentGrounded;

        #region Useful Data

            // todo: All of this stuff is useful: its just not required by the interface right now, maybe create a new one? 
            
            private bool WasGroundedLastFrame { get; set; }
            private float TimeSpentGrounded { get; set; }
            private float TimeSpentFalling { get; set; }
            
            private bool JustEntered => IsGrounded && !WasGroundedLastFrame;
            private bool JustExited => !IsGrounded && WasGroundedLastFrame;

            private Vector3 ContactNormal { get; set; }
            private Collider2D ConnectedCollider { get; set; }

        #endregion

        private Rigidbody2D _rigidbody;
        private Vector3 _previousPosition;
        private Vector3 _currentPosition;
        private Vector3 _velocity;
        private bool _isGrounded;
    
        public float groundDistance = 1;

        private const int MaxHits = 10;
        private RaycastHit2D[] _hits = new RaycastHit2D[MaxHits];
        
        private void CheckIsGrounded()
        {
            int hits = Physics2D.RaycastNonAlloc(transform.position, -transform.up, _hits, groundDistance);
            
            Assert.IsTrue(hits <= MaxHits);

            int bestFit = -1;
            float closestDistance = float.PositiveInfinity;

            for (int i = 0; i < hits; i++)
            {
                var cur = _hits[i];

                // We cannot stand on triggers, so early out.
                if (cur.collider.isTrigger || cur.transform == transform)
                    continue;
                
                // We can only stand on slopes with the desired steepness
                Vector3 upDirection = -gravityDirection;
                Vector3 normalDirection = cur.normal;
                float slopeAngle = Vector3.Angle(upDirection, normalDirection);

                // We only want to check the nearest collider we hit.
                if (slopeAngle <= slopeLimitDegrees && cur.distance < closestDistance)
                    bestFit = i;
            }

            if (bestFit >= 0)
            {
                _isGrounded = true;
                ConnectedCollider = _hits[bestFit].collider;
                ContactNormal = _hits[bestFit].normal;
            }
            else
            {
                _isGrounded = false;
                ConnectedCollider = null;
                ContactNormal = Vector3.zero;
            }
            
            DebugDrawTools.Arrow(transform.position, transform.position - (transform.up * groundDistance), _isGrounded ? Color.green : Color.red, 0);
        }

        #region Debug
    
            private void OnGUI()
            {
                if (showDebug)
                {
                    string connectedCollider = ConnectedCollider ? ConnectedCollider.name : "None";
                        
                    GUILayout.Label($"IsGrounded: {IsGrounded}");
                    GUILayout.Label($"Was Grounded Last Frame: {WasGroundedLastFrame}");
                    GUILayout.Label($"Connected Collider: {connectedCollider}");
                    GUILayout.Label($"Contact Normal: {ContactNormal}");
                    GUILayout.Label($"Time spent grounded: {TimeSpentGrounded}");
                    GUILayout.Label($"Time spent falling: {TimeSpentFalling}");
                    GUILayout.Label($"Velocity: {_velocity}");
                }
            }
    
        #endregion
    }
}