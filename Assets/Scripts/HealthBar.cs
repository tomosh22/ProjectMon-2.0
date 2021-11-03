using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    RectTransform rt;
    Camera cam;
    public GameObject toFollow;
    public Pokemon pokemon;
    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        cam = Camera.main;
        slider = GetComponent<Slider>();
        pokemon = gameObject.GetComponent<BattleController>().pokemon;
    }

    // Update is called once per frame
    void Update()
    {
        rt.position = cam.WorldToScreenPoint(toFollow.transform.position) + Vector3.up * 50;
        slider.value = pokemon.hp;
    }
}
