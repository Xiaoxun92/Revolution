using UnityEngine;

public class IgniteFlash : MonoBehaviour {

    public float lifespan;

    void Update () {
        if (lifespan <= 0)
            Destroy(gameObject);
        lifespan -= Time.deltaTime;
    }
}
