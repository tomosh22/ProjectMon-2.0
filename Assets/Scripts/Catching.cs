using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catching : MonoBehaviour
{
    private bool isWinding;
    private float strength;
    public GameObject pokemon;
    public GameManager gm;
    [SerializeField]
    private GameObject ballPrefab;
    private GameObject ball;
    [SerializeField]
    private float windUpSpeed;
    private Transform cameraTrans;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        pokemon = gm.encounterPokemon;
        isWinding = false;
        Instantiate(pokemon, transform.position, Quaternion.identity);
        cameraTrans = FindObjectOfType<Camera>().transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWinding)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !ball)
            {
                isWinding = true;
                strength = 0;
                //Debug.Log(cameraTrans.position + cameraTrans.forward + cameraTrans.right);
                ball = Instantiate(ballPrefab, cameraTrans.position + cameraTrans.forward + cameraTrans.right, Quaternion.identity);
                ball.GetComponent<Rigidbody>().useGravity = false;
            }
        }
        else {
            strength += windUpSpeed * Time.deltaTime;
            if (Input.GetKeyUp(KeyCode.Mouse0)) {
                isWinding = false;
                Throw();
            }
        }
    }

    void Throw() {
        ball.GetComponent<Rigidbody>().useGravity = true;
        ball.GetComponent<Rigidbody>().velocity = cameraTrans.forward * strength + new Vector3(0,2,0); 
    }
}
