using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;
using D;

public class PlayerMove : MonoBehaviour
{
    public float MoveSpeed = 7; 
    // record W, A, S, D
    private float horizontal; 
    private float vertical; 
    private float gravity = 9.8f; 
    public float JumpSpeed = 5f;
    public CharacterController PlayerController; 
    private Vector3 Player_Move; 
    public GameObject Character;
    private bool ifPassed = false;
    private GUIStyle guiStyle = new GUIStyle();

    // Start is called before the first frame update
    void Start(){
         Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is on the ground
        if(PlayerController.isGrounded) { 
            horizontal = Input.GetAxis("Horizontal"); 
            vertical = Input.GetAxis("Vertical");  
            Player_Move = (transform.forward * vertical + transform.right * horizontal) * MoveSpeed; 
            // Check if the player want to jump
            if (Input.GetAxis("Jump")==1) { 
                Player_Move.y = Player_Move.y + JumpSpeed;
            } 
        }
        // Mimic the gravity so that the player will fall down from the sky
        Player_Move.y = Player_Move.y - gravity * Time.deltaTime;
        PlayerController.Move(Player_Move*Time.deltaTime);
        // Check if the player fall into the canyon
        if(PlayerController.transform.position.y < -25){
            GameOver();
        }
        // Check if the player reach the last row of the maze
        if(PlayerController.transform.position.x > 44){
            Vector3 newPos = new Vector3(72, -28, 30);
            Character.transform.position = newPos;
            ifPassed = true;
            
        }
        // Check if the maze has already done
        if(D.Destroy.CountDestroyed < 9 && PlayerController.transform.position.x > 0){
            GameOver();
        }
        if(Character.transform.position.x < 70 && ifPassed){
            GameOver();
        }
    }

    // Load the game over scene under specific scenarios
    void GameOver(){
        SceneManager.LoadScene("GameOver");
    }

    void OnGUI(){
        if(ifPassed){
            guiStyle.fontSize = 100;
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200f, 200f), "You won!", guiStyle);
        }
    }
}