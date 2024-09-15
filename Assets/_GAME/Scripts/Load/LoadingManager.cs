using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace _GAME.Scripts.Load
{
    public enum LoadingType
    {
        none,
        Game,
        Garage
    }
    public class LoadingManager : MonoBehaviour
    {
        private static List<bl_SceneLoader> _loaders = new List<bl_SceneLoader>();
        private static bool exists;

        private void Awake()
        {
            if (!exists)
            {
                exists = true;
                DontDestroyOnLoad(gameObject);
                _loaders = GetComponentsInChildren<bl_SceneLoader>().ToList();

            }
            else
            {
                Destroy(gameObject);
            }
        }
        public static void LoadScene(LoadingType type)
        {
            DOTween.KillAll();
            if (type == LoadingType.Game)
            {
                 _loaders[0].LoadLevel("GameScene");
            }
            else
            {
                _loaders[1].LoadLevel("Garage");
            }
        }

    }
}
