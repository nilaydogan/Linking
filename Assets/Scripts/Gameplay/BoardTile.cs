using UnityEngine;

public class BoardTile : MonoBehaviour
{
    public Vector2Int Coordinates { get; private set; }
    public BoardItem BoardItem;

    public void SetBoardItem(BoardItem boardItem)
    {
        BoardItem = boardItem;
    }
    
    public void SetCoordinates(Vector2Int coordinates)
    {
        Coordinates = coordinates;
    }
}