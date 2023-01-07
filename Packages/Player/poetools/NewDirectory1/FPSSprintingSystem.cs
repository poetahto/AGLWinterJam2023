using System;
using UnityEngine;

namespace poetools.NewDirectory1
{
    public class FPSSprintingSystem : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private FPSQuakeMovementSystem mvmnt;
        [SerializeField] private float multiplier = 1.5f;
        [SerializeField] private float fovMult = 1.1f;
        [SerializeField] private float fovTime = 15f;
        
        public bool IsSprinting { get; set; }
        private float _normalSpeed;
        private float _sprintSpeed;
        private float _normalFOV;
        private float _sprintFOV;

        private void Awake()
        {
            _normalSpeed = mvmnt.maxGroundSpeed;
            _sprintSpeed = mvmnt.maxGroundSpeed * multiplier;
            _normalFOV = camera.fieldOfView;
            _sprintFOV = _normalFOV * fovMult;
        }

        private void Update()
        {
            if (IsSprinting)
            {
                mvmnt.maxGroundSpeed = _sprintSpeed;
                mvmnt.trueMax = _sprintSpeed;
                camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, _sprintFOV, fovTime * Time.deltaTime);
            }
            else
            {
                mvmnt.maxGroundSpeed = _normalSpeed;
                mvmnt.trueMax = _normalSpeed;
                camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, _normalFOV, fovTime * Time.deltaTime);
            }
        }
    }
}