using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enut4LJR
{
    public class SoundManager : SingletonManager<SoundManager>
    {
        [HideInInspector] public AudioSource audioSource = null;
        [SerializeField] private List<AudioClip> checkClip = new List<AudioClip>();
        private Dictionary<string, AudioClip> clipList = new Dictionary<string, AudioClip>();

        #region 효과음 최적화 변수
        [SerializeField] private int effectSoundCount = 20;
        private int thisSoundCount = 0; //최대 5개까지 재생되에 제어(안드로이드 5개, PC는 무제한)

        //조건 아래 배열들은 effectSoundCount보다 커야 한다.
        private List<GameObject> soundObjList = new List<GameObject>();
        private List<AudioSource> soundSourceList = new List<AudioSource>();

        //각 차일드 오브젝트에 붙을 AudioSource Component 참조를 저장하기 위한 리스트
        private float[] effectSound = new float[30];

        [HideInInspector] public float bgmVolume = .2f;
        [HideInInspector] public bool soundOnOff = true;
        [HideInInspector] public float soundVolume = 1.0f;
        #endregion

        private void LoadChildObj()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
#if UNITY_ANDROID
        effectSoundCount = 5;
#elif UNITY_STANDALONE || UNITY_EDITOR
            //effectSoundCount = 50;
#endif

            for (int i = 0; i < effectSoundCount; i++)
            {
                //안드로이드의 경우 최대 5개까지 재생되게 제어하여 랙 방지
                var soundObj = new GameObject();
                soundObj.transform.SetParent(transform);
                soundObj.transform.localPosition = Vector3.zero;
                var soundSource = soundObj.AddComponent<AudioSource>();
                soundSource.playOnAwake = false;
                soundSource.loop = false;
                soundSource.name = "SoundEffectObject";

                soundObjList.Add(soundObj);
                soundSourceList.Add(soundSource);

            }
        }

        protected override void Init()
        {
            base.Init();

            LoadChildObj();
        }

        public void PlayBGM(string fileName, float volume = 0.2f)
        {
            AudioClip clip = AudioClipCheck(fileName);

            if (!audioSource) return;
            if (audioSource.clip == clip) return;

            audioSource.clip = clip;
            audioSource.volume = volume * bgmVolume;
            bgmVolume = volume;
            audioSource.loop = true;
            audioSource.Play();
        }

        public void PlayerSound(string fileName, float vol = 1.0f)
        {
            if (!soundOnOff) return;

            AudioClip clip = AudioClipCheck(fileName);
            float volume;
            if (GlobalValue.effectMute || GlobalValue.masterMute) 
                volume = 0.0f;
            else
                volume = vol * GlobalValue.masterVolume * GlobalValue.effectVolume;

            int idx = GetFreeSoundIndex();

            var src = soundSourceList[idx];
            src.transform.position = transform.position;
            src.spatialBlend = 0.0f;
            src.clip = clip;
            src.volume = volume;
            src.loop = false;
            src.Play();

            //if (clip && soundSourceList[thisSoundCount])
            //{
            //    soundSourceList[thisSoundCount].transform.position = transform.position;
            //    soundSourceList[thisSoundCount].spatialBlend = 0.0f;
            //    soundSourceList[thisSoundCount].clip = clip;
            //    soundSourceList[thisSoundCount].volume = volume;
            //    soundSourceList[thisSoundCount].loop = false;
            //    soundSourceList[thisSoundCount].Play();
            //    effectSound[thisSoundCount] = volume;
            //
            //    thisSoundCount++;
            //    if (thisSoundCount >= effectSoundCount) thisSoundCount = 0;
            //}
        }

        public int PlaySoundIdx(string fileName)
        {
            if (!soundOnOff) return -1;

            int result = 0;
            AudioClip clip = AudioClipCheck(fileName);
            float volume = GlobalValue.masterVolume * GlobalValue.effectVolume;

            if (clip && soundSourceList[thisSoundCount])
            {
                soundSourceList[thisSoundCount].transform.position = transform.position;
                soundSourceList[thisSoundCount].spatialBlend = 0.0f;
                soundSourceList[thisSoundCount].clip = clip;
                soundSourceList[thisSoundCount].volume = volume;
                soundSourceList[thisSoundCount].loop = false;
                soundSourceList[thisSoundCount].Play();
                effectSound[thisSoundCount] = volume;

                result = thisSoundCount;

                thisSoundCount++;
                if (thisSoundCount >= effectSoundCount)
                    thisSoundCount = 0;

                return result;
            }

            return -1;
        }

        public int PlayLoopSoundIdx(string fileName)
        {
            if (!soundOnOff) return -1;

            int result = 0;
            AudioClip clip = AudioClipCheck(fileName);
            float volume = GlobalValue.masterVolume * GlobalValue.effectVolume;

            if (clip && soundSourceList[thisSoundCount])
            {
                soundSourceList[thisSoundCount].transform.position = transform.position;
                soundSourceList[thisSoundCount].spatialBlend = 0.0f;
                soundSourceList[thisSoundCount].clip = clip;
                soundSourceList[thisSoundCount].volume = volume;
                soundSourceList[thisSoundCount].loop = true;
                soundSourceList[thisSoundCount].Play();
                effectSound[thisSoundCount] = volume;

                result = thisSoundCount;

                thisSoundCount++;
                if (thisSoundCount >= effectSoundCount)
                    thisSoundCount = 0;

                return result;
            }

            return -1;
        }

        public void StopFileSound(string fileName)
        {
            if (!soundOnOff) return;

            for (int i = 0; i < soundSourceList.Count; i++)
            {
                AudioSource src = soundSourceList[i];

                if (!src.isPlaying) continue;
                if (src.clip == null) continue;

                if (src.clip.name == fileName)
                {
                    src.Stop();
                    src.loop = false;
                }
            }
        }

        public void StopSoundIdx(int idx)
        {
            if (!soundOnOff) return;

            if (soundSourceList[idx] && soundSourceList[idx].isPlaying)
            {
                soundSourceList[idx].Stop();
                soundSourceList[idx].loop = false;
            }
        }

        public void PauseAllSound()
        {
            if (!soundOnOff) return;

            for (int i = 0; i < soundSourceList.Count; i++)
                if (soundSourceList[i].isPlaying) soundSourceList[i].Pause();
        }

        public void ResumeAllSound()
        {
            if (!soundOnOff) return;

            for (int i = 0; i < soundSourceList.Count; i++)
                if (soundSourceList[i].clip != null) soundSourceList[i].UnPause();
        }

        /*
        public void PlaySound3D(string fileName, Transform tr)
        {
            if (!soundOnOff) return;

            AudioClip clip = AudioClipCheck(fileName);
            float volume = GlobalValue.masterVolume * GlobalValue.effectVolume;
            if (!clip) Debug.Log($"{fileName}라는 클립없음");
            //else
            //Debug.Log($"마스터볼륨: {100 * GlobalValue.masterVolume}, 효과음볼륨: {GlobalValue.effectVolume * 100}");

            if (clip && soundSourceList[thisSoundCount])
            {
                soundSourceList[thisSoundCount].transform.position = tr.position;
                soundSourceList[thisSoundCount].transform.rotation = tr.rotation;
                soundSourceList[thisSoundCount].spatialBlend = 1.0f;
                soundSourceList[thisSoundCount].clip = clip;
                soundSourceList[thisSoundCount].volume = volume;
                soundSourceList[thisSoundCount].loop = false;
                soundSourceList[thisSoundCount].Play();
                effectSound[thisSoundCount] = volume;

                thisSoundCount++;
                if (thisSoundCount >= effectSoundCount) thisSoundCount = 0;
            }
        }
        */

        public void PlayGUISound(string fileName, float volume = .2f)
        {
            if (!soundOnOff) return;

            AudioClip clip = AudioClipCheck(fileName);

            if (!audioSource) return;

            audioSource.PlayOneShot(clip, volume * soundVolume);
        }

        private AudioClip AudioClipCheck(string fileName)
        {
            AudioClip clip = null;
            if (clipList.ContainsKey(fileName))
                clip = clipList[fileName] as AudioClip;
            else
            {
                clip = Resources.Load("Sounds/" + fileName) as AudioClip;
                clipList.Add(fileName, clip);
            }
            return clip;
        }

        private void Start() => StartFunc();

        private void StartFunc()
        {
            //사운드 미리 로드
            AudioClip clip = null;
            object[] temp = Resources.LoadAll("Sounds");
            Debug.Log($"{temp} Loaded");
            for (int i = 0; i < temp.Length; i++)
            {
                clip = temp[i] as AudioClip;

                if (clipList.ContainsKey(clip.name))
                    continue;

                clipList.Add(clip.name, clip);
                checkClip.Add(clip);
            }
        }

        internal AudioClip FindAudioClip(string name)
        {
            string key = "";

            foreach (KeyValuePair<string, AudioClip> items in clipList)
            {
                if (items.Key.Contains(name))
                {
                    key = items.Key;
                    break;
                }
            }

            return clipList[key];
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
            checkClip.Clear();
            clipList.Clear();
            soundObjList.Clear();
            soundSourceList.Clear();
            Resources.UnloadUnusedAssets();
        }

        public void PlayBGMAfterSound(string soundClipName, string bgmClipName, float bgmVolume = 0.2f)
        {
            StartCoroutine(PlaySequenceCoroutine(soundClipName, bgmClipName, bgmVolume));
        }


        private IEnumerator PlaySequenceCoroutine(string prevClipName, string nextClipName, float bgmVolume)
        {
            int prevIdx = PlaySoundIdx(prevClipName);
            if (prevIdx >= 0)
                yield return new WaitWhile(() => soundSourceList[prevIdx].isPlaying);

            MusicManager.instance.PlayMusic(nextClipName);
            //PlayBGM(nextClipName, bgmVolume);
        }

        public IEnumerator PlaySoundInAdvance()
        {
            foreach (var clip in checkClip)
            {
                audioSource.PlayOneShot(clip, 0.0f);

                yield return null;
            }

            audioSource.Stop();
        }

        private int GetFreeSoundIndex()
        {
            for (int i = 0; i < effectSoundCount; i++)
            {
                if (!soundSourceList[i].isPlaying)
                    return i;
            }

            int idx = thisSoundCount;
            thisSoundCount = (thisSoundCount + 1) % effectSoundCount;
            return idx;
        }

    }
}
