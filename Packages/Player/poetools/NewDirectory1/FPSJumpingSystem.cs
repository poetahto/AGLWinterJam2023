using poetools.Abstraction;
using UnityEngine;
using UnityEngine.Events;

namespace poetools.NewDirectory1
{
    public class FPSJumpingSystem : MonoBehaviour
    {
        #region Settings

            [Header("Standard Jump Settings")] 
            [SerializeField] public bool holdAndJump;
            [SerializeField] public bool scrollToJump;
            [SerializeField] private float jumpDistance = 4.5f;
            [SerializeField] private float jumpHeight = 1.1f;
            [SerializeField] public float assumedInitialSpeed = 5f;
        
            [SerializeField, Range(0.1f, 1)] 
            private float standardSkew = 0.5f;
        
            [Header("Fast Fall Settings")] 
            [SerializeField] private bool enableFastFall;
            [SerializeField] private float minDistance = 2.5f;
            [SerializeField] private float minHeight = 0.55f;
        
            [SerializeField, Range(0.1f, 1)] 
            private float fastFallSkew = 0.5f;

            [Header("Other Settings")] 
            [SerializeField] private int airJumps;
            [SerializeField] private float coyoteTime = 0.15f;
            [SerializeField] public float jumpBufferTime = 0.15f;
            
            public float JumpSpeed { get; private set; }

            public float StandardGravityRising  {get; private set;} 
            public float StandardGravityFalling {get; private set;}
            public float FastFallGravityRising  {get; private set;}
            public float FastFallGravityFalling {get; private set;}

            private Buffer _jumpBuffer = new Buffer();
            
            public void CalculateGravityAndSpeed()
            {
                // Gravity math based on the GDC talk found here:
                // https://youtu.be/hG9SzQxaCm8?t=794
        
                JumpSpeed = 2 * jumpHeight * assumedInitialSpeed / (jumpDistance * standardSkew);
        
                StandardGravityRising = 2 * jumpHeight * Mathf.Pow(assumedInitialSpeed, 2) 
                                        / Mathf.Pow(jumpDistance * standardSkew, 2);
        
                StandardGravityFalling = 2 * jumpHeight * Mathf.Pow(assumedInitialSpeed, 2) 
                                         / Mathf.Pow(jumpDistance * (1 - standardSkew), 2);
        
                FastFallGravityRising = 2 * minHeight * Mathf.Pow(assumedInitialSpeed, 2) 
                                        / Mathf.Pow(minDistance * fastFallSkew, 2);
        
                FastFallGravityFalling = 2 * minHeight * Mathf.Pow(assumedInitialSpeed, 2) 
                                         / Mathf.Pow(minDistance * (1 - fastFallSkew), 2);
            }

            public float GetCurrentGravity(bool rising, bool holdingJump)
            {
                if (enableFastFall && holdingJump == false)
                    return rising ? FastFallGravityRising : FastFallGravityFalling;

                return rising ? StandardGravityRising : StandardGravityFalling;
            }

        #endregion

        public UnityEvent onJump;

        // Internal State
    
        private bool _coyoteAvailable;
        private bool _groundJumpAvailable;
        private int _remainingAirJumps;
        private bool _wasGrounded;

        private bool CoyoteAvailable => _coyoteAvailable && GroundCheck.AirTime < coyoteTime;

        public PhysicsComponent PhysicsComponent;
        public Gravity Gravity;
        public GroundCheck GroundCheck;
        
        public void RefreshJumps()
        {
            _remainingAirJumps = airJumps;
            _coyoteAvailable = true;
            _groundJumpAvailable = true;
        }

        private void TryToJump()
        {
            if (ShouldJump())
                ApplyJump();
        }

        private bool ShouldJump()
        {
            if ((GroundCheck.IsGrounded || CoyoteAvailable) && _groundJumpAvailable)
            {
                _groundJumpAvailable = false;
                return true;
            }

            if (!GroundCheck.IsGrounded && _remainingAirJumps > 0)
            {
                _remainingAirJumps--;
                return true;
            }

            return false;
        }

        private void ApplyJump()
        {
            Vector3 currentVelocity = PhysicsComponent.Velocity;
            currentVelocity.y = JumpSpeed;
            PhysicsComponent.Velocity = currentVelocity;
        
            _coyoteAvailable = false;
            _jumpBuffer.Clear();
            onJump.Invoke();
        }
        
        public bool HoldingSpace { get; set; }

        private void Update()
        {
            if (_jumpBuffer.IsQueued(jumpBufferTime))
                TryToJump();
        }

        private void Awake()
        {
            if (PhysicsComponent == null)
                PhysicsComponent = GetComponent<PhysicsComponent>();

            if (Gravity == null)
                Gravity = GetComponent<Gravity>();
            
            CalculateGravityAndSpeed();
        }

        public void Jump()
        {
            _jumpBuffer.Queue();
        }

        private void FixedUpdate()
        {
            if (Gravity != null)
                UpdateGravity(HoldingSpace);
        }

        public void UpdateGravity(bool holdingJump)
        {
            bool rising = PhysicsComponent.Velocity.y > 0;
            Gravity.amount = GetCurrentGravity(rising, holdingJump);

            if (!_wasGrounded && GroundCheck.IsGrounded)
                RefreshJumps();
            
            _wasGrounded = GroundCheck.IsGrounded;
        }
    }
}