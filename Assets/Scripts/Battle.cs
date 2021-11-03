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
    public Pokemon playerPokemon;
    public Pokemon enemyPokemon;
    public GameObject player;
    public GameObject enemy;
    GameObject playerHealthBar;
    GameObject enemyHealthBar;
    [SerializeField]
    public GameObject healthBar;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        gm = FindObjectOfType<GameManager>();
        enemySpawnPos = new Vector3(16, 1, 13);
        playerSpawnPos = new Vector3(7, 1, 5);
        enemyPokemon = new Pikachu();
        GameObject gameObject;
        Canvas canvas = FindObjectOfType<Canvas>();

        Pokemon.BattlePrefabs.TryGetValue(playerPokemon.name, out gameObject);
        player = Instantiate(gameObject, playerSpawnPos, Quaternion.Euler(0,45,0));
        BattleController playerController = player.GetComponent<BattleController>();
        playerController.isPlayer = true;
        player.GetComponent<NavMeshAgent>().speed = playerPokemon.moveSpeed;
        player.GetComponent<BattleController>().pokemon = playerPokemon;
        player.GetComponent<PokemonMove>().isPlayer = true;

        Pokemon.BattlePrefabs.TryGetValue(enemyPokemon.name, out gameObject);
        enemy = Instantiate(gameObject, enemySpawnPos, Quaternion.Euler(0,-120,0));
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
