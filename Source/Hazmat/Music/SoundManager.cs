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
        public SoundEffect BossIntro01 { get; private set; }
        public SoundEffect BossEnergy02 { get; private set; }
        public SoundEffect BossGear03 { get; private set; }
        public SoundEffect BossEnemy04 { get; private set; }
        public SoundEffect BossBattery05 { get; private set; }
        public SoundEffect BossLootBox06 { get; private set; }
        public SoundEffect BossStreet07A { get; private set; }
        public SoundEffect BossStreet07B { get; private set; }
        public SoundEffect BossPlant08 { get; private set; }
        public SoundEffect BossResolution09 { get; private set; }
        public SoundEffect MatDying { get; private set; }
        public SoundEffect MatHit { get; private set; }
        public SoundEffect MatOk { get; private set; }
        public SoundEffect MatFirstTutorial { get; private set; }
        public SoundEffect MatWin { get; private set; }
        public SoundEffect Ringtone { get; private set; }
        // List of Songs
        public Song InGameSong { get; private set; }
        public Song MenuSong { get; private set; }


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

        /// <summary>
        /// Play a sound effect specifing the change in volume with respect to the global volume value (can be also negative).
        /// Volume [0,1].
        /// Pan (-1,0,1): Left, Both or Right speaker.
        /// Pitch (+ increases octave, - decreases octave).
        /// </summary>
        /// <param name="changeInGlobalVolume"></param>
        public SoundEffectInstance PlaySoundEffectInstance(SoundEffect effect, float? changeVolume = null, float? changePan = null, float? changePitch = null, bool loop = false)
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

            return soundEffectInstance;
        }

        public void StopSoundEffectInstance(SoundEffectInstance sfxInstance, bool immediate = true)
        {
            sfxInstance.Stop(immediate);
        }

        public void PlayBackgroundMusic(Song song, float? changeVolume = null, bool loop = false)
        {
            MediaPlayer.Stop();
            float v = changeVolume == null ? Volume : Volume + changeVolume.Value;
            MediaPlayer.IsRepeating = loop;
            MediaPlayer.Volume = v;
            MediaPlayer.Play(song);
        }


        public void PreLoadSoundEffectsAndSongs()
        {
            // Songs
            this.InGameSong = Content.Load<Song>(@"music\Hazmat_Ingame");
            this.MenuSong = Content.Load<Song>(@"music\Hazmat_MainMenu");

            // Sound Effects
            this.Shooting_Sfx = Content.Load<SoundEffect>(@"music\Sci-Fi-Gun_Sfx");
            this.BossIntro01 = Content.Load<SoundEffect>(@"sounds\Boss_01intro");
            this.MatDying = Content.Load<SoundEffect>(@"sounds\Mat_dying");
            this.MatHit = Content.Load<SoundEffect>(@"sounds\Mat_getHit");
            this.MatOk = Content.Load<SoundEffect>(@"sounds\Mat_ok");
            this.MatFirstTutorial = Content.Load<SoundEffect>(@"sounds\Mat_pickFirstCall");
            this.MatWin = Content.Load<SoundEffect>(@"sounds\Mat_win");
            this.BossEnergy02 = Content.Load<SoundEffect>(@"sounds\Boss_02energyLevels");
            this.BossGear03 = Content.Load<SoundEffect>(@"sounds\Boss_03gearUp");
            this.BossEnemy04 = Content.Load<SoundEffect>(@"sounds\Boss_04enemyEncounter");
            this.BossBattery05 = Content.Load<SoundEffect>(@"sounds\Boss_05batteryPowerup");
            this.BossLootBox06 = Content.Load<SoundEffect>(@"sounds\Boss_06lootBox");
            this.BossStreet07A = Content.Load<SoundEffect>(@"sounds\Boss_07streetA");
            this.BossStreet07B = Content.Load<SoundEffect>(@"sounds\Boss_07streetB");
            this.BossPlant08 = Content.Load<SoundEffect>(@"sounds\Boss_08reachPlant");
            this.BossResolution09 = Content.Load<SoundEffect>(@"sounds\Boss_09resolution");
            this.Ringtone = Content.Load<SoundEffect>(@"sounds\Phone_Ringtone");
        }

    }
}
