using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResourceController : MonoBehaviour
{
    [Header("Health Parameters")]
    public int health;
    public int maxHealth;

    [Header("Fear Parameters")]
    public int fear;
    public int maxFear = 3;
    public float fearDamagePercentage = 0.2f;
    
    [Header("UI Things")]
    public Image damageFlash;
    public Image fearFlash;
    public float visibleAlpha = 0.8f;
    public Image[] healthImages;
    public Sprite fullHealth;
    public Sprite emptyHealth;
    public Image[] fearImages;

    [Header("Invincibility")] // credit Week 4 Lab
    float kInvincibilityDuration = 1.0f;
    float mInvincibleTimer;
    public bool mInvincible;

    GameMasterController gameMaster;

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = FindObjectOfType<GameMasterController>();

        damageFlash.color = new Color(damageFlash.color.r, damageFlash.color.g, damageFlash.color.b, 0f);
        fearFlash.color = new Color(fearFlash.color.r, fearFlash.color.g, fearFlash.color.b, 0f);
    }

    private void Update()
    {
        // Health System from Blackthornprod https://www.youtube.com/watch?v=3uyolYVsiWc
        if (health > maxHealth) health = maxHealth;

        for (int i = 0; i < healthImages.Length; i++)
        {
            if(i < health)
            {
                healthImages[i].sprite = fullHealth;
            }
            else
            {
                healthImages[i].sprite = emptyHealth;
            }

            if(i < maxHealth)
            {
                healthImages[i].enabled = true;
            }
            else
            {
                healthImages[i].enabled = false;
            }
        }

        // Fear
        if (fear > maxFear) fear = maxFear;

        for (int i = 0; i < fearImages.Length; i++)
        {
            if (i < fear)
            {
                fearImages[i].color = new Color32(200,30,240,255);
            }
            else
            {
                fearImages[i].color = new Color32(186,186,186,255);
            }

            if (i < maxHealth)
            {
                healthImages[i].enabled = true;
            }
            else
            {
                healthImages[i].enabled = false;
            }
        }


        if (mInvincible) // credit Week 4 Lab
        {
            mInvincibleTimer += Time.deltaTime;
            if (mInvincibleTimer >= kInvincibilityDuration)
            {
                mInvincible = false;
                mInvincibleTimer = 0.0f;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (!mInvincible)
        {
            health -= amount;
            mInvincible = true;

            if(fear <= maxFear) StartFade(damageFlash); // If damage is not due to maxfear
            if (health <= 0) gameMaster.GameOver();
        }
    }

    public void IncreaseFear(int amount)
    {
        fear += amount;

        if(fear > maxFear) // strickly greater, I want to see the full fear bar before hp decrease
        {
            StartFade(fearFlash);
            TakeDamage((int)(maxHealth * fearDamagePercentage)); // First because of screen flash in TakeDamage
            fear = 0;
        }
    }
    
    public void Heal(int amount)
    {
        if(health < maxHealth) health += amount;
    }

    public void StartFade(Image flashImage)
    {
        StartCoroutine(FadeIn(visibleAlpha, 0.5f, flashImage));
    }

    IEnumerator FadeIn(float aValue, float aTime, Image flashImage)
    {
        float alpha = flashImage.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(flashImage.color.r, flashImage.color.g, flashImage.color.b, Mathf.Lerp(alpha, aValue, t));
            flashImage.color = newColor;
            yield return null;
        }

        StartCoroutine(FadeOut(0, 1.0f, flashImage));
    }

    IEnumerator FadeOut(float aValue, float aTime, Image flashImage)
    {
        float alpha = flashImage.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(flashImage.color.r, flashImage.color.g, flashImage.color.b, Mathf.Lerp(alpha, aValue, t));
            flashImage.color = newColor;
            yield return null;
        }
    }
}
