using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DeveloperNote : MonoBehaviour
{

    public enum developer {Bonnie, Sandra, Nikolai, Alejandro, Iver};
    public enum month {January, February, March, April, May, June, July, August, September, October, November, December};




    public developer selectedDeveloper;
    public month selectedMonth;

    [Range(1, 31)]
    public int dayOfMonth;

    public float showArea;
    public float hideArea;


    [TextArea(3, 20)]
    public string content;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(GameObject.Find("Player").transform.position);
    }

    void OnGui()
    {
        if (content.Length > 300)
        {
            content = content.Substring(0, 299);
        }


    }
}
