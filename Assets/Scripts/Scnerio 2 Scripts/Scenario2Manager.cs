using UnityEngine;

public class Scenario2Manager : MonoBehaviour
{
    [SerializeField]CarParkingSpot cp1;
    [SerializeField]CarParkingSpot cp2;
    [SerializeField]CarParkingSpot cp3;

    [SerializeField] GameObject congText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cp1.matched && cp2.matched && cp3.matched){
            congText.gameObject.SetActive(true);
        }
    }
}
