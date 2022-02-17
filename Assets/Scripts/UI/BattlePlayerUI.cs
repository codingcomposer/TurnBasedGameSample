using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

namespace TurnBasedRPG
{
    public class MouseEvent : UnityEvent<UnitBehaviour> { }
    public class BattlePlayerUI : MonoBehaviour
    {
        public static MouseEvent mouseOver = new MouseEvent();
        public static MouseEvent mouseExit = new MouseEvent();
        public static MouseEvent mouseClick = new MouseEvent();
        public GameObject panel;
        public GameObject selectTargetTextPanel;
        public Button attackButton;
        public Button useItemButton;
        public Button doNothingButton;
        public TextMeshProUGUI unitNameText;
        public TextMeshProUGUI attackButtonText;
        public TextMeshProUGUI skillButtonText;
        private bool selectingUnit = false;
        private UnitBehaviour unitBehaviour;
        private UnitBehaviour selectedTarget;
        public GameObject selectIndicator;
        public GameObject currentPlayingUnitIndicator;
        private readonly Vector3 selectIndicatorOffset = new Vector3(0f, 2f, -1f);
        private bool targetFaction;

        private void Awake()
        {
            attackButton.onClick.AddListener(OnAttackButtonClicked);
            useItemButton.onClick.AddListener(OnUseItemButtonClicked);
            doNothingButton.onClick.AddListener(OnDoNothingButtonClicked);
            mouseOver.AddListener(OnMouseOverTr);
            mouseClick.AddListener(OnMouseClickTr);
            mouseExit.AddListener(OnMouseExitTr);
        }

        public void ShowUI(UnitBehaviour _unitBehaviour)
        {
            unitBehaviour = _unitBehaviour;
            if (unitBehaviour)
            {
                panel.SetActive(true);
                unitNameText.text = unitBehaviour.unitDataModel.unitName;
                attackButtonText.text = unitBehaviour.unitDataModel.attack.Name;
                skillButtonText.text = unitBehaviour.unitDataModel.skill.Name;
                currentPlayingUnitIndicator.transform.position = unitBehaviour.transform.position + selectIndicatorOffset;
                currentPlayingUnitIndicator.SetActive(true);
            }
            else
            {
                Debug.LogError("Unit is null");
            }
        }

        private void OnMouseOverTr(UnitBehaviour mouseUnit)
        {
            if (selectingUnit && targetFaction == mouseUnit.IsPlayerFaction)
            {
                selectIndicator.SetActive(true);
                selectIndicator.transform.position = mouseUnit.transform.position + selectIndicatorOffset;
                unitBehaviour.transform.LookAt(mouseUnit.transform.position);
            }
        }

        private void OnMouseClickTr(UnitBehaviour mouseUnit)
        {
            if (selectingUnit && targetFaction == mouseUnit.IsPlayerFaction)
            {
                if (mouseUnit.gameObject.CompareTag("Character"))
                {
                    selectedTarget = mouseUnit;
                    selectIndicator.SetActive(false);
                    unitBehaviour.Attack(selectedTarget);
                    selectTargetTextPanel.SetActive(false);
                    selectingUnit = false;
                    currentPlayingUnitIndicator.SetActive(false);
                }
            }
        }


        private void OnMouseExitTr(UnitBehaviour mouseUnit)
        {
            selectIndicator.SetActive(false);
        }

        private void OnAttackButtonClicked()
        {
            targetFaction = !unitBehaviour.IsPlayerFaction;
            panel.SetActive(false);
            selectTargetTextPanel.SetActive(true);
            selectingUnit = true;
        }

        private void OnUseItemButtonClicked()
        {

        }


        private void OnDoNothingButtonClicked()
        {
            panel.SetActive(false); 
            selectTargetTextPanel.SetActive(false);
             selectingUnit = false;
            currentPlayingUnitIndicator.SetActive(false);
            StageManager.unitActionFinishedEvent.Invoke();
        }


    }

}
