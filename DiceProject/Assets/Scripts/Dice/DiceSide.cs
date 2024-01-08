using System;
using UnityEngine;
using TMPro;

public class DiceSide : MonoBehaviour
{
    #region Fields

    public static Action<int> RollingResult;
    private DiceController _diceController;
    [SerializeField] private TextMeshProUGUI sideText;
    [SerializeField] private int topSideNumber;
    [SerializeField] private LayerMask faceLayerMask;
    public int SideNumber { get; private set; }

    private bool _sendResult = false;
    private bool _normalThrow = false;

    #endregion

    private void Start()
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
            //with a weak throw, we do not update the result
            RollingResult?.Invoke(_normalThrow ? topSideNumber : -10);
        }
    }

    #region Unlock

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

    #endregion

    
    #region SideSettings
    
    public void SetSideValue(int sideNumber)
    {
        SideNumber = sideNumber;
        //in order not to confuse 9 and 6, we put dots
        sideText.text = (sideNumber == 6 || sideNumber == 9) ? (sideNumber + ".") : sideNumber.ToString();
    }

    public void SetTopSideValue()
    {
        //we get a parallel face so when the collider of the current face hits the table we know what is on the top face
        Ray ray = new Ray(transform.position, -transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, faceLayerMask))
        {
            if(hit.collider != null)
                topSideNumber = hit.collider.gameObject.GetComponent<DiceSide>().SideNumber;
        }
        gameObject.name = $"Face {SideNumber} - {topSideNumber}";
    }

    #endregion
}
