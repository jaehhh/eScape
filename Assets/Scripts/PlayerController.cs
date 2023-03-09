using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input KeyCodes")]
    [SerializeField]
    private KeyCode keyCodeRun = KeyCode.LeftShift;
    [SerializeField]
    private KeyCode keyCodeJump = KeyCode.Space;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipWalk;
    /*[SerializeField]
    private AudioClip audioClipRun;*/

    private RotateToMouse rotateToMouse;
    private MovementCharacterController movement;
    private Status status;
    private PlayerAnimatorController animator;
    private Interact interact;
    private AudioSource audioSource;

    public bool isJumping = false;
    private int interactionType;

    private void Awake()
    {
        CursorLock();

        rotateToMouse = GetComponent<RotateToMouse>();
        movement = GetComponent<MovementCharacterController>();
        status = GetComponent<Status>();
        animator = GetComponent<PlayerAnimatorController>();
        interact = GetComponent<Interact>();
        audioSource = GetComponent<AudioSource>();
    }

    private void CursorLock()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        UpdateRotate();
        UpdateMove();
        UpdateJump();
        UpdateInteract();
    }

    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }

    private void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // 이동중
        if (x != 0 || z != 0)
        {
            bool isRun = false;

            // 앞으로 이동중일 때만
            if (z > 0)
                isRun = Input.GetKey(keyCodeRun);

            movement.MoveSpeed = isRun == true ? status.RunSpeed : status.WalkSpeed;
            animator.MoveSpeed = isRun == true ? 1 : 0.5f;

            if (audioSource.isPlaying == false && isJumping == false)
            {
                audioSource.loop = true;
                audioSource.Play();
            } 
        }
        else
        {
            movement.MoveSpeed = 0;
            animator.MoveSpeed = 0;
            
            if (audioSource.isPlaying == true)
            {
                audioSource.Stop();
            }
        }

        movement.MoveTo(new Vector3(x, 0, z));
    }

    private void UpdateJump()
    {
        if (Input.GetKeyDown(keyCodeJump) && isJumping == false)
        {
            isJumping = movement.Jump();
            if(isJumping)
            {
                audioSource.Stop();
            }
        }
    }

    private void UpdateInteract()
    {
        if (interactionType == 0 && Input.GetMouseButtonDown(0))
        {
            interact.MainInteract();

            interactionType = 1;
        }
        else if(interactionType == 0 && Input.GetKeyDown(KeyCode.E))
        {
            interact.MainInteract();

            interactionType = 2;
        }

        else if (interactionType == 1 && Input.GetMouseButtonUp(0))
        {
            interact.StopMainInteract();

            interactionType = 0;
        }
        else if (interactionType == 2 && Input.GetKeyUp(KeyCode.E))
        {
            interact.StopMainInteract();

            interactionType = 0;
        }
    }
}
