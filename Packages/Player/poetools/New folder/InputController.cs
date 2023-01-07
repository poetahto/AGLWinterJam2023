using UnityEngine;

namespace BeastWhisperer.Prototype.Platformer.Input
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] 
        private InputControlTarget initialTarget;
        
        public InputControlTarget Target { get; private set; }
        
        private bool _hasTarget;

        private void Start()
        {
            SetTarget(initialTarget);
        }

        private void Update()
        {
            if (_hasTarget)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
                    Target.inputJumpEvent.Invoke();

                if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift))
                    Target.inputSprintEvent.Invoke(true);
                
                else if (UnityEngine.Input.GetKeyUp(KeyCode.LeftShift))
                    Target.inputSprintEvent.Invoke(false);

                if (UnityEngine.Input.GetKeyDown(KeyCode.LeftControl))
                    Target.inputCrouchEvent.Invoke(true);
                
                else if (UnityEngine.Input.GetKeyUp(KeyCode.LeftControl))
                    Target.inputCrouchEvent.Invoke(false);
                
                Vector2 inputDirection = new Vector2(
                    UnityEngine.Input.GetAxisRaw("Horizontal"), 
                    UnityEngine.Input.GetAxisRaw("Vertical")
                );

                Vector2 mouseDelta = new Vector2(
                    UnityEngine.Input.GetAxisRaw("Mouse X"),
                    UnityEngine.Input.GetAxisRaw("Mouse Y")
                );
                
                Target.inputMoveEvent.Invoke(inputDirection);
                Target.inputMouseDeltaEvent.Invoke(mouseDelta);
            }
        }

        public void SetTarget(InputControlTarget newTarget)
        {
            if (Target == newTarget)
                return;
                
            if (_hasTarget)
                Target.controlStopEvent.Invoke();

            Target = newTarget;
            _hasTarget = Target != null;
                
            if (_hasTarget)
                Target.controlStartEvent.Invoke();
        }
    }
}