using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class S_DevNote_Final : MonoBehaviour
{

    public enum developer {AlexanderIArt, AlexandruIProgramming, IgnacioIDesign, LarsIDesign, RobertIArt};
    public enum month {January, February, March, April, May, June, July, August, September, October, November, December};


    [Header("Text Fields")]
    public Text contentText;
    public Text developerText;
    public Text roleText;

    [Header("Seperators")]
    public Image topSeperator;
    public Image bottomSeperator;
    public Color programmingColor;
    public Color designColor;
    public Color artColor;
    
    [Header("Adjustable Options")]
    public developer selectedDeveloper;
    public month selectedMonth;
    [Range(1, 31)]
    public int dayOfMonth;

    public float showArea;
    public float hideArea;

    public float timeToDisplay;

    [TextArea(3, 20)]
    public string content;

    // Start is called before the first frame update
    void Start()
    {

            switch (selectedDeveloper)
            {
                case developer.AlexanderIArt:
                    developerText.text = "Alexander";
                    roleText.text = "Art";
                    topSeperator.color = artColor;
                    bottomSeperator.color = artColor;

                    break;
                case developer.AlexandruIProgramming:
                    developerText.text = "Alexandru";
                    roleText.text = "Programming";
                    topSeperator.color = programmingColor;
                    bottomSeperator.color = programmingColor;
                break;
                case developer.IgnacioIDesign:
                    developerText.text = "Ignacio";
                    roleText.text = "Design";
                    topSeperator.color = designColor;
                    bottomSeperator.color = designColor;
                    break;
                case developer.LarsIDesign:
                    developerText.text = "Lars";
                    roleText.text = "Design";
                    topSeperator.color = designColor;
                    bottomSeperator.color = designColor;
                    break;
                case developer.RobertIArt:
                    developerText.text = "Robert";
                    roleText.text = "Art";
                    topSeperator.color = artColor;
                    bottomSeperator.color = artColor;
                    break;
            }

        contentText.text = "- " + selectedMonth.ToString() + ", " + dayOfMonth + " - " + "\n" + content;
        
    }


    //Show Show Area and Hide Area radiuses in Scene View
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, showArea);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hideArea);
    }
}
