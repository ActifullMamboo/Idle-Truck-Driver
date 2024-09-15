using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace _GAME.Scripts.Com
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(NavMeshObstacle))]

    public class TransparentFixes : MonoBehaviour
    {
        [SerializeField] private Material transp;
        [SerializeField] private Material opaque;
        private MeshRenderer rend;
        private void Awake()
        {
            rend = GetComponent<MeshRenderer>();
        }

        public void SetTransp()
        {
            rend.material = transp;
            rend.material.DOFade(0f, 0.1f);
        }

        public void SetOpaque()
        {
            transp.DOFade(1f, 0.1f);
            rend.material.DOFade(1, 0.1f).OnComplete(() =>
            {
                rend.material = opaque;

            });

        }
    }
}
