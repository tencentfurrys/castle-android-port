Shader "Custom/Color_Block" {
Properties {
 _MainTex ("Texture", 2D) = "white" { }
 _RegionTex ("Region Texture", 2D) = "white" { }
 _RedRegionCol ("Region Red Color", Color) = (1,1,1,1)
 _GreenRegionCol ("Region Green Color", Color) = (1,1,1,1)
 _BlueRegionCol ("Region Blue Color", Color) = (1,1,1,1)
 _RedRegionIntensity ("Red Region Intensity", Range(0.1,2)) = 1
 _GreenRegionIntensity ("Green Region Intensity", Range(0.1,2)) = 1
 _BlueRegionIntensity ("Blue Region Intensity", Range(0.1,2)) = 1
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