using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{   
    public TextMeshProUGUI PlayerHp;
    public TextMeshProUGUI PlayerFrag;
    public Slider StaminaSlider;
    
    
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
        PlayerHp.text = "Hp: " + hp;
    }

    public void UpdatePlayerFrag(int frag)
    {
        PlayerFrag.text = "Frag: " + frag;
    }

    public void ChangeStaminaPercentage(float percent)
    {
        StaminaSlider.value = percent;
    }

    private void Update()
    {
        
    }
}
