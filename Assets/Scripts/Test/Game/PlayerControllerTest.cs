using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Utils;

public class PlayerControllerTest : MonoBehaviour
{
    private PanelControllerTest _panelControllerTest;

    private const float Speed = 4.0f;
    private int _idx, _x, _y;
    private BitMask.Bits30Field _bitsIdx;

    enum AnimationState
    {
        Idle = 0,
        MoveFront = 1,
        MoveBack = 2,
        Attack = 3
    }

    private Animator _animator;

    private void InitPos()
    {
        _x = 0;
        _y = 2;
        _idx = _y * 6 + _x;
        _bitsIdx.element = 1 << _idx;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _panelControllerTest = GameObject.Find("GameSceneControllerTest").GetComponent<PanelControllerTest>();
        InitPos();

        string range = "00000 00000 00001 00000 00000";
        BitMask.Bits25Field skillRange = new BitMask.Bits25Field(range);
        BitMask.PrintBits25(skillRange);
        BitMask.Bits30Field fieldRange = BitMask.CvtBits25ToBits30(skillRange);
        BitMask.PrintBits30(fieldRange);
        
        int dx = _x - 3;
        int dy = _y - 2;
        BitMask.ShiftBits30(ref fieldRange, dx, dy);
        BitMask.PrintBits30(fieldRange);

        int mask = (1 << 29);
        for (int i = 0; i < 30; i++)
        {
            if ((fieldRange.element & mask) > 0)
            {
                int px = i % 6;
                int py = i / 6;
                Debug.Log($"{i} / {px} / {py}");
                _panelControllerTest.GetPanel(py, px).GetComponent<MeshRenderer>().material.color = new Color(1, 0.5f, 0.5f);
            }
            mask = mask >> 1;
        }
        
        Move(range);
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
