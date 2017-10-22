using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLinesFromPoints : MonoBehaviour {

    LineRenderer lineRenderer;

	// Use this for initialization
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        GetDataPoints.DataPointsChanged += updateLineRenderer;
        //StartCoroutine(startAddInitialValues());
	}

    private IEnumerator startAddInitialValues() {
        yield return StartCoroutine(addInitialValues());
        //StartCoroutine(test());
    }

    IEnumerator addInitialValues() {
        while (true) {
            yield return new WaitForSeconds(.1f);
            //if (GetDataPoints.getDataPoints() != null) {
                for (int i = 0; i < lineRenderer.positionCount; ++i) {
                    GetDataPoints.getDataPoints().Add(lineRenderer.GetPosition(i));
                }
                yield break;
            //}
        }
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    IEnumerator test() {
         while (true) {
            yield return new WaitForSeconds(3f);
            GetDataPoints.addPoint(GetDataPoints.getDataPoints()[GetDataPoints.getDataPoints().Count - 1] - Vector3.left + Random.insideUnitSphere * 1.5f);
        }
    }

    private void updateLineRenderer() {
        List<Vector3> dataPoints = GetDataPoints.getDataPoints();

        lineRenderer.positionCount = dataPoints.Count;
        for (int i = 0; i < dataPoints.Count; ++i) {
            lineRenderer.SetPosition(i, dataPoints[i]);
        }
    }

    public void OnDestroy() {
        GetDataPoints.DataPointsChanged -= updateLineRenderer;
    }
}
