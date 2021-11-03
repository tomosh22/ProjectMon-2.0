using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject player;
    public List<GameObject> wildPokemon;
    public GameObject encounterPokemon;
    public List<Pokemon> playerPokemon;
    public Vector3 overworldPos;
    public Quaternion overworldRotation;
    public string overworldSceneName;
    public List<string> overworldScenes;
    public Canvas canvas;
    public GameObject inventoryEntryPrefab;
    public GameObject encounterChoicePrefab;
    public enum GameState {
        Overworld, Wild, Battle
    }
    public GameState gameState;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<CharacterController>().gameObject;
        canvas = FindObjectOfType<Canvas>();
        playerPokemon = new List<Pokemon>();
        playerPokemon.Add(new Pikachu());
        overworldScenes = new List<string>() {"InitialScene", "Town0"};

        //GameObject test;
        //Pokemon.WildPrefabs.TryGetValue("Pikachu", out test);
        //CreateEncounterChoiceUI(test);

        gameState = GameState.Overworld;
        SceneManager.sceneLoaded += OnSceneLoad;
        CreateInventoryUI();
    }
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        canvas = FindObjectOfType<Canvas>();
        if (overworldScenes.Contains(scene.name)) {
            //overworld was loaded
            Camera.main.GetComponent<CameraController>().isPlayer = true;
            CreateInventoryUI();
            player = FindObjectOfType<CharacterController>().gameObject;
            if (overworldPos != Vector3.zero)
            {
                //move player to location remembered in scene before other scene was loaded
                player.transform.position = overworldPos;
                player.transform.rotation = overworldRotation;
                overworldPos = Vector3.zero;
                
            }
            return;
        }
        if (SceneManager.GetSceneByName("Catching") == scene)
        {
            Camera.main.GetComponent<CameraController>().isPlayer = false;
            GameObject.Find("CatchingManager").GetComponent<Catching>().pokemon = encounterPokemon;
            return;
        }
        
    }

    void CreateInventoryUI() {
        for (int x = 0; x < 6; x++) {
            if (x < playerPokemon.Count )
            {
                GameObject entry = Instantiate(inventoryEntryPrefab, Vector3.zero, Quaternion.identity);
                entry.transform.parent = canvas.transform;
                RectTransform rt = entry.GetComponent<RectTransform>();
                rt.localPosition = new Vector3(x * (Screen.width / 6), 0, 0);
                entry.transform.Find("Name").GetComponent<TMP_Text>().text = playerPokemon[x].name;
                entry.transform.Find("Level").GetComponent<TMP_Text>().text = "Level: " + playerPokemon[x].level;
                Debug.Log(playerPokemon[x].name + playerPokemon[x].level);
            }
            else {
                return;
            }
            
        }
        

    }

    static GameManager gm;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (gm)
        {
            Destroy(gameObject);
        }
        else {
            gm = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState) {
            case GameState.Overworld: OverworldUpdate();break;
            case GameState.Wild: WildUpdate();break;
            case GameState.Battle: BattleUpdate(); break;
        }
    }

    
    
    void OverworldUpdate() {
        //load new scene if player goes to exit point
        foreach(ExitPoint point in ExitPoints.points) {
            if (SceneManager.GetActiveScene().name == point.sceneFrom && Vector3.Distance(point.posFrom, player.transform.position) < 0.5f)
            {
                SceneManager.LoadScene(point.sceneTo, LoadSceneMode.Single);
            }
        }
        
        foreach (GameObject pokemon in wildPokemon) {
            if (Vector3.Distance(pokemon.transform.position, player.transform.position) < 2f) {

                //remember player location/rotation + scene name and remove all wild pokemon
                PrepareToExitOverworld();
                CreateEncounterChoiceUI(pokemon);
                Time.timeScale = 0;
                //load catch scene for pokemon
                //LoadCatch(pokemon);

                break;
            }
        }
    }

    void CreateEncounterChoiceUI(GameObject pokemon) {
        GameObject ui = Instantiate(encounterChoicePrefab, Vector3.zero, Quaternion.identity);
        RectTransform rt = ui.GetComponent<RectTransform>();
        rt.localPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        ui.transform.parent = canvas.transform;
        ui.transform.Find("Catch").GetComponent<Button>().onClick.AddListener(() => LoadCatch(pokemon));
        ui.transform.Find("Battle").GetComponent<Button>().onClick.AddListener(() => LoadBattle(pokemon));
        //for (int x = 0; x < 6; x++)
        //{
        //    if (x < playerPokemon.Count)
        //    {
        //        Transform button = ui.transform.Find("Button" + x.ToString());
        //        button.GetComponentInChildren<TMP_Text>().text = playerPokemon[x].name;
        //        GameObject instance;
        //        Pokemon.BattlePrefabs.TryGetValue(playerPokemon[x].name, out instance);
        //        //TODO handle setting level
        //        button.GetComponent<Button>().onClick.AddListener(()=> encounterPokemon = instance);
        //    }
        //    else
        //    {
        //        return;
        //    }

        //}
    }
    private void LoadCatch(GameObject pokemon) {
        Pokemon.CatchingPrefabs.TryGetValue(pokemon.GetComponent<PokemonMove>().pokemonName, out encounterPokemon);
        gameState = GameState.Wild;
        SceneManager.LoadScene("Catching", LoadSceneMode.Single);
        Time.timeScale = 1;
    }

    private void LoadBattle(GameObject pokemon)
    {
        Pokemon.BattlePrefabs.TryGetValue(pokemon.GetComponent<PokemonMove>().pokemonName, out encounterPokemon);
        gameState = GameState.Battle;
        SceneManager.LoadScene("Battle", LoadSceneMode.Single);
        Time.timeScale = 1;
    }

    private void PrepareToExitOverworld() {
        overworldPos = player.transform.position;
        overworldRotation = player.transform.rotation;
        overworldSceneName = SceneManager.GetActiveScene().name;
        wildPokemon.Clear();
        foreach (GameObject pokemon in GameObject.FindGameObjectsWithTag("Pokemon"))
        {
            Destroy(pokemon);
        }
    }

    private bool isWinding;
    void WildUpdate() {

    }

    void BattleUpdate() { 
    
    }
}
