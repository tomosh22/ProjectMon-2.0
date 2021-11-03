using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallBehaviour : MonoBehaviour
{
    GameManager gm;
    Func<Pokemon> instanceCreator;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pokemon")){
            Debug.Log("hit pokemon");
            
            Pokemon.InstanceCreators.TryGetValue(gm.encounterPokemon.GetComponent<PokemonMove>().pokemonName, out instanceCreator);
            gm.playerPokemon.Add(instanceCreator());
            gm.gameState = GameManager.GameState.Overworld;
            SceneManager.LoadScene(gm.overworldSceneName);

        }
        else {
            Destroy(gameObject);
        }
    }
}
