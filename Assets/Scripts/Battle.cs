using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    public Canvas canvas;
    public GameObject deathUI;
    public bool paused;
    public GameObject cooldownTimerPrefab;
    GameObject qTimer;
    GameObject wTimer;
    GameObject eTimer;

    void SpawnPlayer() {
        playerPokemon = gm.playerPokemon[gm.playerChosenPokemonIndex];
        player = Instantiate(Pokemon.BattlePrefabs[playerPokemon.name], playerSpawnPos, Quaternion.Euler(0, 45, 0));
        BattleController playerController = player.GetComponent<BattleController>();
        playerController.isPlayer = true;
        player.GetComponent<NavMeshAgent>().speed = playerPokemon.moveSpeed;
        player.GetComponent<BattleController>().pokemon = playerPokemon;

        //TODO move this data off PokemonMove
        player.GetComponent<PokemonMove>().pokemon = playerPokemon;
        player.GetComponent<PokemonMove>().isPlayer = true;

        playerHealthBar = Instantiate(healthBar, cam.WorldToScreenPoint(playerSpawnPos), Quaternion.identity);
        playerHealthBar.transform.parent = canvas.transform;
        HealthBar playerHpBar = playerHealthBar.GetComponent<HealthBar>();
        playerHpBar.toFollow = player;
        playerHpBar.pokemon = playerPokemon;
        Slider playerHpBarSlider = playerHealthBar.GetComponent<Slider>();
        playerHpBarSlider.maxValue = playerPokemon.hp;
        playerHpBarSlider.value = playerPokemon.hp;

        qTimer = Instantiate(cooldownTimerPrefab, Vector3.zero, Quaternion.identity);
        qTimer.transform.SetParent(canvas.transform);
        CooldownTimer qCd = qTimer.GetComponent<CooldownTimer>();
        qCd.pokemon = playerPokemon;
        qCd.index = 0;
        Slider qTimerSlider = qTimer.GetComponent<Slider>();
        qTimerSlider.minValue = 0;
        qTimerSlider.maxValue = playerPokemon.cooldowns[0];
        qTimer.transform.Find("Ability").Find("Text (TMP)").GetComponent<TMP_Text>().text = "Q";

        wTimer = Instantiate(cooldownTimerPrefab, Vector3.zero, Quaternion.identity);
        wTimer.transform.SetParent(canvas.transform);
        CooldownTimer wCd = wTimer.GetComponent<CooldownTimer>();
        wCd.pokemon = playerPokemon;
        wCd.index = 1;
        Slider wTimerSlider = wTimer.GetComponent<Slider>();
        wTimerSlider.minValue = 0;
        wTimerSlider.maxValue = playerPokemon.cooldowns[1];
        wTimer.transform.Find("Ability").Find("Text (TMP)").GetComponent<TMP_Text>().text = "W";

        eTimer = Instantiate(cooldownTimerPrefab, Vector3.zero, Quaternion.identity);
        eTimer.transform.SetParent(canvas.transform);
        CooldownTimer eCd = eTimer.GetComponent<CooldownTimer>();
        eCd.pokemon = playerPokemon;
        eCd.index = 2;
        Slider eTimerSlider = eTimer.GetComponent<Slider>();
        eTimerSlider.minValue = 0;
        eTimerSlider.maxValue = playerPokemon.cooldowns[2];
        eTimer.transform.Find("Ability").Find("Text (TMP)").GetComponent<TMP_Text>().text = "E";

    }

    void SpawnEnemy() {
        enemy = Instantiate(enemy, enemySpawnPos, Quaternion.Euler(0, -120, 0));
        enemy.GetComponent<BattleController>().pokemon = enemyPokemon;
        enemy.GetComponent<BattleController>().enemyObject = player;

        //TODO move this data off PokemonMove
        enemy.GetComponent<PokemonMove>().pokemon = enemyPokemon;
        enemy.GetComponent<PokemonMove>().isPlayer = false;

        enemyHealthBar = Instantiate(healthBar, cam.WorldToScreenPoint(enemySpawnPos), Quaternion.identity);
        enemyHealthBar.transform.parent = canvas.transform;
        enemyHealthBar.GetComponent<HealthBar>().toFollow = enemy;
        enemyHealthBar.GetComponent<HealthBar>().pokemon = enemyPokemon;
        enemyHealthBar.GetComponent<Slider>().maxValue = enemyPokemon.hp;
        enemyHealthBar.GetComponent<Slider>().value = enemyPokemon.hp;

    }
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        
        enemy = gm.encounterPokemon;
        cam = Camera.main;
        
        enemySpawnPos = new Vector3(16, 1, 13);
        playerSpawnPos = new Vector3(7, 1, 5);
        //Debug.Log("pokemon name: " + enemy.GetComponent<PokemonMove>().pokemonName);
        //TODO implement level
        enemyPokemon = Pokemon.InstanceCreators[enemy.GetComponent<PokemonMove>().pokemonName]();
        enemyPokemon.SetLevel(gm.encounterPokemonLevel);
        canvas = FindObjectOfType<Canvas>();


        SpawnPlayer();
        SpawnEnemy();

        


        
    }

    // Update is called once per frame
    void Update()
    {
        if (paused) { return; }
        if (enemyPokemon.hp <= 0) {
            playerPokemon.SetLevel(playerPokemon.level + gm.xpModifier);
            gm.gameState = GameManager.GameState.Overworld;
            gm.battleResult = GameManager.WinOrLose.Win;
            SceneManager.LoadScene(gm.overworldSceneName);
        }
        if (playerPokemon.hp <= 0) {
            Time.timeScale = 0;
            Destroy(playerHealthBar);
            enemy.GetComponent<BattleController>().paused = true;
            Destroy(player);
            paused = true;
            GameObject ui = Instantiate(deathUI, Vector3.zero, Quaternion.identity);
            RectTransform rt = ui.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            ui.transform.SetParent(canvas.transform);
            ui.transform.Find("Run").GetComponent<Button>().onClick.AddListener(()=> {
                Time.timeScale = 1;
                gm.gameState = GameManager.GameState.Overworld;
                gm.battleResult = GameManager.WinOrLose.Lose;
                SceneManager.LoadScene(gm.overworldSceneName);
            });
            for (int x = 0; x < 6; x++)
            {
                int temp = x;
                if (temp < gm.playerPokemon.Count)
                {
                    Transform button = ui.transform.Find("Button" + (temp).ToString());
                    button.Find("Text (TMP)").GetComponent<TMP_Text>().text = temp.ToString() + " " + gm.playerPokemon[temp].name;
                    //TODO handle setting level
                    Debug.Log("temp: " + temp.ToString());
                    button.GetComponent<Button>().onClick.AddListener(() => { 
                        gm.playerChosenPokemonIndex = temp;
                        Time.timeScale = 1;
                        Destroy(ui);
                        paused = false;
                        BattleController enemyController = enemy.GetComponent<BattleController>();
                        enemyController.paused = false;
                        
                        SpawnPlayer();
                        enemyController.data.enemyObject = player;
                    
                    });
                }
                else
                {
                    return;
                }

            }
        }
    }
}
