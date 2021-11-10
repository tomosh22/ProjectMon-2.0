using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PikachuBattle : MonoBehaviour
{
    public void E(AbilityData data)
    {
        StartCoroutine(Coroutine(data));
    }
    public IEnumerator Coroutine(AbilityData data)
    {
        data.agent.isStopped = true;
        Instantiate(Resources.Load("Abilities/Pikachu/PikachuE"), data.gameObject.transform.position, data.gameObject.transform.rotation);
        
        RaycastHit hit;
        Physics.Raycast(data.cam.ScreenPointToRay(Input.mousePosition), out hit);
        Vector3 hitPoint = hit.point;
        hitPoint.y = data.gameObject.transform.position.y;
        yield return new WaitForSeconds(0.35f);
        data.agent.Warp(hitPoint);
        Instantiate(Resources.Load("Abilities/Pikachu/PikachuE"), hitPoint, data.gameObject.transform.rotation);

    }
}
