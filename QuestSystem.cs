using UnityEngine;
using System;

public enum QuestState
{
    NotStarted,
    InProgress,
    Completed,
    Failed
}

public enum QuestCompletionType
{
    Kill,
    Payment
}

[System.Serializable]
public class Quest
{
    public string questID;
    public string questName;
    public string description;
    public QuestState state;
    public QuestCompletionType completionType;
    public bool isKillCompleted;
    public bool isPaymentCompleted;
    public int goldAmountRequired;
    
    public Action<QuestCompletionType> OnQuestCompleted;
}

public class QuestSystem : MonoBehaviour
{
    public static QuestSystem Instance { get; private set; }
    
    [Header("Текущий активный квест")]
    public Quest activeQuest;
    
    [Header("Настройки квеста")]
    [SerializeField] private string questID = "k1ll_k1n1ll_or-pay_gold";
    [SerializeField] private string characterToKillTag = "Character01ed";
    [SerializeField] private int goldPaymentAmount = 1000;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        InitializeQuest();
    }
    
    private void InitializeQuest()
    {
        activeQuest = new Quest
        {
            questID = questID,
            questName = "Устранить Кирилла",
            description = "Убейте Кирилла или заплатите ему золотом",
            state = QuestState.NotStarted,
            completionType = QuestCompletionType.Kill,
            goldAmountRequired = goldPaymentAmount,
            isKillCompleted = false,
            isPaymentCompleted = false
        };
    }
    
    public void StartQuest()
    {
        if (activeQuest.state == QuestState.NotStarted)
        {
            activeQuest.state = QuestState.InProgress;
            Debug.Log($"Квест начат: {activeQuest.questName}");
            LogToConsole("Квест 'Устранить Кирилла' начат!");
        }
    }
    
    public void CompleteQuest(QuestCompletionType completionType)
    {
        if (activeQuest.state != QuestState.InProgress)
            return;
        
        activeQuest.state = QuestState.Completed;
        activeQuest.completionType = completionType;
        
        // Вызываем событие завершения квеста
        activeQuest.OnQuestCompleted?.Invoke(completionType);
        
        // Логируем в консоль
        string completionMessage = completionType == QuestCompletionType.Kill 
            ? "Квест завершён через убийство" 
            : "Квест завершён через выплату";
        
        Debug.Log(completionMessage);
        LogToConsole($"Путь завершения: {completionMessage}");
    }
    
    public void OnCharacterKilled(string characterTag)
    {
        if (activeQuest.state == QuestState.InProgress && 
            characterTag == characterToKillTag && 
            !activeQuest.isKillCompleted)
        {
            activeQuest.isKillCompleted = true;
            CompleteQuest(QuestCompletionType.Kill);
        }
    }
    
    public void OnGoldPayment()
    {
        if (activeQuest.state == QuestState.InProgress && 
            !activeQuest.isPaymentCompleted)
        {
            // Проверяем, есть ли у игрока достаточно золота
            if (PlayerInventory.Instance != null && 
                PlayerInventory.Instance.HasEnoughGold(activeQuest.goldAmountRequired))
            {
                PlayerInventory.Instance.DeductGold(activeQuest.goldAmountRequired);
                activeQuest.isPaymentCompleted = true;
                CompleteQuest(QuestCompletionType.Payment);
            }
            else
            {
                Debug.Log("Недостаточно золота для выплаты!");
                LogToConsole("Недостаточно золота для выплаты!");
            }
        }
    }
    
    private void LogToConsole(string message)
    {
        Debug.Log($"<color=cyan>[КВЕСТ]</color> {message}");
    }
    
    // Для отладки в редакторе
    [ContextMenu("Начать квест")]
    public void DebugStartQuest()
    {
        StartQuest();
    }
    
    [ContextMenu("Завершить убийством")]
    public void DebugCompleteByKill()
    {
        OnCharacterKilled(characterToKillTag);
    }
    
    [ContextMenu("Завершить выплатой")]
    public void DebugCompleteByPayment()
    {
        OnGoldPayment();
    }
}