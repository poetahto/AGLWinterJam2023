using UnityEngine;

namespace poetools.Abstraction
{
    public abstract class CameraComponent : MonoBehaviour
    {
        public abstract float Fov { get; set; }
    }
}