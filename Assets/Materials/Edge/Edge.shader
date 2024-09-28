Shader "Unlit/Edge"
{
    Properties
    {
        [Header(OutLine)]
        _OutLineColor("OutLineColor", Color) = (0, 0, 0, 1)
        _OutLineThickness("OutLineThickness", float) = 0.5
    }
    SubShader
    {
        Tags { 
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }
        LOD 100

        Pass
        {
            Name "Edge"
            Cull Front
            ZWrite On



            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            float _OutLineThickness;
            half4 _OutLineColor;

            struct a2v
            {
                float4 positionOS : POSITION;
                float4 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
            };

            struct v2f
            {
                float4 positionCS: SV_POSITION;
            };


            v2f vert (a2v v)
            {
                v2f o;
                VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(v.normalOS, v.tangentOS);

                float3 normalWS = vertexNormalInput.normalWS;
                float3 normalCS = TransformWorldToHClipDir(normalWS);

                VertexPositionInputs positionInputs = GetVertexPositionInputs(v.positionOS.xyz);
                o.positionCS = positionInputs.positionCS + float4(normalCS.xy * 0.001 * _OutLineThickness, 0, 0);

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = _OutLineColor;

                return col;
            }
            ENDHLSL
        }
    }
}
