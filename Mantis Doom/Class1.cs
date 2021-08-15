using System.IO;
using System.Reflection;
using Modding;
using UnityEngine;

namespace Mantis_Doom
{
    public class Mantis_Doom : Mod, ITogglableMod
    {
        public Mantis_Doom() : base("Mantis Lord Doom Music") { }
        public override string GetVersion() => "Scarab Beetle";

        public string scene;
        public AudioClip[] SongList = new AudioClip[2];


        public override void Initialize()
        {
            if (this.SongList[0] is null)
            {
                LoadAudio();
            }
            On.AudioManager.ApplyMusicCue += ChangeMusic;
            Log("Loaded");
        }

        private void ChangeMusic(On.AudioManager.orig_ApplyMusicCue orig, AudioManager self, MusicCue musicCue, float delayTime, float transitionTime, bool applySnapshot)
        {
            this.scene = GameManager.instance.GetSceneNameString();
            if (this.scene == "Fungus2_15" || this.scene == "GG_Mantis_Lords" || this.scene == "GG_Mantis_Lords_V")
            {
                Log("Mantis Lords Room Found");
                global::MusicCue.MusicChannelInfo[] attr = ReflectionHelper.GetAttr<global::MusicCue, global::MusicCue.MusicChannelInfo[]>(musicCue, "channelInfos");
                ReflectionHelper.SetAttr<global::MusicCue.MusicChannelInfo, AudioClip>(attr[0], "clip", this.SongList[0]);
                Log("Music Changed");
            }
            else
            {
                ReflectionHelper.GetAttr<global::MusicCue, global::MusicCue.MusicChannelInfo[]>(musicCue, "channelInfos");
            }
            orig(self, musicCue, delayTime, transitionTime, applySnapshot);
        }

        private void LoadAudio()
        {
            foreach (string text in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (text.EndsWith(".wav"))
                {
                    Log("Found Files");
                    using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(text))
                    {
                        byte[] array = new byte[manifestResourceStream.Length];
                        manifestResourceStream.Read(array, 0, array.Length);
                        this.SongList[0] = WavUtility.ToAudioClip(array, 0, "wav");
                    }
                    Log("Music Loaded");
                }
            }
        }

        public void Unload()
        {
            On.AudioManager.ApplyMusicCue -= ChangeMusic;
            Log("Unloaded");
        }
    }

}
