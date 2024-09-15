using System;
using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.AI.Base;
using _GAME.Scripts.Base;
using _Game.Scripts.Tools;
using DG.Tweening;
using UnityEngine;

namespace _GAME.Scripts.AI
{
    public class CharactersControllSystem : MonoBehaviour
    {
        private List<PointView> _characterSpawnPoints = new();
        private List<PointView> _characterDestinationPoints = new();

        private PointsSystem _system;
        [SerializeField] private List<MassovkaOnlevel> MassovkaOnlevels;
        private List<BaseAI> _bots = new();


        private void LateUpdate()
        {
            for (var i = 0; i < _bots.Count; i++) _bots[i].UpdateMovement(Time.deltaTime);
        }
        public void InitSceneObjects(params object[] list)
        {
            var points = list[0] as PointsSystem;
            _characterSpawnPoints = points.GetAllPoints(PointType.CharacterSpawnPoint).ToList();
            _characterDestinationPoints = points.GetAllPoints(PointType.CharacterEndPoint).ToList();
            _system = points;

            var characters = MassovkaOnlevels.FirstOrDefault(x => x.Type == MassovkaOnlevel.MassovkaType.human);
            if (characters!=null)
            {
                for (int i = 0; i < characters.countOnLevel; i++)
                {
                    SpawnNewEnvironmentCharacter(characters.prefab,i);
                }
            }
            
            var cars = MassovkaOnlevels.FirstOrDefault(x => x.Type == MassovkaOnlevel.MassovkaType.car);
            if (cars!=null)
            {
                for (int i = 0; i < cars.countOnLevel; i++)
                {
                    SpawnNewEnvironmentCar(cars.CarAI,i);
                }
            }
        }

        private void SpawnNewEnvironmentCar(CarAI behaviour,float delay)
        {
            DOVirtual.DelayedCall(1 * delay,() =>
            {
                CarSp(behaviour,delay);
            } );
        }

        private void CarSp(CarAI behaviour, float i)
        {
            var freePair = _system.GetFreePair();
            if (freePair==null)
            {
                return;
            }
            var spawnPoint = freePair.start;
            var destinationPoint = freePair.end;
            var car = Instantiate(behaviour, transform);
            car.Init(spawnPoint, destinationPoint);
            car.name = "car " + i;

            car.OnCycleEnd += OnCompleteCarCycle;
        }

        private void OnCompleteCarCycle(CarAI beh)
        {
            var freePair = _system.GetFreePair();
            var spawnPoint = freePair.start;
            var destinationPoint = freePair.end;
            
            beh.Init(spawnPoint,destinationPoint);
        }

        PointView GetFurthest(List<PointView> points, PointView point)
        {
            PointView tMin = points[0];

            var closestDist = Vector3.Distance(tMin.transform.position, point.transform.position);
            foreach (PointView t in points)
            {
                float dist =  Vector3.Distance(tMin.transform.position, t.transform.position);
                if (dist > closestDist)
                {
                    closestDist = dist;
                    tMin = t;
                }
            }
            return tMin;
        }

        private void SpawnNewEnvironmentCharacter(CityzenBehaviour character, float delay)
        {
            DOVirtual.DelayedCall(1.2f * delay,() =>
            {
                SpawnC(character);
            } );
        }

        private void SpawnC(CityzenBehaviour character)
        {
            var spawnPoint = _characterSpawnPoints.Shuffle().FirstOrDefault(x => x.IsFree);
            var destinationPoint = _characterDestinationPoints.Shuffle().FirstOrDefault(x => x.IsFree);

            var beh = Instantiate(character, transform);
            beh.Init(spawnPoint, destinationPoint);
            _bots.Add(beh);
            beh.OnCycleEnd += OnCompleteCharacterCycle;
        }

        private void OnCompleteCharacterCycle(CityzenBehaviour beh)
        {
            var spawnPoint = _characterSpawnPoints.Shuffle().FirstOrDefault(x => x.IsFree);
            var destinationPoint = _characterDestinationPoints.Shuffle().FirstOrDefault(x => x.IsFree);

            beh.Init(spawnPoint,destinationPoint);

        }

        [Serializable]
        public class MassovkaOnlevel
        {
            public enum MassovkaType
            {
                car,
                human
            }

            public MassovkaType Type;
            public CityzenBehaviour prefab;
            public CarAI CarAI;
            public int countOnLevel;
        }
    }
}
