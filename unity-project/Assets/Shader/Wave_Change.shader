Shader "Distortion" {
Properties {
 _Refraction ("Refraction", Range(0,10)) = 1
 _Power ("Power", Range(1,10)) = 1
 _AlphaPower ("Vertex Alpha Power", Range(1,10)) = 1
 _BumpMap ("Normal Map", 2D) = "bump" { }
 _Cull ("Face Culling", Float) = 2
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