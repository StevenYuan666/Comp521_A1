using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    void update(){
    }

    void OnCollisionEnter(){
        Destroy(gameObject);
    }
}
