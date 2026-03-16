using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Health
{
    public class HealthBar : MonoBehaviour
    {
        public Image fillBar;

        public TextMeshProUGUI textValue;

        public void UpdateBar(int currentValue, int maxValue)
        {
            fillBar.fillAmount = (float) currentValue / (float) maxValue;
            textValue.text = currentValue.ToString() +"%";
        }


    }
}
