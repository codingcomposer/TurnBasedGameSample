using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBasedRPG
{
    public class UnitBehaviour : MonoBehaviour
    {
        public Animator animator;
        public bool IsPlayerFaction { get; private set; }

        public UnitDataModel unitDataModel;
        public uint level;
        public RPGCharacterAnimsFREE.RPGCharacterController rpgCharacterController;
        private bool moveFinished;
        private WaitUntil waitUntilMoveFinished;
        private WaitUntil waitUntilAttackActionFinished;
        private PartyManager partyManager;
        private bool attackAnimationFinished = false;
        private bool attackActionFinished = false;
        private Stat runtimeStat;
        private Stat fieldStat;
        private UnitBehaviour target;
        public Transform weaponTr;
        private const float speed = 5f;
        private GameObject hpBar;
        [SerializeField]
        private Image hpBarFill;
        private GameObject damageCanvas;
        private TMPro.TextMeshProUGUI damageText;
        private void Awake()
        {
            animator.Play("Idle");
            waitUntilMoveFinished = new WaitUntil(() => moveFinished);
            waitUntilAttackActionFinished = new WaitUntil(() => attackActionFinished);
            damageCanvas = Instantiate(LoadedData.unitBundle.LoadAsset<GameObject>("DamageCanvas"), transform.parent);
            damageText = damageCanvas.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            damageCanvas.transform.rotation = Camera.main.transform.rotation;
            damageCanvas.SetActive(false);
            GameObject sword = Instantiate(LoadedData.unitBundle.LoadAsset<GameObject>("2Hand-Sword"), weaponTr);
            sword.transform.localRotation = Quaternion.identity;
        }

        public void Initialize(bool _faction, PartyManager _partyManager)
        {
            IsPlayerFaction = _faction;
            partyManager = _partyManager;
            transform.LookAt(IsPlayerFaction ? Vector3.right : Vector3.left);
            runtimeStat = StatMethods.CalculateRunTimeStat(unitDataModel.initialStat, level);
            fieldStat = StatMethods.Copy(runtimeStat);
            partyManager.RegisterUnit(this);
            string hpBarName = IsPlayerFaction ? "FriendlyHPBar" : "HostileHPBar";
            hpBar = Instantiate(LoadedData.unitBundle.LoadAsset<GameObject>(hpBarName), transform.parent);
            hpBar.transform.position = transform.position;
            hpBar.transform.rotation = Quaternion.identity;
            hpBarFill = hpBar.transform.GetChild(1).GetComponent<Image>();
            SetHPBar();
        }

        public void Attack(UnitBehaviour _target)
        {
            target = _target;
            if (!target)
            {
                target = partyManager.FindRandomTarget(!IsPlayerFaction);
            }
            StartCoroutine(AttackCoroutine());
        }

        private void OnMouseOver()
        {
            BattlePlayerUI.mouseOver.Invoke(this);
        }

        private void OnMouseExit()
        {
            BattlePlayerUI.mouseExit.Invoke(this);
        }

        private void OnMouseDown()
        {
            BattlePlayerUI.mouseClick.Invoke(this);
        }

        protected virtual IEnumerator AttackCoroutine()
        {
            Vector3 originalPosition = transform.position;
            Quaternion originalQuaternion = transform.rotation;
            hpBar.SetActive(false);
            if (target)
            {
                moveFinished = false;
                StartCoroutine(MoveToRange(target.transform.position, unitDataModel.attack.ActionRange));
            }
            yield return waitUntilMoveFinished;
            attackActionFinished = false;
            StartCoroutine(AttackAction());
            yield return waitUntilAttackActionFinished;
            moveFinished = false;
            StartCoroutine(MoveToRange(originalPosition, 0.1f));
            yield return waitUntilMoveFinished;
            transform.rotation = originalQuaternion;
            hpBar.SetActive(true);
            SetHPBar();
            StageManager.unitActionFinishedEvent.Invoke();
        }

        protected virtual IEnumerator AttackAction()
        {
            attackAnimationFinished = false;
            animator.Play("Attack");
            yield return new WaitUntil(() => attackAnimationFinished);
            attackActionFinished = true;
        }

        // Called from the animation event. Deals damages.
        protected void Hit()
        {
            target.Attacked(this, unitDataModel.attack.PhysicalAttackPower, unitDataModel.attack.MagicAttackPower);
        }

        public void AttackAnimationFinished()
        {
            Debug.Log("here");
            attackAnimationFinished = true;
        }

        private bool Attacked(UnitBehaviour attacker, long attackPower, long magicAttackPower)
        {
            long oldHP = fieldStat.hp;
            attackPower -= fieldStat.defense;
            magicAttackPower -= fieldStat.magicDefense;
            if(attackPower > 0)
            {
                fieldStat.hp -= attackPower;
            }
            if(magicAttackPower > 0)
            {
                fieldStat.hp -= magicAttackPower;
            }
            ShowDamageText(oldHP - fieldStat.hp);
            if (fieldStat.hp < oldHP)
            {
                if (fieldStat.hp > 0)
                {
                    animator.Play("Hit");
                }
                else
                {
                    fieldStat.hp = 0;
                    Die();
                }
            }
            SetHPBar();
            return true;
        }

        private void Die()
        {
            hpBar.SetActive(false);
            partyManager.UnitDead(this);
            animator.Play("Die");
        }

        private IEnumerator MoveToRange(Vector3 targetPosition, float range)
        {
            transform.LookAt(targetPosition);
            while((transform.position - targetPosition).sqrMagnitude > range)
            {
                animator.Play("Run");
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
            animator.Play("Idle");
            moveFinished = true;
        }

        protected void FootL()
        {

        }

        protected void FootR()
        {

        }

        private void SetHPBar()
        {
            hpBar.transform.position = transform.position;
            hpBar.transform.Translate(Vector3.up * 3f);
            hpBarFill.fillAmount = (float)fieldStat.hp / runtimeStat.hp;
        }

        private void ShowDamageText(long damage)
        {
            damageCanvas.transform.position = transform.position;
            damageCanvas.transform.Translate(Vector3.up * 2f);
            damageCanvas.SetActive(true);
            damageText.text = damage > 0L ? damage.ToString() : "MISS";
        }
    }
}
