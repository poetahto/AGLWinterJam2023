using System;
using poetools.New_folder;
using UnityEngine;

namespace DefaultNamespace.ui
{
    public class UILogic : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        public void SetMouseSens(string value)
        {
            InputController.sensitivity = float.Parse(value);
        }

        public void SetFOV(string value)
        {
            Camera.main.fieldOfView = float.Parse(value);
        }

        public void SetSFXVolume(float amount)
        {
            PlayerPrefs.SetFloat("sfxVolume", amount);
        }

        private void Awake()
        {
            _planar = FindObjectOfType<PlanarReflections>();
        }

        private PlanarReflections _planar;

        public void SetQuality(int value)
        {
            switch (value)
            {
                case 0:
                    // High
                    QualitySettings.SetQualityLevel(2);
                    _planar.m_settings.m_Shadows = true;
                    _planar.m_settings.m_ResolutionMultiplier = PlanarReflections.ResolutionMulltiplier.Full;
                    _planar.gameObject.SetActive(true);
                    break;
                case 1:
                    // Mid
                    QualitySettings.SetQualityLevel(1);
                    _planar.m_settings.m_Shadows = false;
                    _planar.m_settings.m_ResolutionMultiplier = PlanarReflections.ResolutionMulltiplier.Half;
                    _planar.gameObject.SetActive(true);
                    break;
                case 2:
                    QualitySettings.SetQualityLevel(0);
                    _planar.m_settings.m_Shadows = false;
                    _planar.m_settings.m_ResolutionMultiplier = PlanarReflections.ResolutionMulltiplier.Quarter;
                    _planar.gameObject.SetActive(false);
                    // low
                    break;
                default: throw new Exception();
            }
        }

        public void SetMusicVolume(float amount)
        {
            PlayerPrefs.SetFloat("musicVolume", amount);
        }
    }
}