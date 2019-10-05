using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//状态效果值
public enum FadeStatuss
{
    FadeIn,
    FadeOut
}

public class Fade : MonoBehaviour
{
    //设置的图片
    public Image m_Sprite;
    //透明值
    private float m_Alpha;
    //淡入淡出状态
    private FadeStatuss m_Statuss;
    //效果更新的速度
    public float m_UpdateTime;
    //场景名称
    public string m_ScenesName;

    private bool IsActive;
    // Use this for initialization
    void Start()
    {
        m_Statuss = FadeStatuss.FadeIn;
        IsActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            //控制透明值变化
            if (m_Statuss == FadeStatuss.FadeIn)
            {
                m_Alpha += m_UpdateTime * Time.deltaTime;
            }
            else if (m_Statuss == FadeStatuss.FadeOut)
            {
                m_Alpha -= m_UpdateTime * Time.deltaTime;
            }

            UpdateColorAlpha();
        }
    }

    public void setIsActive()
    {
        IsActive = true;
        
    }
    

    void UpdateColorAlpha()
    {
        //获取到图片的透明值
        Color ss = m_Sprite.color;
        ss.a = m_Alpha;
        //将更改过透明值的颜色赋值给图片
        m_Sprite.color = ss;
        //透明值等于的1的时候 转换成淡出效果
        if (m_Alpha > 1f)
        {
            Application.LoadLevel("TestCharacterMovementScen");
            m_Alpha = 1f;
            //m_Statuss = FadeStatuss.FadeOut;
        }
        //值为0的时候跳转场景
        else if (m_Alpha < 0)
        {
           // Application.LoadLevel("loadingScene");
        }
    }
}
