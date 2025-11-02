using UnityEngine;

public class EnsureSingleInstance : MonoBehaviour
{
    public static EnsureSingleInstance Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning($"Sahnede zaten bir '{this.name}' nesnesi var. Yeni oluşturulan kopya yok ediliyor.");
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            Debug.Log($"'{this.name}' nesnesi oluşturuldu ve 'Instance' olarak ayarlandı. (İlk nesne)");
        }
    }
}
