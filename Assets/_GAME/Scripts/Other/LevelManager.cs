using _GAME.Scripts.Components;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _GAME.Scripts.Other
{
    public class LevelManager : MonoBehaviour, ILevelStartComponents
    {
        public static int NumberOfLevel;
        public static int PassedLevel;

        private float _fixedDeltaTime;

        public void InitStartLevel()
        {
            NumberOfLevel = PlayerPrefs.GetInt("Level", 0);
            PassedLevel = PlayerPrefs.GetInt("LevelPassed", 1);
        }

        public void Initialize()
        {
            Time.timeScale = 1;
            Application.targetFrameRate = 60;
            NumberOfLevel = PlayerPrefs.GetInt("Level", 0);
            PassedLevel = PlayerPrefs.GetInt("LevelPassed", 1);

            if (SceneManager.GetActiveScene().buildIndex > 0) //AppMetricaEvents.SendLevelStart(PassedLevel, "WORLD " + PassedLevel.ToString(), 1);
            _fixedDeltaTime = Time.fixedDeltaTime;

        }

        public static void LoadStartScene()
        {
            PassedLevel = PlayerPrefs.GetInt("LevelPassed", 1);
            LoadScene(PassedLevel);

        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartLevel();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                PlayerPrefs.SetInt("Weapon",1);
            }
            if (Input.GetKeyDown(KeyCode.F12))
            {
                if (Time.timeScale == 1.0f)
                    Time.timeScale = 0.3f;
                else
                    Time.timeScale = 1.0f;
           
                Time.fixedDeltaTime = _fixedDeltaTime * Time.timeScale;
            }
            if (Input.GetKeyDown(KeyCode.F11))
            {
                if (Time.timeScale == 1.0f)
                    Time.timeScale = 3f;
                else
                    Time.timeScale = 1.0f;
           
                Time.fixedDeltaTime = _fixedDeltaTime * Time.timeScale;
            }
        }

        public static void Win()
        {
            Next();
        }

        private static void Next()
        {
           
        }

        private static int RandomExcept(int fromNr, int exclusiveToNr, int exceptNr)
        {
            var randomNr = exceptNr;

            while (randomNr == exceptNr) randomNr = Random.Range(fromNr, exclusiveToNr);

            return randomNr;
        }

        public static void LevelComplete()
        {
            EventsAnalitycs.GameEvent(TypeOfEvent.Complete, PassedLevel);
        }

        public static void RestartLevel()
        {
            EventsAnalitycs.GameEvent(TypeOfEvent.Restart, PassedLevel);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public static void Reload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public static void Failed()
        {
            EventsAnalitycs.GameEvent(TypeOfEvent.Fail, PassedLevel);
        }

        public static void LoadScene(int v)
        {
            SceneManager.LoadScene(v);
        }

        public static void LoadNextLevel()
        {
            NumberOfLevel++;
            PassedLevel++;
            if (PassedLevel >= 31) NumberOfLevel = RandomExcept(0, 30, 9);
            PlayerPrefs.SetInt("Level", NumberOfLevel);
            //AppMetricaEvents.SendLevelEnd(PassedLevel, "WORLD " + PassedLevel.ToString(), 1,true);
            PlayerPrefs.SetInt("LevelPassed", PassedLevel);
            SceneManager.LoadScene(NumberOfLevel+1);
            
            
        }
    }
}