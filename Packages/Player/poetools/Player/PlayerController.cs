using poetools.Abstraction;
using poetools.Tools;
using UnityEngine;

namespace poetools.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private bool hideCursor;
    
        [Header("References")]
        [SerializeField] private PhysicsComponent playerPhysicsObject;
        [SerializeField] private TransformComponent playerPitchTransform;
        [SerializeField] private TransformComponent playerYawTransform;
        [SerializeField] private TransformComponent playerCameraTransform;

        [Header("Systems")]
        public FPSMovementSystem movementSystem;
        public FPSRotationSystem rotationSystem;
        public FPSInteractionSystem interactionSystem;

        private bool _inputActive = true;

        private void Awake()
        {
            if (hideCursor)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        
            // initialize movement system references (todo: turn into GameObject later?)
            movementSystem.YawRotation = playerYawTransform;
            movementSystem.Physics = playerPhysicsObject;

            // initialize rotation system references (todo: turn into GameObject later?)
            rotationSystem.PitchRotation = playerPitchTransform;
            rotationSystem.YawRotation = playerYawTransform;
        }

        private void Update()
        {
            ApplyMovementInput();
            ApplyRotationInput();
            ApplyInteractionInput();
            
            movementSystem.UpdateMovement();
            rotationSystem.UpdateRotation();
            interactionSystem.UpdateInteraction();
        }

        public void SetInputState(bool active)
        {
            _inputActive = active;
        }

        private void ApplyMovementInput()
        {
            movementSystem.InputDirection = _inputActive
                ? new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
                : Vector2.zero;
        }

        private void ApplyRotationInput()
        {
            rotationSystem.MouseDelta = _inputActive 
                ? new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"))
                : Vector2.zero;
        }

        private void ApplyInteractionInput()
        {
            Vector3 viewOrigin = playerCameraTransform.Position;
            Vector3 viewDirection = playerCameraTransform.Forward;

            interactionSystem.ViewRay = new Ray(viewOrigin, viewDirection);
        
            if (InputTools.GetKeyDown(KeyCode.Mouse0, KeyCode.E))
                interactionSystem.Interact(playerCameraTransform.gameObject);
        }
    }
}