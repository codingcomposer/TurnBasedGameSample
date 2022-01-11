using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TurnBasedRPG {
    public class StageManager : MonoBehaviour
    {
        // stage
        public BattlePlayerUI battlePlayerUI;
        public StageDataModel StageDataModel { get; private set; }
        public GameObject loadingMessage;
        public static UnityEvent unitActionFinishedEvent = new UnityEvent();
        public MeshRenderer groundRenderer;

        private Queue<UnitBehaviour> attackQueue = new Queue<UnitBehaviour>();
        private bool gameEnd;
        private bool unitActionIsFinished = false;
        private WaitUntil waitUntilUnitActionFinished;
        private PartyManager partyManager = new PartyManager();

        private void Awake()
        {
            // instantiate map, characters.
            unitActionFinishedEvent.AddListener(OnUnitActionFinished);
            waitUntilUnitActionFinished = new WaitUntil(() => unitActionIsFinished);
            StartCoroutine(WaitForAssetsAndGo());
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
            for(int i = 0; i < myUnits.Length; i++)
            {
                Instantiate(myUnits[i], GetRandomPositionInsideBattleGround(true), Quaternion.identity).GetComponent<UnitBehaviour>().Initialize(true, partyManager);
            }
        }

        private void SpawnEnemyUnits()
        {
            string[] enemyUnitNames = StageDataModel.EnemyUnitNames;
            for (int i = 0; i < enemyUnitNames.Length; i++)
            {
                Instantiate(LoadedData.unitBundle.LoadAsset<GameObject>(enemyUnitNames[i]), GetRandomPositionInsideBattleGround(false), Quaternion.identity).GetComponent<UnitBehaviour>().Initialize(false, partyManager);
            }
        }

        private Vector3 GetRandomPositionInsideBattleGround(bool faction)
        {
            Vector3 position = new Vector3();
            if (faction)
            {
                position.x = Random.Range(groundRenderer.bounds.min.x, 0f);
                position.z = Random.Range(groundRenderer.bounds.min.z, groundRenderer.bounds.max.z);
            }
            else
            {
                position.x = Random.Range(0f, groundRenderer.bounds.max.z);
                position.z = Random.Range(groundRenderer.bounds.min.z, groundRenderer.bounds.max.z);
            }
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
                if(attackQueue.Count < 1)
                {
                    attackQueue = partyManager.GetAttackQueue();
                    if(attackQueue.Count < 1)
                    {
                        break;
                    }
                }
                Debug.Log("here");
                UnitBehaviour turnUnit = attackQueue.Dequeue();
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
    }
}
