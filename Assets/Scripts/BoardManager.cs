using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject tile;
    public Transform TileParent;
    /*[HideInInspector]*/ public int height = 24;
    /*[HideInInspector]*/ public int width = 11;

    GameManager gameManager;
    SpawnManager spawnManager;
    Transform[,] tileArray;
    int rowCounter;
    int rowDeleteCounter;

    void Start()
    {
        tileArray = new Transform[width, height];
        for (int y = 0; y < height - 2; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Instantiate(tile, new Vector3(x, y, 0), Quaternion.identity,gameObject.transform);
            }
        }
        gameManager = FindObjectOfType<GameManager>();
        spawnManager = GetComponent<SpawnManager>();
        spawnManager.ShapeSpawn();
        spawnManager.ShapeOnBoard();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OccupyTile(Transform shape, bool test)
    {
        //foreach (Transform t in shape) 
        //{
        //    print("occu : " + t.name + " child : " + shape.childCount); 
        //    tileArray[Mathf.RoundToInt(t.position.x), Mathf.RoundToInt(t.position.y)] = t;
        //    t.parent = TileParent;
        //    //print(tileArray[Mathf.RoundToInt(t.position.x), Mathf.RoundToInt(t.position.y)].name);
        //    //print(t.position.x + "  " + t.position.y);
        //}
        for (int x =0; x < shape.childCount;)
        {
            tileArray[Mathf.RoundToInt(shape.GetChild(0).position.x), Mathf.RoundToInt(shape.GetChild(0).position.y)] = shape.GetChild(0);
            shape.GetChild(0).parent = TileParent;
        }
        Destroy(shape.gameObject);
        //RowChecker();
        StartCoroutine(RowCheckerEnum());

        if (test)
        {
            spawnManager.ShapeOnBoard();
            if (!IsSpawnAreaEmpty(spawnManager.GetOnBoardShape()))
            {
                gameManager.GameOver();
                spawnManager.GetOnBoardShape().GetComponent<TileManager>().enabled = false;
            }
        }
    }

    public Transform TileState(Transform shape) //TileManagerdan bu fonksiyoan shape'in tamamý deðil, tile geliyor.
    {
        if(Mathf.RoundToInt(shape.position.y) > 0)
        {
            return tileArray[Mathf.RoundToInt(shape.position.x), Mathf.RoundToInt(shape.position.y) - 1];
        }
        else
        {
            return null;
        }
    }

    public Transform TileHorizontalState(Transform shape, string choose = "forMove")
    {
        if(choose == "forMove")
        {
            return tileArray[Mathf.RoundToInt(shape.position.x) + (int)Input.GetAxisRaw("Horizontal"), Mathf.RoundToInt(shape.position.y)];
        }
        else
        {
            //print("asdfasdf");
            // mevcut tile saðýnda bir nesne var ise ve bu nesne board sýnýrlarý içindeyse iþlem yapýlacak. Else if'de de solunda var mý?
            if (Mathf.RoundToInt(shape.position.x) + 1 < width && Mathf.RoundToInt(shape.position.x) - 1 > 0 && Mathf.RoundToInt(shape.position.y) - 1 >= 0)
            {
                if (tileArray[Mathf.RoundToInt(shape.position.x) + 1, Mathf.RoundToInt(shape.position.y)])
                {
                    print("shape name1 : " + shape.name);
                    return tileArray[Mathf.RoundToInt(shape.position.x) + 1, Mathf.RoundToInt(shape.position.y)];
                }

                else if (tileArray[Mathf.RoundToInt(shape.position.x) - 1, Mathf.RoundToInt(shape.position.y)])
                {
                    print("shape name2 : " + shape.name);
                    return tileArray[Mathf.RoundToInt(shape.position.x) - 1, Mathf.RoundToInt(shape.position.y)];
                }
                else if(tileArray[Mathf.RoundToInt(shape.position.x), Mathf.RoundToInt(shape.position.y) - 1])
                {
                    return tileArray[Mathf.RoundToInt(shape.position.x), Mathf.RoundToInt(shape.position.y) - 1];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }

    public Transform TileHorizontalState2(Transform shape, string choose = "forMove")
    {
        if (choose == "forMove")
        {
            return tileArray[Mathf.RoundToInt(shape.position.x) + (int)Input.GetAxisRaw("Horizontal"), Mathf.RoundToInt(shape.position.y)];
        }
        else
        {
            //print("asdfasdf");
            // mevcut tile saðýnda bir nesne var ise ve bu nesne board sýnýrlarý içindeyse iþlem yapýlacak. Else if'de de solunda var mý?
            if (Mathf.RoundToInt(shape.position.x) + 1 < width && Mathf.RoundToInt(shape.position.x) - 1 > 0 && Mathf.RoundToInt(shape.position.y) - 1 >= 0)
            {
                if (tileArray[Mathf.RoundToInt(shape.parent.position.x) + 1, Mathf.RoundToInt(shape.position.y)])
                {
                    print("shape name1 : " + shape.name);
                    return tileArray[Mathf.RoundToInt(shape.parent.position.x) + 1, Mathf.RoundToInt(shape.position.y)];
                }

                else if (tileArray[Mathf.RoundToInt(shape.parent.position.x) - 1, Mathf.RoundToInt(shape.position.y)])
                {
                    print("shape name2 : " + shape.name);
                    return tileArray[Mathf.RoundToInt(shape.parent.position.x) - 1, Mathf.RoundToInt(shape.position.y)];
                }
                else if (tileArray[Mathf.RoundToInt(shape.position.x), Mathf.RoundToInt(shape.position.y) - 1])
                {
                    return tileArray[Mathf.RoundToInt(shape.position.x), Mathf.RoundToInt(shape.position.y) - 1];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
    public Transform TileState2(Transform t)
    {
        if (tileArray[Mathf.RoundToInt(t.position.x), Mathf.RoundToInt(t.position.y)])
        {
            return tileArray[Mathf.RoundToInt(t.position.x), Mathf.RoundToInt(t.position.y)];
        }
        return null;
    }
    public bool TileState3(Vector3 t)
    {
        if (t.x >= 0 && t.x < width && t.y > 0)
        {
            if (tileArray[Mathf.RoundToInt(t.x), Mathf.RoundToInt(t.y)])
            {
                return false;
            }
            return true;
        }
        return false;
    }

    void RowChecker()
    {
        for(int y = 0;y < height; ++y)
        {
            //print("y üst: " + y);
            for (int x = 0; x < width; ++x)
            {
                if (tileArray[x, y])
                {
                    rowCounter++;
                }
                else
                {
                    break;
                }


                if (rowCounter == width)
                {
                    RowDeleter(y);
                    RowMoveDownAfterDelete(y);
                    //StartCoroutine(RowProcess(y));
                    //print("row deleted" + " y : " + y);
                    y--;  // Satýrlar aþaðý kayýyor, bu yüzden y deðerini azaltarak denegeliyoruz.
                    //print(" y2 : " + y);
                    rowCounter = 0;
                    rowDeleteCounter++;
                }
            }
            rowCounter = 0;
        }
        //if(rowDeleteCounter > 0)
        //{
        //    RowMoveDownAfterDelete();
        //}
        rowDeleteCounter = 0;
    }

    void RowDeleter(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(tileArray[x, y].gameObject);
            tileArray[x, y] = null;
            //StartCoroutine(DeleteProcess(x, y));
            //RowMoveDownAfterDelete(y);
        }
    }

    void RowMoveDownAfterDelete(int y)
    {
        /*Bir satýr tamamen silindikten sonra bir üstteki satýrý kontrol ediyoruz.
         *Eðer o satýrda bir nesne var ise o nesneyi bir alt satýra kaydýrmak için önce nesneyi bir alt
         *satýrdaki tileArray dizisine kaydediyoruz. Sonra da nesnenin kendisinin pozisyonunu kaydýrýyoruz.
        */

        for (; y < height -1; ++y)
        {
            //print("y: " + y);
            for (int x = 0; x < width; ++x)
            {
                if (tileArray[x, y + 1])
                {
                    tileArray[x, y] = tileArray[x, y + 1];
                    tileArray[x, y + 1] = null;
                    tileArray[x, y].transform.position += Vector3.down;
                }
            }
        }
    }

    bool IsSpawnAreaEmpty(Transform shape)
    {
        foreach (Transform t in shape)
        {
            if (!TileState3(t.position))
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator RowProcess(int y)
    {
        RowDeleter(y);
        yield return new WaitForSeconds(0.25f);
        RowMoveDownAfterDelete(y);
    }
    IEnumerator DeleteProcess(int x, int y)
    {

        yield return new WaitForSeconds(1f);
        Destroy(tileArray[x, y].gameObject);
        tileArray[x, y] = null;
    }
    IEnumerator RowCheckerEnum()
    {
        for (int y = 0; y < height; ++y)
        {
            //print("y üst: " + y);
            for (int x = 0; x < width; ++x)
            {
                if (tileArray[x, y])
                {
                    rowCounter++;
                }
                else
                {
                    break;
                }


                if (rowCounter == width)
                {
                    RowDeleter(y);
                    RowMoveDownAfterDelete(y);
                    yield return new WaitForSeconds(0.2f);

                    y--;  // Satýrlar aþaðý kayýyor, bu yüzden y deðerini azaltarak denegeliyoruz.
                    rowCounter = 0;
                    rowDeleteCounter++;
                }
            }
            rowCounter = 0;
        }
        if(rowDeleteCounter > 0)
        {
            gameManager.AddPoint(rowDeleteCounter);
            rowDeleteCounter = 0;
        }
    }

    Transform[,] ArrayReturner()
    {
        return tileArray;
    }
 }
