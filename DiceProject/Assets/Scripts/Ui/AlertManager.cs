using DG.Tweening;
using TMPro;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private CanvasGroup background;
    [SerializeField] private float animDuration = 0.4f;
    [SerializeField] private float alertTimeout  = 2f;
    [SerializeField] private Ease scaleEasy = Ease.InOutBack;
    
    private Vector3 _alertTextDefaultScale;
    private Tween _currentTween;
    void Start()
    {
        _alertTextDefaultScale = alertText.transform.localScale;
        alertText.transform.localScale = Vector3.zero;
        background.alpha = 0;
        
        DragAndThrow.DiceThrown += TryAgainAlert;
    }

    private void OnDestroy()
    {
        DragAndThrow.DiceThrown -= TryAgainAlert;
    }

    void TryAgainAlert(bool normalThrow)
    {
        if(normalThrow) return;
        ShowAlertBox("The dice has too little speed! Try again!");
    }

    void ShowAlertBox(string alert)
    {
        alertText.text = alert;
        if(_currentTween != null)
            _currentTween.Kill();
        
        _currentTween = DOTween.Sequence()
            .Append(background.DOFade(1, animDuration))
            .Append(alertText.transform.DOScale(_alertTextDefaultScale, animDuration).SetEase(scaleEasy))
            .AppendInterval(animDuration + alertTimeout)
            .Append(background.DOFade(0, animDuration))
            .Append(alertText.transform.DOScale(Vector3.zero, animDuration).SetEase(scaleEasy));
    }
}
