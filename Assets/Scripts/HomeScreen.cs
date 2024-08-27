using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdownSizeX, _dropdownSizeY;
    [SerializeField] private TMP_InputField _inputFieldSizeMove, _inputFieldSizeScore;
    [SerializeField] private Button _setButton, _playButton;
    [SerializeField] private TextMeshProUGUI _warningText;
    
    private bool _isInitialized;

    private void Start()
    {
        _warningText.gameObject.SetActive(false);
        _playButton.interactable = false;
        _setButton.onClick.AddListener(OnButtonClicked);
        _playButton.onClick.AddListener(CheckIfValuesSet);
    }

    private void OnButtonClicked()
    {
        var sizeX = _dropdownSizeX.value;
        var sizeY = _dropdownSizeY.value;
        var moveCount = Mathf.Clamp(int.Parse(_inputFieldSizeMove.text),10, 40);
        var targetScore = Mathf.Clamp(int.Parse(_inputFieldSizeScore.text), 3, 150);
        
        GameCoordinator.Instance.GameplaySystem.InitializeGame(sizeX, sizeY, moveCount, targetScore);
        
        _isInitialized = true;
    }

    private void CheckIfValuesSet()
    {
        if (_isInitialized)
        {
            _warningText.gameObject.SetActive(false);
            _playButton.interactable = true;
        }
        else
        {
            _warningText.text = "Please set the values first!";
            _warningText.gameObject.SetActive(true);
            return;
        }
        
        GameCoordinator.Instance.GameplaySystem.OnPlayButtonClicked();
    }
}