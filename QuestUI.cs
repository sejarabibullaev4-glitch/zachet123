using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUI : MonoBehaviour
{
    [Header("UI элементы")]
    [SerializeField] private GameObject questPanel;
    [SerializeField] private TMP_Text questTitleText;
    [SerializeField] private TMP_Text questDescriptionText;
    [SerializeField] private TMP_Text questStatusText;
    
    private void Start()
    {
        QuestSystem.Instance.activeQuest.OnQuestCompleted += OnQuestCompleted;
        UpdateQuestUI();
    }
    
    private void UpdateQuestUI()
    {
        Quest quest = QuestSystem.Instance.activeQuest;
        
        questTitleText.text = quest.questName;
        questDescriptionText.text = quest.description;
        
        string status = quest.state switch
        {
            QuestState.NotStarted => "Не начат",
            QuestState.InProgress => "В процессе",
            QuestState.Completed => $"Завершён ({(quest.completionType == QuestCompletionType.Kill ? "Убийство" : "Выплата")})",
            QuestState.Failed => "Провален",
            _ => "Неизвестно"
        };
        
        questStatusText.text = $"Статус: {status}";
    }
    
    private void OnQuestCompleted(QuestCompletionType completionType)
    {
        UpdateQuestUI();
        
        // Показать сообщение о завершении
        string message = completionType == QuestCompletionType.Kill
            ? "Вы убили Кирилла и завершили квест!"
            : "Вы заплатили золотом и завершили квест!";
        
        Debug.Log(message);
    }
    
    private void OnDestroy()
    {
        if (QuestSystem.Instance != null && QuestSystem.Instance.activeQuest != null)
        {
            QuestSystem.Instance.activeQuest.OnQuestCompleted -= OnQuestCompleted;
        }
    }
}