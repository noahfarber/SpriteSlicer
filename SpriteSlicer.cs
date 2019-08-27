using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSlicer : MonoBehaviour
{
    [Header("Image to Slice")]
    public Texture2D source;
    private Sprite buttonSprite;

    [Header("Enter Number of Rows and Columns")]
    public int rowsAndColumns;

    [Header("Scale of Image")]
    public float boxSizeX;
    public float boxSizeY;

    [Header("Spacing Between Boxes")]
    public float spacing;

    [Header("Border Size")]
    public float border;

    // Private variables for in-script use
    private float xSpacing = 0, ySpacing = 0;
    private int rows = 50, columns = 50;
    private int boxNumber = 0;
    private float xCursor, yCursor, sliceX, sliceY;
    private bool loading = true;
    private Vector2[,] cursor;

    // Start is called before the first frame update
    void Start()
    {
        // Assigment of variables
        cursor = new Vector2[rows, columns];
        rows = rowsAndColumns;
        columns = rowsAndColumns;
        xCursor = (source.width * -.5f) + ((source.width / columns) * .5f) - (0.5f * spacing * (columns - 1));
        yCursor = (source.height * -.5f) + ((source.height / rows) * .5f) - (0.5f * spacing * (rows - 1));
        transform.localScale = new Vector3(boxSizeX, boxSizeY, transform.localScale.z);
        xSpacing = spacing;
        ySpacing = spacing;
        border = 1 + (border / 100f);

    }

    // Update is called once per frame
    void Update()
    {
        // Check whether the process has completed
        while(loading)
        {
            // Loop through rows
            for (int i = 0; i < rows; i++)
            {
                // Loop through columns
                for (int j = 0; j < columns; j++)
                {
                    // Set cursor to array position
                    cursor[i, j] = new Vector2(xCursor, yCursor);

                    // Create a slice of the sprite
                    Sprite newSprite = Sprite.Create(source, new Rect(i*(source.width / columns), j*(source.height / rows), source.width / columns, (source.height / rows)), new Vector2(0.5f, 0.5f));
                    

                    // Define x and y size of a slice
                    sliceX = newSprite.textureRect.size.x;
                    sliceY = newSprite.textureRect.size.y;



                    GameObject slice = new GameObject();
                    slice.transform.position = cursor[i, j];
                    slice.transform.SetParent(gameObject.transform, false);
                    slice.name = boxNumber.ToString();



                    // Create the GameObject that houses the border
                    GameObject borderPrefab = new GameObject();
                    Image borderImage = borderPrefab.AddComponent<Image>();

                    // Set the border properties
                    borderPrefab.transform.localScale = new Vector3(border, border, 0.0f);
                    borderPrefab.name = "Border" + boxNumber.ToString();
                    borderPrefab.transform.position = Vector3.zero;
                    borderPrefab.transform.SetParent(slice.transform, false);

                    borderPrefab.GetComponent<Image>().color = Color.black;
                    if (buttonSprite == null)
                    {
                        borderImage.sprite = newSprite;

                    }
                    else
                    {
                        borderImage.sprite = buttonSprite;
                    }
                    borderImage.rectTransform.sizeDelta = new Vector2(sliceX, sliceY);

                    // Create the GameObject that houses the slice, and add an Image component
                    GameObject spriteImagePrefab = new GameObject();
                    Image image = spriteImagePrefab.AddComponent<Image>();
                    Button button = spriteImagePrefab.AddComponent<Button>();
                    Square square = spriteImagePrefab.AddComponent<Square>();

                    // Set properties of the slice object
                    spriteImagePrefab.name = "Slice" + boxNumber.ToString();
                    spriteImagePrefab.transform.position = Vector3.zero;
                    spriteImagePrefab.transform.SetParent(slice.transform, false);

                    // Set the image properties
                    if(buttonSprite == null)
                    {
                        image.sprite = newSprite;

                    }
                    else
                    {
                        image.sprite = buttonSprite;
                    }

                    image.rectTransform.sizeDelta = new Vector2(sliceX, sliceY);

                    



                    // Add to the box counter
                    boxNumber += 1;

                    // Move array cursor's Y position
                    yCursor += sliceY + ySpacing;
                }
                // Move array cursor's X position
                xCursor += sliceX + xSpacing;

                // Reset Y cursor for a new column
                yCursor = (source.height * -.5f) + ((source.height / rows) * .5f) - (0.5f * spacing * (rows - 1));

                // Check if all boxes are cut and loaded
                if (i == rows - 1)
                {
                    loading = false;
                }
            }
        }
        
        
    }
}
