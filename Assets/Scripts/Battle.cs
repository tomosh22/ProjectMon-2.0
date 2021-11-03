using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour
{
    GameManager gm;
    public Vector3 playerSpawnPos;
    public Vector3 enemySpawnPos;
    public Pokemon enemyPokemon;
    public GameObject player;
    public GameObject enemy;
    public Pokemon playerPokemon;
    GameObject playerHealthBar;
    GameObject enemyHealthBar;
    [SerializeField]
    public GameObject healthBar;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        playerPokemon = gm.playerPokemon[gm.playerChosenPokemonIndex];
        enemy = gm.encounterPokemon;
        cam = Camera.main;
        
        enemySpawnPos = new Vector3(16, 1, 13);
        playerSpawnPos = new Vector3(7, 1, 5);
        Debug.Log("pokemon name: " + enemy.GetComponent<PokemonMove>().pokemonName);
        //TODO implement level
        enemyPokemon = Pokemon.InstanceCreators[enemy.GetComponent<PokemonMove>().pokemonName]();
        GameObject gameObject;
        Canvas canvas = FindObjectOfType<Canvas>();


        
        player = Instantiate(Pokemon.BattlePrefabs[playerPokemon.name], playerSpawnPos, Quaternion.Euler(0,45,0));
        BattleController playerController = player.GetComponent<BattleController>();
        playerController.isPlayer = true;
        player.GetComponent<NavMeshAgent>().speed = playerPokemon.moveSpeed;
        player.GetComponent<BattleController>().pokemon = playerPokemon;
        player.GetComponent<PokemonMove>().pokemon = playerPokemon;
        player.GetComponent<PokemonMove>().isPlayer = true;

        //Pokemon.BattlePrefabs.TryGetValue(enemyPokemon.name, out gameObject);
        //enemy = Instantiate(gameObject, enemySpawnPos, Quaternion.Euler(0,-120,0));
        //Pokemon.BattlePrefabs.TryGetValue(enemyPokemon.name, out gameObject);

        
        enemy = Instantiate(enemy, enemySpawnPos, Quaternion.Euler(0, -120, 0));
        enemy.GetComponent<BattleController>().pokemon = enemyPokemon;
        enemy.GetComponent<PokemonMove>().pokemon = enemyPokemon;
        enemy.GetComponent<PokemonMove>().isPlayer = false;
        enemyHealthBar = Instantiate(healthBar, cam.WorldToScreenPoint(enemySpawnPos), Quaternion.identity);
        enemyHealthBar.transform.parent = FindObjectOfType<Canvas>().transform;
        enemyHealthBar.GetComponent<HealthBar>().toFollow = enemy;
        enemyHealthBar.GetComponent<HealthBar>().pokemon = enemyPokemon;
        enemyHealthBar.GetComponent<Slider>().maxValue = enemyPokemon.hp;
        enemyHealthBar.GetComponent<Slider>().value = enemyPokemon.hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyPokemon.hp <= 0) {
            playerPokemon.LevelUp();
            gm.gameState = GameManager.GameState.Overworld;
            SceneManager.LoadScene(gm.overworldSceneName);
        }
    }
}
