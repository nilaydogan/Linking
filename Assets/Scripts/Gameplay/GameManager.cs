using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Properties
    public GameBoard GameBoard => _gameBoard;
    public bool IsInitialized;
    public List<BoardItem> LinkedBoardItems;

    #endregion

    #region Fields
    
    private GameBoard _gameBoard;

    #endregion

    #region Public Methods
    public void Initialize()
    {
        _gameBoard = FindObjectOfType<GameBoard>();
        LinkedBoardItems = new List<BoardItem>();
        IsInitialized = true;
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