using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {

	public LayerMask touchInputMask;

#if UNITY_EDITOR

    /*
        It seems to detect move and stay actions correctly
        with the 0.0f value so we'll just leave it like that
        for now since it is just for testing purposes in the
        Unity Editor.
    */
    public float mouseMoveThreshold = 0.0f;
    private Vector3 lastMousePosition;

#endif


    private List<GameObject> touchList = new List<GameObject> ();
	private GameObject[] touchListOld;
	private RaycastHit touchHit;

	// Update is called once per frame
	void Update () {

#if UNITY_EDITOR

		if (Input.GetMouseButton(0) ||
			Input.GetMouseButtonDown(0)||
			Input.GetMouseButtonUp(0)) {

#else
        if (Input.touchCount > 0) {
#endif

            touchListOld = new GameObject[touchList.Count];
			touchList.CopyTo (touchListOld);
			touchList.Clear ();


#if UNITY_EDITOR

            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#else
                /*
                This is in case we want to check for multiple touches
                from the player. That is, multiple fingers at the same
                time (only one possible interpretation to that sentence ok?)
                */
                foreach (Touch touch in Input.touches) {                
				Ray ray = Camera.main.ScreenPointToRay (touch.position);
#endif

                RaycastHit touchHit;

				if (Physics.Raycast (ray, out touchHit, 1000, touchInputMask)) {

                    /*
                    We can safely send the message to the gameobject since
                    it will just call every method with that name inside every
                    component in the game object. We need to be careful with not naming some
                    other functions like these. :D
                    */
                    GameObject hitObject = touchHit.transform.gameObject;

#if UNITY_EDITOR    //  BRANCH: TOUCH JUST BEGAN

                    if (Input.GetMouseButtonDown(0)) {
#else
                    if (touch.phase == TouchPhase.Began) {

#endif
                        hitObject.SendMessage ("OnTouchDown", touchHit.point, SendMessageOptions.DontRequireReceiver);

					}else

#if UNITY_EDITOR    //  BRANCH: TOUCH JUST FINISHED

                    if (Input.GetMouseButtonUp(0)) {
#else
                    if (touch.phase == TouchPhase.Ended) {
#endif
                        hitObject.SendMessage ("OnTouchUp", touchHit.point, SendMessageOptions.DontRequireReceiver);

					}else

#if UNITY_EDITOR    //  BRANCH: THE USER IS TOUCHING AND DRAGGING

                    if (Input.GetMouseButton(0) && (((lastMousePosition - Input.mousePosition).magnitude) > mouseMoveThreshold)) {
#else
					if (touch.phase == TouchPhase.Moved) {
#endif
                        hitObject.SendMessage ("OnTouchMove", touchHit.point, SendMessageOptions.DontRequireReceiver);

					}else

#if UNITY_EDITOR    //  BRANCH: THE USER IS TOUCHING AND NOT DRAGGING

                    if (Input.GetMouseButton(0))
                    {
#else
					if (touch.phase == TouchPhase.Stationary) {
#endif
                        hitObject.SendMessage("OnTouchStay", touchHit.point, SendMessageOptions.DontRequireReceiver);

                    }
                    else

#if UNITY_EDITOR    //  BRANCH: SOMETHING NOT REALLY DESIRED HAPPENED AND WE HAVE TO CANCEL THE TOUCH

                    if (Input.GetMouseButton(1)) {  // In Unity we simulate it with a right click.
#else
					if (touch.phase == TouchPhase.Canceled) {
#endif
                        hitObject.SendMessage ("OnTouchExit", touchHit.point, SendMessageOptions.DontRequireReceiver);

					}
				}
			}

            //In case we are not touching something we were previously touching
			foreach (GameObject G in touchListOld) {
				if (!touchList.Contains (G)) {
                    /*
                    I'm not sure if this should be "OnTouchExit" or "OnTouchUp". Both are likely to have 
                    a similar funcionality on the final implementation of the component
                    */
					G.SendMessage ("OnTouchExit",touchHit.point, SendMessageOptions.DontRequireReceiver);
				}
			}
			#if UNITY_EDITOR    //  BRANCH:no mouse on touchpad

            lastMousePosition = Input.mousePosition;
			#endif

        }//End of if(input)


    }//End of Update()

}//End of Class
