using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BackgroundAnimation : MonoBehaviour
{
    Renderer renderer;
    public GameObject player;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(new Vector3(player.transform.position.x, 2.95f, 1.36f), Quaternion.identity);
        renderer.material.SetTextureOffset("_MainTex", new Vector2(5 * speed * player.transform.position.x, 0));
    }
}
