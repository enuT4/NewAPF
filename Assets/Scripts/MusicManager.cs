using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enut4LJR
{
    public class MusicManager : SingletonManager<MusicManager>
    {
        internal AudioSource audioSource = null;
        private Dictionary<string, AudioClip> clipList = new Dictionary<string, AudioClip>();
        float pauseTime;

        protected override void Init()
        {
            base.Init();
            if (!audioSource)
                audioSource = gameObject.AddComponent<AudioSource>();

            AudioClip clip = null;
            object[] temp = Resources.LoadAll("Musics/");
            for (int i = 0; i < temp.Length; i++)
            {
                clip = temp[i] as AudioClip;

                if (clipList.ContainsKey(clip.name))
                    continue;

                clipList.Add(clip.name, clip);
            }
        }

        internal void PlayMusic(string fileName)
        {
            AudioClip clip = AudioClipCheck(fileName);

            if (!clip) return;
            if (!audioSource)
                audioSource = gameObject.AddComponent<AudioSource>();
            if (audioSource.clip == clip) return;
            float volume;
            if (GlobalValue.masterMute || GlobalValue.musicMute)
                volume = .0f;
            else
                volume = .3f * (GlobalValue.musicVolume * GlobalValue.masterVolume);

            audioSource.Stop();
            audioSource.clip = clip;
            //audioSource.volume = volume * bgmVolume;
            //bgmVolume = volume;
            audioSource.volume = volume;
            if (!audioSource.loop) audioSource.loop = true;
            audioSource.Play();
        }

        internal void StopMusic()
        {
            audioSource.Stop();
            audioSource.clip = null;
        }

        internal void PauseResumeMusic(bool isPause)
        {
            if (isPause)
            {
                audioSource.Pause();
                pauseTime = audioSource.time;
            }
            else
            {
                audioSource.Play();
                audioSource.time = pauseTime;
            }
        }

        private AudioClip AudioClipCheck(string fileName)
        {
            AudioClip clip = null;
            if (clipList.ContainsKey(fileName))
                clip = clipList[fileName];
            else
            {
                clip = Resources.Load("Musics/" + fileName) as AudioClip;
                if (clip)
                    clipList.Add(fileName, clip);
            }
            return clip;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            MemoryClear();
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            MemoryClear();
        }

        private void MemoryClear()
        {
            clipList.Clear();
            Resources.UnloadUnusedAssets();
        }
    }
}
