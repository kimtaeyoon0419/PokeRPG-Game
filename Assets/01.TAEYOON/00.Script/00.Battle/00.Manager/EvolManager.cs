// # System
using PokeRPG.Battle.Unit;
using System.Collections;
using System.Collections.Generic;
using TMPro;

// # Unity
using UnityEngine;

public class EvolManager : MonoBehaviour
{
    public static EvolManager instance;
    public List<GameObject> evolUnits = new List<GameObject>();

    public Transform evolPos;
    public TextMeshProUGUI evolText;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public IEnumerator EvolustionMonster()
    {
        GameObject unitMonster = Instantiate(evolUnits[0].gameObject, evolPos.position, Quaternion.identity);
        UnitProfile unitProfile = evolUnits[0].GetComponent<UnitProfile>();

        yield return StartCoroutine(FadeManager.instance.Co_FadeIn());

        Debug.Log("페이드 끄ㅌ");

        for (int i = 0; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
        
        evolText.text = unitProfile.unitName + "의 모습이...!!!!!";

        yield return new WaitForSeconds(1);

        evolText.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.3f);
    }
}
