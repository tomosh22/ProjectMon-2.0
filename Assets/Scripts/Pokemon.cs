using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pokemon
{
    public GameObject wildPrefab;
    public GameObject catchingPrefab;
    public GameObject battlePrefab;
    public Func<Pokemon> CreateInstance;
    public abstract void Q(AbilityData data);
    public abstract void W(AbilityData data);
    public float moveSpeed;
    public string name;
    public Type type;
    public int level;
    public int xp;
    public int hp;
    public int baseHp;
    public float[] cooldowns;
    public float[] timeRemaining;
    public static Dictionary<string,Attack> Attacks = new Dictionary<string, Attack>() {
        {"Thunderbolt", new Attack(80,1f,Type.Electric,20,"Thunderbolt") }
    };
    public static Dictionary<string, GameObject> WildPrefabs = new Dictionary<string, GameObject>() {
        {"Pikachu", Pikachu.wildPrefab }
    };
    public static Dictionary<string, GameObject> CatchingPrefabs = new Dictionary<string, GameObject>() {
        {"Pikachu", Pikachu.catchingPrefab }
    };
    public static Dictionary<string, GameObject> BattlePrefabs = new Dictionary<string, GameObject>() {
        {"Pikachu", Pikachu.battlePrefab }
    };
    public static Dictionary<string, Func<Pokemon>> InstanceCreators = new Dictionary<string, Func<Pokemon>>() {
        {"Pikachu", Pikachu.CreateInstance }
    };
    public enum Type
    {
        Normal,Fighting,Flying,Poison, Ground,Rock,Bug,
        Ghost, Steel,Fire, Water,Grass,Electric,Psychic,
        Ice,Dragon,Dark
    }


    public Pokemon() {
        this.level = 1;
        this.xp = 0;
    }

    public void LevelUp() {
        Debug.Log("levelling up " + this.name);
        this.level += 1;
        this.hp = this.baseHp * this.level;
    }

    public void SetLevel(int level) {
        this.level = level;
    }

    public struct Attack {
        public int damage;
        public float accuracy;
        public Type type;
        public int uses;
        public string name;
        public Attack(int damage, float accuracy, Type type, int uses, string name) {
            this.damage = damage;
            this.accuracy = accuracy;
            this.type = type;
            this.uses = uses;
            this.name = name;
        }
    }
}

public struct AbilityData {
    public Camera cam;
    public GameObject player;
    public Pokemon playerPokemon;
}

public class Pikachu : Pokemon {
    new public static GameObject wildPrefab = Resources.Load("Pokemon/Wild/PikachuWild") as GameObject;
    new public static GameObject catchingPrefab = Resources.Load("Pokemon/Catching/PikachuCatching") as GameObject;
    new public static GameObject battlePrefab = Resources.Load("Pokemon/Battle/PikachuBattle") as GameObject;
    new public static Func<Pikachu> CreateInstance = () => {return new Pikachu();};
    
    public Pikachu() {
        this.name = "Pikachu";
        this.type = Type.Electric;
        this.moveSpeed = 6f;
        this.hp = 10;
        this.baseHp = 10;
        this.cooldowns = new float[4]{2f,3f,0f,0f};
        this.timeRemaining = new float[4] { 0, 0, 0, 0 };
    }

    public override void Q(AbilityData data)
    {
        RaycastHit hit;
        Physics.Raycast(data.cam.ScreenPointToRay(Input.mousePosition), out hit);
        Vector3 hitPoint = hit.point;
        hitPoint.y = data.player.transform.position.y;
        data.player.transform.LookAt(hitPoint);
        GameObject q = UnityEngine.Object.Instantiate(Resources.Load("Abilities/Pikachu/PikachuQ"), data.player.transform.position + data.player.transform.forward, data.player.transform.rotation) as GameObject;
        q.GetComponent<PikachuQ>().level = data.playerPokemon.level;
        
    }
    public override void W(AbilityData data)
    {
        RaycastHit hit;
        Physics.Raycast(data.cam.ScreenPointToRay(Input.mousePosition), out hit);
        Vector3 hitPoint = hit.point;
        hitPoint.y = data.player.transform.position.y;
        data.player.transform.LookAt(hitPoint);
        UnityEngine.Object.Instantiate(Resources.Load("Abilities/Pikachu/PikachuW"), data.player.transform.position + data.player.transform.forward, data.player.transform.rotation);
        
        //Time.timeScale = 0;
    }
}
