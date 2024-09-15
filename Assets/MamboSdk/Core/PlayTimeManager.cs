using System.Collections;
using UnityEngine;

namespace MamboSdk
{
    public class PlayTimeManager : MonoBehaviour
    {
        private const string PLAY_TIME_KEY = "play_time";
        
        public int PlayTimePlayerInSeconds = 0;
        private void Awake()
        {
            PlayTimePlayerInSeconds = PlayerPrefs.GetInt(PLAY_TIME_KEY);
            StartCoroutine(StartTimer());
        }
        private void OnApplicationPause(bool pauseStatus) => SavePlayTimePlayer();
        private void OnApplicationQuit() => SavePlayTimePlayer();

        private IEnumerator StartTimer()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(1);
                PlayTimePlayerInSeconds += 1;
            }
        }

        private void SavePlayTimePlayer()
        {
            Debug.Log($"PlayTimePlayerInSeconds {PlayTimePlayerInSeconds}");
            PlayerPrefs.SetInt(PLAY_TIME_KEY, PlayTimePlayerInSeconds);
        }
    }
}