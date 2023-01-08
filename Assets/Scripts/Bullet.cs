using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed, timeToDestroy;
    public GameObject impact;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(PlayerController.instance.transform.localScale.x, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x > 0)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.right * -moveSpeed * Time.deltaTime);
        }
        Destroy(this.gameObject, timeToDestroy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(impact, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(impact, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
