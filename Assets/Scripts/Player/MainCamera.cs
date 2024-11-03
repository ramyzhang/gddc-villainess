using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject playerCharacter;
    // private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        // offset = (transform.position - playerCharacter.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y + 1f, transform.position.z);
    }

    public IEnumerator screenShake() {
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < 1f) {
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);

            transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z - 3f);
            elapsed += Time.deltaTime;
            yield return 0;
        }

        transform.position = originalPosition;
    }
}
