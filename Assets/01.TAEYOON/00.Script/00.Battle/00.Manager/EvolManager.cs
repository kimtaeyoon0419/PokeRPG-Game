// # System
using PokeRPG.Battle;
using PokeRPG.Battle.Unit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;

// # Unity
using UnityEngine;

public class EvolManager : MonoBehaviour
{
    public static EvolManager instance;
    public List<GameObject> evolUnits = new List<GameObject>();

    public Transform evolPos;
    public GameObject evolLook;
    public GameObject evolSphere;
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
        unitMonster.transform.LookAt(evolLook.transform);
        float rotateTime = 10f;
        float turnspeed = 1f;
        Vector3 originaScale = evolSphere.transform.localScale;

        //BattleManager.instance.text.gameObject.SetActive(false);
        //BattleManager.instance.textPanel.gameObject.SetActive(false);

        yield return StartCoroutine(BattleManager.instance.Co_FadeIn());

        //BattleManager.instance.text.gameObject.SetActive(true);
        BattleManager.instance.textPanel.gameObject.SetActive(true);


        BattleManager.instance.text.text = "어랏 " + unitProfile.unitName + "의 모습이.";

        yield return new WaitForSeconds(0.5f);

        BattleManager.instance.text.text = "어랏 " + unitProfile.unitName + "의 모습이..";

        yield return new WaitForSeconds(0.5f);

        BattleManager.instance.text.text = "어랏 " + unitProfile.unitName + "의 모습이...";

        yield return new WaitForSeconds(0.5f);

        BattleManager.instance.text.text = "어랏 " + unitProfile.unitName + "의 모습이...!";

        yield return new WaitForSeconds(0.5f);

        BattleManager.instance.textPanel.gameObject.SetActive(false);

        evolSphere.SetActive(true);

        while (rotateTime > 0)
        {
            Debug.Log("회전중");
            evolSphere.transform.localScale = originaScale * Time.deltaTime;
            unitMonster.transform.Rotate(new Vector3(0, 1, 0) * turnspeed * Time.deltaTime);
            rotateTime -= Time.deltaTime;
            turnspeed += 0.5f;
            yield return null;
        }
        unitMonster.transform.LookAt(evolLook.transform);
    }
}
