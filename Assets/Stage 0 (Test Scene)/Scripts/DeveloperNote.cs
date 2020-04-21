using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class DeveloperNote : MonoBehaviour
{

    public enum developer {BonnieIProgramming, SandraIDesign, NikolaiIDesign, AlejandroIArt, IverIArt, Traitor};
    public enum month {January, February, March, April, May, June, July, August, September, October, November, December};


    [Header("Text Fields")]
    public Text contentText;
    public Text developerText;
    public Text roleText;
    public Text dateText;

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
        if (selectedDeveloper == developer.Traitor)
        {
            developerText.text = "";
            roleText.text = "";
            dateText.text = "";
        }
        else
        {
            switch (selectedDeveloper)
            {
                case developer.BonnieIProgramming:
                    developerText.text = "Bonnie";
                    roleText.text = "Programming";
                    break;
                case developer.SandraIDesign:
                    developerText.text = "Sandra";
                    roleText.text = "Design";
                    break;
                case developer.NikolaiIDesign:
                    developerText.text = "Nikolai";
                    roleText.text = "Design";
                    break;
                case developer.AlejandroIArt:
                    developerText.text = "Alejandro";
                    //roleText.text = "Art & Sound";
                    roleText.text = "Art";
                    break;
                case developer.IverIArt:
                    developerText.text = "Iver";
                    roleText.text = "Art";
                    break;
            }
            dateText.text = dayOfMonth + "." + selectedMonth.ToString();

        }

        contentText.text = content;
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(GameObject.Find("Player").transform.position);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, showArea);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hideArea);
    }
}
