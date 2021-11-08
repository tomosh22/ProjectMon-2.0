using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PikachuW : MonoBehaviour
{
    public Vector3 velocity;
    public float lifeTime;
    public float startTime;
    public bool isAttached;
    public Pokemon markedPokemon;
    public GameObject caster;
    // Start is called before the first frame update
    void Start()
    {
        markedPokemon = null;
        lifeTime = 1.75f;
        velocity = transform.forward * 4;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > lifeTime)
        {
            Destroy(gameObject);
        }
        if (markedPokemon == null)
        {
            transform.position = transform.position += velocity * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Pokemon")) {
            if (other.gameObject != caster) {
                lifeTime = 5f;
                startTime = Time.time;
                gameObject.transform.position = other.gameObject.transform.position;
                gameObject.transform.parent = other.gameObject.transform;
                markedPokemon = other.gameObject.GetComponent<BattleController>().pokemon;
                GetComponent<SphereCollider>().radius = 3;
            }
            
        }
    }

    
}
