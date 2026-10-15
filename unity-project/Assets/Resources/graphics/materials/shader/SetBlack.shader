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
SubShader {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
    Cull Off
    Lighting Off
    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha
    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
    #pragma multi_compile _ PIXELSNAP_ON
    #include "UnityCG.cginc"

    sampler2D _MainTex;
    sampler2D _RegionTex;
    fixed4 _RedRegionCol;
    fixed4 _GreenRegionCol;
    fixed4 _BlueRegionCol;
    float _RedRegionIntensity;
    float _GreenRegionIntensity;
    float _BlueRegionIntensity;

    #ifdef ETC1_EXTERNAL_ALPHA
        sampler2D _AlphaTex;
    #endif

    struct appdata_t
    {
        float4 vertex : POSITION;
        fixed4 color : COLOR;
        float2 texcoord : TEXCOORD0;
    };

    struct v2f
    {
        float4 vertex : SV_POSITION;
        fixed4 color : COLOR;
        float2 texcoord : TEXCOORD0;
    };

    fixed4 _Color;

    v2f vert (appdata_t IN)
    {
        v2f OUT;
        OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
        OUT.texcoord = IN.texcoord;
        OUT.color = IN.color;
        #ifdef PIXELSNAP_ON
        OUT.vertex = UnityPixelSnap (OUT.vertex);
        #endif
        return OUT;
    }

    fixed4 SampleMain (float2 uv)
    {
        fixed4 c = tex2D (_MainTex, uv);
        #ifdef ETC1_EXTERNAL_ALPHA
        c.a = tex2D (_AlphaTex, uv).r;
        #endif
        return c;
    }

    fixed4 frag (v2f IN) : SV_Target
    {
        fixed4 c = SampleMain (IN.texcoord) * IN.color;
        fixed4 rg = tex2D (_RegionTex, IN.texcoord);
        c.rgb *= (_RedRegionCol.rgb * rg.r * _RedRegionIntensity
            + _GreenRegionCol.rgb * rg.g * _GreenRegionIntensity
            + _BlueRegionCol.rgb * rg.b * _BlueRegionIntensity);

        return c;
    }
    ENDCG
}
}
