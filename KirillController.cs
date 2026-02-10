using UnityEngine;

public class KirillController : MonoBehaviour
{
    [Header("Настройки персонажа")]
    [SerializeField] private string characterTag = "Character01ed";
    [SerializeField] private int health = 100;
    
    [Header("Диалоговая система")]
    [SerializeField] private DialogueTrigger dialogueTrigger;
    
    private bool isAlive = true;
    
    private void Start()
    {
        if (dialogueTrigger != null)
        {
            dialogueTrigger.OnDialogueOptionSelected += HandleDialogueOption;
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (!isAlive) return;
        
        health -= damage;
        
        if (health <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        isAlive = false;
        
        // Уведомляем систему квестов
        QuestSystem.Instance.OnCharacterKilled(characterTag);
        
        // Отключаем коллайдер и рендерер
        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;
        
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) renderer.enabled = false;
        
        Debug.Log("Кирилл убит!");
    }
    
    private void HandleDialogueOption(int optionIndex)
    {
        if (optionIndex == 1) // Опция "pay-gold"
        {
            QuestSystem.Instance.OnGoldPayment();
        }
    }
    
    private void OnDestroy()
    {
        if (dialogueTrigger != null)
        {
            dialogueTrigger.OnDialogueOptionSelected -= HandleDialogueOption;
        }
    }
}