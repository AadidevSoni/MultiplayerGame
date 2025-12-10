using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PurrNet;
using TMPro;

public class TestNetwork : NetworkIdentity
{
    // Spawning a cube
    [SerializeField] private NetworkIdentity _networkIdentity;

    // Setting the color of the cube
    [SerializeField] private Color _color;
    [SerializeField] private Renderer _renderer;

    //Synchronization
    [SerializeField] private SyncVar<int> _health = new(initialValue: 100);

    // Seeing how the above works
    [SerializeField] private int _localHealth = 100;

    [SerializeField] private TMP_Text _healthText;

    // Start of Color
    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.A))
            //SetColor() - for local color
            SetColor(_color);
        */

        //Using Structs
        if (Input.GetKeyDown(KeyCode.A))
        {
            var myStruct = new TestStruct()
            {
                color = _color,
                intValue = 10
            };
            SetColor(myStruct);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SetHealth(_localHealth - 10);
        }
    }

    //[ServerRpc] // If this is not there, only server will run it and it wont b synchroized with the clients
    //Now, if we press A in client, it i nstructs the server to run the SetColor and color chanmges in Server only

    [ObserversRpc(bufferLast: true)] // This makes the server as well as all the clients to run the method
    // private void SetColor() - This makes it such that when client hits A, server and client has their local colors that might not be synced
    private void SetColor(TestStruct myStruct)
    {
        _renderer.material.color = myStruct.color;
    }

    //RPCs - command or instruction ffor someone to run some code. Instruct ServerRpc to run this function
    // ServerRpc, ObserversRpc and TargetRpc
    // ServerPc: Client or Server -> Server
    // ObserversRpc: Serveror Client -> All clients 
    // TargetRpc: Server or Client -> single client

    //Stroing global variables in structs
    public struct TestStruct
    {
        public Color color;
        public int intValue;
    }

    // If a new player joins, any instruction that  changed the colro of cube befgore wont be applicable to this player to fix this:
    // TO do this, we add bufferlast: true in the ObserversRpc

    //End of Color

    // Start of Spawn and Synchronization OnDestory
    private void Awake()
    {
        //This givce false and shows that the server only spawns objects once initial setup is done
        Debug.Log($"IsSpawned: {isSpawned}");

        //Callbacks in SyncVar
        _health.onChanged += OnChanged;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _health.onChanged -= OnChanged;
    }

    private void OnChanged(int newValue)
    {
        Debug.Log($"[{(isServer ? "SERVER" : "CLIENT")}] Health Changed: {newValue}");
        _healthText.text = newValue.ToString();
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

    // Start of synchronization 

    [ServerRpc] //since the health is server owned, all clients can tell the server to remove 10 from health
    private void TakeDamage(int damage)
    {
        _health.value -= damage;

        if (_health.value <= 0)
        {
            _healthText.text = "Dead";
        }
    }

    // Same as how above works internally but is worse becuase it wont know if the takeDamage happens at the same time so above preffered
    [ObserversRpc(bufferLast: true)]
    private void SetHealth(int health)
    {
        _localHealth = health;
    }

}
