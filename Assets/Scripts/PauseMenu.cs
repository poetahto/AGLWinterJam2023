using poetools.New_folder;
using UnityEngine;

namespace DefaultNamespace
{
    public struct PauseStartEvent {}
    public struct PauseStopEvent {}

    public class PauseMenu : MonoBehaviour
    {
        public GameObject ui;
        private bool _isPaused;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isPaused)
                {
                    FindObjectOfType<InputController>().SetTarget(FindObjectOfType<InputControlTarget>());
                    ui.SetActive(false);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Confined;
                }
                else
                {
                    FindObjectOfType<InputController>().SetTarget(null);
                    ui.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }

                _isPaused = !_isPaused;
            }
        }
    }
}