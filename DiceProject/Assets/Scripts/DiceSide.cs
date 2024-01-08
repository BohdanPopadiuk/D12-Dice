using System;
using UnityEngine;
using TMPro;

public class DiceSide : MonoBehaviour
{
    public static Action<int> RollingResult;
    private DiceController _diceController;
    [SerializeField] private TextMeshProUGUI sideText;

    private bool _sendResult = false;
    private bool _normalThrow = false;

    [SerializeField] private int topSideNumber;
    void Start()
    {
        _diceController = transform.parent.gameObject.GetComponent<DiceController>();
        DiceController.RollDice += UnlockSendingResults;
        DragAndThrow.DiceThrown += UnlockSendingResults;
    }

    private void OnDestroy()
    {
        DiceController.RollDice -= UnlockSendingResults;
        DragAndThrow.DiceThrown -= UnlockSendingResults;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_sendResult && _diceController.DiceOnGround)
        {
            _sendResult = false;
            RollingResult?.Invoke(_normalThrow ? topSideNumber : -10);
        }
    }

    private void UnlockSendingResults()
    {
        _normalThrow = true;
        _sendResult = true;
    }
    private void UnlockSendingResults(bool normalThrow)
    {
        _normalThrow = normalThrow;
        _sendResult = true;
    }
    
    public void SetSideValue(int sideNumber, int topSideNumber)
    {
        this.topSideNumber = topSideNumber;
        sideText.text = (sideNumber == 6 || sideNumber == 9) ? (sideNumber + ".") : sideNumber.ToString();
    }
}
