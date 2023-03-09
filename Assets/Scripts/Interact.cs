using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField]
    private float rayDistance = 2f;
    [SerializeField]
    private float carryDistance = 2;

    private Transform target;
    [HideInInspector]
    public Vector3 carryPosition;

    public void MainInteract()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);

        if(Physics.Raycast(ray, out hit, rayDistance))
        {
            if(hit.transform.GetComponent<InteractionObject>() != null)
            {
                target = hit.transform;

                target.GetComponent<InteractionObject>().StartMainInteract(this);

                StartCoroutine("CarryObject");
            }
        }
    }

    private IEnumerator CarryObject()
    {
        Ray ray;

        while (true)
        {
            ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);

            carryPosition = ray.origin + ray.direction * carryDistance;

            yield return null;
        }
    }

    public void StopMainInteract()
    {
        if (target != null)
        {
            target.GetComponent<InteractionObject>().StopMainInteract(this);

            target = null;
        }
    }
}
