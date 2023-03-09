using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObject : InteractionObject
{
    [Header("Setting Values")]
    [SerializeField]
    private int needPoint = 1;
    private int currentPoint = 0;

    // 수동설정문
    [SerializeField]
    private Vector3 movingDistance;
    [SerializeField]
    private float movingAnimationTime;

    private Animation doorAnimation;

    private void Awake()
    {
        doorAnimation = GetComponentInChildren<Animation>();
    }

    public override void AddPoint(int point)
    {
        currentPoint += point;

        CheckSuccess();
    }

    private void CheckSuccess()
    {
        if(currentPoint == needPoint)
        {
            base.successEvent.Invoke();

            // StartCoroutine("MoveAnimation");

            doorAnimation.Play("open");

            GetComponent<AudioSource>().Play();
        }
    }

    private IEnumerator MoveAnimation()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().Play();

        float percent = 0;
        float currentTime = 0;
        Vector3 currentPosition = transform.position;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / movingAnimationTime;

            transform.position = new Vector3(currentPosition.x + (movingDistance.x * percent),
                                             currentPosition.y + (movingDistance.y * percent),
                                             currentPosition.z + (movingDistance.z * percent));

            yield return null;
        }
    }
}
