
using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public bool shouldRotate = true;

    // The target we are following
    public Transform target;
    // The distance in the x-z plane to the target
    public float distance = 10.0f;
    // the height we want the camera to be above the target
    public float height = 4.0f;
    // How much we
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;
    float wantedRotationAngle;

    float wantedHeight;
    float currentRotationAngle;
    float currentHeight;

	Vector3 birdseyePosition = new Vector3(0, 50.0f, 0); 
	Vector3 birdseyeRotation = new Vector3 (90, 0, 0);

    Quaternion currentRotation;
	public bool isPixelCamera = false; 
	public bool birdseye = false; 


    void LateUpdate ()
    {
		if (birdseye && isPixelCamera) {
			transform.position = Vector3.Lerp(transform.position, birdseyePosition, heightDamping * Time.deltaTime);
			transform.rotation = Quaternion.Euler(Vector3.Lerp (transform.eulerAngles, birdseyeRotation, rotationDamping * Time.deltaTime)); 

		}
       else if (target){
           // Calculate the current rotation angles
           wantedRotationAngle = target.eulerAngles.y;
           wantedHeight = target.position.y + height;
           currentRotationAngle = transform.eulerAngles.y;
           currentHeight = transform.position.y;
           // Damp the rotation around the y-axis


           currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
           // Damp the height
           currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
           // Convert the angle into a rotation
			currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
           // Set the position of the camera on the x-z plane to:
           // distance meters behind the target
           transform.position = target.position;
           transform.position -= currentRotation * Vector3.forward * distance;
           // Set the height of the camera
           transform.position = new Vector3 (transform.position.x, currentHeight, transform.position.z);
           // Always look at the target
           if (shouldRotate)
               transform.LookAt (target);
       }


		if (Input.GetKeyDown("q")) {
			birdseye = !birdseye; 
		}

    }



}