using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] Shapes;

    GameObject exhibitionShape;
    Transform onBoardShape;

    BoardManager boardManager;
    Vector3 spawnPosition;
    Vector3 exhibitonPosition;
    int randomValue;
    int randomValue2;

    void Awake()
    {
        boardManager = GetComponent<BoardManager>();
        spawnPosition = new Vector3(5, 20, 0);
        exhibitonPosition = new Vector3(12.5f, 19f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShapeSpawn()
    {
        while (randomValue == randomValue2)
        {
            randomValue = Random.Range(0, 7);
            
        }
        randomValue2 = randomValue;
        exhibitionShape = Instantiate(Shapes[randomValue], exhibitonPosition, Quaternion.identity, boardManager.TileParent);
        exhibitionShape.GetComponent<TileManager>().enabled = false;
        ShapePositionForExhibionArea();
    }

    public void ShapeOnBoard()
    {
        exhibitionShape.transform.position = spawnPosition;
        exhibitionShape.GetComponent<TileManager>().enabled = true;
        onBoardShape = exhibitionShape.transform;
        ShapeSpawn();
    }

    void ShapePositionForExhibionArea()
    {
        if (exhibitionShape.name.Contains("Shape_I"))
        {
            exhibitionShape.transform.position = new Vector3(13, 18.5f, 0);
        }
        else if (exhibitionShape.name.Contains("Shape_O"))
        {
            exhibitionShape.transform.position = new Vector3(13, 19, 0);
        }
    }

    public GameObject GetExhibitionShape() 
    {
        return exhibitionShape;
    }
     public Transform GetOnBoardShape()
    {
        return onBoardShape;
    }
}
