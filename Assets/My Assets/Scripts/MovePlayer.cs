using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float movementSpeed;
    public SpriteRenderer spriteRenderer;
    public Animator playerAnimator;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        if (
            !Input.GetKey(KeyCode.LeftArrow) &&
            !Input.GetKey(KeyCode.RightArrow) &&
            !Input.GetKey(KeyCode.UpArrow) &&
            !Input.GetKey(KeyCode.DownArrow)
            )
            playerAnimator.SetBool("IsMoving", false);

        // déplacement GAUCHE
        if (Input.GetKey (KeyCode.LeftArrow) && GameManager.Inst.gameIsRunning)
        {
            transform.position = new Vector3(
                transform.position.x - movementSpeed * Time.deltaTime,
                transform.position.y,
                transform.position.z);
            
            //rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);

            transform.rotation = Quaternion.Euler(0, 0, 180);
            spriteRenderer.flipY = true;
            playerAnimator.SetBool("IsMoving", true);
        }

        // déplacement DROITE
        if (Input.GetKey(KeyCode.RightArrow) && GameManager.Inst.gameIsRunning)
        {
            
            transform.position = new Vector3(
                transform.position.x + movementSpeed * Time.deltaTime,
                transform.position.y,
                transform.position.z);
            
            //rb.velocity = new Vector2(movementSpeed, rb.velocity.y);

            transform.rotation = Quaternion.Euler(0, 0, 0);
            spriteRenderer.flipY = false;
            playerAnimator.SetBool("IsMoving", true);
        }

        // déplacement HAUT
        if (Input.GetKey(KeyCode.UpArrow) && GameManager.Inst.gameIsRunning)
        {
            
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + movementSpeed * Time.deltaTime,
                transform.position.z);
            
            //rb.velocity = new Vector2(rb.velocity.x, movementSpeed);

            transform.rotation = Quaternion.Euler(0, 0, 90);
            playerAnimator.SetBool("IsMoving", true);
        }

        // déplacement BAS
        if (Input.GetKey(KeyCode.DownArrow) && GameManager.Inst.gameIsRunning)
        {
            
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y - movementSpeed * Time.deltaTime,
                transform.position.z);
            
            //rb.velocity = new Vector2(rb.velocity.x, -movementSpeed);

            transform.rotation = Quaternion.Euler(0, 0, -90);
            playerAnimator.SetBool("IsMoving", true);
        }

        if (Input.GetKey (KeyCode.R))
        {
            StartCoroutine(GameManager.Inst.ResetGame());
        }

        // Diagonale bas-gauche
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow) && GameManager.Inst.gameIsRunning)
        {
            transform.rotation = Quaternion.Euler(0, 0, -135);
        }

        // Diagonale bas-droite
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow) && GameManager.Inst.gameIsRunning)
        {
            transform.rotation = Quaternion.Euler(0, 0, -45);
        }

        // Diagonale haut-gauche
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow) && GameManager.Inst.gameIsRunning)
        {
            transform.rotation = Quaternion.Euler(0, 0, 135);
        }

        // Diagonale haut-droite
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow) && GameManager.Inst.gameIsRunning)
        {
            transform.rotation = Quaternion.Euler(0, 0, 45);
        }
    }
}
