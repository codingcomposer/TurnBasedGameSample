using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedRPG
{
    public class UnitBehaviour : MonoBehaviour
    {
        public Animator animator;
        public bool IsPlayerFaction { get; private set; }

        public UnitDataModel unitDataModel;
        private const float MOVE_SPEED = 5f;
        private bool moveFinished;
        private WaitUntil waitUntilMoveFinished;
        private PartyManager partyManager;

        private void Awake()
        {
            animator.Play("Idle");
            waitUntilMoveFinished = new WaitUntil(() => moveFinished);
        }

        public void Initialize(bool _faction, PartyManager _partyManager)
        {
            IsPlayerFaction = _faction;
            partyManager = _partyManager;
            partyManager.RegisterUnit(this);
        }

        public void Attack(UnitBehaviour target)
        {
            if (!target)
            {
                target = partyManager.FindRandomTarget(!IsPlayerFaction);
            }
            StartCoroutine(AttackCoroutine(target));
        }

        protected virtual IEnumerator AttackCoroutine(UnitBehaviour target)
        {
            if (target)
            {
                StartCoroutine(MoveToRange(target.transform.position, unitDataModel.attack.ActionRange));
            }
            yield return waitUntilMoveFinished;
            StartCoroutine(AttackAction(target));
        }

        protected virtual IEnumerator AttackAction(UnitBehaviour target)
        {
            bool attackAnimationFinished = false;
            animator.Play("Attack");
            AnimatorStateExit.exit.AddListener(delegate { attackAnimationFinished = true; });
            yield return new WaitUntil(() => attackAnimationFinished);
            animator.Play("Idle");
            StageManager.unitActionFinishedEvent.Invoke();
            //yield return null;
        }

        private IEnumerator MoveToRange(Vector3 targetPosition, float range)
        {
            while((transform.position - targetPosition).sqrMagnitude > range)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * MOVE_SPEED);
                yield return null;
            }
            moveFinished = true;
        }
    }
}
