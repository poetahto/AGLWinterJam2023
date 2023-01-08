using poetools.New_folder;
using UnityEngine;

namespace DefaultNamespace.ui
{
    public class UILogic : MonoBehaviour
    {
        public void SetMouseSens(string value)
        {
            InputController.sensitivity = float.Parse(value);
        }
    }
}