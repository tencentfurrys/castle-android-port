Shader "Custom/Glow" {
Properties {
[PerRendererData]  _MainTex ("Sprite Texture", 2D) = "white" { }
 _Color ("Color", Color) = (1,0.6,0,1)
 _GlowColor ("Glow Color", Color) = (1,1,0,1)
 _Strength ("Glow Strength", Range(5,1)) = 2
 _GlowRange ("Glow Range", Range(0.1,1)) = 0.6
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