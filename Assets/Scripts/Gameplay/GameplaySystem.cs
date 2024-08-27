using System.Collections.Generic;
using UnityEngine;

public class GameplaySystem : MonoBehaviour
{
    private GameManager _gameManager;
    private GameData _gameData;
    private int _score;
    private int _movesLeft;
    
    public void OnPlayButtonClicked()
    {
        GameCoordinator.Instance.LoadScene(SceneName.GamePlayScene.ToString(), OnSceneLoadingCompleted: () =>
        {
            GameCoordinator.Instance.Screens.HideHomeScreen();
            GameCoordinator.Instance.Screens.ShowGamePlayScreen();
            
            _gameManager.GameplayScreen.Initialize(_gameData.TargetScore, _gameData.MoveLimit);
            StartGame();
        });
    }

    public void InitializeGame(int boardSizeX, int boardSizeY, int moveLimit, int scoreLimit)
    {
        _gameData = new GameData
        {
            BoardSize = new Vector2Int(boardSizeX, boardSizeY),
            MoveLimit = moveLimit,
            TargetScore = scoreLimit
        };
    }
    private void StartGame()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if(!_gameManager.IsInitialized)
            _gameManager.Initialize(_gameData);
    }

    #region Additional Classes

    public class GameData
    {
        public int MoveLimit;
        public int TargetScore;
        public Vector2Int BoardSize;
    }

    #endregion
}