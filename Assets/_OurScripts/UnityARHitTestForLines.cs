using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.XR.iOS {

    public class UnityARHitTestForLines : MonoBehaviour {

        [SerializeField] private GameObject lineRendererPrefab;
        private bool canDestroy = false;


        bool HitTestWithResultType(ARPoint point, ARHitTestResultType resultTypes) {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
            if (hitResults.Count > 0) {
                foreach (var hitResult in hitResults) {
                    Debug.Log("Got hit!");
                    //m_HitTransform.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                    //m_HitTransform.rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
                    Vector3 worldPosHit = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                    GetDataPoints.addPoint(worldPosHit);
                    //Debug.Log(string.Format("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));
                    return true;
                }
            }
            return false;
        }

        /*
        float x, y;
        int distanceZ = 1;
        void Update() {
            if (Input.GetMouseButton(0)) {
                canDestroy = true;
                Vector2 screenToView = ((Vector2) Camera.main.ScreenToViewportPoint(Input.mousePosition) - new Vector2(.5f, .5f)) * Random.Range(-3, 3);
                x += screenToView.x;
                y += screenToView.y;
                Vector3 newPosition = new Vector3(x, y, distanceZ++);
                GetDataPoints.addPoint(newPosition);
            }
            else if (canDestroy) {
                print("Destroying now");
                canDestroy = false;
                disableAndCreateNewRenderer();
            }
        }
        */
        
        // Update is called once per frame
        void Update() {
            if (Input.touchCount > 0) {
                var touch = Input.GetTouch(0);
                if (!EventSystem.current.IsPointerOverGameObject() && (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)) {
                    canDestroy = true;
                    //var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
                    Vector3 centerPosition = new Vector3(Screen.width/2, Screen.height/2);
                    var centerOfScreenPosition = Camera.main.ScreenToViewportPoint(centerPosition);
                    ARPoint point = new ARPoint {
                        x = centerOfScreenPosition.x,
                        y = centerOfScreenPosition.y
                    };

                    // prioritize reults types
                    ARHitTestResultType[] resultTypes = {
                        ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                        // if you want to use infinite planes use this:
                        //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                        ARHitTestResultType.ARHitTestResultTypeHorizontalPlane,
                        ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                    };

                    foreach (ARHitTestResultType resultType in resultTypes) {
                        if (HitTestWithResultType(point, resultType)) {
                            return;
                        }
                    }
                }
            }
            else if (canDestroy) {
                canDestroy = false;
                disableAndCreateNewRenderer();
            }
        }

        private void disableAndCreateNewRenderer() {
            GetDataPoints.clearData();
            GameObject newRendererObject = Instantiate(lineRendererPrefab);
            newRendererObject.name = "New Object Renderer ()";

            name = "Old ()";
            Destroy(GetComponent<DrawLinesFromPoints>());
            Destroy(this);

        }

    }
}