using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
    
        public AudioSource sfxPrefab;
        public int poolSize = 10;     
    
        private Queue<AudioSource> pool = new Queue<AudioSource>();
    
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitPool();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    
        private void InitPool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                CreateAudioSource();
            }
        }
    
        private AudioSource CreateAudioSource()
        {
            AudioSource newSource = Instantiate(sfxPrefab, transform);
            newSource.playOnAwake = false;
            newSource.gameObject.SetActive(false);
            pool.Enqueue(newSource);
            return newSource;
        }
    
        private AudioSource GetAudioSource()
        {
            if (pool.Count == 0)
            {
                CreateAudioSource();
            }
    
            AudioSource src = pool.Dequeue();
            src.gameObject.SetActive(true);
            return src;
        }
    
        private void ReturnToPool(AudioSource src)
        {
            src.Stop();
            src.gameObject.SetActive(false);
            pool.Enqueue(src);
        }
    
        public void PlaySfx(AudioClip clip, float volume = 1f, float pitch = 1f)
        {
            if (clip == null)
            {
                return;
            }
    
            AudioSource src = GetAudioSource();
            src.clip = clip;
            src.volume = volume;
            src.pitch = pitch;
            src.Play();
    
            StartCoroutine(ReturnToPoolAfterPlayCoroutine(src, clip.length / pitch));
        }
    
        private IEnumerator ReturnToPoolAfterPlayCoroutine(AudioSource src, float delay)
        {
            yield return new WaitForSeconds(delay);
            ReturnToPool(src);
        }
    }
}


