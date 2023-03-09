using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    [System.Serializable]
    public class SuccessEvent : UnityEngine.Events.UnityEvent { };
    [HideInInspector]
    public SuccessEvent successEvent = new SuccessEvent();

    [HideInInspector]
    public bool ableToInteract = true;

    public virtual void StartMainInteract(Interact interact)
    {

    }

    public virtual void StopMainInteract(Interact interact)
    {

    }

    public virtual void AddPoint(int point)
    {

    }

    public virtual void AddPointInOrder(int point, GameObject gameObject)
    {

    }

    public virtual void ResetAsFail()
    {

    }
}
