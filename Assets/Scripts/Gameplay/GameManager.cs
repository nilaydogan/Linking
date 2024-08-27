using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Properties
    public GameBoard GameBoard => _gameBoard;
    public GameplayScreen GameplayScreen => _gameplayScreen;
    
    public bool IsInitialized;
    public List<BoardItem> LinkedBoardItems;

    #endregion

    #region Fields
    
    private GameBoard _gameBoard;
    private GameplayScreen _gameplayScreen;
    private GameplaySystem.GameData _gameData;
    private int _score;
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
        RefreshGameplayScreen(_gameData.TargetScore, _gameData.MoveLimit);
        IsInitialized = true;
    }

    public void RefreshGameplayScreen(int score, int move)
    {
        _gameplayScreen.Initialize(score, move);
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
            RefreshGameplayScreen(_score, _movesLeft);
        }
        else
        {
            linkedBoardItems.ForEach(item => item.RemoveHighlight());
            linkedBoardItems.Clear();
        }
        
        //todo: Check if the game is over
        if (_movesLeft <= 0)
        {
            if (_score >= _gameData.TargetScore)
            {
                //todo: Handle game win
            }
            else
            {
                //todo: Handle game over
            }
        }
    }
    
    #endregion
    
    #region Private Methods

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
    
    #endregion
}