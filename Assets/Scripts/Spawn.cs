using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField]
    public string pokemonName;
    public GameObject wildPrefab;
    public float chance;
    public int minLevel;
    public int maxLevel;

    
    void Start()
    {
        Pokemon.WildPrefabs.TryGetValue(pokemonName, out wildPrefab);
    }
}
