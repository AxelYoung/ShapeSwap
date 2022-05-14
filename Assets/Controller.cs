using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public string currentShape;

    private GameObject circle;
    private SpriteRenderer csr;

    private GameObject triangle;
    private SpriteRenderer tsr;

    private GameObject square;
    private SpriteRenderer ssr;

    private GameObject diamond;
    private SpriteRenderer dsr;

    private GameObject curShape;
    private Vector2 cShapePos;
    private SpriteRenderer cursr;

    public Color32 disColor;
    public Color32 actColor;

    public float seconds;

    private bool canSwap = true;

    // Start is called before the first frame update
    void Start()
    {
        circle = transform.GetChild(0).gameObject;
        triangle = transform.GetChild(1).gameObject;
        square = transform.GetChild(2).gameObject;
        diamond = transform.GetChild(3).gameObject;
        csr = circle.GetComponent<SpriteRenderer>();
        tsr = triangle.GetComponent<SpriteRenderer>();
        ssr = square.GetComponent<SpriteRenderer>();
        dsr = diamond.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SwapShapeSquare();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            SwapShapeDiamond();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SwapShapeTriangle();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SwapShapeCircle();
        }
    }

    public void SwapShapeCircle()
    {
        StartCoroutine(SwapShape(circle, csr));
    }

    public void SwapShapeSquare()
    {
        StartCoroutine(SwapShape(square, ssr));
    }

    public void SwapShapeTriangle()
    {
        StartCoroutine(SwapShape(triangle, tsr));
    }

    public void SwapShapeDiamond()
    {
        StartCoroutine(SwapShape(diamond, dsr));
    }

    public IEnumerator SwapShape(GameObject swap, SpriteRenderer swapsr)
    {
        if (canSwap)
        {
            currentShape = null;
            canSwap = false;
            float elapsedTime = 0;
            Vector3 startingPos = swap.transform.position;
            while (elapsedTime < seconds)
            {
                if(swap != curShape)
                {
                    swap.transform.position = Vector3.Lerp(startingPos, Vector3.zero, (elapsedTime / seconds));
                    swapsr.color = Color32.Lerp(disColor, actColor, (elapsedTime / seconds));
                }
                if(curShape != null)
                {
                    curShape.transform.position = Vector3.Lerp(Vector3.zero, cShapePos, (elapsedTime / seconds));
                    cursr.color = Color32.Lerp(actColor, disColor, (elapsedTime / seconds));
                }
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            if(swap != curShape)
            {
                swap.transform.position = Vector3.zero;
                swapsr.color = actColor;
            }
            if (curShape != null)
            {
                curShape.transform.position = cShapePos;
                cursr.color = disColor;
            }
            if(swap != curShape)
            {
                cShapePos = startingPos;
                curShape = swap;
                cursr = swapsr;
                currentShape = swap.name;
            }
            else
            {
                cShapePos = Vector2.zero;
                curShape = null;
                cursr = null;
                currentShape = null;
            }
            canSwap = true;
        }

    }
}
