using System.Collections;
using System.Collections.Generic;
using Manager;
using Unity.Netcode;
using UnityEngine;
using Utils;

public class WizardController : NetworkBehaviour
{
    public Material mat1, mat2, mat3;

    private PanelController _panelController;

    private int hp, cost;

    private const float Speed = 4.0f;

    enum AnimationState
    {
        Idle = 0,
        MoveFront = 1,
        MoveBack = 2,
        Attack = 3
    }

    private Animator _animator;

    #region Network methods

    public override void OnNetworkSpawn()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (IsOwner)
            {
                transform.position = new Vector3(0, 0.3f, 6.4f);
                transform.localEulerAngles = new Vector3(0, 90, 0);
            }
            else
            {
                transform.position = new Vector3(21, 0.3f, 6.4f);
                transform.localEulerAngles = new Vector3(0, -90, 0);
                GetComponentInChildren<SkinnedMeshRenderer>().material = mat2;
            }
        }
        else
        {
            if (IsOwner)
                GetComponentInChildren<SkinnedMeshRenderer>().material = mat2;
        }
    }

    #endregion


    #region Custom methods



    #endregion


    void Start()
    {
        _panelController = GameObject.Find("GameSceneController").GetComponent<PanelController>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {

    }

    public void Move(int[,] range)
    {

        (int asisI, int asisJ) = _panelController.GetIdx(transform.position);
        int tobeI = asisI + CustomUtils.ParseRange(range)[0].i;
        int tobeJ = asisJ + CustomUtils.ParseRange(range)[0].j;

        Vector3 tobePosition = _panelController.GetPosition(tobeI, tobeJ);
        StartCoroutine(MoveAction(tobePosition));
    }

    public IEnumerator MoveAction(Vector3 destination)
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
}
