using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public GameObject[] nodes;
    int currentNode = 0;
    public float speed = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,
                                                 nodes[currentNode].transform.position,
                                                 speed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, nodes[currentNode].transform.position);

        if (distance <= 0.5f)
        {
            currentNode++;
        }
        if (currentNode >= nodes.Length)
        {
            currentNode = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.CompareTag("Player"))
        {
            if (transform.position.y < collision.transform.position.y - 0.5f)
                collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
