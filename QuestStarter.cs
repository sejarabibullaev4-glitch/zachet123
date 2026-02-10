using UnityEngine;

public class QuestStarter : MonoBehaviour
{
    [Header("Настройки запуска квеста")]
    [SerializeField] private string questID;
    [SerializeField] private bool autoStartOnTrigger = true;
    
    private void Start()
    {
        if (!autoStartOnTrigger)
        {
            StartQuest();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (autoStartOnTrigger && other.CompareTag("Player"))
        {
            StartQuest();
        }
    }
    
    public void StartQuest()
    {
        QuestSystem.Instance.StartQuest();
        Debug.Log("Квест активирован!");
    }
}