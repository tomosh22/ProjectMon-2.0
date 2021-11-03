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
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        Debug.Log(pokemon.name);
    }
    private void Update()
    {
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
        for (int x = 0; x < 4; x++) {
            if (pokemon.timeRemaining[x] > 0) {
                pokemon.timeRemaining[x] -= Time.deltaTime;
            }
            if (pokemon.timeRemaining[x] < 0) {
                pokemon.timeRemaining[x] = 0;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            RaycastHit hit;
            Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit);
            NavMeshHit navHit;
            NavMesh.SamplePosition(hit.point, out navHit, 10f, -1);
            agent.destination = navHit.position;
        }
        if (Input.GetKeyDown(KeyCode.Q) &&  pokemon.timeRemaining[0] == 0) {
            AbilityData data = new AbilityData();
            data.cam = cam;
            data.player = gameObject;
            pokemon.Q(data);
            pokemon.timeRemaining[0] = pokemon.cooldowns[0];
        }
        if (Input.GetKeyDown(KeyCode.W) && pokemon.timeRemaining[1] == 0)
        {
            AbilityData data = new AbilityData();
            data.cam = cam;
            data.player = gameObject;
            pokemon.W(data);
            pokemon.timeRemaining[1] = pokemon.cooldowns[1];
        }
    }

    void AIUpdate() { 
        //TODO implement battle ai
    }

}
