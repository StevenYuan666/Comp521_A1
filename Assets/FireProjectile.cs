using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public Rigidbody projectile;
    public float speed = 2000;
    public Camera camera;
    private bool canFire = true;
    private Rigidbody bullet;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && canFire){
            bullet = Instantiate(projectile, camera.transform.position, Quaternion.identity);
            bullet.AddForce(camera.transform.forward * speed);
            canFire = false;
        }
        // Can only fire one bullet at one time
        if(bullet == null){
            canFire = true;
        }
    }

}
