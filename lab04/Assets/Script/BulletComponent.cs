using UnityEngine;
using System.Collections;

public class BulletComponent : MonoBehaviour
{
   void start()
   {
      //Destroy the bullet after 5 seconds
      Destroy(gameObject, 5f);
   }
}
