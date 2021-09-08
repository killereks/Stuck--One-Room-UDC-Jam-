//Shady
using UnityEngine;

[ExecuteInEditMode]
public class Reveal : MonoBehaviour
{
    [SerializeField] Material Mat;
    [SerializeField] Light SpotLight;
	
	void Update ()
    {
        Mat.SetVector("_LightPosition",  SpotLight.transform.position);
        Mat.SetVector("_LightDirection", -SpotLight.transform.forward );
        Mat.SetFloat("_LightAngle", SpotLight.innerSpotAngle);
        Mat.SetColor("_Color", SpotLight.color);
    }//Update() end
}//class end