using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SLA
{
    [ExecuteInEditMode()]
    public class SoundFXManager : MonoBehaviour
    {
        static SoundFXManager _instance;

        [SerializeField]
        int _maxAudioCount = 100;

        [SerializeField]
        string _path = "Sounds/Fx";

        Dictionary<AudioClip, AudioSource> _audios;
        Dictionary<string, AudioClip> _cache = new Dictionary<string, AudioClip>();

        void Awake()
        {
            _instance = this;
            _audios = new Dictionary<AudioClip, AudioSource>();
        }
        
        static SoundFXManager Instance
        {
            get{
                if (_instance == null)
                {
                    var obj = new GameObject("SoundFXManager");
                    obj.AddComponent<SoundFXManager>();
                }
                return _instance;
            }
        }

        static public void Play(string filename, Vector2 pos)
        {
            if (AudioListener2D.CurrentInstance != null)
            {
                Vector2 relativePos = pos - (Vector2)AudioListener2D.CurrentInstance.transform.position;
                if (relativePos.sqrMagnitude < Mathf.Pow(AudioListener2D.CurrentInstance.range, 2f))
                {
                    Play(filename);
                }
            }
            else
            {
                Play(filename);
            }
        }

        static public void Play(string filename, bool isNeccessarily=false, bool isLoop=false)
        {
            if (Instance._cache.ContainsKey(filename))
            {
                Play(Instance._cache[filename], isNeccessarily, isLoop);
            }
            else
            {
                var clip = Resources.Load<AudioClip>(Instance._path + "/" + filename);
                Instance._cache.Add(filename, clip);

                Play(clip, isNeccessarily, isLoop);
            }
        }

        static public void Play(AudioClip clip, bool isNeccessarily=false, bool isLoop=false)
        {
            if (Instance._audios.ContainsKey(clip))
            {
                if (isNeccessarily || !Instance._audios[clip].isPlaying)
                {
                    Instance._audios[clip].loop = isLoop;
                    Instance._audios[clip].Play();
                }
            } 
            else if (Instance._audios.Count < _instance._maxAudioCount)
            {
                var src = Instance.gameObject.AddComponent<AudioSource>();
                Instance._audios[clip] = src;
                src.loop = isLoop;
                src.clip = clip;
                src.Play();
            }
        }

        static public void Stop(string filename)
        {
            if (Instance._cache.ContainsKey(filename))
            {
                Stop(Instance._cache[filename]);
            }
        }

        static public void Stop(AudioClip clip)
        {
            if (Instance._audios.ContainsKey(clip))
            {
                Instance._audios[clip].Stop();
            }
        }

        static public void Pause()
        {
            foreach (AudioSource src in Instance._audios.Values)
            {
                src.Pause();
            }
        }

        static public void Resume()
        {
            foreach (AudioSource src in Instance._audios.Values)
            {
                src.Play();
            }
        }
    }
}
