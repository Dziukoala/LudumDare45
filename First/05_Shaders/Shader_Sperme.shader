Shader "First/Sperme" {
	Properties{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_DispTex ("Displacement Texture", 2D) = "white" {}
		_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
		_Displacement("max Displacement", Range(0,2)) = 0.1
		_TimeX("Timer X", Float) = 1
		_TimeY("Timer Y", Float) = 1
//Mouvement texture
		_ScrollXSpeed("X", Range(0,10)) = 0
		_ScrollYSpeed("Y", Range(0,10)) = 0
		_SpeedScroll("SpeedScroll",Range(0,10)) = 0
//Gestion Contour
		_RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimPower("Rim Power", Range(0.1,2.0)) = 3.0
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

	CGPROGRAM
	#pragma surface surf ToonRamp vertex:vert addshadow

	sampler2D _Ramp;

	// custom lighting function that uses a texture ramp based
	// on angle between light direction and normal
	#pragma lighting ToonRamp exclude_path:prepass
	inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
	{
		#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
		#endif

		half d = dot(s.Normal, lightDir) * 0.5 + 0.5;
		half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;

		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
		c.a = 0;
		return c;
	}


	sampler2D _MainTex, _DispTex;
	float4 _Color;
	float _Displacement, _TimeX, _TimeY;
//Gestion Contour
	float4 _RimColor;
	float _RimPower;
//Mouvement texture
	fixed _ScrollXSpeed;
	fixed _ScrollYSpeed;
	fixed _SpeedScroll;

	struct Input {
		float2 uv_MainTex : TEXCOORD0;
		float4 dispTex;
		float3 viewDir;


	};

	void vert(inout appdata_full v, out Input o)
	{
		float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
		half4 d = tex2Dlod(_DispTex, float4(worldPos.x + _Time.x * _TimeX, worldPos.y + _Time.y * _TimeY,0,0));
		UNITY_INITIALIZE_OUTPUT(Input, o);
		v.vertex.xyz += _Displacement * v.normal * d;
		o.dispTex = d;
	}

	void surf(Input IN, inout SurfaceOutput o) {

		fixed2 scrolledUV = IN.uv_MainTex;

		fixed xScrollValue = (_ScrollXSpeed * _Time) * _SpeedScroll;
		fixed yScrollValue = (_ScrollYSpeed * _Time) * _SpeedScroll;
		scrolledUV += fixed2(xScrollValue, yScrollValue);

		half4 c = tex2D(_MainTex, scrolledUV) * _Color;
		//o.Albedo = c.rgb +(IN.dispTex * _Color);
		o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
		o.Albedo += c.rgb;

		half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
		o.Emission = _RimColor.rgb * pow(rim, _RimPower);
	}
	ENDCG

	}

		Fallback "Diffuse"
}