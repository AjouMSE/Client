using System.Collections;
using System.Collections.Generic;
using Manager;
using Unity.Netcode;
using UnityEngine;

public class WizardController : NetworkBehaviour
{
    public Material mat1, mat2, mat3;
    enum AnimationState
    {
        Idle = 0,
        MoveFront = 1,
        MoveBack = 2,
        Attack = 3
    }

    private Animator _animator;

    public override void OnNetworkSpawn()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (IsOwner)
            {
                transform.position = new Vector3(0, 0.3f, 6);
                transform.localEulerAngles = new Vector3(0, 90, 0);
            }
            else
            {
                transform.position = new Vector3(20, 0.3f, 6);
                transform.localEulerAngles = new Vector3(0, -90, 0);
                GetComponentInChildren<SkinnedMeshRenderer>().material = mat2;
            }
        }
        else
        {
            if(IsOwner)
                GetComponentInChildren<SkinnedMeshRenderer>().material = mat2;
        }
    }
    
    [ServerRpc]
    public void SyncCardServerRpc(int[] args)
    {
        if(NetworkManager.Singleton.IsServer)
        {
            Debug.Log("Server RPC!");
            GameManager.Instance.selectedCardListHostile.Clear();
            for (int i = 0; i < args.Length; i++)
            {
                GameManager.Instance.selectedCardListHostile.Add(args[i]);
            }   
        }
    }
    
    [ClientRpc]
    public void SyncCardClientRpc(int[] args)
    {
        if (NetworkManager.Singleton.IsClient)
        {
            GameManager.Instance.selectedCardListHostile.Clear();
            for (int i = 0; i < args.Length; i++)
            {
                GameManager.Instance.selectedCardListHostile.Add(args[i]);
            }   
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        /*if (Input.GetKeyDown(KeyCode.Alpha0))
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
        }*/
    }
}
