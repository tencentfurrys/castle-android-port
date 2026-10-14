Shader "UI/Unlit/Image_Additive" {
Properties {
[PerRendererData]  _MainTex ("Sprite Texture", 2D) = "white" { }
 _Color ("Tint", Color) = (1,1,1,1)
[MaterialToggle]  _OffSet ("OffSet", Float) = 0
[MaterialToggle]  PixelSnap ("Pixel snap", Float) = 0
 _FlowlightMaskTex ("Mask Texture", 2D) = "white" { }
 _FlowlightTex ("Add Move Texture", 2D) = "white" { }
 _FlowlightColor ("Flowlight Color", Color) = (0,0,0,1)
 _Power ("Power", Float) = 1
 _SpeedX ("SpeedX", Float) = 1
 _SpeedY ("SpeedY", Float) = 0
 _StencilComp ("Stencil Comparison", Float) = 8
 _Stencil ("Stencil ID", Float) = 0
 _StencilOp ("Stencil Operation", Float) = 0
 _StencilWriteMask ("Stencil Write Mask", Float) = 255
 _StencilReadMask ("Stencil Read Mask", Float) = 255
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