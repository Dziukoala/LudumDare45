Shader "Toon/Grass" {
	Properties{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
		_Speed("MoveSpeed", Range(20,50)) = 25 // speed of the swaying
		_Rigidness("Rigidness", Range(1,50)) = 25 // lower makes it look more "liquid" higher makes it look rigid
		_SwayMax("Sway Max", Range(0, 0.1)) = .005 // how far the swaying goes
		_YOffset("Y offset", float) = 0.5// y offset, below this is no animation
		_TimeY("Timer Y", Float) = 1

		_RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimPower("Rim Power", Range(0.0,5.0)) = 2.0

	}

		SubShader{
			Tags { "RenderType" = "Opaque" "DisableBatching" = "True" }// disable batching lets us keep object space
			LOD 200


	CGPROGRAM
	#pragma surface surf ToonRamp vertex:vert addshadow // addshadow applies shadow after vertex animation

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

		float _Speed;
		float _SwayMax;
		float _YOffset;
		float _Rigidness;

		float4 _RimColor;
		float _RimPower;


		struct Input {
			float2 uv_MainTex : TEXCOORD0;
			float4 dispTex;
			float3 viewDir;
		};
		void vert(inout appdata_full v)//
		{
			float3 wpos = mul(unity_ObjectToWorld, v.vertex).xyz;// world position
			float x = sin(wpos.x / _Rigidness + (_Time.x * _Speed)) * (v.vertex.y - _YOffset) * 5;// x axis movements
			float z = sin(wpos.z / _Rigidness + (_Time.x * _Speed)) * (v.vertex.y - _YOffset) * 5;// z axis movements
			v.vertex.x += step(0,v.vertex.y - _YOffset) * x * _SwayMax;// apply the movement if the vertex's y above the YOffset
			v.vertex.z += step(0,v.vertex.y - _YOffset) * z * _SwayMax;
		}

		void surf(Input IN, inout SurfaceOutput o) {
			half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;

			half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			o.Emission = _RimColor.rgb * pow(rim, _RimPower);
		}
		ENDCG

		}

			Fallback "Diffuse"
}