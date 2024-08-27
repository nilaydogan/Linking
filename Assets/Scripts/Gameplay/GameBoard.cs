using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameBoard : MonoBehaviour
{
    #region Fields

    public Vector2Int BoardSize = new Vector2Int(8, 8);
    public BoardTile[,] BoardTiles { get; private set; }

    [SerializeField] private Transform _gridParent;
    [SerializeField] private BoardTile _gridPrefab;
    [SerializeField] private BoardItem _boardItemPrefab;
    [SerializeField] private List<BoardItems> _boardItems;
    
    private int _moveLimit { get; set; }
    private int _targetScore { get; set; }

    private GridLayoutGroup _gridLayoutGroup;
    private RectTransform _rectTransform;
    private float _cellSize;

    #endregion
    
    #region Public Methods

    public void Initialize(GameplaySystem.GameData data)
    {
        _gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();
        _rectTransform = GetComponent<RectTransform>();
        
        _moveLimit = data.MoveLimit;
        _targetScore = data.TargetScore;
        
        SetBoardSize(data.BoardSize);
        CreateBoard();
        PlaceBoardItems();

        if (!HasPotentialMatches())
        {
            ShuffleBoard();
        }
    }
    
    

    private void SetBoardSize(Vector2Int size)
    {
        BoardSize = size;
        _cellSize = Mathf.Min(_rectTransform.rect.width / BoardSize.x, _rectTransform.rect.height / BoardSize.y, 200f);
        _gridLayoutGroup.cellSize = new Vector2(_cellSize, _cellSize);
    }
    
    private void CreateBoard()
    {
        BoardTiles = new BoardTile[BoardSize.x, BoardSize.y];
        for (var x = 0; x < BoardSize.x; x++)
        {
            for (var y = 0; y < BoardSize.y; y++)
            {
                BoardTiles[x, y] = Instantiate(_gridPrefab, _gridParent);
                BoardTiles[x, y].SetCoordinates(new Vector2Int(x, y));
                BoardTiles[x, y].name = $"Grid_{x}_{y}";
            }
        }
    }

    private void PlaceBoardItems()
    {
        for (var x = 0; x < BoardSize.x; x++)
        {
            for (var y = 0; y < BoardSize.y; y++)
            {
                var boardItem = Instantiate(_boardItemPrefab, BoardTiles[x,y].transform);
                var randomIndex = Random.Range(0, _boardItems.Count);
                boardItem.Initialize(_boardItems[randomIndex].Icon, _boardItems[randomIndex].Type, BoardTiles[x,y].Coordinates);
                BoardTiles[x,y].SetBoardItem(boardItem);
            }
        }
    }

    private bool HasPotentialMatches()
    {
        for (var y = 0; y < BoardSize.y; y++)
        {
            for (var x = 0; x < BoardSize.x; x++)
            {
                var boardItemToCheck = BoardTiles[x, y].BoardItem;

                if (CheckHorizontalPotentialMatch(x, y, boardItemToCheck.ItemType) ||
                    CheckVerticalPotentialMatch(x, y, boardItemToCheck.ItemType) ||
                    CheckDiagonalPotentialMatch(x, y, boardItemToCheck.ItemType))
                {
                    return true;
                }
            }
        }

        return false;
    }
    
    private bool CheckHorizontalPotentialMatch(int x, int y, BoardItem.BoardItemType type)
    {
        var matchCount = 1;

        for (var i = x + 1; i < BoardSize.y && BoardTiles[y, i].BoardItem.ItemType == type; i++)
        {
            matchCount++;
            if (matchCount >= 3)
                return true;
        }

        return false;
    }
    
    private bool CheckVerticalPotentialMatch(int x, int y, BoardItem.BoardItemType type)
    {
        var matchCount = 1;

        for (var i = y + 1; i < BoardSize.x && BoardTiles[i, x].BoardItem.ItemType == type; i++)
        {
            matchCount++;
            if (matchCount >= 3)
                return true;
        }

        return false;
    }

    private bool CheckDiagonalPotentialMatch(int x, int y, BoardItem.BoardItemType type)
    {
        var matchCount = 1;

        // bottom right
        for (var i = 1; x + i < BoardSize.y && y + i < BoardSize.x && BoardTiles[y + i, x + i].BoardItem.ItemType == type; i++)
        {
            matchCount++;
            if (matchCount >= 3)
                return true;
        }

        matchCount = 1;

        // bottom left
        for (var i = 1; x - i >= 0 && y + i < BoardSize.x && BoardTiles[y + i, x - i].BoardItem.ItemType == type; i++)
        {
            matchCount++;
            if (matchCount >= 3)
                return true;
        }

        return false;
    }

    private void ShuffleBoard()
    {
        Debug.Log("Shuffling board...");
        while (true)
        {
            var itemsList = new List<BoardItem>();

            for (var x = 0; x < BoardSize.x; x++)
            {
                for (var y = 0; y < BoardSize.y; y++)
                {
                    itemsList.Add(BoardTiles[x, y].BoardItem);
                }
            }

            // Fisher-Yates shuffle algorithm
            for (var i = itemsList.Count - 1; i > 0; i--)
            {
                var randomIndex = Random.Range(0, i + 1);
                (itemsList[i], itemsList[randomIndex]) = (itemsList[randomIndex], itemsList[i]);
            }

            var index = 0;
            for (var x = 0; x < BoardSize.x; x++)
            {
                for (var y = 0; y < BoardSize.y; y++)
                {
                    var boardItem = itemsList[index];
                    boardItem.PlaceBoardItemOnCell(new Vector2Int(x,y),BoardTiles[x,y].transform);
                    BoardTiles[x,y].SetBoardItem(boardItem);
                    index++;
                }
            }

            if (!HasPotentialMatches()) continue;

            break;
        }
    }

    public void RePositionBoard()
    {
        for (var i = 0; i < BoardSize.x; i++)
        {
            for (var j = 0; j < BoardSize.y; j++)
            {
                if(BoardTiles[i,j].BoardItem != null) continue;

                if(j + 1 < BoardSize.y)
                {
                    for (var k = j + 1; k < BoardSize.y; k++)
                    {
                        if (BoardTiles[i, k].BoardItem != null)
                        {
                            var boardItem = BoardTiles[i, k].BoardItem;
                            // boardItem.transform.SetParent(BoardTiles[i, j].transform);
                            // boardItem.transform.localPosition = Vector3.zero;
                            // boardItem.SetCoordinates(new Vector2Int(i, j));
                            boardItem.PlaceBoardItemOnCell(new Vector2Int(i, j),BoardTiles[i, j].transform);
                            BoardTiles[i, j].SetBoardItem(boardItem);
                            BoardTiles[i, k].SetBoardItem(null);
                            break;
                        }
                    }
                    if(BoardTiles[i,j].BoardItem == null)
                    {
                        var randomIndex = Random.Range(0, _boardItems.Count);
                        var boardItem = Instantiate(_boardItemPrefab, BoardTiles[i,j].transform);
                        boardItem.Initialize(_boardItems[randomIndex].Icon, _boardItems[randomIndex].Type, BoardTiles[i,j].Coordinates);
                        BoardTiles[i,j].SetBoardItem(boardItem);
                    }
                }
                if(j == BoardSize.y - 1 && BoardTiles[i,j].BoardItem == null)
                {
                    var randomIndex = Random.Range(0, _boardItems.Count);
                    var boardItem = Instantiate(_boardItemPrefab, BoardTiles[i,j].transform);
                    boardItem.Initialize(_boardItems[randomIndex].Icon, _boardItems[randomIndex].Type, BoardTiles[i,j].Coordinates);
                    BoardTiles[i,j].SetBoardItem(boardItem);
                }
            }
        }
    }

    #endregion

    #region Additional Structs

    [Serializable]
    public struct BoardItems
    {
        public Sprite Icon;
        public BoardItem.BoardItemType Type;
    }

    #endregion
}