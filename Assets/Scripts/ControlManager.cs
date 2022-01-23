using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class ControlManager : MonoBehaviour
{
    [Header("InputFields to values entered")] 
    [SerializeField] private TMP_InputField speedStart;
    [SerializeField] private TMP_InputField angleStart;
    [SerializeField] private TMP_InputField estimatedDistance;

    [Header("Panles notifications")] 
    [SerializeField] private GameObject panelEmptyFields;
    [SerializeField] private GameObject panelFinalResult;

    [Header("Text to final result")] 
    [SerializeField] private TextMeshProUGUI textFinalResult;

    [Header("Texts to show user")] 
    [SerializeField] private string textCongratulations;
    [SerializeField] private string textIncorrect;

    [SerializeField] private float valueMarginError = 5f;
    [SerializeField] private float gravity = 10f;

    [Header("Circle object to move")] 
    [SerializeField] private GameObject circlePlayer;
    
    [SerializeField] private float speedMove = 5;

    private float speedStartUser;
    private float angleStartUser;
    private float distanceEstimatedUser;

    private bool canStartSimulation;

    private float distanceFinalSimulation;
    private float maxHeight;
    
    private Vector3 startPosCircle;
    private Vector3 posTarget;
    
    private float radians;

    private void Awake()
    {
        startPosCircle = circlePlayer.transform.position;
    }
    
    public void LimitPositiveSpeed()
    {
        if (speedStart.text != "" && speedStart.text != "-")
        {
            float speedValueEnter = float.Parse(speedStart.text);
            if (speedValueEnter > 0)
            {
                speedStartUser = speedValueEnter;
            }
            else
            {
                speedStart.text = 0.ToString();
            }
        }
    }
    
    public void LimitRangeAngle()
    {
        if (angleStart.text != "")
        {
            float angleValueEnter = float.Parse(angleStart.text); 
            if (angleValueEnter > 0 && angleValueEnter <= 90)
            {
                angleStartUser = angleValueEnter;
            }
            else
            {
                angleStart.text = 0.ToString();
            }
        }
    }

    public void StartSimulation()
    {
        CheckEmptyFields();
        CalculateDistanceTravelled();
        CalculateMaxHeight();
        posTarget = new Vector3(circlePlayer.transform.position.x + distanceFinalSimulation, circlePlayer.transform.position.y, 0);
        if (canStartSimulation)
        {
            StartCoroutine(StartSimulationParabolicMovement());
        }
    }

    public void RestartSimulation()
    {
        circlePlayer.transform.position = startPosCircle;
        speedStart.text = "";
        angleStart.text = "";
        estimatedDistance.text = "";
    }

    private void CheckEmptyFields()
    {
        if (speedStart.text != "" && angleStart.text != "" && estimatedDistance.text != "")
        {
            canStartSimulation = true;
            distanceEstimatedUser = float.Parse(estimatedDistance.text);
        }
        else
        {
            canStartSimulation = false;
            panelEmptyFields.SetActive(true);
            
        }
    }

    private IEnumerator StartSimulationParabolicMovement()
    {
        while (Vector3.Distance(circlePlayer.transform.position, posTarget) >= 0.1f)
        {
            float displacementX = Mathf.MoveTowards(circlePlayer.transform.position.x, posTarget.x, speedMove * Time.deltaTime);
            float HighY = Mathf.Lerp(startPosCircle.y, posTarget.y, (displacementX - startPosCircle.x) / distanceFinalSimulation);
            float arch = angleStartUser * (displacementX - startPosCircle.x) * (displacementX - posTarget.x) / (-0.25f * distanceFinalSimulation * distanceFinalSimulation);
            Vector3 posFinal = new Vector3(displacementX, HighY + arch, 0);
		
            circlePlayer.transform.position = posFinal;
            yield return null;
        }

        StartCoroutine(FinalResultToComparison());
    }

    IEnumerator FinalResultToComparison()
    {
        yield return new WaitForSeconds(0.6f);
        canStartSimulation = false;
        float finalMarginError = (distanceFinalSimulation * valueMarginError) / 100;
        panelFinalResult.SetActive(true);
        
        if (distanceEstimatedUser > (distanceFinalSimulation - finalMarginError) && distanceEstimatedUser < (distanceFinalSimulation + finalMarginError))
        {
            textFinalResult.text = textCongratulations;
        }
        else
        {
            textFinalResult.text = textIncorrect;
        }
    }

    private void CalculateDistanceTravelled()
    {
        float radianes = angleStartUser * Mathf.Deg2Rad;
        radians = radianes;
        double sin = Mathf.Sin(radianes * 2);
        var distance = Mathf.Pow(speedStartUser,2) * sin / gravity;
        distanceFinalSimulation = (float)distance;
    }

    private void CalculateMaxHeight()
    {
        double sin = Mathf.Sin(radians);
        var maxHeightTemp = (Mathf.Pow(speedStartUser, 2) * (sin * sin)) / (2 * gravity);
        maxHeight = (float)maxHeightTemp;
    }
}
