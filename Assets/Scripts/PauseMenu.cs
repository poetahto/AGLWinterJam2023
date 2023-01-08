using UnityEngine;

namespace DefaultNamespace
{
    public struct PauseStartEvent {}
    public struct PauseStopEvent {}

    public class PauseMenu : MonoBehaviour
    {
        private bool _isPaused;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isPaused)
                {
                    Services.EventBus.Invoke(new PauseStopEvent(), "Pause Menu");
                }

                else
                {
                    Services.EventBus.Invoke(new PauseStartEvent(), "Pause Menu");
                }
            }
        }
    }
}