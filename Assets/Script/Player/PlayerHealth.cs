using Assets.Script.Health;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Script.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        // HIEU
        [SerializeField] int maxHealth;
        int currentHealth;
        public HealthBar healthBar;

        public UnityEvent OnDeath;

        private void OnEnable()
        {
            OnDeath.AddListener(Death);
        }

        private void OnDisable()
        {
            OnDeath.RemoveListener(Death);  
        }

        void Start()
        {
            // Hieu
            currentHealth = maxHealth;
            healthBar.UpdateBar(currentHealth, maxHealth);
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;


            if(currentHealth < 0)
            {
                currentHealth = 0;
                OnDeath.Invoke();
            }
            healthBar.UpdateBar(currentHealth, maxHealth);
        }

        public void Death()
        {
            Destroy(gameObject);
        }

        public void Falling()
        {

        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.K))
            {
                TakeDamage(20);
            }
        }
    }
}
