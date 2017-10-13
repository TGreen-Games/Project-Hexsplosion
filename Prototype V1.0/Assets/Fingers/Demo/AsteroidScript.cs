using UnityEngine;
using System.Collections;

namespace DigitalRubyShared
{
	public class AsteroidScript : MonoBehaviour
	{
		private void Start ()
		{
		
		}

		private void Update ()
		{
			
		}

		private void OnBecameInvisible()
		{
			GameObject.Destroy(gameObject);
		}
	}
}