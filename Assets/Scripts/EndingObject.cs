using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingObject : InteractionObject
{
    [Header("GameObject")]
    [SerializeField]
    private Image screen;
    [SerializeField]
    private Image successUI;

    [Header("Setting Values")]
    [SerializeField]
    private int needPoint = 1;
    private int currentPoint = 0;
    private float fadeTime = 5;

    public override void AddPoint(int point)
    {
        currentPoint += point;

        CheckSuccess();
    }

    private void CheckSuccess()
    {
        if (currentPoint == needPoint)
        {
            base.successEvent.Invoke();

            StartCoroutine("ScreenFade");
        }
    }
    
    private IEnumerator ScreenFade()
    {
        successUI.gameObject.SetActive(true);
        screen.gameObject.SetActive(true);

        float percent = 0;

        yield return new WaitForSeconds(2f);

        while(percent < 1)
        {
            percent += Time.deltaTime / fadeTime;

            Color color = screen.color;

            color.a = percent;

            screen.color = color;

            yield return null;
        }

        SceneChange();
    }

    private void SceneChange()
    {
        SceneManager.LoadScene("Ending");
    }
}
