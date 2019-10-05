using System;
using UnityEngine;
using UnityEngine.UI;


//状态效果值

public class FadeIn : MonoBehaviour
{
    //设置的图片
    public Image m_Sprite;
    //透明值
    private float m_Alpha;
    //淡入淡出状态
    private FadeStatuss m_Statuss;
    //效果更新的速度
    public float m_UpdateTime;

    private void Awake()
    {
        Debug.Log("??");
        Debug.Log(m_UpdateTime);
    }

    void Start()
    {
        //默认设置为淡入效果
        m_Statuss = FadeStatuss.FadeOut;
    }

    // Update is called once per frame
    void Update()
    {
        //控制透明值变化
        if (m_Statuss == FadeStatuss.FadeOut)
        {
            m_Alpha -= m_UpdateTime * Time.deltaTime;
        }
        UpdateColorAlpha();
    }

    void UpdateColorAlpha()
    {
        //获取到图片的透明值
        Color ss = m_Sprite.color;
        ss.a = m_Alpha;
        //将更改过透明值的颜色赋值给图片
        m_Sprite.color = ss;

        //值为0的时候跳转场景
        if (m_Alpha < 0)
        {
            //
        }
    }
}