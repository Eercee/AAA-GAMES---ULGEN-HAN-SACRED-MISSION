using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    public Image damageImage;
    public float fadeSpeed = 5f;
    public Color damageColor = new Color(1f, 0f, 0f, 0.5f);

    private bool isTakingDamage = false;

    void Update()
    {
        if (damageImage == null)
        {
            Debug.LogError("DamageImage not assigned in the inspector.");
            return;
        }

        if (isTakingDamage)
        {
            damageImage.color = damageColor;
            isTakingDamage = false; 
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, fadeSpeed * Time.deltaTime);
        }

        
    }

    public void TriggerDamageEffect()
    {
        isTakingDamage = true;
    }
}
