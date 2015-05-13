using UnityEngine;
using System.Collections;

public class MaterialFlashReset : MonoBehaviour {

    private Color flashColor;
    private Color resetColor;

    private Renderer renderer;

    private int frameCount = 0;

	void Start () {

        renderer = gameObject.GetComponent<Renderer>();
        resetColor = renderer.material.color;
	}

    public void Flash()
    {
        renderer.material.color = Color.white;
        frameCount = 6;
    }

    // Update is called once per frame
    void Update()
    {
        if (frameCount > 0)
        { 

            frameCount--;

            if (frameCount == 0)
                renderer.material.color = resetColor;
        }
    }
}
