using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    #region Fields
    
    [SerializeField] GameManager _gameManager;

    private EventSystem _eventSystem = EventSystem.current;
    private PointerEventData _pointerEventData;
    private List<RaycastResult> _raycastResults = new List<RaycastResult>();
    
    private Vector2 _initialTouchPosition;
    private BoardItem _initialLinkedItem;
    //private List<BoardItem> _linkedBoardItems = new List<BoardItem>();

    #endregion

    #region Unity Methods

    private void Update()
    {
        if(!_gameManager.IsInitialized) return;
        
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {

                _initialTouchPosition = touch.position;
                _initialLinkedItem = GetBoardItemAtPosition(_initialTouchPosition);
                if (_initialLinkedItem != null)
                {
                    _gameManager.LinkedBoardItems.Add(_initialLinkedItem);
                    _initialLinkedItem.Highlight();
                    Debug.Log("Linked Board Items: " + _initialLinkedItem.Coordinates);
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                var currentTouchPosition = touch.position;
                if (_initialLinkedItem == null)
                {
                    _initialLinkedItem = GetBoardItemAtPosition(currentTouchPosition);
                    if (_initialLinkedItem != null)
                    {
                        _gameManager.LinkedBoardItems.Add(_initialLinkedItem);
                        _initialLinkedItem.Highlight();
                        Debug.Log("Linked Board Items: " + _initialLinkedItem.Coordinates);
                    }
                }
                _gameManager.HandleLinking(_initialLinkedItem, GetBoardItemAtPosition(currentTouchPosition));
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                GameCoordinator.Instance.GameplaySystem.ConfirmMatching(_gameManager.LinkedBoardItems);
                _gameManager.LinkedBoardItems.Clear();
            }
        }
    }
    
    #endregion

    private BoardItem GetBoardItemAtPosition(Vector2 position)
    {
        _pointerEventData = new PointerEventData(_eventSystem);
        _pointerEventData.position = position;

        _raycastResults.Clear();
        _eventSystem.RaycastAll(_pointerEventData, _raycastResults);

        foreach (var result in _raycastResults)
        {
            var hitObject = result.gameObject;
            var boardItem = hitObject.GetComponent<BoardItem>();

            if (boardItem != null)
            {
                return boardItem;
            }
        }

        return null;
    }
}