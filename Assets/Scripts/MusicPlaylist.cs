using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class MusicPlaylist : MonoBehaviour
    {
        public KeyCode skipKey = KeyCode.K;
        public List<AudioClip> songs;
        public AudioSource source;

        private IEnumerator Start()
        {
            int currentSong = 0;
            source.loop = false;

            for (int i = 0; i < songs.Count; i++)
            {
                var randomOther = Random.Range(0, songs.Count);
                (songs[i], songs[randomOther]) = (songs[randomOther], songs[i]);
            }
            
            while (true)
            {
                var s = songs[currentSong];
                source.clip = s;
                source.Play();

                yield return new WaitUntil(() => source.isPlaying == false || Input.GetKeyDown(skipKey));
                currentSong = (currentSong + 1) % songs.Count;
                yield return null;
            }
        }

        private void Update()
        {
            source.volume = PlayerPrefs.GetFloat("musicVolume");
        }
    }
}