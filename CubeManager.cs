using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CubeManager : MonoBehaviour
{
    [Header("Main Component")]
    [SerializeField] private Score score;

    [Header("Prefab Settings")]
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject collisionParticle;
    [SerializeField] private GameObject newObjectParticle;
    [SerializeField] private Transform spawnPosition;


    public event UnityAction TimeToShowAds;
    private int boxCollisionCountToAd = 0;
    public int maxValue = 1;

    #region Logic For Creating A New Cube

    private void Awake()
    {
        boxCollisionCountToAd = Random.Range(10, 20);
    }

    private void StartSpawning(CubeController pushCube)
    {
        pushCube.TheStartOfTheCubeIsMade -= StartSpawning;
        StartCoroutine(SpawnNewCubePause());
    }

    private IEnumerator SpawnNewCubePause()
    {
        yield return new WaitForSeconds(.5f);
        SpawnNewCube();
    }

    public void SpawnNewCube()
    {
        Cube newCube = Instantiate(cubePrefab, spawnPosition.position, Quaternion.identity, transform).GetComponent<Cube>();
        newCube.InitialSetup(ChoosingANumberForANewCube());

        newCube.CollisionOfIdenticalCubes += CollisionOfAPairOfCubes;
        newCube.gameObject.GetComponent<CubeController>().TheStartOfTheCubeIsMade += StartSpawning;
    }

    private int ChoosingANumberForANewCube()
    {
        int val = Random.Range(1, maxValue);

        return (int) Mathf.Pow(2, val);
    } //Сhoose a number for a new cube

    #endregion

    #region Logic When Interacting Between Two Cubes

    private void CollisionOfAPairOfCubes(int number, Cube cubeOne, Cube cubeTwo)
    {
        cubeOne.CollisionOfIdenticalCubes -= CollisionOfAPairOfCubes;
        cubeTwo.CollisionOfIdenticalCubes -= CollisionOfAPairOfCubes;

        Vector3 newCubePosition = cubeOne.transform.position;

        Destroy(Instantiate(collisionParticle, cubeTwo.transform.position, Quaternion.identity), 2f);

        Destroy(cubeOne.gameObject);
        Destroy(cubeTwo.gameObject);

        Cube newCube = Instantiate(cubePrefab, newCubePosition, Quaternion.identity, transform).GetComponent<Cube>();
        newCube.CollisionOfIdenticalCubes += CollisionOfAPairOfCubes;
        newCube.InitialSetup(number * 2);
        newCube.PushCube(Vector3.forward * 1f + Vector3.up * 30f);

        Instantiate(newObjectParticle, newCube.transform.position, Quaternion.identity, newCube.transform);

        Destroy(newCube.gameObject.GetComponent<CubeController>());

        score.SetScore(number * 2);

        maxValue = (IsPowerOfTwo(number) > maxValue) ? IsPowerOfTwo(number) : maxValue;

        AdServingCheck();
    } //Insertion of two identical cubes

    private int IsPowerOfTwo(int number)
    {
        int count = 0;
        for (int x = 1; x <= number; x *= 2)
        {
            count++;
            if (x == number) return count;
        }
        return count;
    }

    #endregion

    #region ADS Logic

    private void AdServingCheck()
    {
        if(boxCollisionCountToAd <= 0)
        {
            ShowADS();
            boxCollisionCountToAd = Random.Range(10, 20);
        }
        else
        {
            boxCollisionCountToAd--;
        }
    }

    private void ShowADS()
    {
        TimeToShowAds?.Invoke();
    }

    #endregion
}
