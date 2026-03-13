using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HorseSlideshowScript : MonoBehaviour
{
    public RawImage displayImage;
    public float interval = 4.0f;
    public float maxSize = 1f;

    private List<Texture> horsePictures = new List<Texture>();
    private int currentIndex = 0;
    private RectTransform rectTransform;

    void Awake()
    {
        if (displayImage == null)
        {
            displayImage = GetComponent<RawImage>();
        }
    }

    void Start()
    {
        if (displayImage == null)
        {
            Debug.LogError("No RawImage assigned.");
            return;
        }

        rectTransform = displayImage.GetComponent<RectTransform>();

        Object[] loadedAssets = Resources.LoadAll("Horses", typeof(Texture));
        Debug.Log("Found " + loadedAssets.Length + " files in Resources/Horses.");

        foreach (var asset in loadedAssets)
        {
            horsePictures.Add((Texture)asset);
        }

        if (horsePictures.Count > 0)
        {
            StartCoroutine(PlaySlideshow());
        }
        else
        {
            Debug.LogError("STOPPED: No textures found.");
        }
    }

    IEnumerator PlaySlideshow()
    {
        while (true)
        {
            Texture currentTex = horsePictures[currentIndex];
            displayImage.texture = currentTex;
            float width = currentTex.width;
            float height = currentTex.height;
            float aspectRatio = width / height;
            if (width > height)
                rectTransform.sizeDelta = new Vector2(maxSize, maxSize / aspectRatio);
            else
                rectTransform.sizeDelta = new Vector2(maxSize * aspectRatio, maxSize);
            Debug.Log("Size is " + rectTransform.sizeDelta);
            rectTransform.anchoredPosition = Vector2.zero;

            currentIndex = (currentIndex + 1) % horsePictures.Count;
            yield return new WaitForSeconds(interval);
        }
    }
}