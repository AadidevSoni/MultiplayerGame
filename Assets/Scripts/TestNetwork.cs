using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PurrNet;

public class TestNetwork : NetworkIdentity
{
    // Spawning a cube
    [SerializeField] private NetworkIdentity _networkIdentity;

    // Setting the color of the cube
    [SerializeField] private Color _color;
    [SerializeField] private Renderer _renderer;

    // Start of Color
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            SetColor();
    }

    private void SetColor()
    {
        _renderer.material.color = _color;
    }

    //End of Color

    // Start of Spawn
    private void Awake()
    {
        //This givce false and shows that the server only spawns objects once initial setup is done
        Debug.Log($"IsSpawned: {isSpawned}");
    }

    protected override void OnSpawned()
    {
        base.OnSpawned();

        //If below code is not there, the spaning will be done by both server and all the clients and hence it will spawn it multple times
        if (!isServer)
            return;

        Instantiate(_networkIdentity, Vector3.zero, Quaternion.identity);
        //If an object has NetworkIdentity, it will be spawned

    }

    // End of Spawn
}
