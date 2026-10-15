Shader "Skybox/6 Sided" {
Properties {
_Tint ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
[Gamma]  _Exposure ("Exposure", Range(0,8)) = 1
 _Rotation ("Rotation", Range(0,360)) = 0
[NoScaleOffset]  _FrontTex ("Front [+Z]   (HDR)", 2D) = "grey" { }
[NoScaleOffset]  _BackTex ("Back [-Z]   (HDR)", 2D) = "grey" { }
[NoScaleOffset]  _LeftTex ("Left [+X]   (HDR)", 2D) = "grey" { }
[NoScaleOffset]  _RightTex ("Right [-X]   (HDR)", 2D) = "grey" { }
[NoScaleOffset]  _UpTex ("Up [+Y]   (HDR)", 2D) = "grey" { }
[NoScaleOffset]  _DownTex ("Down [-Y]   (HDR)", 2D) = "grey" { }
}
SubShader {
    Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
    Cull Off
    ZWrite Off
    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #include "UnityCG.cginc"
    sampler2D _FrontTex; sampler2D _BackTex;
    sampler2D _LeftTex; sampler2D _RightTex;
    sampler2D _UpTex; sampler2D _DownTex;
    fixed4 _Tint;
    struct appdata_t { float4 vertex : POSITION; };
    struct v2f { float4 vertex : SV_POSITION; float3 dir : TEXCOORD0; };
    v2f vert (appdata_t IN)
    {
        v2f OUT;
        OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
        OUT.dir = IN.vertex.xyz;
        return OUT;
    }
    fixed4 frag (v2f IN) : SV_Target
    {
        float3 d = normalize(IN.dir);
        float ax = abs(d.x), ay = abs(d.y), az = abs(d.z);
        fixed4 c;
        if (ax > ay && ax > az)
            c = d.x > 0 ? tex2D(_RightTex, float2(-d.z / ax, d.y / ax) * 0.5 + 0.5)
                        : tex2D(_LeftTex, float2(d.z / ax, d.y / ax) * 0.5 + 0.5);
        else if (ay > az)
            c = d.y > 0 ? tex2D(_UpTex, float2(d.x / ay, -d.z / ay) * 0.5 + 0.5)
                        : tex2D(_DownTex, float2(d.x / ay, d.z / ay) * 0.5 + 0.5);
        else
            c = d.z > 0 ? tex2D(_FrontTex, float2(d.x / az, d.y / az) * 0.5 + 0.5)
                        : tex2D(_BackTex, float2(-d.x / az, d.y / az) * 0.5 + 0.5);
        return c * _Tint;
    }
    ENDCG
}
}
