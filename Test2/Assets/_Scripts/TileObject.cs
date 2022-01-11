using UnityEngine;
using UnityEngine.UI;

public class TileObject : MonoBehaviour
{
    [HideInInspector]public bool tileActive = true;
    [SerializeField] private SwipeController swipeController;
    public void Init(TileType tileType,Vector2Int listId, GridGenerator grid)
    {
        SetType(tileType);
        _listId = listId;
        _grid = grid;
        swipeController.SwipeCollBack = MoveTile;
    }
    public void SetType(TileType tileType)
    {
        _tileType = tileType;
        if      (_tileType == TileType.Red)    GetComponent<Image>().color = Color.red;
        else if (_tileType == TileType.Blue)   GetComponent<Image>().color = Color.blue;
        else if (_tileType == TileType.Yellow) GetComponent<Image>().color = Color.yellow;
        else if (_tileType == TileType.Green)  GetComponent<Image>().color = Color.green;
        else if (_tileType == TileType.Gray)   GetComponent<Image>().color = Color.gray;
        else if (_tileType == TileType.White)  GetComponent<Image>().color = Color.white;
    }
    public TileType Type => _tileType;
    public void SetPosition(Vector2Int listId)
    {
        _listId = listId;
    }
    TileType _tileType;
    Vector2Int _listId;
    GridGenerator _grid;
    public enum TileType
    {
        Red,
        Blue,
        Yellow,
        Green,
        Gray,
        White
    }

    private void MoveTile(SwipeController.DirectionSwipe direction)
    {
        if (!tileActive) return;
        Vector2Int vectorDirection = _listId;

        if      (direction == SwipeController.DirectionSwipe.Right) vectorDirection.x += 1;
        else if (direction == SwipeController.DirectionSwipe.Left)  vectorDirection.x -= 1;
        else if (direction == SwipeController.DirectionSwipe.Up)    vectorDirection.y += 1;
        else if (direction == SwipeController.DirectionSwipe.Down)  vectorDirection.y -= 1;

        if (vectorDirection.x < 9 && vectorDirection.y < 9 && vectorDirection.x >= 0 && vectorDirection.y >= 0)
            _grid.ChangeAndCheckTiles(_listId, vectorDirection);
    }
    
}
