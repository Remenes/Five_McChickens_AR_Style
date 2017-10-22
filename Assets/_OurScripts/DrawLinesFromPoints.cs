#define TestMouseClickCase

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLinesFromPoints : MonoBehaviour {

    List<LineRenderer> lineRenderers = new List<LineRenderer>();

    private LineRenderer lineRenderer { get { return lineRenderers[lineRenderers.Count - 1]; } }

    [SerializeField] private LineRenderer lineRendererCopy;

    // Use this for initialization
    void Start () {
        print("SCript starting");
        lineRenderers.Add(GetComponent<LineRenderer>());
        lineRenderer.enabled = false;
        GetDataPoints.DataPointsChanged += updateLineRenderer;
        print("Adding initial values");
        StartCoroutine(startAddInitialValues());
	}

    private IEnumerator startAddInitialValues() {
        yield return StartCoroutine(addInitialValues());
        //StartCoroutine(test());
    }

    IEnumerator addInitialValues() {
        while (true) {
            yield return new WaitForSeconds(.1f);
            print("still checking");
            //if (GetDataPoints.getDataPoints() != null) {
                for (int i = 0; i < lineRenderer.positionCount; ++i) {
                    GetDataPoints.getDataPoints().Add(lineRenderer.GetPosition(i));
                }
                yield break;
            //}
        }
    }

#if TestMouseClickCase
    int distanceAway = 1;
    float x, y;
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 relativePosition = ((Vector2) Camera.main.ScreenToViewportPoint(Input.mousePosition) - new Vector2(.5f, .5f)) * Random.Range(2,4);
            x += relativePosition.x;
            y += relativePosition.y;
            Vector3 relativePosition3D = new Vector3(x, y, distanceAway = distanceAway + (distanceAway < 10 ? 1 : Random.Range(-1,1)));
            GetDataPoints.addPoint(relativePosition3D);
        }
	}
#endif

    IEnumerator test() {
         while (true) {
            yield return new WaitForSeconds(3f);
            GetDataPoints.addPoint(GetDataPoints.getDataPoints()[GetDataPoints.getDataPoints().Count - 1] - Vector3.left + Random.insideUnitSphere * 1.5f);
        }
    }

    private void updateLineRenderer() {
        lineRenderer.enabled = true;
        List<Vector3> dataPoints = GetDataPoints.getDataPoints();
        lineRenderer.positionCount = dataPoints.Count;
        for (int i = 0; i < dataPoints.Count; ++i) {
            lineRenderer.SetPosition(i, dataPoints[i]);
        }
    }
    

    private void changeLineColor(Color newColor) {
        lineRenderers.Add(Instantiate(lineRendererCopy));
        print("Changing renderer");
        lineRenderer.startColor = newColor;
        print(lineRenderer.startColor);
        lineRenderer.endColor = newColor;
    }

    public void ChangeLineColorToBlue() {
        changeLineColor(Color.white);
    }

    public void ChangeLineColorToRed() {
        changeLineColor(new Color(1f, 0.01f, .15f));
    }
}
