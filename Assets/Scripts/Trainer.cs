using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Trainer : MonoBehaviour
{
    public GameManager gm;
    public Pokemon trainerPokemon;
    public GameObject trainerSpeechPrefab;
    public TrainerPokemon tp;
    public int trainerID;
    private bool isActive;
    private int index;
    private TMP_Text textComponent;
    List<string> speech;
    public Material readyMat;
    public Material beatenMat;
    public bool beaten;
    public bool waitForHitboxExit;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        tp = GetComponent<TrainerPokemon>();
        isActive = false;
        foreach (int id in gm.beatenTrainerIDs) {
            if (id == trainerID) {
                beaten = true;
                GetComponent<Renderer>().material = beatenMat;
                return;
            }
        }
        Func<Pokemon> pokemonCreator;
        Pokemon.InstanceCreators.TryGetValue(tp.pokemonName, out pokemonCreator);
        trainerPokemon = pokemonCreator();
        trainerPokemon.SetLevel(tp.level);
    }

    // Update is called once per frame
    void Update()
    {
        if (beaten) { return; }
        if (isActive && Input.GetKeyDown(KeyCode.Return)) {
            index++;
            if (index < speech.Count)
            {
                textComponent.text = speech[index];
            }
            else {
                gm.PrepareToExitOverworld();
                Debug.Log("in trainer.cs level = " + trainerPokemon.level.ToString() + " and hp = " + trainerPokemon.hp.ToString());
                gm.xpModifier = 1;
                gm.currentTrainerID = trainerID;
                
                gm.LoadBattle(trainerPokemon);

            }
            
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (beaten || waitForHitboxExit) { return; }
        
        if (other.gameObject.CompareTag("Player")) {
            isActive = true;
            TrainerSpeeches.trainerSpeeches.TryGetValue(trainerID, out speech);
            Debug.Log("speech: " + speech[0]);
            gm.PrepareToExitOverworld();
            Time.timeScale = 0;
            trainerSpeechPrefab = Instantiate(trainerSpeechPrefab, Vector3.zero, Quaternion.identity);
            trainerSpeechPrefab.transform.SetParent(FindObjectOfType<Canvas>().transform);
            RectTransform rt = trainerSpeechPrefab.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(0,Screen.width / 5,0);
            index = 0;

            textComponent = trainerSpeechPrefab.transform.Find("Speech").GetComponent<TMP_Text>();
            textComponent.text = speech[index];
            
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        waitForHitboxExit = false;
    }
}
