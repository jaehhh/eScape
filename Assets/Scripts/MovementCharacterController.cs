using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 해당 스크립트(MovememtCharacterController)를 게임 오브젝트에 넣으면 매개변수(CharacterController)컴포넌트도 같이 넣어짐
[RequireComponent(typeof(CharacterController))]

public class MovementCharacterController : MonoBehaviour
{
    private PlayerController playerController;
    private CharacterController characterController;

    [SerializeField]
    private float moveSpeed; // 이동속도
    private Vector3 moveForce; // 실제 연산에 적용되는 이동 힘

    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float gravity;

    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);
        get => moveSpeed;
    }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!characterController.isGrounded)
        {
            moveForce.y += gravity * Time.deltaTime;
        }

        // 1초당 moveForce 속력으로 이동
        characterController.Move(moveForce * Time.deltaTime);
    }

    public void MoveTo(Vector3 direction)
    {
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);

        moveForce = new Vector3(direction.x * moveSpeed, moveForce.y, direction.z * moveSpeed);
    }

    public bool Jump()
    {
        if(characterController.isGrounded)
        {
            moveForce.y = jumpForce;

            StartCoroutine("DoneJump");

            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator DoneJump()
    {
        yield return StartCoroutine("JumpTrigger");

        while(!characterController.isGrounded)
        {
            yield return null;
        }

        playerController.isJumping = false;
    }

    // isGrounded 가 false가 될 때 까지
    private IEnumerator JumpTrigger()
    {
        while(characterController.isGrounded)
        {
            yield return null;
        }
    }
}
