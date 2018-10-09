
using UnityEngine;


public class No_Marker : MonoBehaviour {

	 AudioSource audioSource;
	private void Awake()
	{
		this.enabled = false;
	}
	private void Start()
	{
		audioSource = gameObject.GetComponent<AudioSource>();
	}
	private void OnEnable()
	{
		audioSource.Play();
	}
}
