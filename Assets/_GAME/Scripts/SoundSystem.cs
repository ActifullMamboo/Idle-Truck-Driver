using System;
using System.Collections.Generic;
using _Game.Scripts.Tools;
using _Game.Scripts.Ui.Base;
using UnityEngine;
using UnityEngine.Rendering;

namespace _Game.Scripts.Systems
{
    public enum GameSoundType
    {
        None,
        MoneyDraw,
        ButtonClick,
        Window,
        MoneyPick,
        ItemEquip,
        PickUp,
        Upgrade,
        Steps,
        CarSignal,
        Saw,
        TreeHit,
        Conveyor,
        Buy,
        DogWalk,
        DogLaugh,
        BaranBee,
        HappySeller
    }
    
    [Serializable]
    public class GameSound
    {
        public GameSoundType Type;
        public AudioClip[] Clip;
        [Range(0,1)]
        public float Volume;
    }
    
    public class SoundSystem : MonoBehaviour
    {
        [SerializeField] private List<GameSound> _sounds;
        [SerializeField] private AudioSource _sourceMusic;
        [SerializeField] private AudioSource _sourceSound;

        [SerializeField] private List<AudioSource> _sources;
        private void Init()
        {
           // BaseButton.ClickSoundEvent += _ => PlaySound((GameSoundType)_);
          //  BaseWindow.SoundEvent += _ => PlaySound((GameSoundType)_);
        }

        private void UpdateSoundsSettings()
        {
             //_sourceMusic.mute = _settings.IsMuteMusic;
             //_sourceSound.mute = _settings.IsMuteSound;
        }



        public void PlaySound(GameSoundType gameSoundType, Transform target)
        {
            var srs = _sources.Find(x => !x.isPlaying);
            var clip = _sounds.Find(s => s.Type == gameSoundType);
           
            if (clip != null && srs!=null)
            {
                srs.loop = false;
                srs.clip = null;
                srs.pitch = 1;
                srs.transform.position = target.position;
                srs.volume = clip.Volume;
                srs.PlayOneShot(clip.Clip.RandomValue());
            }
        }

        public void PlayLoop(GameSoundType gameSoundType, Transform target)
        {
            var srs = _sources.Find(x => !x.isPlaying);
            var clip = _sounds.Find(s => s.Type == gameSoundType);
            if (clip != null && srs!=null)
            {
                srs.loop = true;
                srs.pitch = 1;
                srs.transform.position = target.position;
                srs.volume = clip.Volume;
                srs.clip = clip.Clip.RandomValue();
                srs.Play();
            }
        }

        public void PlayInPitch(float count, Transform target)
        {
            var srs = _sources.Find(x => !x.isPlaying);
            var clip = _sounds.Find(s => s.Type == GameSoundType.PickUp);
            if (clip != null && srs!=null)
            {
                srs.pitch = 1;
                srs.pitch +=count*0.1f;
                srs.transform.position = target.position;
                srs.volume = clip.Volume;
                srs.PlayOneShot(clip.Clip.RandomValue());
            }
        }
        public void PlayInPitch(GameSoundType gameSoundType,float count, Transform target)
        {
            var srs = _sources.Find(x => !x.isPlaying);
            var clip = _sounds.Find(s => s.Type == gameSoundType);
            if (clip != null && srs!=null)
            {
                srs.pitch = 1;
                srs.pitch +=count*0.1f;
                srs.transform.position = target.position;
                srs.volume = clip.Volume;
                srs.PlayOneShot(clip.Clip.RandomValue());
            }
        }
    }

    public interface ISoundPlayer
    {
        public void InitSound(SoundSystem soundSystem);
    }
}