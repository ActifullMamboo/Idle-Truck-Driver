using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _GAME.Scripts.Base
{
    public class PointsSystem : MonoBehaviour
    {
        private List<PointView> _points = new();
        private List<PointsPair> _pair = new();

        public PointView GetPoint(PointType type)
        {
            return _points.Find(x => x.Type == type);
        }

        public PointsPair GetFreePair()
        {
            return _pair.Find(x => x.start.IsFree);
        }
        public List<PointView> GetAllPoints(PointType type)
        {
            return _points.FindAll(x => x.Type == type);
        }

        
        public void Initialize(GameObject root)
        {
            _points = root.GetComponentsInChildren<PointView>(true).ToList();
            foreach (var VARIABLE in _points.ToArray())
                if (VARIABLE.Type == PointType.None)
                    _points.Remove(VARIABLE);
            _pair = root.GetComponentsInChildren<PointsPair>(true).ToList();
        }

        public PointView GetClosestPoint(Transform prodCTransform, PointType type)
        {
            var allPts = _points.FindAll(x => x.Type == type);

            return GetClosestPoint(allPts.ToArray(), prodCTransform);
        }

        private PointView GetClosestPoint(PointView[] points, Transform target)
        {
            PointView tMin = null;
            var minDist = Mathf.Infinity;
            var currentPos = target.position;
            foreach (var t in points)
            {
                var dist = Vector3.Distance(t.transform.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }

            return tMin;
        }
    }
}