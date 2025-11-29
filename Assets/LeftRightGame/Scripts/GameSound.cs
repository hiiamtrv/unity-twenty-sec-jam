using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Playables;

namespace LeftRightGame
{
    public class GameSound : MonoBehaviour
    {
        [SerializeField] private AudioSource whistle;
        [SerializeField] private PlayableDirector bgmDirector;

        public Action OnFixedPopDance;

        public void StartBgm()
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                bgmDirector.time = -0.4f;
            }
            else
            {
                bgmDirector.time = -0.1f;
            }
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