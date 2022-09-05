using UnityEngine;
using TMPro;

namespace Controllers
{
    public class FinishController : MonoBehaviour
    {
        [SerializeField] private GameObject finishCube;
        [SerializeField] private int finishCubeAmount;

        float t = 0f;
        int counter = 1;

        private void Start()
        {
            for (int i = 0; i < finishCubeAmount; i++)
            {
                GameObject obj = Instantiate(finishCube, transform);

                if (i == 0)
                {
                    obj.transform.position = new Vector3(0f, i + 0.75f, (transform.GetChild(0).position.z + transform.localScale.z) + (i * 3) + 75f);
                    obj.GetComponent<FinishCubeController>().TextMeshFinishCube.text = "x" + counter;
                    obj.GetComponent<FinishCubeController>().MaterialFinishCube.material.color = Random.ColorHSV();
                    counter++;
                }
                else
                {
                    t += 1.5f;
                    obj.transform.position = new Vector3(0f, t + 0.75f, (transform.GetChild(0).position.z + transform.localScale.z) + (i * 3) + 75f);
                    obj.GetComponent<FinishCubeController>().TextMeshFinishCube.text = "x" + counter;
                    obj.GetComponent<FinishCubeController>().MaterialFinishCube.material.color = Random.ColorHSV();
                    counter++;
                }
               
            }
        }


    }

}

