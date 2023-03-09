using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InOrderDoorObject : InteractionObject
{
    [Header("Setting Values")]
    [SerializeField]
    private int needPoint = 1;
    private int currentPoint = 0;
    [SerializeField]
    private Vector3 movingDistance;
    [SerializeField]
    private float movingAnimationTime;
    [SerializeField]
    private GameObject[] buttonInOrder; // 순서대로 등록해줄 것

    private List<GameObject> buttons; // 입력된 버튼 저장
    private Animation doorAnimation;

    private void Awake()
    {
        buttons = new List<GameObject>();
        doorAnimation = GetComponentInChildren<Animation>();
    }

    public override void AddPointInOrder(int point, GameObject InputTarget)
    {
        int index = buttons.Count;

        // 알맞는 순서의 오브젝트
        if (InputTarget.gameObject == buttonInOrder[index].gameObject)
        {
            buttons.Add(InputTarget);

            currentPoint += point;

            CheckSuccess();
        }

        else
        {
            buttons.Add(InputTarget);

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].GetComponent<InteractionObject>().ResetAsFail();

                currentPoint = 0;
            }

            buttons.Clear();
        }
    }

    private void CheckSuccess()
    {
        if (currentPoint == needPoint)
        {
            base.successEvent.Invoke();

            // StartCoroutine("MoveAnimation");

            doorAnimation.Play("open");

            GetComponent<AudioSource>().Play();
        }
    }

    private IEnumerator MoveAnimation()
    {
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
