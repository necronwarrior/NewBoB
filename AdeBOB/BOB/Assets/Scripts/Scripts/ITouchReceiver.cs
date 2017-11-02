using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

    Interface that should be implemented in the components that
    are meant to react to touch inputs received in "TouchInput.cs"

    The names of the functions are pretty selfexplanatory.

*/
public interface ITouchReceiver {

    void OnTouchDown(Vector3 point);

    void OnTouchUp(Vector3 point);

	void OnTouchMove(Vector3 point);

    void OnTouchStay(Vector3 point);

    void OnTouchExit(Vector3 point);


}
