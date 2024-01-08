using System;
using UnityEngine;
using TMPro;

public class DiceSide : MonoBehaviour
{
    public static Action<int> RollingResult;
    private DiceController _diceController;
    [SerializeField] private TextMeshProUGUI sideText;
    [SerializeField] private int topSideNumber;
    [SerializeField] private LayerMask faceLayerMask;
    public int SideNumber { get; private set; }

    private bool _sendResult = false;
    private bool _normalThrow = false;

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
    
    public void SetSideValue(int sideNumber)
    {
        SideNumber = sideNumber;
        sideText.text = (sideNumber == 6 || sideNumber == 9) ? (sideNumber + ".") : sideNumber.ToString();
    }

    public void SetTopSideValue()
    {
        Ray ray = new Ray(transform.position, -transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, faceLayerMask))
        {
            if(hit.collider != null)
                topSideNumber = hit.collider.gameObject.GetComponent<DiceSide>().SideNumber;
        }
    }
}
