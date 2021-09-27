using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    public int Width;
    public int Height;
    private float Cellsize;
    private Vector3 OriginPosition;
    public TGridObject[,] gridArray;
    public const int SortingOrderDefault = 5000;
    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.Width = width;
        this.Height = height;
        this.Cellsize = cellSize;
        this.OriginPosition = originPosition;

        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                //CreateTextIntoWorld(SetValue(x, y, 10), null, GetWorldPosition(x - 4, y - 7) + new Vector3(Cellsize, Cellsize, 0) * .5f, 16, Color.black, TextAnchor.MiddleCenter);
                gridArray[x, y] = createGridObject(this, x, y);
            }
    }
    private Vector3 GetWorldPosition(int x,int y)
    {
        return new Vector3(x, y) * Cellsize + OriginPosition;
    }
    public int GetWidth()
    {
        return Width;
    }
    public int GetHeight()
    {
        return Height;
    }
    public float GetCellsize()
    {
        return Cellsize;
    }
    public string SetValue(int x,int y, TGridObject value)
    {
        if (x < Width && y < Height)
        {
            gridArray[x, y] = value;

        }            
        return gridArray[x, y].ToString();
    }
    public Vector3 GetWorldLocation(int x,int y)
    {
        return new Vector3(x, y) * Cellsize + OriginPosition;
    }
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - OriginPosition).x / Cellsize);
        y = Mathf.FloorToInt((worldPosition - OriginPosition).y / Cellsize);
    }

    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
        {
            gridArray[x, y] = value;
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }
    public static TextMesh CreateTextIntoWorld(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = SortingOrderDefault)
    {
        if (color == null) color = Color.white;
        return CreateTextIntoWorld(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }
    public static TextMesh CreateTextIntoWorld(Transform parent,string text, Vector3 localPosition, int fontsize, Color color, TextAnchor textAnchor,TextAlignment textAlignment = TextAlignment.Left,int sortingOrder = SortingOrderDefault)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontsize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
    
}
