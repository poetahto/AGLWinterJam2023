using UnityEngine;

namespace poetools.Player
{
    public interface IInteractable
    {
        void HandleInteract(GameObject grabber);
    }
}