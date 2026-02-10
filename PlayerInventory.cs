using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }
    
    [Header("Ресурсы игрока")]
    [SerializeField] private int currentGold = 500;
    
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
    
    public bool HasEnoughGold(int amount)
    {
        return currentGold >= amount;
    }
    
    public void DeductGold(int amount)
    {
        if (HasEnoughGold(amount))
        {
            currentGold -= amount;
            Debug.Log($"Списано {amount} золота. Остаток: {currentGold}");
        }
    }
    
    public void AddGold(int amount)
    {
        currentGold += amount;
        Debug.Log($"Добавлено {amount} золота. Всего: {currentGold}");
    }
    
    public int GetCurrentGold()
    {
        return currentGold;
    }
}