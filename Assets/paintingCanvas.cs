using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;


public class paintingCanvas : MonoBehaviour
{
    public AudioSource audioSource;
    public List<GameObject> clickedPositions = new List<GameObject>();


    private string[] spriteImages = { "Sprites/cambellsPixels 1", "Sprites/marilyn" };
    private string spriteImage;
    private int sptireImageIndex;
    private bool isEnabled = true;


    public void setIsEnabled(bool nextIsEnabled)
    {
        isEnabled = nextIsEnabled;
    }

    void setRandomSpriteImage()
    {
        sptireImageIndex = Random.Range(0, spriteImages.Length);
        spriteImage = spriteImages[sptireImageIndex];

        Debug.Log("Next sprite stamp: " + spriteImage);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting painting canvas");
        setRandomSpriteImage();
    }

    // Update is called once per frame
    void Update()
    {
        // change paint brush on space for debugging — maybe disable in prod
        if (Input.GetKeyDown("space"))
        {
            setRandomSpriteImage();
        }
    }

    void OnMouseDown()
    {
        if (!isEnabled) return;

        Debug.Log("Clciked canvas"); 
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = -1;
        Debug.Log(position);

        GameObject go = new GameObject();
        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load<Sprite>(spriteImage);

        float randRed = Random.Range(0.2f, 1.0f);
        float greenRed = Random.Range(0.2f, 1.0f);
        float blueRed = Random.Range(0.2f, 1.0f);
        Color color = new Color(randRed, greenRed, blueRed, 1.0f);
        renderer.color = color;

        renderer.sprite = sprite;

        go.transform.position = position;


        clickedPositions.Add(go);

        audioSource.Play();
    }

    public void hideAllPaintedSprites()
    {
        foreach (GameObject go in clickedPositions)
        {
            Destroy(go);
        }

        clickedPositions = new List<GameObject>();

        sptireImageIndex = (sptireImageIndex + 1) % spriteImages.Length;
        spriteImage = spriteImages[sptireImageIndex];
    }

    public int getCount()
    {
        return clickedPositions.Count;
    }
}
