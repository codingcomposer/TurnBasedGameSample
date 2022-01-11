using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TurnBasedRPG {
    public class StageNameDisplay : MonoBehaviour
    {
        public StageManager stageManager;
        public TextMeshProUGUI text;

        private void Start()
        {
            StartCoroutine(View());
        }

        private IEnumerator View()
        {
            yield return new WaitUntil(() => stageManager.StageDataModel);
            text.text = stageManager.StageDataModel.StageName;
        }
    }
}
