using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    RectTransform rt;
    Camera cam;
    public GameObject toFollow;
    public Pokemon pokemon;
    Slider slider;
    TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        cam = Camera.main;
        slider = GetComponent<Slider>();
        text = transform.Find("Current").GetComponent<TMP_Text>();
        transform.Find("Level").Find("Text (TMP)").GetComponent<TMP_Text>().text = pokemon.level.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        rt.position = cam.WorldToScreenPoint(toFollow.transform.position) + Vector3.up * 50;
        slider.value = pokemon.hp;
        text.text = pokemon.hp.ToString();
    }
}
