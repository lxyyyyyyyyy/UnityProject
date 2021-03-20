using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    public UnityEvent event1;
    private Button btn;


    void Awake()
    {
        btn = GetComponent<Button>();
        event1.AddListener(GameObject.Find("DomChange").GetComponent<ChangeDominatorScript>().ChangeDominator);
    }

    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(() =>
        {
            event1.Invoke();
        });

    }

    void Update()
    {

    }   
}
