using UnityEngine;

public class Screens : MonoBehaviour
{
    [SerializeField] private Transform _screensParent;
    [SerializeField] private GameObject _homeScreen;
    [SerializeField] private GameObject _gamePlayScreen;
    
    private GameObject _currentHomeScreen;
    private GameObject _currentGameplayScreen;
    
    public void ShowHomeScreen()
    {
        _currentHomeScreen = Instantiate(_homeScreen, _screensParent);
    }
    
    public void HideHomeScreen()
    {
        Destroy(_currentHomeScreen);
    }

    public void ShowGamePlayScreen()
    {
        _currentGameplayScreen = Instantiate(_gamePlayScreen, _screensParent);
    }
    
    public void HideGamePlayScreen()
    {
        Destroy(_currentGameplayScreen);
    }
}
