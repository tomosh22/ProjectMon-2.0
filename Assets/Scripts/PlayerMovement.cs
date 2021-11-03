using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    [SerializeField]
    public float moveSpeed;
    private bool grounded;
    private float yVelocity;
    public bool canMove;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        yVelocity = 0f;
        grounded = false;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(IsGrounded());
        if (BangedHead()) {
            yVelocity = -0.5f;
        }
        if (!IsGrounded())
        {
            yVelocity -= 0.05f;
            //Debug.Log(yVelocity);
        }
        else {
            yVelocity = 0;
            if (Input.GetKey(KeyCode.Space)) {
                yVelocity = 5f;
                grounded = false;
            }
        }
        controller.Move(transform.up * yVelocity * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Escape)) { 
            Application.Quit();
        }
        if (canMove) {
            controller.Move(transform.right * Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime + transform.forward * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position - Vector3.up * 0.5f, Vector3.down *0.58f);
        Gizmos.DrawRay(transform.position - Vector3.up * 0.5f, new Vector3(-0.4f,-0.5f,-0.4f));
    }
    bool IsGrounded() {
        return Physics.Raycast(transform.position - Vector3.up * 0.5f, Vector3.down, 0.58f) ||
            Physics.Raycast(transform.position - Vector3.up * 0.5f, new Vector3(-0.4f, -0.5f, -0.4f), 0.852f) ||
            Physics.Raycast(transform.position - Vector3.up * 0.5f, new Vector3(0.4f, -0.5f, -0.4f), 0.852f) ||
            Physics.Raycast(transform.position - Vector3.up * 0.5f, new Vector3(0.4f, -0.5f, 0.4f), 0.852f) ||
            Physics.Raycast(transform.position - Vector3.up * 0.5f, new Vector3(-0.4f, -0.5f, 0.4f), 0.852f);
        
    }

    bool BangedHead()
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.up, 0.58f) ||
            Physics.Raycast(transform.position + Vector3.up * 0.5f, new Vector3(-0.4f, 0.5f, -0.4f), 0.852f) ||
            Physics.Raycast(transform.position + Vector3.up * 0.5f, new Vector3(0.4f, 0.5f, -0.4f), 0.852f) ||
            Physics.Raycast(transform.position + Vector3.up * 0.5f, new Vector3(0.4f, 0.5f, 0.4f), 0.852f) ||
            Physics.Raycast(transform.position + Vector3.up * 0.5f, new Vector3(-0.4f, 0.5f, 0.4f), 0.852f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") {
            grounded = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            grounded = false;
        }
    }
}
