using System;
using poetools.Abstraction;
using UnityEngine;

namespace poetools.Player
{
    [Serializable]
    public class FPSRotationSystem
    {
        public float sensitivity = 1;
        public bool invertY;

        public Vector2 MouseDelta { get; set; }
        public IRotationComponent PitchRotation { get; set; }
        public IRotationComponent YawRotation { get; set; }
    
        public void UpdateRotation()
        {
            float yawAngle = MouseDelta.x * sensitivity;
            float pitchAngle = MouseDelta.y * sensitivity * (invertY ? 1 : -1);
        
            PitchRotation.Rotation *= Quaternion.Euler(pitchAngle, 0, 0);
            YawRotation.Rotation *= Quaternion.Euler(0, yawAngle, 0);
        }
    }
}