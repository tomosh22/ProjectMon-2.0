using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PokemonSpawner : MonoBehaviour
{
    
    [SerializeField]
    private NavMeshModifierVolume[] volumes;
    private Spawn[] spawns;
    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        volumes = GetComponents<NavMeshModifierVolume>();
        spawns = GetComponents<Spawn>();
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //spawn pokemon when player moves into range
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) { 
            //Debug.Log("entered spawn area");
            NavMeshModifierVolume volume = volumes[Random.Range(0, volumes.Length)];
            float spawnX = Random.Range(transform.position.x + volume.center.x - volume.size.x / 2, transform.position.x + volume.center.x + volume.size.x / 2);
            float spawnZ = Random.Range(transform.position.z + volume.center.z - volume.size.z / 2, transform.position.z + volume.center.z + volume.size.z / 2);

            NavMeshHit hit;
            NavMesh.SamplePosition(new Vector3(spawnX, volume.center.y, spawnZ), out hit, 5f,3);
            foreach (Spawn spawn in spawns) {
                if(Random.Range(0f,1f) < spawn.chance)
                {
                    GameObject spawnedPokemon = Instantiate(spawn.wildPrefab, hit.position, Quaternion.identity);
                    spawnedPokemon.transform.parent = gameObject.transform;
                    spawnedPokemon.GetComponent<PokemonMove>().level = Random.Range(spawn.minLevel, spawn.maxLevel + 1);
                    gm.wildPokemon.Add(spawnedPokemon);
                }
                
            }
            
        }
        
    }
}
