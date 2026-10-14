Shader "Blendmodes/ColorDodge" {
Properties {
[Header(Properties)]  _MainTex ("Blend Texture", 2D) = "white" { }
 _Tint1 ("Tint on Texture", Color) = (1,1,1,0)
 _Tint2 ("Tint on Blended Result", Color) = (1,1,1,0)
 _Alpha ("Opacity of Blended Result", Range(0,1)) = 1
[Header(Blending)] [Enum(UnityEngine.Rendering.BlendMode)]  _BlendSrc ("Blend mode Source", Float) = 5
[Enum(UnityEngine.Rendering.BlendMode)]  _BlendDst ("Blend mode Destination", Float) = 10
[Header(Stencil)] [Enum(UnityEngine.Rendering.CompareFunction)]  _StencilComp ("Stencil Comparison", Float) = 8
 _Stencil ("Stencil ID", Float) = 0
[Enum(UnityEngine.Rendering.StencilOp)]  _StencilOp ("Stencil Operation", Float) = 0
 _StencilWriteMask ("Stencil Write Mask", Float) = 255
 _StencilReadMask ("Stencil Read Mask", Float) = 255
[Enum(None,0,Alpha,1,Red,8,Green,4,Blue,2,RGB,14,RGBA,15)]  _ColorMask ("Color Mask", Float) = 15
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