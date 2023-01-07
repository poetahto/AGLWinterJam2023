using UnityEngine;

namespace BeastWhisperer.Prototype.Platformer
{
    [CreateAssetMenu]
    public class MovementSettings : ScriptableObject
    {
        public float speed;
        public float acceleration;
    }
}