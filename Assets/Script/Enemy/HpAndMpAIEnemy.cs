using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpAndMpEnemy : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Image healthBarFill; // Kéo thả thanh HP_Fill vào đây

    [Header("Energy Settings")]
    public float maxEnergy = 100f;
    public float currentEnergy = 0f;
    public Image energyBarFill; // Kéo thả thanh MP_Fill vào đây

    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        currentEnergy = 0f;
        UpdateUI();
    }
    // hàm trừ hp
    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Giữ máu không bị âm

        // Bật animation bị đánh trúng
        //animator.SetTrigger("Hurt");

        //if (currentHealth <= 0)
        //{
        //    Die();
        //}

        UpdateUI();
    }
    //hàm cộng mp
    public void GainEnergy(float energyAmount)
    {
        if (isDead) return;

        currentEnergy += energyAmount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        UpdateUI();
    }
    //hàm reset mp sau khi dùng nộ
    public void ResetEnergy()
    {
        currentEnergy = 0f;
        UpdateUI();
    }
    //hàm update chỉ số mp và hp trên unity
    private void UpdateUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }

        if (energyBarFill != null)
        {
            energyBarFill.fillAmount = currentEnergy / maxEnergy;
        }
    }
    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        animator.SetBool("IsDead", true);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        this.enabled = false;
    }
}
