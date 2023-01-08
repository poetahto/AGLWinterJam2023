using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class SfxVolumeApplier : MonoBehaviour
    {
        public AudioSource source;

        private void Update()
        {
            source.volume = PlayerPrefs.GetFloat("sfxVolume");
        }
    }
}