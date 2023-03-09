using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTrigger : MonoBehaviour
{
    private enum DisenableWay {MouseClick =0}

    [SerializeField]
    private string key;
    [SerializeField]
    private GameObject enableTarget;
    [SerializeField]
    private DisenableWay disenableWay = DisenableWay.MouseClick;

    private bool disenableInput = false;
    private bool endTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (endTrigger == true) return;
 
        if (other.transform.CompareTag(key))
        {
            enableTarget.SetActive(true);
            endTrigger = true;

            StartCoroutine("DisenableUpdate");
            StartCoroutine("InputUpdate");

        }
    }

    private IEnumerator DisenableUpdate()
    {
        while(disenableInput == false)
        {
            yield return null;
        }

        enableTarget.SetActive(false);

        StopAllCoroutines();
    }

    private IEnumerator InputUpdate()
    {
        while(true)
        {
            if(disenableWay == DisenableWay.MouseClick && Input.GetMouseButtonDown(0))
            {
                break;
            }

            yield return null;
        }

        disenableInput = true;
    }
}
