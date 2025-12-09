using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PurrNet;

public class TestNetwork : NetworkIdentity
{
    [SerializeField] private NetworkIdentity _networkIdentity;

    private void Awake()
    {

    }

    private void Start()
    {

    }

    protected override void OnSpawned()
    {
        base.OnSpawned();

        if (!isServer)
            return;

        Instantiate(_networkIdentity, Vector3.zero, Quaternion.identity);
        //If an object has NetworkIdentity, it will be spawned

    }
}
