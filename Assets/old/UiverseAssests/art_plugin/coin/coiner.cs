using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;

public class coiner : MonoBehaviour {
	int time;
	// Use this for initialization
	void Start () {
		
		gameObject.transform.localScale = Vector3.zero;
		gameObject.transform.eulerAngles = new Vector3(70, 0, 0);
		
		time = Program.TimePassed();
	
	}
	public void coin_app()
	{
		iTween.ScaleTo(gameObject, new Vector3(10, 10, 8.743f), 1f);
	}
	public void dice_app()
	{
		iTween.ScaleTo(gameObject, new Vector3(0.2f, 0.2f, 0.2f), 1f);
	}
	public void disapp()
	{
		iTween.ScaleTo(gameObject, new Vector3(0,0,0), 1f);
	}
	public void tocoin(bool up)
	{
		if (up)
		{
			iTween.RotateTo(gameObject, new Vector3(70, 0, 0) + (new Vector3(360, 0, 0)) * 15, 3f);
		}
		else
		{
            iTween.RotateTo(gameObject, new Vector3(250, 0, 0) + (new Vector3(360, 0, 0)) * 15, 3f);
		}
	}
	public void todice(int num)
	{
		switch (num)
		{
            case 1:
				this.transform.DORotate(new Vector3(-70, 180, 180) + (new Vector3(360, 360, 360)) * 10, 2f, RotateMode.FastBeyond360);
                //iTween.RotateTo(gameObject, new Vector3(-70, 180, 180) + (new Vector3(360, 360, 360)) * 15, 3f);
				break;
            case 2:
                this.transform.DORotate(new Vector3(20, 180, 180) + (new Vector3(360, 360, 360)) * 10, 2f, RotateMode.FastBeyond360);
                //iTween.RotateTo(gameObject, new Vector3(20, 180, 180) + (new Vector3(360, 360, 360)) * 15, 3f);
                break;
            case 3:
                this.transform.DORotate(new Vector3(-20, 0, -90) + (new Vector3(360, 360, 360)) * 10, 2f, RotateMode.FastBeyond360);
                //iTween.RotateTo(gameObject, new Vector3(-20, 0, -90) + (new Vector3(360, 360, 360)) * 15, 3f);
                break;
            case 4: 
                this.transform.DORotate(new Vector3(-20, 0, 90) + (new Vector3(360, 360, 360)) * 10, 2f, RotateMode.FastBeyond360);
                //iTween.RotateTo(gameObject, new Vector3(-20, 0, 90) + (new Vector3(360, 360, 360)) * 15, 3f);
                break;
            case 5: 
                this.transform.DORotate(new Vector3(-20, 0, 0) + (new Vector3(360, 360, 360)) * 10, 2f, RotateMode.FastBeyond360);
                //iTween.RotateTo(gameObject, new Vector3(-20, 0, 0) + (new Vector3(360, 360, 360)) * 15, 3f);
                break;
            case 6: 
                this.transform.DORotate(new Vector3(70, 0, 0) + (new Vector3(360, 360, 360)) * 10, 2f, RotateMode.FastBeyond360);
                //iTween.RotateTo(gameObject, new Vector3(70, 0, 0) + (new Vector3(360, 360, 360)) * 15, 3f);
                break;
		}	
	}
	// Update is called once per frame
	void Update () {
		if(Program.TimePassed()-time>3000){
			time = Program.TimePassed();
			Destroy(gameObject, 3000);
			iTween.ScaleTo(gameObject, Vector3.zero, 1f);
		}
	}
}
