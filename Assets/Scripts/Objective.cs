using UnityEngine;

namespace DefaultNamespace
{
    public class Objective : MonoBehaviour
    {
        [SerializeField] private Color color;

        private void Awake()
        {
            foreach (var particles in GetComponentsInChildren<ParticleSystem>())
            {
                var main = particles.main;
                main.startColor = color;
            }

            foreach (var lights in GetComponentsInChildren<Light>())
                lights.color = color;
        }
    }
}