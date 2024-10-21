using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Terrain"), true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var rigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Terrain"), false);
    }
}
