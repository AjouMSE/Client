using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Utils;

public class PlayerControllerTest : MonoBehaviour
{
    private PanelControllerTest _panelControllerTest;

    private const float Speed = 4.0f;

    enum AnimationState
    {
        Idle = 0,
        MoveFront = 1,
        MoveBack = 2,
        Attack = 3
    }

    private Animator _animator;

    public void Start()
    {
        _panelControllerTest = GameObject.Find("GameSceneControllerTest").GetComponent<PanelControllerTest>();

        _animator = GetComponent<Animator>();

        string range = "00100 00000 00000 00000 00000";
        Move(range);
    }

    public void Update()
    {

    }

    private void Move(string range)
    {

        (int asisI, int asisJ) = _panelControllerTest.GetIdx(transform.position);
        int tobeI = asisI + CustomUtils.ParseRange(range)[0].i;
        int tobeJ = asisJ + CustomUtils.ParseRange(range)[0].j;

        Vector3 tobePosition = _panelControllerTest.GetPosition(tobeI, tobeJ);

        StartCoroutine(MoveAction(tobePosition));
    }

    private IEnumerator MoveAction(Vector3 destination)
    {
        _animator.SetInteger("AnimationState", (int)AnimationState.MoveFront);

        Vector3 destinationPosition = new Vector3(destination.x, transform.position.y, destination.z);
        Vector3 dir = destinationPosition - transform.position;
        Vector3 dirNormalized = (destinationPosition - transform.position).normalized;

        while (dir.sqrMagnitude > 0.1f)
        {
            dir = destinationPosition - transform.position;
            dirNormalized = (destinationPosition - transform.position).normalized;

            transform.position += dirNormalized * Speed * Time.deltaTime;

            yield return null;
        }

        transform.position = destinationPosition;

        _animator.SetInteger("AnimationState", (int)AnimationState.Idle);
    }

    private List<(int i, int j)> ParseRange(int[,] range)
    {
        List<(int i, int j)> list = new List<(int, int)>();

        for (int i = 0; i < range.GetLength(0); i++)
        {
            for (int j = 0; j < range.GetLength(1); j++)
            {
                if (range[i, j] == 0)
                {
                    continue;
                }

                list.Add((2 - i, j - 2));
            }
        }

        return list;
    }
}
