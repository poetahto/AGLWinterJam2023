using poetools.Abstraction;
using poetools.New_folder;
using poetools.NewDirectory1;
using UnityEngine;

namespace DefaultNamespace
{
    public class QuickSaveSystem : MonoBehaviour
    {
        private Vector3 _savedPosition;
        private Vector3 _savedRotation;
        private Vector3 _savedVelocity;
        private Transform _playerTransform;
        private FPSRotationSystem _rotationSystem;
        private PhysicsComponent _physics;

        private void Awake()
        {
            _playerTransform = FindObjectOfType<InputControlTarget>().transform;
            _rotationSystem = FindObjectOfType<FPSRotationSystem>();
            _physics = FindObjectOfType<PhysicsComponent>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _savedPosition = _playerTransform.position;
                _savedRotation = _rotationSystem.GetRotation();
                _savedVelocity = _physics.Velocity;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                _playerTransform.position = _savedPosition;
                _rotationSystem.SetRotation(_savedRotation);
                _physics.Velocity = _savedVelocity;
            }
            
        }
    }
}