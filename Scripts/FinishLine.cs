using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [Header("UI Елементи")]
    // Сюди ми перетягнемо наш вимкнений текст з ієрархії
    public GameObject finishUI;

    private bool isFinished = false;

    void Start()
    {
        // Переконуємося, що на старті гри напис точно схований
        if (finishUI != null)
        {
            finishUI.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Перевіряємо, чи це гравець
        if (other.CompareTag("Player") && !isFinished)
        {
            isFinished = true;

            // Вмикаємо UI-повідомлення на екрані
            if (finishUI != null)
            {
                finishUI.SetActive(true);
            }

            // щоб гра (і машина) повністю зупинялася на фініші
            Time.timeScale = 0f; 
        }
    }
}