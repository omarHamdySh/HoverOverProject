using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Advertisements;


public class Unityad : MonoBehaviour
{
    private string androidgameid = "3166017";
    private string iosgameid = "3166016"; 


	public static Unityad instance;

	public void Awake()
	{
		if (instance == null) {

			instance = this;

		} else {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}
		
	public UnityEvent onEndAD;

    void Start()
    {
		#if UNITY_ANDROID
        	Advertisement.Initialize(androidgameid);
       	#endif

      	#if UNITY_IOS
         	Advertisement.Initialize(iosgameid);
       	#endif
    }
    public void ShowAD()
    {
       // Advertisement.Show("video");
    }

    public void ShowRewardVedio()
    {
        //ShowOptions options = new ShowOptions { resultCallback = HandleShowResult };
        //Advertisement.Show("rewardedVideo",options);
    }

	//void HandleShowResult(ShowResult result)
 //   {
 //       if (result == ShowResult.Finished) {

	//		Debug.Log ("Give Him His Reward");
	//		AdvertisingAwards.instance.ShowAward ();
	//		GameManager.instance.RevivePlayer ();
	//		//FindObjectOfType<WinLoseMenu> ().ShowMenu (false);
 //       }

 //       if (result == ShowResult.Skipped)
 //       {

 //           Debug.Log("Ad Skipped");

 //       }

 //       if (result == ShowResult.Failed)
 //       {

 //           Debug.Log("Ad Failed Try Again");

 //       }
 //   }

}
