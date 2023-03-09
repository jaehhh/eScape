using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickButtonObject : InteractionObject
{
    [Header("Serializable Others")]
    [SerializeField]
    private GameObject nextInteractionObject;

    // My Component
    private AudioSource audioSource;

    [Header("Setting Values")]
    [SerializeField]
    private Vector3 movingDistance;
    [SerializeField]
    private float movingAnimationTime;
    [SerializeField]
    private bool isKey = false; // 키로써 작용하는가
    [SerializeField]
    private bool canReClick = false; // 다시 눌러서 버튼을 취소할 수 있는가
    [SerializeField]
    private bool canClickWhileMove = false; // 버튼이 움직이는 동안 누를 수 있는가

    // Button State
    private IsButtonClicked isButtonClicked = IsButtonClicked.noClicked; // 버튼을 눌러서 활성화한 상태인가
    private bool isMoving = false; // 눌러서 버튼이 움직이는 상태인가
    private bool isFreezing = false; // 누를 수 있는 상태인가. 퍼즐 완료하면 true

    private Vector3 originalPosition;
    private Vector3 destinationPosition;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        originalPosition = transform.position;
        destinationPosition = originalPosition + movingDistance;

        nextInteractionObject.GetComponent<InteractionObject>().successEvent.AddListener(FreezeButton);
    }

    public override void StartMainInteract(Interact interact)
    {
        if (isFreezing == true) return;

        // 움직임 중에 못 누르는 오브젝트가 움직이고 상태면 누를 수 없음
        if (canClickWhileMove == false && isMoving == true) return;

        // 이미 버튼이 눌려있고 다시 클릭할 수 없으면 누를 수 없음
        if (isButtonClicked == IsButtonClicked.clicked && canReClick == false) return;

        if(isButtonClicked == IsButtonClicked.noClicked)
        {
            ButtonClick();
        }
        else
        {
            ButtonReClick();
        }
    }

    private void ButtonClick()
    {
        if(isKey)
        {
            nextInteractionObject.GetComponent<InteractionObject>().AddPoint(1);
        }

        StopCoroutine("MoveAnimation");
        StartCoroutine("MoveAnimation", true);

        isButtonClicked = IsButtonClicked.clicked;
    }

    private void ButtonReClick()
    {
        if (isKey)
        {
            nextInteractionObject.GetComponent<InteractionObject>().AddPoint(-1);
        }

        StopCoroutine("MoveAnimation");
        StartCoroutine("MoveAnimation", false);

        isButtonClicked = IsButtonClicked.noClicked;
    }

    private IEnumerator MoveAnimation(bool clickFirstStep)
    {
        audioSource.Stop();
        audioSource.Play();

        isMoving = true;

        float totalDistance = Vector3.Distance(originalPosition, destinationPosition);
        float currentDistance = Vector3.Distance(originalPosition, transform.position);

        // percent 1 : 다 눌렸을 때, percent = 0 눌리지 않았을 때
        float percent = currentDistance/totalDistance;
        percent = Mathf.Abs(percent);

        if(clickFirstStep)
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
        else
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

        isMoving = false;
    }

    private void FreezeButton()
    {
        isFreezing = true;
    }
}
