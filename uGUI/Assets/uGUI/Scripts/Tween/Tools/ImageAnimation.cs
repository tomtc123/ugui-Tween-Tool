using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
public class ImageAnimation : MonoBehaviour
{
    [SerializeField]
    private float m_frameGap = 0.25f;
    [SerializeField]
    private bool m_autoNativeSize = false;
    [SerializeField]
    private Sprite[] m_sprites; 

    public Image image
    {
        get;
        private set;
    }
    int m_curFrame = 0;
    int m_spriteCount = 0;
    float m_lastTime = 0;

    void Awake()
    {
        image = this.GetComponent<Image>();

        CheckSpriteCount(m_sprites);

        if (m_spriteCount > 0)
            image.sprite = m_sprites[0];
    }

    void Update()
    {
        if (m_spriteCount <= 0)
            return;

        if (Time.realtimeSinceStartup - m_lastTime > m_frameGap)
        {
            m_lastTime = Time.realtimeSinceStartup;
            m_curFrame++;
            if (m_curFrame >= m_spriteCount)
                m_curFrame = 0;

            image.sprite = m_sprites[m_curFrame];

            if (m_autoNativeSize)
                image.SetNativeSize();
        }
    }

    // 自动截断null后面的sprite
    void CheckSpriteCount(Sprite[] sprites)
    {
        m_spriteCount = 0;
        if (sprites != null)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i] == null)
                    break;

                m_spriteCount++;
            }
        }
    }

    public void SetSprites(Sprite[] sprites, float frameGap = 0.25f, bool nativeSize = false)
    {
        if (m_sprites == null || sprites.Length > m_sprites.Length)
            m_sprites = new Sprite[sprites.Length];

        for (int i = 0; i < m_sprites.Length; i++)
        {
            if (i < sprites.Length)
                m_sprites[i] = sprites[i];
            else
                m_sprites[i] = null;
        }

        m_frameGap = frameGap;
        m_autoNativeSize = nativeSize;

        CheckSpriteCount(m_sprites);

        if (m_spriteCount > 0)
            image.sprite = m_sprites[0];

        if (m_autoNativeSize)
            image.SetNativeSize();
    }

    public static ImageAnimation Begin(Image image, Sprite[] sprites, float frameGap = 0.25f, bool nativeSize = false)
    {
        ImageAnimation com = image.GetComponent<ImageAnimation>();
        if (com == null)
        {
            com = image.gameObject.AddComponent<ImageAnimation>();
        }

        com.SetSprites(sprites, frameGap, nativeSize);
        return com;
    }
}
