using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayScreen : MonoBehaviour
{
    public GameBoard GameBoard;
    
    [SerializeField] private TextMeshProUGUI _scoreText, moveText, _gameEndText, _targetScoreText;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _gameEnd;
    
    private bool _hasGameEnded;

    private void Start()
    {
        _gameEnd.SetActive(false);
    }

    public void SetValues(int score, int move, int targetScore)
    {
        _scoreText.text = score.ToString();
        moveText.text = move.ToString();
        _targetScoreText.text = targetScore.ToString();
    }

    public void ShowGameEnd(string message)
    {
        if(_hasGameEnded) return;
        
        _hasGameEnded = true;
        
        _gameEndText.text = message;
        _gameEnd.SetActive(true);
        
        _button.onClick.AddListener(() =>
        {
            GameCoordinator.Instance.Screens.HideGamePlayScreen();
            GameCoordinator.Instance.Screens.ShowHomeScreen();
            _button.onClick.RemoveAllListeners();
        });
    }
}