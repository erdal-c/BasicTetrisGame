using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Range(1,4)]
    public int RotateAmount = 4;
    bool isShapeMove = true;

    public bool testOccupy = true;

    int rotateMultiplier = 1;
    BoardManager boardManager;
    GameManager gameManager;
    int unRotatableTiles;
    float buttonPressCounter;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        boardManager = FindObjectOfType<BoardManager>();
        //StartCoroutine("MoveTime");
        InvokeRepeating("MoveDown", gameManager.ShapeSpeed, gameManager.ShapeSpeed); //InvokeRepeating de ilk önce kaç t zaman sonra başlayacağını, sonra da kaç t zamanda tekrar edeceğini belirliyoruz.
    }

    // Update is called once per frame
    void Update()
    {
        if (isShapeMove)
        {
            RotateShapes4();
            MoveShapes();
        }
    }

    void RotateShapes()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            //if (IsRotatable())
            //{
            //    transform.Rotate(new Vector3(0, 0, Input.GetAxisRaw("Horizontal") * 90), Space.World);
            //}
            if (IsRotatable4())
            {
                transform.Rotate(new Vector3(0, 0, Input.GetAxisRaw("Horizontal") * -90), Space.World);
                IsRotatable3();
            }
        }
    }
    void RotateShapes2()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //if (IsRotatable())
            //{
            //    transform.Rotate(new Vector3(0, 0, Input.GetAxisRaw("Horizontal") * 90), Space.World);
            //}
            if (IsRotatable4() && IsRotatable())
            {
                switch (RotateAmount)
                {
                    case 1:
                        break;
                    case 2:
                        transform.Rotate(new Vector3(0, 0, rotateMultiplier * 90), Space.World);
                        rotateMultiplier = - rotateMultiplier;
                        IsRotatable3();
                        break;
                    case 4:
                        transform.Rotate(new Vector3(0, 0, 90), Space.World);
                        break;
                }
            }
        }
    }
    void RotateShapes3()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            switch (RotateAmount)
            {
                case 1:
                    break;
                case 2:
                    transform.Rotate(new Vector3(0, 0, rotateMultiplier * 90), Space.World);
                    if (!IsRotate())
                    {
                        transform.Rotate(new Vector3(0, 0, rotateMultiplier * -90), Space.World);
                    }
                    else
                    {
                        rotateMultiplier = -rotateMultiplier;
                    }
                    break;
                case 4:
                    transform.Rotate(new Vector3(0, 0, 90), Space.World);
                    if (!IsRotate())
                    {
                        transform.Rotate(new Vector3(0, 0, -90), Space.World);
                    }
                    break;
            }
        }
    }
    void RotateShapes4()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && IsRotate2())
        {
            switch (RotateAmount)
            {
                case 1:
                    break;
                case 2:
                    //print("before swt : " + rotateMultiplier);
                    transform.Rotate(new Vector3(0, 0, rotateMultiplier * 90), Space.World);
                    rotateMultiplier = -rotateMultiplier;
                    //print("after swt : " + rotateMultiplier);

                    break;
                case 4:
                    transform.Rotate(new Vector3(0, 0, 90), Space.World);
                    break;
            }
        }

        foreach (Transform t in transform)
        {
            if(t.position.y >= boardManager.height -2)
            {
                t.gameObject.SetActive(false);
            }
            else { t.gameObject.SetActive(true); }
        }
    }

    void MoveShapes()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            buttonPressCounter = Time.time + 0.25f;
            if (IsMovable()) 
            {
                transform.position += Vector3.right * Input.GetAxisRaw("Horizontal");
            }   
        }
        /*GetKey tuşa basıldığı anda çok hızlı çalışıyor. bu yüzden GetKeyDown' da bir gecikme sayacı ayarlıyoruz
         * bu sayaç kısıtı bitince GetKey başlıyor. Ancak tekrar 0.1 ekleyip kısıtlama yapıyoruz yoksa yine
         * çok hızlı çalışıyor.
        */
        else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && Time.time > buttonPressCounter)
        {
            buttonPressCounter = Time.time + 0.1f;
            if (IsMovable())
            {
                transform.position += Vector3.right * Input.GetAxisRaw("Horizontal");
            }
        }

        if (Input.GetAxisRaw("Vertical") < 0 && Time.time > buttonPressCounter)
        {
            if (isShapeMove)
            {
                buttonPressCounter = Time.time + 0.05f;
                MoveDown();
            }
        }

    }

    void MoveDown()
    {
        foreach (Transform t in transform) 
        {
            if ((Mathf.RoundToInt(t.position.y) <= 0 || IsTileFilled(t)) && isShapeMove)
            {
                if(t.position.y < -0.1f)
                {
                    transform.position += Vector3.up;
                }
                //print(IsTileFilled(t));
                boardManager.OccupyTile(this.transform, testOccupy);
                //print("occupy eylendi");
                CancelInvoke("MoveDown");
                isShapeMove = false;
                //print("Moveover" + this.gameObject.name);
                return;
            }
        }
        transform.position += Vector3.down;
    }

    bool IsRotatable()
    {
        foreach (Transform t in transform)
        {
            if(t.transform.position.x == boardManager.width - 1 || t.transform.position.x < 0.1f || transform.position.x == 0)
            {
                unRotatableTiles++;
                
                if(unRotatableTiles > 2)
                {
                    return false;
                }
            }
        }
        unRotatableTiles = 0;
        return true;
    }
    bool IsRotatable2()
    {
        Vector3 temp = transform.rotation.eulerAngles + new Vector3(0, 0, 90);
        foreach (Transform t in transform)
        {
            if (t.transform.position.x == boardManager.width || t.transform.position.x == 0)
            {
                unRotatableTiles++;

                if (unRotatableTiles > 1)
                {
                    return false;
                }
            }
        }
        unRotatableTiles = 0;
        return true;
    }
    void IsRotatable3()
    {
        foreach (Transform t in transform)
        {
            if (t.transform.position.x > 10)
            {
                transform.position += Vector3.left;
            }
            else if (t.transform.position.x < 0)
            {
                transform.position += Vector3.right;
            }
            else if(t.transform.position.y < 0)
            {
                transform.position += Vector3.up;
            }
        }
    }
    bool IsRotatable4()
    {
        foreach (Transform t in transform)
        {
            if(!IsTileHorizontalEmpty(t, "forRotate"))
            {
                unRotatableTiles++;
                if (unRotatableTiles > 1)
                {
                    return IsTileHorizontalEmpty(t, "forRotate");
                }
            }
        }
        unRotatableTiles = 0;
        return true;
    }

    bool IsRotate()
    {
        foreach(Transform t in transform)
        {
            if (IsTileFilledForRotate(t))
            {
                return false;
            }
        }
        return true;
    }

    bool IsRotate2()
    {
        foreach (Transform t in transform)
        {
            if (!boardManager.TileState3(CalculateCoordinates(t)))
            {
                return false;
            }
        }
        return true;
    }

    Vector3 CalculateCoordinates(Transform t)
    {
        /* (x,y) koordinatındaki bir noktanın, (0,0) merkez noktasından, saat yönünün tersinde θ açı kadar 
         * dönmesinin formülü: 
         *  x′= xcos(θ) − ysin(θ)
         *  y′= xsin(θ) + ycos(θ)
         * Not: Eğer saat yönünde dönmesini istiyorsan -θ açı yapması gerekiyor. rotateMulpitlier ile pozitif negatif dönüştüürüyoruz.
         * 
         * (x,y) koordinatındaki bir noktanın, (a,b) merkez noktasından, saat yönünün tersinde θ açı kadar 
         * dönmesinin formülü:
         * x′= (x - a)cos(θ) − (y - b)sin(θ) + a
         * y′= (x - a)sin(θ) + (y - b)cos(θ) + b
         * Not: t.parent.position, shapelerin parentı ve aynı zamanda pivot noktasının olduğu yer. Yani merkez.
        */

        Vector2 tempCoordinate = new Vector2();
        int posX = Mathf.RoundToInt(t.position.x);
        int posY = Mathf.RoundToInt( t.position.y);
        int angleCos = Mathf.RoundToInt( Mathf.Cos(rotateMultiplier * 90 * Mathf.Deg2Rad));
        int angleSin = Mathf.RoundToInt( Mathf.Sin(rotateMultiplier * 90 * Mathf.Deg2Rad));

        tempCoordinate.x = (posX - t.parent.position.x) * angleCos - (posY - t.parent.position.y) * angleSin + t.parent.position.x;
        tempCoordinate.y = (posX - t.parent.position.x) * angleSin + (posY - t.parent.position.y) * angleCos + t.parent.position.y;

        return tempCoordinate;
    }


    /* Ismovable'da; eğer bir shape'in bir tile'ı 11. kareye veya 0. kareye gelmişse hareketini
     * kısıtlıyorum. Kısıtlamayı sola ve sağa ayrı ayrı yapmak yerine, mevcut bulunduğu konumdan 
     * sağa gittinde veya sola gittiğinde posizyonunda meydana gelecek olana +1 lik veya -1 lik 
     * değişimi Input.GetAxisRaw("Horizontal") ile temsil ediyorum. Tuşa basıldığında hem kontrol 
     * sağlayıp hem de yöne uygun şeilde doğrudan +1 veya -1 verecek. Örneğin en sağ olan 11. 
     * karedeysek ve sağa hareket eytmeye çalışırsak 11 + 1 yaparak false verecek. +1 veya -1 gibi 
     * bir toplama işlemi yapıyoruz çünkü tuşa basıldığında daha unityde hareket olmadan kontrol 
     * sağlamaya çalışıyoruz. Önce hareket edip ondan sonra kontrol etmeye çalışırsak, sağı ve
     * solu ayrı ayrı, true false değerleri ile kullanmamız gerekecek. Ben sadece tek bir 
     * fonkisyonla halletmeye çalıştım.
    */
    bool IsMovable()
    {
        foreach (Transform t in transform)
        {
            
            if (t.transform.position.x + Input.GetAxisRaw("Horizontal") >= boardManager.width || t.transform.position.x + Input.GetAxisRaw("Horizontal") <= -0.5f) // <=-1 de yazılır ancak mavi shape de sorun çıkıyor. Daha geniş kısıtlamak için <=0.5f yaptım. 
            {
                return false;
            }
            if (!IsTileHorizontalEmpty(t))
            {
                return false;
            }
        }
        return true;
    }

    bool IsTileFilled(Transform shape)
    {
        return boardManager.TileState(shape) != null && boardManager.TileState(shape).parent != shape.parent;
    }

    bool IsTileHorizontalEmpty(Transform shape, string choose = "forMove")
    {
        //print("ti,le :  " + boardManager.TileHorizontalState(shape).name);
        return boardManager.TileHorizontalState(shape, choose) == null;
    }

    bool IsTileFilledForRotate(Transform shape)
    {
        return boardManager.TileState2(shape) != null;
    }

    void HorizontalTileCheck(Transform shape)
    {
        foreach (Transform t in shape)
        {
            boardManager.TileHorizontalState(shape);
        }
    }
}
