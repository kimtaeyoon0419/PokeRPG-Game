namespace PokeRPG.Sound
{
    // # System
    using System.Collections;
    using System.Collections.Generic;
    using System.Net.NetworkInformation;

    // # Unity
    using UnityEngine;
    using UnityEngine.Animations;

    #region Music class
    [System.Serializable]
    public class Music
    {
        public string musicName;

        public AudioClip musicSource;
    }
    #endregion

    #region Sfx class
    [System.Serializable]
    public class Sfx
    {
        public string sfxName;

        public AudioClip sfxSource;
    }
    #endregion

    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;

        public List<Music> musicList;
        public List<Sfx> sfxList;

        public AudioSource music;
        public AudioSource sfx;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #region PlayMusic
        public void PlayMusic(string musicName)
        {
            foreach (var musics in musicList)
            {
                if (musics.musicName == musicName)
                {
                    music.Stop();
                    music.clip = musics.musicSource;
                    music.Play();
                    break;
                }
            }
        }
        #endregion

        #region PlaySfx
        public IEnumerator PlaySfx(string sfxName)
        {
            foreach (var sfxs in sfxList)
            {
                if (sfxs.sfxName == sfxName)
                {
                    if(sfxName == "LevelUp!")
                    {
                        music.Pause();
                        sfx.PlayOneShot(sfxs.sfxSource);
                        yield return new WaitForSeconds(sfxs.sfxSource.length);
                        music.UnPause();
                        break;
                    }
                    sfx.PlayOneShot(sfxs.sfxSource);
                    break;
                }
            }
        }
        #endregion
    }
}