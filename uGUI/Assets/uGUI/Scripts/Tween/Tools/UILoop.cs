using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UILoop : MonoBehaviour
{
    public delegate void UILoopCallBack(int index, GameObject go);

    enum Direction
    {
        Horizontal,
        Vertical
    }

    [SerializeField]
    protected RectTransform m_Cell;

    [SerializeField]
    protected Vector2 m_CellGap;

    [SerializeField]
    protected Vector2 m_Page;

    [SerializeField]
    Direction direction = Direction.Horizontal;

    [SerializeField, Range(0, 10)]
    private int m_BufferNo;

    [SerializeField]
    private int m_ItemsCount = 0;

    private List<RectTransform> m_InstantiateItems = new List<RectTransform>();

    private Vector2 CellRect { get { return m_Cell != null ? m_Cell.sizeDelta + m_CellGap : new Vector2(100, 100); } }

    protected float CellScale { get { return direction == Direction.Horizontal ? CellRect.x : CellRect.y; } }

    private float m_PrevPos = 0;
    private float DirectionPos { get { return direction == Direction.Horizontal ? m_Rect.anchoredPosition.x : m_Rect.anchoredPosition.y; } }

    private int m_CurrentIndex;//页面的第一行（列）在整个conten中的位置

    public UILoopCallBack onUpdate;
    public UILoopCallBack onCreate;
    public UILoopCallBack onRemove;

    private Vector2 m_InstantiateSize = Vector2.zero;
    private Vector2 InstantiateSize
    {
        get
        {
            if (m_InstantiateSize == Vector2.zero)
            {
                float rows, cols;
                if (direction == Direction.Horizontal)
                {
                    rows = m_Page.x;
                    cols = m_Page.y + (float)m_BufferNo;
                }
                else
                {
                    rows = m_Page.x + (float)m_BufferNo;
                    cols = m_Page.y;
                }
                m_InstantiateSize = new Vector2(rows, cols);
            }
            return m_InstantiateSize;
        }
    }

    private int PageCount { get { return (int)m_Page.x * (int)m_Page.y; } }

    private int PageScale { get { return direction == Direction.Horizontal ? (int)m_Page.x : (int)m_Page.y; } }

    private ScrollRect m_ScrollRect;

    private RectTransform m_Rect;
    private int InstantiateCount { get { return (int)InstantiateSize.x * (int)InstantiateSize.y; } }
    protected float scale { get { return direction == Direction.Horizontal ? 1f : -1f; } }


    void Awake()
    {
        m_ScrollRect = GetComponentInParent<ScrollRect>();
        m_ScrollRect.horizontal = direction == Direction.Horizontal;
        m_ScrollRect.vertical = direction == Direction.Vertical;

        m_Rect = GetComponent<RectTransform>();
        m_Rect.anchoredPosition = Vector2.zero;

        m_Cell.gameObject.SetActive(false);
    }

    void Start()
    {
        if (m_InstantiateItems.Count == 0)
        {
            for (int i = 0; i < InstantiateCount; i++)
            {
                CreateItem(i);
            }
        }

        HideAllItem();

        m_CurrentIndex = 0;
        m_PrevPos = 0;

        //先排序
        m_InstantiateItems.Sort(delegate (RectTransform rect1, RectTransform rect2)
        {
            return int.Parse(rect1.gameObject.name).CompareTo(int.Parse(rect2.gameObject.name));
        });

        if (m_ItemsCount > PageCount)
        {
            SetBound(GetRectByNum(m_ItemsCount));
        }
        else
        {
            SetBound(m_Page);
        }

        if (m_ItemsCount > InstantiateCount)
        {
            for (int i = 0; i < InstantiateCount; i++)
            {
                ShowItem(i);
                MoveItemToIndex(i, m_InstantiateItems[i]);
            }
        }
        else
        {
            for (int i = 0; i < m_ItemsCount; i++)
            {
                ShowItem(i);
                MoveItemToIndex(i, m_InstantiateItems[i]);
            }
        }
    }

    private void ShowItem(int index)
    {
        m_InstantiateItems[index].gameObject.SetActive(true);
    }

    private void HideItem(int index)
    {
        m_InstantiateItems[index].gameObject.SetActive(false);
    }

    private void HideAllItem()
    {
        for (int i = 0; i < InstantiateCount; i++)
        {
            HideItem(i);
        }
    }

    private void CreateItem(int index)
    {
        RectTransform item = GameObject.Instantiate(m_Cell);
        item.SetParent(transform, false);
        item.anchorMax = Vector2.up;
        item.anchorMin = Vector2.up;
        item.pivot = Vector2.up;
        item.name = "" + index;//"item_" + 

        item.anchoredPosition = direction == Direction.Horizontal ?
            new Vector2(Mathf.Floor(index / InstantiateSize.x) * CellRect.x, -(index % InstantiateSize.x) * CellRect.y) :
            new Vector2((index % InstantiateSize.y) * CellRect.x, -Mathf.Floor(index / InstantiateSize.y) * CellRect.y);
        m_InstantiateItems.Add(item);
        item.gameObject.SetActive(true);

        if (onCreate != null)
            onCreate(index, item.gameObject);
    }

    protected void RemoveItem(int index)
    {
        RectTransform item = m_InstantiateItems[index];
        m_InstantiateItems.Remove(item);
        RectTransform.Destroy(item.gameObject);

        if (onRemove != null)
            onRemove(index, item.gameObject);
    }

    protected void ClearAll()
    {
        if (m_Rect == null) return;//如果没有被初始化，则不需要清除
        foreach (Transform trans in transform)
        {
            if (trans != m_Cell)
                Destroy(trans.gameObject);
        }
        m_InstantiateItems = new List<RectTransform>();
        m_Rect.anchoredPosition = Vector2.zero;
    }

    protected void Reset()
    {
        m_Rect.anchoredPosition = Vector2.zero;
    }

    /// <summary>
    /// 由格子数量获取多少行多少列
    /// </summary>
    /// <param name="num"></param>格子个数
    /// <returns></returns>
    private Vector2 GetRectByNum(int num)
    {
        return direction == Direction.Horizontal ?
            new Vector2(m_Page.x, Mathf.CeilToInt(num / m_Page.x)) :
            new Vector2(Mathf.CeilToInt(num / m_Page.y), m_Page.y);

    }
    /// <summary>
    /// 设置content的大小
    /// </summary>
    /// <param name="rows"></param>行数
    /// <param name="cols"></param>列数
    private void SetBound(Vector2 bound)
    {
        m_Rect.sizeDelta = new Vector2(bound.y * CellRect.x, bound.x * CellRect.y);
    }

    protected float MaxPrevPos
    {
        get
        {
            float result;
            Vector2 max = GetRectByNum(m_ItemsCount);
            if (direction == Direction.Horizontal)
            {
                result = max.y - m_Page.y;
            }
            else
            {
                result = max.x - m_Page.x;
            }
            return result * CellScale;
        }
    }

    void Update()
    {
        if (m_ItemsCount == 0) return;

        while (scale * DirectionPos - m_PrevPos < -CellScale * 2)
        {
            if (m_PrevPos <= -MaxPrevPos) return;

            m_PrevPos -= CellScale;

            List<RectTransform> range = m_InstantiateItems.GetRange(0, PageScale);
            m_InstantiateItems.RemoveRange(0, PageScale);
            m_InstantiateItems.AddRange(range);
            for (int i = 0; i < range.Count; i++)
            {
                MoveItemToIndex(m_CurrentIndex * PageScale + m_InstantiateItems.Count + i, range[i]);
            }
            m_CurrentIndex++;
        }

        while (scale * DirectionPos - m_PrevPos > -CellScale)
        {
            if (Mathf.RoundToInt(m_PrevPos) >= 0) return;

            m_PrevPos += CellScale;

            m_CurrentIndex--;

            if (m_CurrentIndex < 0) return;

            List<RectTransform> range = m_InstantiateItems.GetRange(m_InstantiateItems.Count - PageScale, PageScale);
            m_InstantiateItems.RemoveRange(m_InstantiateItems.Count - PageScale, PageScale);
            m_InstantiateItems.InsertRange(0, range);
            for (int i = 0; i < range.Count; i++)
            {
                MoveItemToIndex(m_CurrentIndex * PageScale + i, range[i]);
            }
        }
    }

    protected void MoveItemToIndex(int index, RectTransform item)
    {
        item.anchoredPosition = getPosByIndex(index);
        UpdateItem(index, item.gameObject);
    }

    private Vector2 getPosByIndex(int index)
    {
        float x, y;
        if (direction == Direction.Horizontal)
        {
            x = index % m_Page.x;
            y = Mathf.FloorToInt(index / m_Page.x);
        }
        else
        {
            x = Mathf.FloorToInt(index / m_Page.y);
            y = index % m_Page.y;
        }
        return new Vector2(y * CellRect.x, -x * CellRect.y);
    }

    private void UpdateItem(int index, GameObject item)
    {
        item.SetActive(index < m_ItemsCount);
        
        if (item.activeSelf)
        {
            if (onUpdate != null)
                onUpdate(index, item);
        }
    }

    protected int GetFirstItemCell()
    {
        return int.Parse(m_InstantiateItems[0].name);
    }

    /// <summary>
    /// 通过总的index得到现在它在哪个编号的格子中
    /// </summary>
    /// <returns></returns>
    public int GetCellIndexByItemIndex(int index)
    {
        if (index < m_CurrentIndex * PageScale)
        {
            return -1;
        }
        else if (index >= m_CurrentIndex * PageScale + InstantiateCount)
        {
            return -1;
        }
        else
        {
            return GetFirstItemCell() + index - m_CurrentIndex * PageScale;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetRealCellIndexByCurCellIndex(int curCell)
    {
        int firtItemCell = GetFirstItemCell();
        int realCell;//实际总的index

        if (curCell >= firtItemCell)
        {
            realCell = m_CurrentIndex * (int)m_Page.y + curCell - firtItemCell;
        }
        else
        {
            realCell = m_CurrentIndex * (int)m_Page.y + InstantiateCount - firtItemCell + curCell;
        }

        return realCell;
    }
}

