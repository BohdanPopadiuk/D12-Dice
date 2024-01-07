using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Button rollDiceButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    private int _totalScore;
    void Start()
    {
        rollDiceButton.onClick.AddListener(DiceInAir);
        rollDiceButton.onClick.AddListener(() => DiceController.RollDice?.Invoke());
        
        DiceSide.RollingResult += UpdateScore;
        DragAndThrow.DiceThrown += DiceInAir;
    }
    private void OnDestroy()
    {
        DiceSide.RollingResult -= UpdateScore;
        DragAndThrow.DiceThrown -= DiceInAir;
    }
    
    void UpdateScore(int score)
    {
        _totalScore += score;
        rollDiceButton.interactable = true;
        scoreText.text = "Result: " + score;
        totalScoreText.text = "Total: " + _totalScore;
    }
    void DiceInAir()
    {
        rollDiceButton.interactable = false;
        scoreText.text = "Result: ?";
    }
}
