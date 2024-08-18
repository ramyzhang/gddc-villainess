using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CGController : MonoBehaviour
{
    public Image CGImage;

    // Start is called before the first frame update
    void Start()
    {
        CGImage = gameObject.GetComponent<Image>();
        CGImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setupCG(Sprite CG) {
        CGImage.enabled = true;
        CGImage.sprite = CG;
        CGImage.preserveAspect = true;
    }

    public void removeCG() {
        CGImage.sprite = null;
        CGImage.enabled = false;
    }

    public IEnumerator fadeBlack() {
        CGImage.enabled = true;
        Sprite background = Resources.Load<Sprite>($"CGs/Square");
        CGImage.sprite = background;
        CGImage.color = new Vector4(0f, 0f, 0f, 0f);
        CGImage.preserveAspect = false;

        float elapsed = 0f;

        // Initial fadeout
        while (elapsed <= 1f) {
            Color currentColor = CGImage.color;
            currentColor.a = elapsed;
            CGImage.color = currentColor;

            elapsed += Time.deltaTime;
            yield return 0;
        }

        elapsed = 0f;

        // Hold the black for a little while
        while (elapsed < 1f) {
            elapsed += Time.deltaTime;
            yield return 0;
        }

        CGImage.color = new Vector4(1f, 1f, 1f, 1f);
        CGImage.sprite = null;
        CGImage.enabled = false;
    }
}
