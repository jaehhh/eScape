using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 들 수 있는 사물
public class CarriedObject : InteractionObject
{
    [SerializeField]
    private Vector3 adjustCarriedPosition; // 위치 조정

    private Interact interact;

    public override void StartMainInteract(Interact interact)
    {
        if(base.ableToInteract == false)
        {
            return;
        }

        this.interact = interact;

        StartCoroutine("Carried");
    }

    private IEnumerator Carried()
    {
        // 바닥버튼과 부모자식 해체
        this.transform.parent.SetParent(null);

        GetComponent<Rigidbody>().useGravity = false;

        while(true)
        {
            transform.position = interact.carryPosition + adjustCarriedPosition;

            yield return null;
        }
    }

    public override void StopMainInteract(Interact interact)
    {
        StopCoroutine("Carried");

        GetComponent<Rigidbody>().useGravity = true;

        this.interact = null;
    }

    private void OnCollisionStay(Collision collision)
    {
        StopMainInteract(null);
    }
}
