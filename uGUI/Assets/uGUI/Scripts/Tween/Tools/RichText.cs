using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//富文本
//格式: <quad name=Internal#thumb size=45 width=1/>
//说明: #分隔符, 0图集名, 1图片名
//图片名gif起始为动态表情

[AddComponentMenu("UI/Rich Text")]
public class RichText : Text, IPointerClickHandler
{
    /// <summary>
    /// 图片池
    /// </summary>
    private readonly List<Image> m_ImagesPool = new List<Image>();

    /// <summary>
    /// 图片的最后一个顶点的索引
    /// </summary>
    private readonly List<int> m_ImagesVertexIndex = new List<int>();

    /// <summary>
    /// 正则取出所需要的属性
    /// </summary>
    private static readonly Regex s_Regex =
        new Regex(@"<quad name=(.+?) size=(\d*\.?\d+%?) width=(\d*\.?\d+%?)/>", RegexOptions.Singleline);


    private int m_EmojiCharSize = 4;//图片占用字符大小

    public int RealLength { get; private set; }

#if UNITY_EDITOR
    private string[] spriteSuffix = new string[] { ".png", ".jpg", ".tga", ".psd", ".bmp" };
#endif


    protected override void Start()
    {
        base.Start();
    }

    public override void SetVerticesDirty()
    {
        base.SetVerticesDirty();
        UpdateQuadImage();
    }

    IEnumerator LoadSprite(Image image, string atlasname, List<string> spritenames)
    {
        int count = spritenames.Count;
        Sprite[] sprites = new Sprite[count];
        for (int i = 0; i < count; i++)
        {
#if UNITY_EDITOR
            yield return null;
            for (int k = 0; k < spriteSuffix.Length; k++)
            {
                image.sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(string.Format("Assets/UGUI/Atlas/{0}/{1}{2}", atlasname, spritenames[i], spriteSuffix[k]));
                if (image.sprite != null)
                    break;
            }
#else
            yield return UIManager.SetSpriteAsync(image, atlasname, spritenames[i]);
#endif
            sprites[i] = image.sprite;
        }
        ImageAnimation.Begin(image, sprites);
    }

    private void SetImageAnimationActive(Image _image, string atlasname, string _name, bool active)
    {
        if (active)
        {
            if (this.gameObject.activeInHierarchy)
            {
                List<string> names = new List<string>();
                string s = _name.Split('_')[0];
                for (int i = 0; i < 3; i++)
                {
                    string str = string.Format("{0}_{1}", s, i);
                    names.Add(str);
                }

                this.StartCoroutine(LoadSprite(_image, atlasname, names));
            }
        }
        else
        {
            ImageAnimation ia = _image.GetComponent<ImageAnimation>();
            if (ia != null)
                ia.enabled = false;
        }
    }

    /// <summary>
    /// 解析完最终的文本
    /// </summary>
    private string m_OutputText;

    protected void UpdateQuadImage()
    {
#if UNITY_EDITOR
        if (UnityEditor.PrefabUtility.GetPrefabType(this) == UnityEditor.PrefabType.Prefab)
        {
            return;
        }
#endif
        m_OutputText = GetOutputText();
        m_ImagesVertexIndex.Clear();

        MatchCollection matchcollection = s_Regex.Matches(m_OutputText);
        int matchLength = 0;
        foreach (Match match in matchcollection)
        {
            matchLength += match.Length;
            var picIndex = match.Index;
            var endIndex = picIndex * 4 + 3;
            m_ImagesVertexIndex.Add(endIndex);

            m_ImagesPool.RemoveAll(image => image == null);
            if (m_ImagesPool.Count == 0)
            {
                GetComponentsInChildren<Image>(true, m_ImagesPool);
            }
            if (m_ImagesVertexIndex.Count > m_ImagesPool.Count)
            {
                var resources = new DefaultControls.Resources();
                var go = DefaultControls.CreateImage(resources);
                go.layer = gameObject.layer;
                var rt = go.transform as RectTransform;
                if (rt)
                {
                    rt.SetParent(rectTransform);
                    //rt.localPosition = Vector3.zero;
                    rt.anchoredPosition3D = Vector3.zero;
                    rt.localRotation = Quaternion.identity;
                    rt.localScale = Vector3.one;
                }
                m_ImagesPool.Add(go.GetComponent<Image>());
            }

            var spriteFullName = match.Groups[1].Value.Split('#');
            var spriteName = spriteFullName[1];
            var size = float.Parse(match.Groups[2].Value);
            var img = m_ImagesPool[m_ImagesVertexIndex.Count - 1];
            if (img.sprite == null || img.sprite.name != spriteName)
            {
#if UNITY_EDITOR
                for (int k = 0; k < spriteSuffix.Length; k++)
                {
                    img.sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(string.Format("Assets/UGUI/Atlas/{0}/{1}{2}", spriteFullName[0], spriteName, spriteSuffix[k]));
                    if (img.sprite != null)
                        break;
                }
#else
                UIManager.SetSpriteAsync(img, spriteFullName[0], spriteName);
#endif
            }
            img.rectTransform.sizeDelta = new Vector2(size, size);
            img.gameObject.SetActive(true);
            if (spriteName.StartsWith("gif"))
            {
                SetImageAnimationActive(img, spriteFullName[0], spriteName, true);
            }
            else
            {
                SetImageAnimationActive(img, spriteFullName[0], spriteName, false);
            }
        }

        RealLength = text.Length - matchLength + m_EmojiCharSize * matchcollection.Count;

        for (var i = m_ImagesVertexIndex.Count; i < m_ImagesPool.Count; i++)
        {
            if (m_ImagesPool[i])
            {
                m_ImagesPool[i].gameObject.SetActive(false);
            }
        }
    }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        var orignText = m_Text;
        m_Text = m_OutputText;
        base.OnPopulateMesh(toFill);
        m_Text = orignText;

        UIVertex vert = new UIVertex();
        for (var i = 0; i < m_ImagesVertexIndex.Count; i++)
        {
            var endIndex = m_ImagesVertexIndex[i];
            var rt = m_ImagesPool[i].rectTransform;
            var size = rt.sizeDelta;
            if (endIndex < toFill.currentVertCount)
            {
                toFill.PopulateUIVertex(ref vert, endIndex);
                rt.anchoredPosition = new Vector2(vert.position.x + size.x / 2, vert.position.y + size.y / 2);

                // 抹掉左下角的小黑点
                for (int j = endIndex, m = endIndex - 3; j > m; j--)
                {
                    toFill.SetUIVertex(new UIVertex(), j);
                }
            }
        }

        if (m_ImagesVertexIndex.Count != 0)
        {
            m_ImagesVertexIndex.Clear();
        }

        // 处理超链接包围框
        foreach (var hrefInfo in m_HrefInfos)
        {
            hrefInfo.boxes.Clear();
            if (hrefInfo.startIndex >= toFill.currentVertCount)
            {
                continue;
            }

            // 将超链接里面的文本顶点索引坐标加入到包围框
            toFill.PopulateUIVertex(ref vert, hrefInfo.startIndex);
            var pos = vert.position;
            var bounds = new Bounds(pos, Vector3.zero);
            for (int i = hrefInfo.startIndex, m = hrefInfo.endIndex; i < m; i++)
            {
                if (i >= toFill.currentVertCount)
                {
                    break;
                }

                toFill.PopulateUIVertex(ref vert, i);
                pos = vert.position;
                if (pos.x < bounds.min.x) // 换行重新添加包围框
                {
                    hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
                    bounds = new Bounds(pos, Vector3.zero);
                }
                else
                {
                    bounds.Encapsulate(pos); // 扩展包围框
                }
            }
            hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
        }
    }

    /// <summary>
    /// 超链接信息列表
    /// </summary>
    private readonly List<HrefInfo> m_HrefInfos = new List<HrefInfo>();

    /// <summary>
    /// 文本构造器
    /// </summary>
    private static readonly StringBuilder s_TextBuilder = new StringBuilder();

    /// <summary>
    /// 超链接正则
    /// </summary>
    private static readonly Regex s_HrefRegex =
        new Regex(@"<a href=([^>\n\s]+)>(.*?)(</a>)", RegexOptions.Singleline);

    [Serializable]
    public class HrefClickEvent : UnityEvent<string> { }

    [SerializeField]
    private HrefClickEvent m_OnHrefClick = new HrefClickEvent();

    /// <summary>
    /// 超链接点击事件
    /// </summary>
    public HrefClickEvent onHrefClick
    {
        get { return m_OnHrefClick; }
        set { m_OnHrefClick = value; }
    }

    /// <summary>
    /// 获取超链接解析后的最后输出文本
    /// </summary>
    /// <returns></returns>
    protected string GetOutputText()
    {
        s_TextBuilder.Length = 0;
        m_HrefInfos.Clear();
        var indexText = 0;
        foreach (Match match in s_HrefRegex.Matches(text))
        {
            s_TextBuilder.Append(text.Substring(indexText, match.Index - indexText));
            s_TextBuilder.Append("<color=blue>");  // 超链接颜色

            var group = match.Groups[1];
            var hrefInfo = new HrefInfo
            {
                startIndex = s_TextBuilder.Length * 4, // 超链接里的文本起始顶点索引
                endIndex = (s_TextBuilder.Length + match.Groups[2].Length - 1) * 4 + 3,
                name = group.Value
            };
            m_HrefInfos.Add(hrefInfo);

            s_TextBuilder.Append(match.Groups[2].Value);
            s_TextBuilder.Append("</color>");
            indexText = match.Index + match.Length;
        }
        s_TextBuilder.Append(text.Substring(indexText, text.Length - indexText));
        return s_TextBuilder.ToString();
    }

    /// <summary>
    /// 点击事件检测是否点击到超链接文本
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 lp;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, eventData.position, eventData.pressEventCamera, out lp);

        foreach (var hrefInfo in m_HrefInfos)
        {
            var boxes = hrefInfo.boxes;
            for (var i = 0; i < boxes.Count; ++i)
            {
                if (boxes[i].Contains(lp))
                {
                    m_OnHrefClick.Invoke(hrefInfo.name);
                    Debug.LogError(hrefInfo.name);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 超链接信息类
    /// </summary>
    private class HrefInfo
    {
        public int startIndex;

        public int endIndex;

        public string name;

        public readonly List<Rect> boxes = new List<Rect>();
    }
}