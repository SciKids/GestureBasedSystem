using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] fallingObjectsPrefabs;



    [Range(0.5f, 2f)]
    [SerializeField]
    float minTimeInterval = 1;

    [Range(2f, 4f)]
    [SerializeField]
    float maxTimeInterval = 2;
    float halfWidth;
    /*float[] positionx = { -450, -150, 150, 450 };
    float[] positiony = { 0, 0, 0, 0 }; 
    */

    float[] positions = { -450, -150, 150, 450 };

    public void StartSpawn(float widthImage)
    {
        halfWidth = widthImage / 2;
        //setLocation(); 
        StartCoroutine(SpawnObject(10f,0));
        StartCoroutine(SpawnObject(10f, 1));
        StartCoroutine(SpawnObject(10f, 2));
        StartCoroutine(SpawnObject(10f, 3));

    }

    IEnumerator SpawnObject(float waitingTime,int pos)
    {
        /* yield return new WaitForSeconds(waitingTime);
         setLocation(); 
         float randX = positionx[pos];
         float randy = positiony[pos];
         Vector3 localSpawnPosition = new Vector3(randX, randy, 0);

         GameObject currentObject = Instantiate(fallingObjectsPrefabs[pos]);
         currentObject.transform.SetParent(gameObject.transform, true);
         currentObject.transform.localPosition = localSpawnPosition;
         StartCoroutine(SpawnObject(waitingTime,pos));*/
        yield return new WaitForSeconds(waitingTime);
        float randX = positions[pos];
        Vector3 localSpawnPosition = new Vector3(randX, 0, 0);

        GameObject currentObject = Instantiate(fallingObjectsPrefabs[pos]);
        currentObject.transform.SetParent(gameObject.transform, true);
        currentObject.transform.localPosition = localSpawnPosition;
        StartCoroutine(SpawnObject(waitingTime, pos));
    }

    /*public void setLocation()
    {
        positionx[0] = Random.Range(-500, 500);
        positionx[1] = Random.Range(-500, 500);
        positionx[2] = Random.Range(-500, 500);
        positionx[3] = Random.Range(-500, 500);
        positiony[0] = Random.Range(-500, -150);
        positiony[1] = Random.Range(150, 500);
        positiony[2] = Random.Range(-500, -150);
        positiony[3] = Random.Range(150, 500);
        Debug.Log(positionx);
        Debug.Log(positionx);
    }*/
}
