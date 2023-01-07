using System;
using poetools.Tools;
using UnityEngine;

namespace poetools.Player
{
    [Serializable]
    public class FPSInteractionSystem
    {
        public float range = 4f;

        public Ray ViewRay { get; set; }
        
        private bool _hasFacingObject;
        private IInteractable _facingObject;

        public void UpdateInteraction()
        {
            if (RaycastTools.Raycast3D(ViewRay, out var hit, range, ~LayerMask.GetMask(), QueryTriggerInteraction.Collide, 0.0f))
                _hasFacingObject = hit.transform.TryGetComponent(out _facingObject);
        }
    
        public void Interact(GameObject sender)
        {
            if (_hasFacingObject)
                _facingObject.HandleInteract(sender);
        }
    }
}