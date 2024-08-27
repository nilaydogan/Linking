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
        _playButton.onClick.AddListener(GameCoordinator.Instance.GameplaySystem.OnPlayButtonClicked);
    }

    private void OnButtonClicked()
    {
        try
        {
            var sizeX = int.Parse(_dropdownSizeX.options[_dropdownSizeX.value].text);
            var sizeY = int.Parse(_dropdownSizeY.options[_dropdownSizeY.value].text);
            var moveCount = Mathf.Clamp(int.Parse(_inputFieldSizeMove.text),10, 40);
            var targetScore = Mathf.Clamp(int.Parse(_inputFieldSizeScore.text), 15, 150);
            
            GameCoordinator.Instance.GameplaySystem.InitializeGame(sizeX, sizeY, moveCount, targetScore);
            _warningText.gameObject.SetActive(false);
            _playButton.interactable = true;
        }
        catch (System.Exception e)
        {
            _warningText.gameObject.SetActive(true);
        }
    }
}