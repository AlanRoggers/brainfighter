using UnityEngine;
using Nova;
public class HealthBars : MonoBehaviour
{
    private int healthHandler;
    private CharacterV5 fighter;
    public UIBlock2D VisualHealth = null;
    public TextBlock Health = null;
    private void Awake()
    {
        fighter = GetComponent<CharacterV5>();

        healthHandler = fighter.Health;

        if (Health != null)
            Health.Text = $"{fighter.Health}";
    }
    private void Update()
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (fighter.Health != healthHandler)
        {
            healthHandler = fighter.Health;
            if (Health != null)
                Health.Text = $"{fighter.Health}";
            if (VisualHealth != null)
            {
                VisualHealth.AutoSize.X = AutoSize.None;
                VisualHealth.Size.X = Length.Percentage(fighter.Health / 100f * (1 - VisualHealth.CalculatedMargin.X.Sum().Percent));
            }
        }
    }
}
