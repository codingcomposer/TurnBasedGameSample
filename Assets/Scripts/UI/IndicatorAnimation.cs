using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorAnimation : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        float t = 0f;
        Vector3 scale = transform.localScale;
        while (gameObject.activeInHierarchy)
        {
            t += Time.deltaTime;
            scale.x = 1f + 0.1f * Mathf.Sin(10f * t);
            scale.y = 1f + 0.1f * Mathf.Sin(10f * t);
            transform.localScale = scale;
            yield return null;
        }
    }
}
