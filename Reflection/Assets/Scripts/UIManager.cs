using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{   
    public TextMeshProUGUI PlayerHp;
    public TextMeshProUGUI PlayerFrag;
    public Slider StaminaSlider;
    public Slider BossHpSlider;
    public Slider ShieldSlider;
    
    
    private static UIManager _instance;

    public static UIManager Instance
    {
        get{
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }

    public void UpdatePlayerHp(int hp)
    {
        PlayerHp.text = "X " + hp;
    }

    public void UpdatePlayerFrag(int frag)
    {
        PlayerFrag.text = "X " + frag + " / 12";
    }

    public void ChangeStaminaPercentage(float percent)
    {
        StaminaSlider.value = percent;
    }

    public void ChangeBossHpPercentage(float percent)
    {
        BossHpSlider.value = percent;
    }

    public void ChangeShieldPercentage(float percent)
    {
        ShieldSlider.value = percent;
        if (percent > 0.25f)
        {
            
        }
        else
        {
            
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
}
