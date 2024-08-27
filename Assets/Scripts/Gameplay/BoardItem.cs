using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BoardItem : MonoBehaviour
{
    public BoardItemType ItemType { get; private set; }
    public Vector2Int Coordinates { get; private set; }
    
    [SerializeField] private Image _image;
    
    private Tween _tween;

    private void OnDestroy()
    {
        _tween.Kill();
    }

    public void Initialize(Sprite icon, BoardItemType itemType, Vector2Int coordinates)
    {
        _image.sprite = icon;
        ItemType = itemType;
        Coordinates = coordinates;
    }
    
    public void Highlight()
    {
        _tween = transform.DOScale(Vector3.one * 1.1f, .25f)
            .OnComplete(() => transform.localScale = Vector3.one * 1.1f);
    }
    
    public void RemoveHighlight()
    {
        _tween.Kill();
        transform.localScale = Vector3.one;
    }
    
    public void PlaceBoardItemOnCell(Vector2Int coordinates, Transform parentCell)
    {
        Coordinates = coordinates;
        transform.SetParent(parentCell);
        transform.localPosition = Vector3.zero;
    }
    
    public enum BoardItemType
    {
        Yellow,
        Red,
        Blue,
        Green
    }
}