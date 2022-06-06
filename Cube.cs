using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cube : MonoBehaviour
{
    [Header("")]
    public bool captureStatus = false;
    public int cubeNumber = 2;


    [Header("Cube Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private List<TextMeshProUGUI> textCube;
    [SerializeField] private List<Material> materialsList;

    public event UnityAction<int, Cube, Cube> CollisionOfIdenticalCubes; // Event of collision of two identical cubes

    #region Initial Cube Setup

    public void InitialSetup(int number)
    {
        cubeNumber = number;

        SetupText();
        SetupColor();
    }

    private void SetupText()
    {
        for (int i = 0; i < textCube.Count; i++)
        {
            textCube[i].text = cubeNumber.ToString();
        }
    }

    private void SetupColor()
    {
        int colorId = (cubeNumber != 8) ? (int)Mathf.Sqrt(cubeNumber) - 1 : 2;
        while (colorId >= materialsList.Count) colorId -= materialsList.Count;

        GetComponent<MeshRenderer>().material = materialsList[colorId];
    }

    #endregion

    #region The Logic Of The Interaction Of The Cube With The Environment

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
            StopAllCoroutines();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Cube"))
        {
            StopAllCoroutines();
            transform.GetChild(0).gameObject.SetActive(false);

            Cube cubeCollision = collision.gameObject.GetComponent<Cube>();

            if (cubeCollision.cubeNumber == cubeNumber && !captureStatus && !cubeCollision.captureStatus)
            {
                cubeCollision.captureStatus = true;
                captureStatus = true;
                ConnectingCubes(cubeCollision);
            }

        }
    }

    private void ConnectingCubes(Cube collisionCube)
    {
        CollisionOfIdenticalCubes?.Invoke(cubeNumber, this, collisionCube);
    }

    #endregion

    #region Cube Shot Logic

    public void PushCube(Vector3 forceVector)
    {
        StartCoroutine(StartsPushing(forceVector));
    }

    private IEnumerator StartsPushing(Vector3 forceVector)
    {
        float time = 0f;
        float totalTime = (forceVector.y != 0) ? .1f : 3;

        while(time < totalTime)
        {
            if(forceVector.y > 0)
            {
                rb.AddExplosionForce(forceVector.y * 20f, transform.position + Vector3.down * .5f + Vector3.right * Random.Range (-.25f, .25f) + Vector3.forward * Random.Range(-.25f, .25f), 1f);
            }
            else
            {
                rb.velocity = forceVector;
            }

            time += Time.deltaTime;
            yield return null;
        }

        transform.GetChild(0).gameObject.SetActive(false);
    }

    #endregion
}
