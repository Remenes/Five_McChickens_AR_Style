using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDataPoints : MonoBehaviour {

    static private List<Vector3> dataPoints = new List<Vector3>();
    static public List<Vector3> getDataPoints() {
        return dataPoints;
    }
    static private float closenessThreshold = .1f;

    public delegate void dataPointsChanged();
    public static event dataPointsChanged DataPointsChanged;

	// Use this for initialization
	void Awake() {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void addPoint(Vector3 pointToAdd, float? newThreshold = null) {
        if (newThreshold.HasValue) {
            closenessThreshold = newThreshold.Value;
        }
        if (dataPoints.Count == 0 || Vector3.Distance(pointToAdd, dataPoints[dataPoints.Count-1]) > closenessThreshold) {
            dataPoints.Add(pointToAdd);
            DataPointsChanged();
        }
    }

}
