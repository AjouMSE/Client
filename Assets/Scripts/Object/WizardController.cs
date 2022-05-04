using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : MonoBehaviour
{
    enum AnimationState
    {
        Idle = 0,
        MoveFront = 1,
        MoveBack = 2,
        Attack = 3
    }

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _animator.SetInteger("AnimationState", (int)AnimationState.Idle);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _animator.SetInteger("AnimationState", (int)AnimationState.MoveFront);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _animator.SetInteger("AnimationState", (int)AnimationState.MoveBack);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _animator.SetInteger("AnimationState", (int)AnimationState.Attack);
        }
    }
}
