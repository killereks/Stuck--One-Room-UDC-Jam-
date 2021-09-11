Shader "Hidden/VLightingShader"
{
	Properties
	{
		[HideInInspector]
		_MainTex ("source", 2D) = "white"
	}

	// Stuff that is common for all of your shader passes
	HLSLINCLUDE

	// includes
	// - "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	// - struct ls_appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; }
	// - struct ls_v2f { float4 vertex : SV_POSITION; float2 uv : TEXCOORD0; }
	// - ls_v2f vert (ls_appdata)
	#include "Assets/LoneStack/Editor/LoneStack.cginc"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
	#include "noiseSimplex.cginc"

	#pragma multicompile  _MAIN_LIGHT_SHADOWS
	#pragma multicompile  _MAIN_LIGHT_SHADOWS_CASCADE

	sampler2D _MainTex;
	float _Blend;

	float random(float2 st) {
		return frac(sin(dot(st.xy,
			float2(12.9283231, 89.27831))) * 28391.58923578);
	}

	ENDHLSL

		Subshader
	{

		Cull Off ZWrite Off ZTest Always

		Pass // 0 - First pass
		{

			Blend One One

			HLSLPROGRAM

			#pragma vertex ls_vert
			#pragma fragment frag

			float3 _CamPos;
			float3 _CamFwd;
			float3 _CamUp;
			float3 _CamRight;

			int _StepCount;
			float _Distance;

			float _BrightnessMultiplier;

			float3 _color;

			float noiseMult;

			TEXTURE2D(_CameraDepthTexture);
			SAMPLER(sampler_CameraDepthTexture);

			half4 frag(ls_v2f i) : SV_Target
			{
				float camDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, i.uv.xy), _ZBufferParams);
				float3 ray = normalize(_CamFwd + _CamUp * (i.uv.y * 2 - 1) + _CamRight * (i.uv.x * 2 - 1));
				float stepSize = 1.0 / (float)_StepCount;

				float sampleDepth = random(i.uv) * stepSize * _Distance;

				float pixelBrightness = 0;

				ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
				half4 shadowParams = GetMainLightShadowParams();

				[unroll(50)]
				for (int s = 0; s < _StepCount; s++) {
					sampleDepth += stepSize * _Distance;

					if (sampleDepth >= camDepth) break;

					float4 wpos = float4(_CamPos + ray * sampleDepth, 1);
					int cascadeIndex = ComputeCascadeIndex(wpos.xyz);
					float4 lightPos = mul(_MainLightWorldToShadow[cascadeIndex], wpos);

					// 0 - in shadow
					// 1 - not in shadow
					float shadowMap = SampleShadowmap(TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), lightPos, shadowSamplingData, shadowParams, false);

					pixelBrightness += stepSize * lerp(shadowMap, 1, float(cascadeIndex >= 4 || lightPos.z < 0));
				}

				return half4(pixelBrightness * _color,1) * _BrightnessMultiplier;
			}

			ENDHLSL
		}
	}
}