Shader "Noise" {
Properties {
 _MainTex ("Texture", 2D) = "white" { }
 _NoiseFrequency ("Noise Frequency", Float) = 1
 _NoiseSpeed ("Noise Speed", Float) = 1
 _NoiseOctave ("Noise Octave", Range(1,8)) = 1
 _NoiseLacunarity ("Noise Lacunarity", Float) = 0.5
}
	//DummyShaderTextExporter
	
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Lambert
#pragma target 3.0
		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutput o)
		{
			float4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
}