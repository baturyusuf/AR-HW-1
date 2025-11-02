using System.Collections;
using UnityEngine;

public class CarSpawn : MonoBehaviour
{
    [Tooltip("İçinden rastgele seçilip spawn edilecek araba prefab'ları")]
    public GameObject[] cars;

    void Start()
    {
        if (cars == null || cars.Length == 0)
        {
            Debug.LogError("Spawn edilecek araba listesi (cars) boş!");
            return;
        }

        StartCoroutine(SpawnCarRoutine());
    }

    IEnumerator SpawnCarRoutine()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, cars.Length);
            GameObject carToSpawn = cars[randomIndex];

            float waitTime = Random.Range(3.0f, 7.0f);

            yield return new WaitForSeconds(waitTime);
        }
    }
}
