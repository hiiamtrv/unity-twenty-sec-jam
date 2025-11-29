using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace LeftRightGame
{
    public class GameSound : MonoBehaviour
    {
        [SerializeField] private AudioSource whistle;
        [SerializeField] private PlayableDirector bgmDirector;

        public Action OnFixedPopDance;

        public void StartBgm()
        {
            bgmDirector.Play();
        }

        public void StopBgm()
        {
            bgmDirector.Stop();
        }

        public void PlayWhistle()
        {
            whistle.Play();
        }

        public void PopDanceCommand()
        {
            OnFixedPopDance?.Invoke();
        }
    }
}