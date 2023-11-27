using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text TotalPointText;
    public Text LevelText;
    public Text LinesText;
    public Text HighScoreText;
    public Text GameOverScoreText;
    public Text NewRecordText;
    public Button PauseButton;
    public GameObject PausePanel;
    public GameObject GameOverPanel;
    public Button MainMenuButton;
    public Button PlayAgainButton;

    //public int Level = 1;
    //public int LevelLimit = 384;
    //public float TotalPoint;
    //public int Point;



    //public void AddPoint(int multiplier)
    //{
    //    // point katlanarak artsýn diye (0.5f + multiplier / 2) ekliyorum.
    //    // Point 20 olursa : 20, 50, 120, 200 þeklinde olucak
    //    TotalPoint = Point * multiplier * (0.5f + multiplier / 2);
    //    TotalPointText.text = "Total Point : " + TotalPoint.ToString();
    //    LevelUP();
    //}

    //void LevelUP()
    //{
    //    if (TotalPoint >= LevelLimit)
    //    {
    //        Level++;
    //        LevelText.text = "Level : " + Level.ToString();
    //        if (ShapeSpeed > 0.1f)
    //        {
    //            ShapeSpeed -= 0.05f;
    //        }
    //        else if (ShapeSpeed > 0.01f)
    //        {
    //            ShapeSpeed -= 0.01f;
    //        }
    //        LevelLimit = (int)(LevelLimit * 1.5f);
    //    }
    //}


}
