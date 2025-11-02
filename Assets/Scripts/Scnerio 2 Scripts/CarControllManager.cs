using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CarControlManager : MonoBehaviour
{
    [Header("XR")]
    public XRInteractionManager xrManager;

    [Header("Drive Forces")]
    public float forwardForce = 6000f;
    public float reverseForce = 4000f;
    public float turnTorque  = 1500f;

    [Header("Limits")]
    public float maxSpeed = 20f;
    public float maxReverseSpeed = 8f;

    bool fwdHeld, backHeld, leftHeld, rightHeld;

    [SerializeField]Rigidbody currentCarRb;

    [SerializeField]
    GameObject currentSelectedGameObject;

    static readonly List<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable> _tmpInteractables = new();

    void Awake()
    {
        if (!xrManager) xrManager = FindObjectOfType<XRInteractionManager>(true);
    }

    void OnEnable()
    {
        if (!xrManager) xrManager = FindObjectOfType<XRInteractionManager>(true);

        if (xrManager != null)
        {
            xrManager.interactableRegistered += OnInteractableRegistered;
            xrManager.interactableUnregistered += OnInteractableUnregistered;

            _tmpInteractables.Clear();
            xrManager.GetRegisteredInteractables(_tmpInteractables);
            foreach (var it in _tmpInteractables)
                TryHookInteractable(it);
        }
        else
        {
            foreach (var baseIt in FindObjectsOfType<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>(true))
                HookBaseInteractable(baseIt);
        }
    }

    void OnDisable()
    {
        if (xrManager != null)
        {
            xrManager.interactableRegistered -= OnInteractableRegistered;
            xrManager.interactableUnregistered -= OnInteractableUnregistered;

            _tmpInteractables.Clear();
            xrManager.GetRegisteredInteractables(_tmpInteractables);
            foreach (var it in _tmpInteractables)
                UnhookInteractable(it);
        }
        else
        {
            foreach (var baseIt in FindObjectsOfType<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>(true))
                UnhookBaseInteractable(baseIt);
        }
    }

    void OnInteractableRegistered(InteractableRegisteredEventArgs args)
    {
        TryHookInteractable(args.interactableObject);
    }

    void OnInteractableUnregistered(InteractableUnregisteredEventArgs args)
    {
        UnhookInteractable(args.interactableObject);
    }

    void TryHookInteractable(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable it)
    {
        if (it is UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable baseIt)
            HookBaseInteractable(baseIt);
    }

    void UnhookInteractable(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable it)
    {
        if (it is UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable baseIt)
            UnhookBaseInteractable(baseIt);
    }

    void HookBaseInteractable(UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable baseIt)
    {
        baseIt.selectEntered.AddListener(OnSelectEntered);
        baseIt.selectExited.AddListener(OnSelectExited);
    }

    void UnhookBaseInteractable(UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable baseIt)
    {
        baseIt.selectEntered.RemoveListener(OnSelectEntered);
        baseIt.selectExited.RemoveListener(OnSelectExited);
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        var tr = args.interactableObject?.transform;
        if (!tr) return;

        var rb = tr.GetComponentInParent<Rigidbody>();
        if (rb != null)
        {
            currentCarRb = rb;
        }
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        var tr = args.interactableObject?.transform;
        if (!tr) return;

        var rb = tr.GetComponentInParent<Rigidbody>();
        if (rb != null && rb == currentCarRb)
        {
        }
    }

    void FixedUpdate()
    {
        if (!currentCarRb) return;

        currentSelectedGameObject = currentCarRb.gameObject;

        float forwardSpeed = Vector3.Dot(currentCarRb.linearVelocity, currentCarRb.transform.forward);

        if (fwdHeld && forwardSpeed < maxSpeed)
            currentCarRb.AddForce(currentCarRb.transform.forward * forwardForce, ForceMode.Force);

        if (backHeld && forwardSpeed > -maxReverseSpeed)
            currentCarRb.AddForce(-currentCarRb.transform.forward * reverseForce, ForceMode.Force);

        if (leftHeld)
            currentCarRb.AddTorque(Vector3.up * -turnTorque, ForceMode.Force);

        if (rightHeld)
            currentCarRb.AddTorque(Vector3.up *  turnTorque, ForceMode.Force);
    }

    public void HoldForward(bool hold)  { fwdHeld  = hold; Debug.Log("HOLD FORWARD");}
    public void HoldBackward(bool hold) { backHeld = hold; Debug.Log("HOLD BACKWARD");  currentSelectedGameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward);}
    public void HoldLeft(bool hold)     { leftHeld = hold; Debug.Log("HOLD LEFT");}
    public void HoldRight(bool hold)    { rightHeld = hold; Debug.Log("HOLD RIGHT");}
}
