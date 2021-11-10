using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CooldownTimer : MonoBehaviour
{
    RectTransform rt;
    Camera cam;
    public GameObject toFollow;
    public Pokemon pokemon;
    Slider slider;
    TMP_Text text;
    public int index;
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        slider = GetComponent<Slider>();
        text = transform.Find("Time").GetComponent<TMP_Text>();
        rt.position = new Vector3((Screen.width / 12) * 3 * (index+1), Screen.height/24, 0);
        image = transform.Find("Ability").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = pokemon.cooldowns[index] - pokemon.timeRemaining[index];
        text.text = pokemon.timeRemaining[index].ToString("n2");
        if (pokemon.timeRemaining[index] > 0)
        {
            image.color = Color.grey;
        }
        else {
            image.color = Color.green;
        }
    }
}
