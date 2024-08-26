using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : MonoBehaviour
{
    [SerializeField] private Button _playButton;

    private async Task Start()
    {
        await GameCoordinator.Instance.Screens.ShowPopup();
        
        _playButton.onClick.AddListener(GameCoordinator.Instance.GameplaySystem.OnPlayButtonClicked);
    }
}