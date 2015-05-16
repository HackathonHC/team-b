using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SLA
{
    [RequireComponent(typeof(AudioSource))]
    [ExecuteInEditMode()]
    public class MusicManager : MonoBehaviour
    {
        const string VolumeKey = "Audio.Volume";
        static MusicManager _instance;

        [SerializeField] string path = "Sounds/Music";
        private float _originalValume;
        private bool _isFadeOut = false;

        AudioSource _audioSource;
        AudioSource AudioSource
        {
            get
            {
                return _audioSource ?? (_audioSource = GetComponent<AudioSource>());
            }
        }

        void Awake()
        {
            _instance = this;
            AudioListener.volume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        }

        void ProcessFadeOut()
        {
            _originalValume = AudioListener.volume;
            _isFadeOut = true;
        }

        void ResetFadeOut()
        {
            if (_isFadeOut)
            {
                AudioListener.volume = _originalValume;
                _isFadeOut = false;
            }
        }

        void Update()
        {
            if (_isFadeOut)
            {
                AudioListener.volume -= 0.01f;
                if (AudioListener.volume < 0)
                {
                    AudioSource.Stop();
                    AudioListener.volume = _originalValume;
                    _isFadeOut = false;
                }
            }
        }

        static MusicManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = new GameObject("MusicManager");
                    obj.AddComponent<MusicManager>();
                }
                return _instance;
            }
        }

        static public void Play(string name, bool loop=true)
        {
            AudioClip clip = Resources.Load<AudioClip>(Instance.path + "/" + name);
            if (clip == null)
            {
                Debug.LogError("not found audio clip \"" + name + "\"");
                return;
            }
            Play(clip, loop);
        }

        static public void Play(AudioClip clip, bool loop=true)
        {
            if (Instance.AudioSource.clip != null)
            {
                if (!Instance.AudioSource.clip.name.Equals(clip.name))
                {
                    Stop();
                }
            }
            Instance.ResetFadeOut();
            if (!Instance.AudioSource.isPlaying)
            {
                Instance.AudioSource.clip = clip;
                Instance.AudioSource.Play();
                Instance.AudioSource.loop = loop;
            }
        }

        static public void Stop()
        {
            if (Instance.AudioSource != null)
            {
                Instance.AudioSource.Stop();
            }
        }

        static public bool IsPlaying()
        {
            if (Instance.AudioSource != null)
            {
                return Instance.AudioSource.isPlaying;
            }
            else
            {
                return false;
            }
        }

        static public void Pause()
        {
            if (Instance.GetComponent<AudioSource>() != null)
            {
                Instance.GetComponent<AudioSource>().Pause();
            }
        }

        static public void Resume()
        {
            if (Instance.AudioSource != null)
            {
                Instance.AudioSource.Play();
            }
        }

        static public void FadeOut()
        {
            if (Instance != null)
            {
                Instance.ProcessFadeOut();
            }
        }
    }
}
