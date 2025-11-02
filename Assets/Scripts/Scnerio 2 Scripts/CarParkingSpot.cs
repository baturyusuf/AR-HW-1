using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(AudioSource))]
public class CarParkingSpot : MonoBehaviour
{
    private string myName;
    private Renderer myRenderer;
    private Color originalColor;

    [Header("SFX Settings")]
    [Tooltip("Eşleşme doğru olduğunda (yeşil) çalınacak ses (mp3 desteklenir).")]
    [SerializeField] private AudioClip correctSfx;
    [Tooltip("Eşleşme yanlış olduğunda (kırmızı) çalınacak ses (mp3 desteklenir).")]
    [SerializeField] private AudioClip wrongSfx;

    [Range(0f, 1f)]
    [SerializeField] private float correctVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float wrongVolume = 1f;

    private AudioSource audioSource;

    public bool matched = false;

    void Awake()
    {
        myName = this.gameObject.name;

        myRenderer = GetComponent<Renderer>();
        if (myRenderer.material != null)
        {
            originalColor = myRenderer.material.color;
        }

        Collider myCollider = GetComponent<Collider>();
        if (!myCollider.isTrigger)
        {
            Debug.LogWarning($"'{myName}' üzerindeki Collider 'Is Trigger' olarak ayarlanmadı. Kod tarafından ayarlanıyor.");
            myCollider.isTrigger = true;
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        string otherName = other.gameObject.name;
        Debug.Log($"Ben: '{myName}', Bana çarpan: '{otherName}'");

        if (myName == otherName)
        {
            myRenderer.material.color = Color.green;
            if (correctSfx != null){
                audioSource.PlayOneShot(correctSfx, correctVolume);
                matched = true;
            }
            else
                Debug.LogWarning("Correct SFX atanmadı.");
            Debug.Log("İsimler EŞLEŞTİ! Renk yeşil oldu. (Correct SFX)");
        }
        else
        {
            myRenderer.material.color = Color.red;
            if (wrongSfx != null)
                audioSource.PlayOneShot(wrongSfx, wrongVolume);
            else
                Debug.LogWarning("Wrong SFX atanmadı.");
            Debug.Log("İsimler FARKLI! Renk kırmızı oldu. (Wrong SFX)");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (myRenderer.material != null)
        {
            myRenderer.material.color = originalColor;
            Debug.Log($"'{other.name}' alandan çıktı. Renk normale döndü.");
        }
    }
}
