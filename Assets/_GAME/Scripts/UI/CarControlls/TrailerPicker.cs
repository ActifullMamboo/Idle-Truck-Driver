using System;
using System.Collections;
using _GAME.Scripts.Components;
using _GAME.Scripts.Zone;
using UnityEngine;

namespace _GAME.Scripts.UI.CarControlls
{
    public class TrailerPicker : MonoBehaviour, IComponentInitializer
    {
        private bool _withoutTrailer = true;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private PrometeoCarController carController;

        public float dist=10;
        public float needDist=1.5f;
        public LayerMask mask;
        private void FixedUpdate()
        {
            if (!_withoutTrailer)
            {
                return;
            }

            var transform1 = transform;
            var ray = new Ray(transform1.position, -transform1.forward);
            
            Physics.Raycast(ray,out RaycastHit hit, dist, mask);
            if (hit.collider!=null)
            {
                
                var dist = Vector3.Distance(transform.position, hit.point);

                if (dist <needDist)
                {
                    if (hit.collider.TryGetComponent(out TruckPickZone pickZone))
                    {
                        _withoutTrailer = false;
                        pickZone.ClaimTrailer( rb,transform.position);
                        StartCoroutine(PickRoutine(pickZone));
                    }
                }
                
               
            }
            Debug.DrawRay(ray.origin,ray.direction*dist, Color.red);
        }

        IEnumerator PickRoutine(TruckPickZone pickZone)
        {
            yield return new WaitForSeconds(.1f);
            pickZone.Claim(rb,transform.position);
            carController.AddWheels(pickZone.wheelColliders);

            var p =GetComponentInChildren<CalculatePath>();
            p.ShowNavigation();
        }

        public void Initialize()
        {
        }
        
    }
}
