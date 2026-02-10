using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    [System.Serializable]
    public class DialogueOption
    {
        public string optionText;
        public UnityEvent onOptionSelected;
    }
    
    [Header("Настройки диалога")]
    public string dialogueTitle;
    public string dialogueText;
    public DialogueOption[] options;
    
    [Header("События")]
    public UnityEvent<int> OnDialogueOptionSelected;
    
    public void TriggerDialogue()
    {
        // Здесь реализация открытия UI диалога
        Debug.Log($"Диалог: {dialogueTitle}");
        Debug.Log($"Текст: {dialogueText}");
        
        for (int i = 0; i < options.Length; i++)
        {
            Debug.Log($"[{i}] {options[i].optionText}");
        }
        
        // Симулируем выбор второй опции (pay-gold)
        SelectOption(1);
    }
    
    public void SelectOption(int optionIndex)
    {
        if (optionIndex >= 0 && optionIndex < options.Length)
        {
            options[optionIndex].onOptionSelected?.Invoke();
            OnDialogueOptionSelected?.Invoke(optionIndex);
            Debug.Log($"Выбрана опция: {options[optionIndex].optionText}");
        }
    }
}