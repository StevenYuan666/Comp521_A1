using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Generic;

public class DisplayTree : MonoBehaviour
{
    public CharacterController playerController;
    public Rigidbody tree1;
    public Rigidbody tree2;
    public Rigidbody tree3;
    public Rigidbody tree4;
    public Rigidbody tree5;
    private Rigidbody[] trees = new Rigidbody[5];
    private System.Random rnd = new System.Random();

    private bool pass = true;

    void Start(){
        /* Explanation here
            Initially I wanted to use 5 different tree models, but they used 
            tremendous amount of storage, which are more than 2GB.
            So I finally set all trees be same in the array, I'm too lazy to modify my code..
        */
        trees[0] = tree1;
        trees[1] = tree2;
        trees[2] = tree3;
        trees[3] = tree4;
        trees[4] = tree5;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.transform.position.x > -65 && pass){
            pass = false;
            HashSet<(float, float)> used = new HashSet<(float, float)>();
            int numOfTree = 100;
            while(numOfTree > 0){
                int index = rnd.Next(5);
                Rigidbody randTree = trees[index];
                // Ensure the trees don't cover each other
                while(true){
                    float rand_x = rnd.Next(0, 8);
                    float rand_z = rnd.Next(0, 18);
                    if(!used.Contains((rand_x, rand_z))){
                        used.Add((rand_x, rand_z));
                        Vector3 pos = new Vector3(-55 + rand_x * 5, 8, -45 + rand_z * 5);
                        Rigidbody newTree =  Instantiate(randTree, pos, playerController.transform.rotation);
                        break;
                    }
                }
                numOfTree --;
            }
        }
    }
}
