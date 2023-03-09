using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 밟을 수 있는 발판 오브젝트
// 밟으면 연결되어 있는 DoorObject에 정보 전달
// DoorObject 가 열고 닫힐 수 있도록 컨트롤한다
public class GroundButtonObject : InteractionObject
{
    [Header("Serializable Others")]
    [SerializeField]
    private GameObject nextInteractionObject;

    [Header("Setting Values")]
    [SerializeField]
    private Vector3 movingDistance;
    [SerializeField]
    private float movingAnimationTime;
    [SerializeField]
    private string key; 
    [SerializeField]
    private bool isKey = false; // 키로써 작용하는가
    [SerializeField]
    private bool FreezeWhenClick = false; // 한 번 클릭했을 때 반영구적으로 누른 상태로 전환되는가
    [SerializeField]
    private bool needInOrder = false; // 순서대로 눌러야 하는가. true 세팅할 시 FreezeWhenClick을 true 해줄 것

    // Button Status
    private bool isClicked = false;

    private Vector3 originalPosition;
    private Vector3 destinationPosition;

    private Transform objectThatInteractsThis;
    private bool doesSuccess = false;

    private void Awake()
    {
        originalPosition = transform.position;
        destinationPosition = originalPosition + movingDistance;

        nextInteractionObject.GetComponent<InteractionObject>().successEvent.AddListener(Success);

        StartCoroutine("CheckSuccess");
    }

    public void OnCollisionEnter(Collision collision)
    {
        // 이미 성공했으면 리턴
        if (doesSuccess) return;
        // 눌러져있다면 리턴
        if (FreezeWhenClick && isClicked == true) return;

        isClicked = true;

        StopCoroutine("MoveAnimation");
        StartCoroutine("MoveAnimation", true);

        // 버튼 위에 올라온 물건을(플레이어가 아닐경우에만) 자식으로 설정
        if (collision.transform.CompareTag("Player") == false)
        {
            collision.gameObject.transform.parent.SetParent(this.transform);
        }

        if (isKey && collision.transform.CompareTag(key))
        {
            if (needInOrder)
            {
                nextInteractionObject.GetComponent<InteractionObject>().AddPointInOrder(1, this.gameObject);
            }
            else
            {
                nextInteractionObject.GetComponent<InteractionObject>().AddPoint(1);
            }
        }
        else if (isKey == false && collision.transform.CompareTag(key) && needInOrder)
        {
            nextInteractionObject.GetComponent<InteractionObject>().AddPointInOrder(1, this.gameObject);
        }

        objectThatInteractsThis = collision.transform;
    }

    public void OnCollisionExit(Collision collision)
    {
        // 정답을 이미 맞춘 상태면 리턴
        if (doesSuccess == true) return;
        // 누른 상태로 유지되는 버튼이면 버튼 위에서 벗어나도 계속 눌려 있다
        if (FreezeWhenClick == true) return;

        isClicked = false;

        StopCoroutine("MoveAnimation");
        StartCoroutine("MoveAnimation", false);

        if (isKey && collision.transform.CompareTag(key))
        {
            if (needInOrder)
            {
                // 순서대로 눌러야하는 버튼이면 일단 아무것도 안함
            }
            else
            {
                nextInteractionObject.GetComponent<InteractionObject>().AddPoint(-1);
            }
        }

        objectThatInteractsThis = null;
    }


    private IEnumerator MoveAnimation(bool colliderEnter)
    {
        if(GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().Play();
        }

        float totalDistance = Vector3.Distance(originalPosition, destinationPosition);
        float currentDistance = Vector3.Distance(originalPosition, transform.position);

        // percent 1 : 다 눌렸을 때, percent = 0 눌리지 않았을 때
        float percent = currentDistance / totalDistance;
        percent = Mathf.Abs(percent);

        if (colliderEnter && totalDistance != 0)
        {
            while (percent < 1)
            {
                percent += Time.deltaTime / movingAnimationTime;

                transform.position = new Vector3(originalPosition.x + (movingDistance.x * percent),
                                                                     originalPosition.y + (movingDistance.y * percent),
                                                                     originalPosition.z + (movingDistance.z * percent));

                yield return null;
            }
        }
        else if(colliderEnter == false && totalDistance != 0)
        {
            while (percent > 0)
            {
                percent -= Time.deltaTime / movingAnimationTime;

                transform.position = new Vector3(originalPosition.x + (movingDistance.x * percent),
                                                                     originalPosition.y + (movingDistance.y * percent),
                                                                     originalPosition.z + (movingDistance.z * percent));

                yield return null;
            }
        }
    }

    public void Success()
    {
        doesSuccess = true;
    }

    private IEnumerator CheckSuccess()
    {
        while(true)
        {
            if(doesSuccess)
            {
                FreezeObjectThatInteractsThis();

                break;
            }

            yield return null;
        }
    }

    // 오브젝트를 올려서 상호작용하는 버튼이면 정답에 도달할 시 그 오브젝트를 다시 들 수 없도록 한다
    private void FreezeObjectThatInteractsThis()
    {
        if (objectThatInteractsThis == null) return;

        if (objectThatInteractsThis.CompareTag("Player")) return;

        objectThatInteractsThis.GetComponent<InteractionObject>().ableToInteract = false;
    }

    public override void ResetAsFail()
    {
        isClicked = false;

        StopCoroutine("MoveAnimation");
        StartCoroutine("MoveAnimation", false);
    }
}   

