using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceController : MonoBehaviour
{
    public static Action RollDice;
    private Rigidbody _rb;
    [SerializeField] private float rollingForce = 50000;
    [SerializeField] private float maxRandomTorque = 50000;
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

    private void Roll()//automatic roll
    {
        _rb.AddForce(Vector3.up * rollingForce);
        Vector3 randomTorque = new Vector3(
            Random.Range(0, maxRandomTorque),
            Random.Range(0, maxRandomTorque),
            Random.Range(0, maxRandomTorque));
        _rb.AddTorque(randomTorque);
    }
}
