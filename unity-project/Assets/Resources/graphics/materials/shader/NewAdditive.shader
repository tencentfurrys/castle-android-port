Shader "Shader Forge/Additive" {
Properties {
 _HSV ("HSV", Color) = (1,0.5,1,1)
 _RGB ("RGB", Color) = (1,1,1,1)
 _Texture ("Texture", 2D) = "bump" { }
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