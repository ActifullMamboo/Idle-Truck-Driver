using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Tools;
using Cinemachine;
using DG.Tweening;
using FluffyUnderware.Curvy;
using FluffyUnderware.Curvy.Generator;
using FluffyUnderware.Curvy.Generator.Modules;
using InsaneSystems.RoadNavigator;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace _GAME.Scripts
{
    public class CalculatePath : MonoBehaviour
    {
        public Transform _target;
        private NavMeshPath _path;
        public float dist;
        private Transform _secondTarget;
        public Map map;

        [Button]
        public void Distance()
        {
            _path = new NavMeshPath();
            var filter = new NavMeshQueryFilter() { areaMask = NavMesh.AllAreas, agentTypeID = 0 };
            
            NavMesh.CalculatePath(transform.position, _target.position, filter, _path);

            //mat.DOOffset(-Vector2.up, 3f).SetRelative(true).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
            dist = 0;
            for (var i = 0; i < _path.corners.Length - 1; i++)
            {
                dist += Vector3.Distance(_path.corners[i], _path.corners[i + 1]);
            }
        }

        public void ShowNavigation()
        {
            map.ShowAsNavigator();
        }

        public void DisableNavigation()
        {
            map.ShowAsNavigator();
        }
    }
}