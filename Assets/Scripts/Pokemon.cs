using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public abstract class Pokemon
{
    public GameObject wildPrefab;
    public GameObject catchingPrefab;
    public GameObject battlePrefab;
    public Func<Pokemon> CreateInstance;
    public abstract void Q(AbilityData data);
    public abstract void W(AbilityData data);
    public abstract void E(AbilityData data);
    public abstract void BattleBehaviour(NavMeshAgent agent, GameObject enemyObject);
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
        this.hp = this.baseHp * level;
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
    public GameObject gameObject;
    public Pokemon currentPokemon;
    public NavMeshAgent agent;
    public Vector3 targetPos;
    public bool isPlayer;
    public GameObject enemyObject;
}

public class Pikachu : Pokemon {
    new public static GameObject wildPrefab = Resources.Load("Pokemon/Wild/PikachuWild") as GameObject;
    new public static GameObject catchingPrefab = Resources.Load("Pokemon/Catching/PikachuCatching") as GameObject;
    new public static GameObject battlePrefab = Resources.Load("Pokemon/Battle/PikachuBattle") as GameObject;
    new public static Func<Pikachu> CreateInstance = () => { return new Pikachu(); };

    public Pikachu() {
        this.name = "Pikachu";
        this.type = Type.Electric;
        this.moveSpeed = 6f;
        this.hp = 10;
        this.baseHp = 10;
        this.cooldowns = new float[4] { 2f, 3f, 0f, 0f };
        this.timeRemaining = new float[4] { 0, 0, 0, 0 };
    }

    enum Quadrant
    {
        topRight, topLeft, bottomRight, bottomLeft,None
    }
    Dictionary<Quadrant, Quadrant> OppositeQuadrants = new Dictionary<Quadrant, Quadrant>() {
        {Quadrant.topRight,Quadrant.bottomLeft },
        {Quadrant.topLeft,Quadrant.bottomRight },
        {Quadrant.bottomRight,Quadrant.topLeft },
        {Quadrant.bottomLeft,Quadrant.topRight }
    };
    Dictionary<Quadrant, Vector3> QuadrantCenters = new Dictionary<Quadrant, Vector3>() {
        {Quadrant.topLeft, new Vector3(6,0,13) },
        {Quadrant.topRight, new Vector3(15,0,12) },
        {Quadrant.bottomLeft, new Vector3(8,0,5) },
        {Quadrant.bottomRight, new Vector3(15,0,4) }

    };
    public override void BattleBehaviour(NavMeshAgent agent, GameObject enemyObject)
    {
        
        Quadrant quadrant;
        Vector3 enemyPos = enemyObject.transform.position;
        
        if (enemyPos.x >= 11)
        {
            if (enemyPos.z >= 8)
            {
                quadrant = Quadrant.topRight;
            }
            else {
                quadrant = Quadrant.bottomRight;
            }
        }
        else {
            if (enemyPos.z >= 8)
            {
                quadrant = Quadrant.topLeft;
            }
            else {
                quadrant = Quadrant.bottomLeft;
            }
        }
        Vector3 destination;
        if (agent.remainingDistance < 0.1f) {
            Vector2 offset = UnityEngine.Random.insideUnitCircle * 3;
            destination = QuadrantCenters[OppositeQuadrants[quadrant]] + new Vector3(offset.x, 0, offset.y);
            NavMeshHit hit;
            NavMesh.SamplePosition(destination, out hit, 3, 1);
            agent.destination = hit.position;
            
        }
    }

    public override void Q(AbilityData data)
    {
        Vector3 target;
        if (data.isPlayer)
        {
            RaycastHit hit;
            Physics.Raycast(data.cam.ScreenPointToRay(Input.mousePosition), out hit);
            target = hit.point;
            target.y = data.gameObject.transform.position.y;
        }
        else {
            target = data.enemyObject.transform.position;
        }
        data.gameObject.transform.LookAt(target);
        GameObject q = UnityEngine.Object.Instantiate(Resources.Load("Abilities/Pikachu/PikachuQ"), data.gameObject.transform.position + data.gameObject.transform.forward, data.gameObject.transform.rotation) as GameObject;
        q.GetComponent<PikachuQ>().level = data.currentPokemon.level;
        q.GetComponent<PikachuQ>().caster = data.gameObject;
        
    }
    public override void W(AbilityData data)
    {
        Vector3 target;
        if (data.isPlayer)
        {
            RaycastHit hit;
            Physics.Raycast(data.cam.ScreenPointToRay(Input.mousePosition), out hit);
            target = hit.point;
            target.y = data.gameObject.transform.position.y;
        }
        else {
            target = data.enemyObject.transform.position; 
        }
        
        data.gameObject.transform.LookAt(target);
        GameObject w = UnityEngine.Object.Instantiate(Resources.Load("Abilities/Pikachu/PikachuW"), data.gameObject.transform.position + data.gameObject.transform.forward, data.gameObject.transform.rotation) as GameObject;
        w.GetComponent<PikachuW>().caster = data.gameObject;
        //Time.timeScale = 0;
    }

    public override void E(AbilityData data)
    {

        data.gameObject.GetComponent<PikachuBattle>().E(data);
    }

    
}
