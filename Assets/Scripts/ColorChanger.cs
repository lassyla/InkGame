using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColorChanger : MonoBehaviour {


	PixelSetter ps; 
	Animator m_Animator;
	AudioSource audio; 
	Color cyan = new Color(0, 1, 1, 1); 

	Color magenta = new Color(1, 0, 1, 1); 
	Color yellow = new Color(1, 1, 0, 1); 

	// Use this for initialization
	void Start () {
		ps = (PixelSetter) GameObject.Find("Plane").GetComponent("PixelSetter"); 
		m_Animator = GetComponent<Animator>();
		audio = GetComponent<AudioSource> (); 
	}
	
	void OnCollisionEnter(Collision collision)
    {
		if(collision.gameObject.tag == "cyan")
        {
			audio.Play (0); 
			m_Animator.SetTrigger("Eating");
            Destroy(collision.gameObject);
			ps.SetColor(new Color(0, 0, 0, 1), cyan);
        }
			

		if(collision.gameObject.tag == "magenta")
        {
			audio.Play (0); 
			m_Animator.SetTrigger("Eating");
            Destroy(collision.gameObject);
			ps.SetColor(new Color(.25f, .25f, .25f, 1), magenta); 
        }		
		
		if(collision.gameObject.tag == "yellow")
        {
			audio.Play (0); 
			m_Animator.SetTrigger("Eating");
            Destroy(collision.gameObject);
			ps.SetColor(new Color(.5f, .5f, .5f, 1), yellow); 

        }

	}

	// Update is called once per frame
	void Update () {
		
	}
}
