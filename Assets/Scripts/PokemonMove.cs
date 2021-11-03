using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PokemonMove : MonoBehaviour
{
    [SerializeField]
    public NavMeshAgent agent;
    public NavMeshModifierVolume[] volumes;
    [SerializeField]
    public string pokemonName;
    public Pokemon pokemon;
    private GameManager gm;
    private GameManager.GameState gameState;
    public bool isPlayer;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        gameState = gm.gameState;
        if (gameState == GameManager.GameState.Overworld)
        {
            volumes = GetComponentsInParent<NavMeshModifierVolume>();
        }
        agent = GetComponent<NavMeshAgent>();
        if (gameState == GameManager.GameState.Battle) {
            agent.speed = pokemon.moveSpeed;
        }
    }

    // Update is called once per frame
    void OverworldUpdate()
    {
        if (agent.remainingDistance < 0.5f)
        {
            NavMeshModifierVolume volume = volumes[Random.Range(0, volumes.Length - 1)];
            float destX = Random.Range(transform.parent.transform.position.x - (volume.center.x - volume.size.x / 2), transform.parent.transform.position.x - (volume.center.x + volume.size.x / 2));
            float destZ = Random.Range(transform.parent.transform.position.z - (volume.center.z - volume.size.z / 2), transform.parent.transform.position.z - (volume.center.z + volume.size.z / 2));
            NavMeshHit hit;
            NavMesh.SamplePosition(new Vector3(destX, volume.transform.position.y, destZ), out hit, 5f, 3);
            agent.destination = hit.position;
        }
    }

    void WildUpdate()
    {
        return;
        if (agent.remainingDistance < 0.1f)
        {
            Vector2 randomDir = Random.insideUnitCircle * 5;
            Vector3 samplePos = new Vector3((transform.position.x + randomDir.x), transform.position.y, (transform.position.z + randomDir.y));
            NavMeshHit hit;
            NavMesh.SamplePosition(samplePos, out hit, 5, 1);
            agent.destination = hit.position;

        }
    }

    void BattleUpdate() {
        if (agent.remainingDistance < 0.1f)
        {
            Vector2 randomDir = Random.insideUnitCircle * 5;
            Vector3 samplePos = new Vector3((transform.position.x + randomDir.x), transform.position.y, (transform.position.z + randomDir.y));
            NavMeshHit hit;
            NavMesh.SamplePosition(samplePos, out hit, 5, 1);
            agent.destination = hit.position;

        }
    }


    void Update()
    {
        if (isPlayer) { return; }
        switch (gameState)
        {
            
            case GameManager.GameState.Overworld: OverworldUpdate(); break;
            case GameManager.GameState.Wild: WildUpdate(); break;
            case GameManager.GameState.Battle: BattleUpdate(); break;
        }
    }
}