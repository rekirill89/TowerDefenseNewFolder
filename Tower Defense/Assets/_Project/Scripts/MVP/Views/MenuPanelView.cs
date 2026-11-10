using System;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.MVP
{
    public class MenuPanelView : BasePanelView
    {
        [field:SerializeField] public CustomButton StartButton { get; private set; }
        [field:SerializeField] public TextMeshProUGUI MetaBalanceText { get; private set; }
        
        public void SetBalanceText(int balance)
        {
            MetaBalanceText.text = balance.ToString();
        }
    }
}