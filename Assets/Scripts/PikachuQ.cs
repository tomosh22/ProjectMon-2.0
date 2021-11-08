using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PikachuQ : MonoBehaviour
{
    public Vector3 velocity;
    public float lifeTime;
    public float startTime;
    public int level;
    public GameObject caster;
    // Start is called before the first frame update
    void Start()
    {
        lifeTime = 0.75f;
        velocity = transform.forward * 5;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > lifeTime) {
            Destroy(gameObject);
        }
        transform.position = transform.position += velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PikachuW"))
        {
            Pokemon target = other.gameObject.GetComponent<PikachuW>().markedPokemon;
            target.hp -= 2 * level;
            Debug.Log("dealt extra dmg, hp now " + target.hp);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Pokemon")) {
            if (other.gameObject != caster)
            {
                other.gameObject.GetComponent<BattleController>().pokemon.hp -= level;
                Destroy(gameObject);
            }
        }

    }

}
