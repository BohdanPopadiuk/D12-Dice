using System;
using UnityEngine;
using TMPro;

public class DiceSide : MonoBehaviour
{
    public static Action<int> RollingResult;
    private DiceController _diceController;
    [SerializeField] private TextMeshProUGUI sideText;

    private bool _sendResult = false; 

    [SerializeField] private int topSideNumber;
    void Start()
    {
        _diceController = transform.parent.gameObject.GetComponent<DiceController>();
        DiceController.RollDice += UnblockSendingResults;
        DragAndThrow.DiceThrown += UnblockSendingResults;
    }

    private void OnDestroy()
    {
        DiceController.RollDice -= UnblockSendingResults;
        DragAndThrow.DiceThrown -= UnblockSendingResults;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_sendResult) return;
        if (_diceController.DiceOnGround)
        {
            _sendResult = false;
            RollingResult?.Invoke(topSideNumber);
        }
    }

    private void UnblockSendingResults()
    {
        _sendResult = true;
    }

    public void SetSideValue(int sideNumber, int topSideNumber)
    {
        this.topSideNumber = topSideNumber;
        sideText.text = (sideNumber == 6 || sideNumber == 9) ? (sideNumber + ".") : sideNumber.ToString();
    }
}
