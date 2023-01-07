using poetools.Abstraction.Unity;
using UnityEngine;

namespace poetools.NewDirectory1
{
    public class FPSCrouchingSystem : MonoBehaviour
    {
        // [SerializeField] private CharacterController controller;
        [SerializeField] private BoxCollider controller;
        [SerializeField] private Trigger safeStandTrigger;
        [SerializeField] private Vector3 crouchingOffset;
        [SerializeField] private float crouchingOffsetGC;
        [SerializeField] private Vector3 crouchingHeight = Vector3.one;
        [SerializeField] private float crouchingSpeedMultiplier = 0.5f;
        [SerializeField] private float crouchSpeed = 15f;
        [SerializeField] private GroundCheck groundCheck;

        private float CrouchingYPos { get; set; }
        private float StandingYPos { get; set; }
        private Vector3 StandingHeight { get; set; }
        
        public bool IsCrouching { get; set; }

        private StandingState Standing { get; } = new StandingState();
        private CrouchingState Crouching { get; } = new CrouchingState();

        private abstract class State
        {
            public FPSCrouchingSystem Parent;
            
            public virtual void Enter() {}
            public virtual void Update() {}
            public virtual void Exit() {}
        }

        private class StandingState : State
        {
            public override void Enter()
            {
                Parent.controller.size = Parent.StandingHeight;
                Parent.controller.center += Vector3.up * 0.3f;
                Parent.groundCheck.transform.localPosition = Vector3.zero;
            }

            public override void Update()
            {
                Vector3 center = Parent.controller.center;
                center.y = Mathf.Lerp(center.y, Parent.StandingYPos, Parent.crouchSpeed * Time.deltaTime);
                Parent.controller.center = center;
                
                if (Parent.IsCrouching)
                    Parent.TransitionTo(Parent.Crouching);
            }
        }
        
        private class CrouchingState : State
        {
            private float _originalSpeed;
            private float _originalFric;
            
            public override void Enter()
            {
                // Vector3 center = Parent.controller.center;
                // center.y = Parent.CrouchingYPos;
                // Parent.controller.center = center;
                Parent.controller.size = Parent.crouchingHeight;
                // Parent.physics.groundCheckOffset.y = Parent.crouchingOffsetGC;
                var lp = Parent.groundCheck.transform.localPosition;
                lp.y = Parent.crouchingOffsetGC;
                Parent.groundCheck.transform.localPosition = lp;
                
                if (Parent.TryGetComponent(out FPSQuakeMovementSystem movement))
                {
                    _originalSpeed = movement.maxGroundSpeed;
                    _originalFric = movement.friction;
                    movement.maxGroundSpeed *= Parent.crouchingSpeedMultiplier;
                    movement.friction /= Parent.crouchingSpeedMultiplier;
                }
            }
            
            public override void Exit()
            {
                if (Parent.TryGetComponent(out FPSQuakeMovementSystem movement))
                {
                    movement.maxGroundSpeed = _originalSpeed;
                    movement.friction = _originalFric;
                }
            }

            public override void Update()
            {
                Vector3 center = Parent.controller.center;
                center.y = Mathf.Lerp(center.y, Parent.CrouchingYPos, Parent.crouchSpeed * Time.deltaTime);
                Parent.controller.center = center;
                
                if (Parent.IsCrouching == false && Parent.safeStandTrigger.Colliders.Count <= 0)
                    Parent.TransitionTo(Parent.Standing);
            }
        }

        private State _currentState;
        
        private void TransitionTo(State state)
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState?.Enter();
        }

        private void Awake()
        {
            Standing.Parent = this;
            Crouching.Parent = this;

            var center = controller.center.y;
            StandingYPos = center;
            CrouchingYPos = center + crouchingOffset.y;
            StandingHeight = controller.size;
            
            TransitionTo(Standing);
        }

        private void FixedUpdate()
        {
            _currentState?.Update();
        }
    }
}