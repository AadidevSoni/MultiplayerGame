using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PurrNet;

public class PlayerMovement : NetworkIdentity
{
    [SerializeField]
    private float _speed = 5f;

    //If below is not there, oly the server can use the controls the client cant
    protected override void OnSpawned()
    {
        base.OnSpawned();

        enabled = isOwner; // This will disable if we are not the owner and update will only run if you are the owner
    }
    //Now, you have to make the prefab the player and spwawn it using PlayerSpawner component so that it is now your cube and you can control it
    // When ytou start the game in client side, anmother cube will be spawned and only ythew client can control it
    private void Update()
    {
        Vector3 moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += moveVector * (Time.deltaTime * _speed);
    }
}
