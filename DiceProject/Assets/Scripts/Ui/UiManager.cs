using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Button rollDiceButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    private int _totalScore;
    void Start()
    {
        Application.targetFrameRate = 60;
        rollDiceButton.onClick.AddListener(DiceInAir);
        rollDiceButton.onClick.AddListener(() => DiceController.RollDice?.Invoke());
        restartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        
        DiceSide.RollingResult += UpdateScore;
        DragAndThrow.DiceSelected += DiceInAir;
    }
    private void OnDestroy()
    {
        DiceSide.RollingResult -= UpdateScore;
        DragAndThrow.DiceSelected -= DiceInAir;
    }
    
    void UpdateScore(int score)
    {
        rollDiceButton.interactable = true;
        
        if (score > 0) _totalScore += score;
        scoreText.text = "Result: " + (score > 0 ? score : "?");
        totalScoreText.text = "Total: " + _totalScore;
    }
    void DiceInAir()
    {
        rollDiceButton.interactable = false;
        scoreText.text = "Result: ?";
    }
}
