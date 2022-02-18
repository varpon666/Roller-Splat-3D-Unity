using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    public void Spawn(Vector3 position)
    {
        Instantiate(_player, position, Quaternion.identity);
    }
}
