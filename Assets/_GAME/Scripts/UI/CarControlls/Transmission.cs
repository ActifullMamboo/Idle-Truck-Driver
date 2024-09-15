using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _GAME.Scripts.UI.CarControlls
{
    [Serializable]
    public class TransmissionPosition
    {
        public Vector3 pos;
        public Vector3 euler;
    }
    public class Transmission : MonoBehaviour
    {
        [SerializeField] private MeshRenderer forward;
        [SerializeField] private MeshRenderer revers;
        [SerializeField] private Transform transmission;

        [SerializeField] private List<TransmissionPosition> positions;
        private static readonly int BaseMap = Shader.PropertyToID("_BaseColor");

        public void ChangeControls(bool b)
        {
            int k = 0;
            if (b)
            {
                forward.material.SetColor(BaseMap,Color.red);
                revers.material.SetColor(BaseMap,Color.white);

            }
            else
            {
                forward.material.SetColor(BaseMap,Color.white);
                revers.material.SetColor(BaseMap,Color.red);
                
                k = 1;
               
            }
            transmission.DOLocalMove(positions[k].pos, 0.3f).SetEase(Ease.Linear);
            transmission.DOLocalRotate(positions[k].euler, 0.3f).SetEase(Ease.Linear);
        }
    }
}
