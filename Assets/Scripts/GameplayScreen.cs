using TMPro;
using UnityEngine;

public class GameplayScreen : MonoBehaviour
{
    public GameBoard GameBoard;
    
    [SerializeField] private TextMeshProUGUI _scoreText, moveText;
    
    public void Initialize(int score, int move)
    {
        _scoreText.text = score.ToString();
        moveText.text = move.ToString();
    }
}
