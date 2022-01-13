using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TurnBasedRPG
{
    public class StageManager : MonoBehaviour
    {
        // stage
        public BattlePlayerUI battlePlayerUI;
        public StageDataModel StageDataModel { get; private set; }
        public GameObject loadingMessage;
        public static UnityEvent unitActionFinishedEvent = new UnityEvent();
        public MeshRenderer groundRenderer;
        public Transform friendlyUnitsTr;
        public Transform hostileUnitsTr;

        public static UnityEvent win = new UnityEvent();
        public static UnityEvent lose = new UnityEvent();
        public GameObject resultPanel;
        private bool gameEnd;
        private bool unitActionIsFinished = false;
        private WaitUntil waitUntilUnitActionFinished;
        private PartyManager partyManager = new PartyManager();
        private WaitForSecondsRealtime termBetweenTurns = new WaitForSecondsRealtime(0.5f);

        private void Awake()
        {
            // instantiate map, characters.
            unitActionFinishedEvent.AddListener(OnUnitActionFinished);
            waitUntilUnitActionFinished = new WaitUntil(() => unitActionIsFinished);
            StartCoroutine(WaitForAssetsAndGo());
            win.AddListener(delegate { PrintResult("Win!"); });
            lose.AddListener(delegate { PrintResult("Lose!"); });
        }

        private IEnumerator WaitForAssetsAndGo()
        {
            GameObject loadingMessageInstance = Instantiate(loadingMessage);

            yield return new WaitUntil(() => LoadedData.assetsLoaded);
            Destroy(loadingMessageInstance);
            string stageName = GameObject.Find("DataHolder").GetComponent<StageDataHolder>().stageName;
            if (!string.IsNullOrEmpty(stageName))
            {
                StageDataModel = LoadedData.stageBundle.LoadAsset<StageDataModel>(stageName);
            }
            if (StageDataModel)
            {
                SpawnMyUnits();
                SpawnEnemyUnits();
                StartBattle();
            }
        }

        private void SpawnMyUnits()
        {
            GameObject[] myUnits = GameObject.Find("DataHolder").GetComponent<StageDataHolder>().myUnits;
            for (int i = 0; i < myUnits.Length; i++)
            {
                GameObject instance = Instantiate(myUnits[i], friendlyUnitsTr);
                instance.transform.position = GetSpawnPosition(true, i, myUnits.Length);
                instance.transform.rotation = Quaternion.identity;
                instance.GetComponent<UnitBehaviour>().Initialize(true, partyManager);
            }
        }

        private void SpawnEnemyUnits()
        {
            string[] enemyUnitNames = StageDataModel.EnemyUnitNames;
            for (int i = 0; i < enemyUnitNames.Length; i++)
            {
                GameObject instance = Instantiate(LoadedData.unitBundle.LoadAsset<GameObject>(enemyUnitNames[i]), hostileUnitsTr);
                instance.transform.position = GetSpawnPosition(false, i, enemyUnitNames.Length);
                instance.transform.rotation = Quaternion.identity;
                instance.GetComponent<UnitBehaviour>().Initialize(false, partyManager);
            }
        }

        private Vector3 GetSpawnPosition(bool faction, int factionUnitIndex, int factionUnitCount)
        {
            float minX = faction ? groundRenderer.bounds.min.x : 0f;
            float maxX = faction ? 0f : groundRenderer.bounds.max.x;
            Vector3 position = new Vector3();
            position.x = minX + (maxX - minX) * ((float)factionUnitIndex / factionUnitCount);
            position.z = Random.Range(groundRenderer.bounds.min.z, groundRenderer.bounds.max.z);
            return position;
        }

        private void StartBattle()
        {
            StartCoroutine(TakeTurns());
        }

        private void OnUnitActionFinished()
        {
            unitActionIsFinished = true;
        }

        private IEnumerator TakeTurns()
        {
            while (!gameEnd)
            {
                yield return termBetweenTurns;
                if (partyManager.attackQueue.Count < 1)
                {
                    partyManager.FillAttackQueue();
                    if (partyManager.attackQueue.Count < 1)
                    {
                        break;
                    }
                }
                UnitBehaviour turnUnit = partyManager.DequeueWhenAttack();
                if (turnUnit.IsPlayerFaction)
                {
                    battlePlayerUI.ShowUI(turnUnit);
                }
                else
                {
                    turnUnit.Attack(null);
                }
                yield return waitUntilUnitActionFinished;
                unitActionIsFinished = false;
            }
        }

        private void PrintResult(string result)
        {
            gameEnd = true;
            resultPanel.SetActive(true);
            TMPro.TextMeshProUGUI text = resultPanel.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            text.text = result;
        }
    }
}
