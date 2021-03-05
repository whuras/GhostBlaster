using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public float visibleAlpha = 0.5f;

    private void Start()
    {
        Color newColor = new Color(1, 1, 1, 0f);
        transform.GetComponent<SpriteRenderer>().material.color = newColor;
    }

    public void StartFade()
    {
        StartCoroutine(FadeIn(visibleAlpha, 1.0f));
    }

    IEnumerator FadeIn(float aValue, float aTime)
    {
        float alpha = transform.GetComponent<SpriteRenderer>().material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            transform.GetComponent<SpriteRenderer>().material.color = newColor;
            yield return null;
        }
        StartCoroutine(FadeOut(0, 1.0f));
    }

    IEnumerator FadeOut(float aValue, float aTime)
    {
        float alpha = transform.GetComponent<SpriteRenderer>().material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            transform.GetComponent<SpriteRenderer>().material.color = newColor;
            yield return null;
        }

    }
}