using System.Collections;
using System.Collections.Generic;
using System;
using Core;
using Manager;
using Unity.Netcode;
using UnityEngine;
using Utils;

public class PlayerControllerTest : MonoBehaviour
{
    private PanelControllerTest _panelControllerTest;

    private const float Speed = 4.0f;
    private int _idx, _x, _y;
    private BitMask.BitField30 _bitsIdx;

    enum AnimationState
    {
        Idle = 0,
        MoveFront = 1,
        MoveBack = 2,
        Attack = 3
    }

    private Animator _animator;
    private string _animationState = "BattleState";

    // public override void OnNetworkSpawn()
    // {
    //     if (NetworkManager.Singleton.IsServer)
    //     {
    //         if (IsOwner)
    //         {
    //             transform.position = new Vector3(0, 0.3f, 6.4f);
    //             transform.localEulerAngles = new Vector3(0, 90, 0);
    //             GetComponentInChildren<SkinnedMeshRenderer>().material.color = new Color(0, 0, 1);
    //         }
    //         else
    //         {
    //             transform.position = new Vector3(21, 0.3f, 6.4f);
    //             transform.localEulerAngles = new Vector3(0, -90, 0);
    //             GetComponentInChildren<SkinnedMeshRenderer>().material.color = new Color(0, 1, 0);
    //         }
    //     }
    //     else
    //     {
    //         if (IsOwner)
    //             GetComponentInChildren<SkinnedMeshRenderer>().material.color = new Color(0, 1, 0);
    //         else
    //             GetComponentInChildren<SkinnedMeshRenderer>().material.color = new Color(0, 0, 1);
    //     }
    // }

    private void InitPos()
    {
        _x = 0;
        _y = 2;
        _idx = 12;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        //_panelControllerTest = GameObject.Find("GameSceneControllerTest").GetComponent<PanelControllerTest>();
        InitPos();
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     string range = "00000 00000 00001 00000 00000";
        //     BitMask.BitField25 skillRange = new BitMask.BitField25(range);
        //     skillRange.PrintBy2D();
        //     BitMask.BitField30 fieldRange = skillRange.CvtBits25ToBits30();
        //     fieldRange.PrintBy2D();
        //
        //     int dx = _x - 3;
        //     int dy = _y - 2;
        //     fieldRange.Shift(dx, dy);
        //     fieldRange.PrintBy2D();
        //
        //     int mask = BitMask.BitField30Msb;
        //     for (int i = 0; i < 30; i++)
        //     {
        //         if ((fieldRange.element & mask) > 0)
        //         {
        //             int px = i % 6;
        //             int py = i / 6;
        //             Debug.Log($"{i} / {px} / {py}");
        //             _panelControllerTest.GetPanel(px, py).GetComponent<MeshRenderer>().material.color = new Color(1, 0.5f, 0.5f);
        //         }
        //         mask >>= 1;
        //     }
        //
        //     _x += 2;
        //
        //     Move(range);
        // }

        if (!UserManager.Instance.IsHost)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _animator.SetInteger(_animationState, 1);
            }
        
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _animator.SetInteger(_animationState, 2);
            }
        
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _animator.SetInteger(_animationState, 3);
            }
        
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _animator.SetInteger(_animationState, 4);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                _animator.SetInteger(_animationState, 5);
            }
        
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                _animator.SetInteger(_animationState, 0);
            }
        }
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
