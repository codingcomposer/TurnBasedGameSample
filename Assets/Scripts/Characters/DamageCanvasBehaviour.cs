using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCanvasBehaviour : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    private Color textColor;
    private void Awake()
    {
        textColor = text.color;
    }
    private void OnEnable()
    {
        text.color = textColor;
        StartCoroutine(Bounce());
    }

    private IEnumerator Bounce()
    {
        Color color = text.color; 
        float t = 0f;
        while (t < 0.8f)
        {
            t += Time.deltaTime;
            color.a -= Time.deltaTime;
            text.color = color;
            transform.Translate(Vector3.up * Time.deltaTime * Mathf.Cos(t));
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
