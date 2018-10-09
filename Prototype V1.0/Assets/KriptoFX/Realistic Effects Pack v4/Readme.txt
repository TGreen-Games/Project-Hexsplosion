Pack includes prefabs of main effects + additional effects (character effects, collision effects, etc). 
For unity 5.5 use "Optimized" if it possible. Optimized effects uses native unity turbulence (it faster then myself turbulence).

------------------------------------------------------------------------------------------------------------------------------------------
NOTE for PC:
If you want to use posteffect for PC like in the demo:

*) Remove "RFX4_Bloom.cs" from camera if you used this script before. 
1) Download unity free posteffects 
https://assetstore.unity.com/packages/essentials/post-processing-stack-83912
2) Add "PostProcessingBehaviour.cs" on main Camera.
3) Set the "PostEffects" profile. (path "Assets\KriptoFX\Realistic Effects Pack v4\PostEffects.asset")
4) You should turn on "HDR" on main camera for correct posteffects. 
If you have forward rendering path (by default in Unity), you need disable antialiasing "edit->project settings->quality->antialiasing"
or turn of "MSAA" on main camera, because HDR does not works with msaa. If you want to use HDR and MSAA then use "post effect msaa". 

------------------------------------------------------------------------------------------------------------------------------------------
NOTE for MOBILES:
For correct work on mobiles in your project scene you need:

1) Add script "RFX4_DistortionAndBloom.cs" on main camera. It's allow you to see correct distortion, soft particles and physical bloom 
The mobile bloom posteffect work if mobiles supported HDR textures or supported openGLES 3.0

------------------------------------------------------------------------------------------------------------------------------------------
Support platforms:

PC/Mobile/Consoles/VR
For mobile platform use myself script for optimized distortions and optimized physically correct bloom.
Just add script "RFX4_DistortionAndBloom.cs" to main camera.
VR distortions and bloom supported. All effects tested on Oculus Rift CV1 with single and dual mode rendering and work perfect. 

------------------------------------------------------------------------------------------------------------------------------------------
Using effects:

Just drag and drop prefab of effect on scene and use that :)
If you want use effects in runtime, use follow code:

"Instantiate(prefabEffect, position, rotation);"

Using projectile collision event:
void Start ()
{
	var tm = GetComponentInChildren<RFX4_TransformMotion>(true);
	if (tm!=null) tm.CollisionEnter += Tm_CollisionEnter;
}

private void Tm_CollisionEnter(object sender, RFX4_TransformMotion.RFX4_CollisionInfo e)
{
        Debug.Log(e.Hit.transform.name); //will print collided object name to the console.
}


------------------------------------------------------------------------------------------------------------------------------------------
Effect modification:

All effects includes helpers scripts (collision behaviour, light/shader animation etc) for work out of box. 
Also you can add additional scripts for easy change of base effects settings.
 
RFX4_EffectSettingColor - for change color of effect (uses HUE color). Can be added on any effect.
RFX4_EffectSettingPhysxForce - for change physx force of effects with rocks levitation (and for effect 6 with black hole force)
RFX4_EffectSettingProjectile - for change projectile fly distance, speed and collided layers. 
RFX4_EffectSettingVisible - for change visible status of effect using smooth fading by time. 


