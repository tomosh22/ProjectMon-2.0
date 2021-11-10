using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BattleController : MonoBehaviour
{
    public bool isPlayer;
    public Camera cam;
    public NavMeshAgent agent;
    public Pokemon pokemon;
    public GameManager gm;
    public GameObject enemyObject;
    public AbilityData data;
    public bool paused;
    public List<Pokemon.Tag> tags;
    // Start is called before the first frame update
    void Start()
    {
        tags = new List<Pokemon.Tag>();
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        gm = FindObjectOfType<GameManager>();
        data = new AbilityData();
        data.cam = cam;
        data.gameObject = gameObject;
        data.currentPokemon = pokemon;
        data.isPlayer = isPlayer ? true : false;
        data.enemyObject = enemyObject;
        data.agent = agent;
    }
    private void Update()
    {
        if (paused) { return; }
        for (int x = 0; x < 4; x++)
        {
            if (pokemon.timeRemaining[x] > 0)
            {
                pokemon.timeRemaining[x] -= Time.deltaTime;
            }
            if (pokemon.timeRemaining[x] < 0)
            {
                pokemon.timeRemaining[x] = 0;
            }

        }
        if (isPlayer)
        {
            PlayerUpdate();
        }
        else {
            AIUpdate();
        }
    }
    // Update is called once per frame
    void PlayerUpdate()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            RaycastHit hit;
            Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit);
            NavMeshHit navHit;
            NavMesh.SamplePosition(hit.point, out navHit, 10f, -1);
            agent.destination = navHit.position;
        }
        if (Input.GetKeyDown(KeyCode.Q) &&  pokemon.timeRemaining[0] == 0) {
            pokemon.Q(data);
            pokemon.timeRemaining[0] = pokemon.cooldowns[0];
        }
        if (Input.GetKeyDown(KeyCode.W) && pokemon.timeRemaining[1] == 0)
        {
            pokemon.W(data);
            pokemon.timeRemaining[1] = pokemon.cooldowns[1];
        }
        if (Input.GetKeyDown(KeyCode.E) && pokemon.timeRemaining[2] == 0)
        {
            pokemon.E(data);
            pokemon.timeRemaining[2] = pokemon.cooldowns[2];
        }
    }

    void AIUpdate() {

        if (gm.xpModifier == 1)
        {
            //easy mode
            if (pokemon.timeRemaining[0] == 0) {
                pokemon.Q(data);
                pokemon.timeRemaining[0] = pokemon.cooldowns[0];
            }
            if (pokemon.timeRemaining[1] == 0)
            {
                pokemon.W(data);
                pokemon.timeRemaining[1] = pokemon.cooldowns[1];
            }

        }
        else {if (gm.xpModifier == 2) {
                pokemon.BattleBehaviour(agent, enemyObject);
        } 
            
        }
    }

}
