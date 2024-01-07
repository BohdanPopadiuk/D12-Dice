using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceController : MonoBehaviour
{
    public static Action RollDice;
    private Rigidbody _rb;
    [SerializeField] private float rollingForce = 50000;
    [SerializeField] private float maxRandomTorque = 50000;
    private float RandomTorque() => Random.Range(0, maxRandomTorque);
    public bool DiceOnGround => _rb.velocity == Vector3.zero;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        RollDice += Roll;
    }

    private void OnDestroy()
    {
        RollDice -= Roll;
    }

    void Roll()
    {
        Vector3 direction = Vector3.up;
        _rb.AddForce(direction * rollingForce);
        _rb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque());
    }
}
