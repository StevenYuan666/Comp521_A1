using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace D{
    public class Destroy : MonoBehaviour
    {
        public static int CountDestroyed = -1;

        void OnCollisionEnter(){
            Destroy(gameObject);
            //Reveal Maze
            CountDestroyed ++;
        }


    }
}
