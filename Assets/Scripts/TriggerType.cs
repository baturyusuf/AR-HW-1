using UnityEngine;

public enum TriggerType { Repair, GasStation, CarWash }

[RequireComponent(typeof(Collider))]
public class TriggerZone : MonoBehaviour
{
    public TriggerType triggerType;
    public ScenarioController controller;

    private void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("car")) return;

        if (controller == null)
        {
            Debug.LogError("[TriggerZone] ScenarioController not assigned.");
            return;
        }

        controller.UpdateStatus(triggerType);
    }
}
