using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject[] backgrounds;
    public float Speed;
    private float[] initial_pos;
    private float[] newPos;
    public float[] speeds;
    // Start is called before the first frame update
    void Start()
    {
        newPos = new float[backgrounds.Length];
        initial_pos = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i += 1) {
            newPos[i] = backgrounds[i].transform.position.x;
            initial_pos[i] = backgrounds[i].transform.position.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i += 1) {
            if (backgrounds[i].transform.position.x > -initial_pos[i]) {
                    newPos[i] -= speeds[i] + ((1 + i) * Speed) * Time.deltaTime;
                    backgrounds[i].transform.position = new Vector3(newPos[i], 0f, 0f);
            }
            else {
                backgrounds[i].transform.position = new Vector3(initial_pos[i], 0f, 0f);
                newPos[i] = initial_pos[i];
            }
        }
    }
}
