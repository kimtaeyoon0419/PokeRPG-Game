// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;

    public Image FadePanel;

    private void Awake()
    {
        instance = this;
    }

    public IEnumerator Co_FadeOut()
    {
        Color color = FadePanel.color;

        while (color.a < 1)
        {
            color.a += Time.deltaTime;
            yield return null;
            FadePanel.color = color;
        }

        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator Co_FadeIn()
    {
        Color color = FadePanel.color;
        color.a = 1;
        FadePanel.color = color;

        while (color.a >= 0)
        {
            color.a -= Time.deltaTime;
            yield return null;
            FadePanel.color = color;
        }
    }
}
