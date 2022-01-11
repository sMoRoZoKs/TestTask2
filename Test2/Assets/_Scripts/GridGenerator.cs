using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private  TileObject tileExample;
    private Vector2[,] _tableCoordinate;
    private TileObject[,] _tiles;
    private float _animationSpeed = 250f;
    private float _sizeX, _sizeY;
    Action destroyTileAction;
    public void Init(int sizeX,int sizeY, float animationSpeed,Action collBackForDestroyTile)
    {
        _sizeX = sizeX;
        _sizeY = sizeY;
        _animationSpeed = animationSpeed;
        _tableCoordinate = new Vector2[sizeX,sizeY];
        _tiles = new TileObject[sizeX, sizeY];
        destroyTileAction = collBackForDestroyTile;
        GenerateBoard();
    }
    private void GenerateBoard()
    {
        RectTransform bgRect = bg.GetComponent<RectTransform>();
        bgRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 450);
        bgRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 450);
        RectTransform tileExampleRect = tileExample.GetComponent<RectTransform>();

        tileExampleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 450 / _sizeX);
        tileExampleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 450 / _sizeY);



        for (int i = 0;i < _sizeX;i++)
        {
            for(int j = 0; j < _sizeY; j++)
            {
                _tableCoordinate[i,j] = new Vector2(i * 450 / _sizeX, j * 450 / _sizeY);

                _tiles[i, j] = Instantiate(tileExample, bg.transform);
                _tiles[i, j].Init(GetRandomTypeTile(), new Vector2Int(i, j), this);
                _tiles[i, j].GetComponent<RectTransform>().localPosition = new Vector3(_tableCoordinate[i, j].x, _tableCoordinate[i, j].y, 0);
            }
        }
        CheckMathHorizontal(true);
        CheckMathVertical(true);
    }
    private TileObject.TileType GetRandomTypeTile()
    {
        List<TileObject.TileType> returnType = new List<TileObject.TileType>();
        foreach (TileObject.TileType type in Enum.GetValues(typeof(TileObject.TileType)))
            returnType.Add(type);
        return returnType[UnityEngine.Random.Range(0,returnType.Count - 1)];
    }
    private void ChangeTiles(Vector2Int id1, Vector2Int id2)
    {
        TileObject temp = _tiles[id1.x, id1.y];
        _tiles[id1.x, id1.y] = _tiles[id2.x, id2.y];
        _tiles[id2.x, id2.y] = temp;

        _tiles[id1.x, id1.y].SetPosition(new Vector2Int(id1.x, id1.y));
        _tiles[id2.x, id2.y].SetPosition(new Vector2Int(id2.x, id2.y));
    }
    bool touchBlock = false;
    public void ChangeAndCheckTiles(Vector2Int id1, Vector2Int id2) { ChangeAndCheckTiles(id1, id2, true); }
    private void ChangeAndCheckTiles(Vector2Int id1,Vector2Int id2,bool returnIfNotMatch)
    {
        if (touchBlock && returnIfNotMatch) return;
        touchBlock = true;
        ChangeTiles(id1,id2);
        StartCoroutine(Tweener.MoveTo(_tiles[id2.x, id2.y].transform, _tableCoordinate[id2.x, id2.y], _animationSpeed));
        StartCoroutine(Tweener.MoveTo(_tiles[id1.x, id1.y].transform, _tableCoordinate[id1.x, id1.y], _animationSpeed, () => 
        {
            if(!CheckMath() && returnIfNotMatch)
                ChangeAndCheckTiles(id1, id2,false); 
        }));
    }
    private bool CheckMath()
    {
        if (CheckMathVertical() || CheckMathHorizontal()) 
        {
            ResetBoard();
            return true;
        }
        else touchBlock = false;
        return false;
    }
    //08 88
    //00 80
    private void ResetBoard()
    {
        for (int i = 1; i <_sizeY; i++)
        {
            bool needBack = false;
            for (int j = 0; j < _sizeX; j++)
            {
                if (_tiles[j, i].tileActive == true && _tiles[j, i - 1].tileActive == false)
                {
                    ChangeTiles(new Vector2Int(j, i), new Vector2Int(j, i - 1));
                    needBack = true;
                }
                
            }
            if (needBack) i = 0;
        }
        for (int i = 0; i < _sizeX; i++)
        {
            for (int j = 0; j < _sizeY; j++)
            {

                if (_tiles[i, j].tileActive == false) ResetTile(new Vector2Int(i, j));
                else StartCoroutine(Tweener.MoveTo(_tiles[i, j].transform, _tableCoordinate[i, j], _animationSpeed));
            }
        }
        Invoke(nameof(CheckMath),2f);
    }
    private bool CheckMathVertical(bool dontDestroy = false)
    {
        int countMatch = 1;
        bool matchFound = false;
        for (int i = 0; i <_sizeX; i++)
        {
            for (int j = 1; j < _sizeY; j++)
            {
                int x = 1;
                if (_tiles[i, j].Type == _tiles[i, j - 1].Type && _tiles[i, j - 1].tileActive && _tiles[i, j].tileActive)
                {
                    countMatch++;
                    if (j < _sizeY - 1) continue;
                    else x = 0;
                }
                if (countMatch >= 3)
                {
                    if (dontDestroy)
                    {
                        for (int k = 0; k < countMatch && k <= _sizeY; k++)
                        {
                            _tiles[i , j - k - x].SetType(GetRandomTypeTile());
                        }
                        j = 0;
                    }
                    else
                    {
                        for (int k = 0; k < countMatch && k <= _tableCoordinate.GetLength(1); k++)
                        {
                            _tiles[i, j - k - x].tileActive = false;
                        }
                        matchFound = true;
                    }
                }
                countMatch = 1;
            }
        }
        return matchFound;
    }
    private bool CheckMathHorizontal(bool dontDestroy = false)
    {
        int countMatch = 1;
        bool matchFound = false;
        for (int j = 0; j <_sizeY; j++)
        {
            for (int i = 1; i < _sizeX; i++)
            {

                int x = 1;
                if (_tiles[i, j].Type == _tiles[i - 1, j].Type && _tiles[i - 1, j].tileActive && _tiles[i, j].tileActive)
                {
                    countMatch++;
                    if (i < _sizeX - 1) continue;
                    else x = 0;
                }
                if (countMatch >= 3)
                {
                    if (dontDestroy)
                    {
                        for (int k = 0; k < countMatch && k <= _sizeX; k++)
                        {
                            _tiles[i - k - x, j].SetType(GetRandomTypeTile());
                        }
                        i = 0;
                    }
                    else
                    {
                        for (int k = 0; k < countMatch && k <= _sizeX; k++)
                        {
                            _tiles[i - k - x, j].tileActive = false;
                        }
                        matchFound = true;
                    }
                }
                countMatch = 1;

            }
        }
        return matchFound;
    }
    private void ResetTile(Vector2Int id,Action collBack = null)
    {
        _tiles[id.x, id.y].transform.localPosition += Vector3.up * 500;
        _tiles[id.x, id.y].tileActive = false;
        _tiles[id.x, id.y].SetType(GetRandomTypeTile());
        destroyTileAction.Invoke();
        StartCoroutine(Tweener.MoveTo(_tiles[id.x, id.y].transform, _tableCoordinate[id.x, id.y], _animationSpeed,() => 
        {
            _tiles[id.x, id.y].tileActive = true;
            if (collBack != null) collBack.Invoke();
        }));
    }
}
