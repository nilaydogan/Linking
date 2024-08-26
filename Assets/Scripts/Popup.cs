using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TMP_Dropdown _dropdownSizeX, _dropdownSizeY;
    [SerializeField] private TMP_InputField _inputFieldSizeMove, _inputFieldSizeScore;
    [SerializeField] private Button _button;

    public void Initialize()
    {
        transform.localScale = Vector3.zero;
        _button.onClick.AddListener(OnButtonClicked);
    }
    
    public async Task Show()
    {
        _canvas.sortingOrder = 11;
        await transform.DOScale(Vector3.one, .35f).SetEase(Ease.OutBack).AsyncWaitForCompletion();
    }
    
    private void OnButtonClicked()
    {
        var sizeX = _dropdownSizeX.value;
        var sizeY = _dropdownSizeY.value;
        var sizeMove = int.Parse(_inputFieldSizeMove.text);
        var sizeScore = int.Parse(_inputFieldSizeScore.text);
        
        GameCoordinator.Instance.GameplaySystem.InitializeGame(sizeX, sizeY, sizeMove, sizeScore);
        
        Destroy(gameObject);
    }
}