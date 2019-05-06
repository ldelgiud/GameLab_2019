using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Hazmat;

namespace Hazmat.Music
{
    public class SoundManager
    {
        private ContentManager Content;
        private float Volume;
        private float Pan;
        private float Pitch;

        // List of SoundEffects
        public SoundEffect Shooting_Sfx { get; private set; }


        // List of Songs
        public Song InGameSong { get; private set; }

        public SoundManager(ContentManager Content, float? Volume = null, float? Pan = null, float? Pitch = null)
        {
            this.Content = Content;
            this.Volume = Volume == null ? 1 : Volume.Value;
            this.Pan = Pan == null ? 0 : Pan.Value;
            this.Pitch = Pitch == null ? 0 : Pitch.Value;
            
            PreLoadSoundEffectsAndSongs();
        }


        /// <summary>
        /// Play a sound effect specifing the change in volume with respect to the global volume value (can be also negative).
        /// Volume [0,1].
        /// Pan (-1,0,1): Left, Both or Right speaker.
        /// Pitch (+ increases octave, - decreases octave).
        /// </summary>
        /// <param name="changeInGlobalVolume"></param>
        public void PlaySoundEffect(SoundEffect effect, float? changeVolume = null, float? changePan = null, float? changePitch = null, bool loop = false)
        {
            float v = changeVolume == null ? Volume : Volume + changeVolume.Value;
            float pa = changePan == null ? Pan : Pan + changePan.Value;
            float pi = changePitch == null ? Pan : Pan + changePitch.Value;

            SoundEffectInstance soundEffectInstance = effect.CreateInstance();
            soundEffectInstance.Volume = v;
            soundEffectInstance.Pan = pa;
            soundEffectInstance.Pitch = pi;
            soundEffectInstance.IsLooped = loop;
            soundEffectInstance.Play();
        }


        public void PlayBackgroundMusic(Song song, float? changeVolume = null, bool loop = false)
        {
            float v = changeVolume == null ? Volume : Volume + changeVolume.Value;
            MediaPlayer.IsRepeating = loop;
            MediaPlayer.Volume = v;
            MediaPlayer.Play(song);
        }


        public void PreLoadSoundEffectsAndSongs()
        {
            // Songs
            InGameSong = Content.Load<Song>(@"music\inGameSong");

            // Sound Effects
            Shooting_Sfx = Content.Load<SoundEffect>(@"music\Sci-Fi-Gun_Sfx");
        }

    }
}
