using UnityEngine;

namespace _GAME.Scripts.Com
{
   public class TransparentTrigger : MonoBehaviour
   {
      private void OnTriggerEnter(Collider other)
      {
         if (other.TryGetComponent(out TransparentFixes fixes))
         {
            fixes.SetTransp();
         }
      }

      private void OnTriggerExit(Collider other)
      {
         if (other.TryGetComponent(out TransparentFixes fixes))
         {
            fixes.SetOpaque();
         }
      
      }
   }
}
