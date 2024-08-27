using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Properties
    
    public bool IsInitialized;
    public List<BoardItem> LinkedBoardItems;

    #endregion

    #region Fields
    
    private GameBoard _gameBoard;
    private GameplayScreen _gameplayScreen;
    private GameplaySystem.GameData _gameData;
    private int _score;
    private int _targetScore;
    private int _movesLeft;

    #endregion

    #region Public Methods
    public void Initialize(GameplaySystem.GameData data)
    {
        _gameplayScreen = FindObjectOfType<GameplayScreen>();
        _gameBoard = _gameplayScreen.GameBoard;
        LinkedBoardItems = new List<BoardItem>();
        
        _gameData = data;
        _gameBoard.Initialize(_gameData);
        _targetScore = _gameData.TargetScore;
        _movesLeft = _gameData.MoveLimit;
        RefreshGameplayScreen(_score, _gameData.MoveLimit, _gameData.TargetScore);
        
        IsInitialized = true;
    }

    public void RefreshGameplayScreen(int score, int move, int targetScore)
    {
        _gameplayScreen.SetValues(score, move, targetScore);
    }
    
    public void ConfirmMatching(List<BoardItem> linkedBoardItems)
    {
        if (linkedBoardItems.Count >= 3)
        {
            //todo: Handle match logic, such as removing items, updating the score, etc.
            
            _movesLeft--;
            _score += linkedBoardItems.Count;

            foreach (var boardItem in linkedBoardItems)
            {
                Destroy(boardItem.gameObject);
                _gameBoard.BoardTiles[boardItem.Coordinates.x, boardItem.Coordinates.y].SetBoardItem(null);
            }
            
            _gameBoard.RePositionBoard();
            RefreshGameplayScreen(_score, _movesLeft, _targetScore);
        }
        else
        {
            linkedBoardItems.ForEach(item => item.RemoveHighlight());
            linkedBoardItems.Clear();
        }
        
        if (_movesLeft <= 0)
        {
            OnGameEnd(_score >= _targetScore);
        }
        else if (_score >= _targetScore)
        {
            OnGameEnd(true);
        }
    }
    
    public void HandleLinking(BoardItem initialLinkedItem, BoardItem currentBoardItem)
    {
        if (initialLinkedItem != null)
        {
            if (currentBoardItem != null && currentBoardItem != initialLinkedItem)
            {
                if (!LinkedBoardItems.Contains(currentBoardItem))
                {
                    // Check if the current board item is aligned and matches the initial selected board item type/color
                    if (IsAligned(currentBoardItem) && currentBoardItem.ItemType == initialLinkedItem.ItemType)
                    {
                        LinkedBoardItems.Add(currentBoardItem);
                        currentBoardItem.Highlight();
                        Debug.Log("Linked Board Items: " + currentBoardItem.Coordinates);
                    }
                }
                else
                {
                    UnlinkBoardItem(currentBoardItem);
                }
            }
            else
            {
                UnlinkBoardItem(currentBoardItem);
            }
        }
    }
    
    #endregion
    
    #region Private Methods
    private bool IsAligned(BoardItem currentItem)
    {
        // Check if the current board item is aligned horizontally, vertically or diagonally with the last selected board item
        var lastSelectedItem = LinkedBoardItems[^1];
        var dx = Mathf.Abs(currentItem.Coordinates.x - lastSelectedItem.Coordinates.x);
        var dy = Mathf.Abs(currentItem.Coordinates.y - lastSelectedItem.Coordinates.y);

        return (dx == 0 || dy == 0 || dx == dy);
    }
    
    private void UnlinkBoardItem(BoardItem boardItem)
    {
        if(LinkedBoardItems.Count > 1 && LinkedBoardItems[^2] == boardItem)
        {
            var lastLinkedBoardItem = LinkedBoardItems[^1];
            LinkedBoardItems.Remove(lastLinkedBoardItem);
            lastLinkedBoardItem.RemoveHighlight();
            Debug.Log("Unlinked Board Items: " + lastLinkedBoardItem.Coordinates);
        }
    }

    private void OnGameEnd(bool hasWon)
    {
        _gameplayScreen.ShowGameEnd(hasWon ? "You Win!" : "Game Over");
    }

    #endregion
}