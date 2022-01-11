using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TurnBasedRPG
{
    public class BattlePlayerUI : MonoBehaviour
    {
        public GameObject panel;
        public TextMeshProUGUI unitNameText;
        public TextMeshProUGUI attackButtonText;
        public TextMeshProUGUI skillButtonText;

        public void ShowUI(UnitBehaviour unitBehaviour)
        {
            panel.SetActive(true);
            unitNameText.text = unitBehaviour.unitDataModel.unitName;
            attackButtonText.text = unitBehaviour.unitDataModel.attack.Name;
            skillButtonText.text = unitBehaviour.unitDataModel.skill.Name;
        }

        private void DoNothing()
        {
            StageManager.unitActionFinishedEvent.Invoke();
        }
    }
}
