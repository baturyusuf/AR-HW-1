using UnityEngine;
using TMPro;

public enum CityStep { NeedRepair = 0, NeedFuel = 1, NeedWash = 2, Done = 3 }

public class ScenarioController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI statusText;

    [Header("Current State (ReadOnly in Play)")]
    [SerializeField] private CityStep state = CityStep.NeedRepair;

    [Header("Audio")]
    [SerializeField] private AudioClip dingSfx;
    [SerializeField] private AudioClip wrongSfx;
    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        statusText = GameObject.Find("ScreenText").GetComponent<TextMeshProUGUI>();
        if (statusText != null)
            statusText.text = "Step 1: Repair your car first!";

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    public void UpdateStatus(TriggerType triggerType)
    {
        switch (state)
        {
            case CityStep.NeedRepair:
                if (triggerType == TriggerType.Repair)
                {
                    SetText("The car is repaired. Please go to the gas station!");
                    state = CityStep.NeedFuel;
                    PlayDing();
                }
                else
                {
                    SetText("Please repair your car first!");
                    PlayWrong();
                }
                break;

            case CityStep.NeedFuel:
                if (triggerType == TriggerType.GasStation)
                {
                    SetText("Fuel tank is full. Now you can wash your car!");
                    state = CityStep.NeedWash;
                    PlayDing();
                }
                else
                {
                    SetText("The fuel tank is empty! Go to the gas station.");
                    PlayWrong();
                }
                break;

            case CityStep.NeedWash:
                if (triggerType == TriggerType.CarWash)
                {
                    SetText("Car is washed. Congratulations! (Scenario complete)");
                    state = CityStep.Done;
                    PlayDing();
                }
                else
                {
                    SetText("The car is not clean yet. Please wash your car.");
                    PlayWrong();
                }
                break;

            case CityStep.Done:
                SetText("Scenario already completed.");
                break;
        }
    }

    private void SetText(string msg)
    {
        if (statusText != null) statusText.text = msg;
        else Debug.LogWarning($"[ScenarioController] StatusText is not assigned. Msg: {msg}");
    }

    private void PlayDing()
    {
        if (dingSfx == null)
        {
            Debug.LogWarning("[ScenarioController] dingSfx (AudioClip) atanmamış.");
            return;
        }

        if (audioSource != null)
            audioSource.PlayOneShot(dingSfx);
        else
            AudioSource.PlayClipAtPoint(dingSfx, transform.position);
    }

    private void PlayWrong()
    {
        if (wrongSfx == null)
        {
            Debug.LogWarning("[ScenarioController] wrongSfx (AudioClip) atanmamış.");
            return;
        }

        if (audioSource != null)
            audioSource.PlayOneShot(wrongSfx);
        else
            AudioSource.PlayClipAtPoint(wrongSfx, transform.position);
    }
}
