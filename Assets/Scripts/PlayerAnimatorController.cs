using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;

    public float MoveSpeed
    {
        set => animator.SetFloat("movementSpeed", value);
        get => animator.GetFloat("movementSpeed");
    }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Play(string stateName, int layer, float normalizedTime)
    {
        animator.Play(stateName, layer, normalizedTime);
    }

    public bool CurrentAnimationIs(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}
