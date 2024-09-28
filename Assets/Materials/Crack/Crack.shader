Shader "CustomRenderTexture/Crack"
{
    Properties
    {
        [Header(Albedo)]
        [MainColor] _BaseColor("Base Color", Color) = (1.0, 1.0, 1.0, 1.0)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}

        [Header(Smoothness and Metallic)]
        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.0
        _Metallic("Metallic", Range(0.0, 1.0)) = 0.0

        [Header(Crack)]
        _CrackProgress("Crack Progression", Range(0.0, 1.0)) = 0.0 //�i�s�
        [HDR] _CrackColor("Crack Color", Color) = (0.0, 0.0, 0.0, 1.0) //�Ђт̐F
        _CrackDetailedness("Detailedness", Range(0.0, 10.0)) = 3.0 //�ׂ���
        _CrackDepth("CrackDepth", Range(0.0, 1.0)) = 0.5 //�Ђт̐[��
        _CrackWidth("CrackWidth", Range(0.01, 0.1)) = 0.05 //�Ђт̕�
        _CrackWallWidth("WallWidth", Range(0.001, 0.2)) = 0.08 //�Ђѕǂ̕�

        [Space]

        _RandomSeed("CrackSeed", Int) = 0
    }
     SubShader
     {
        Tags {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "UniversalMaterialType" = "Lit"
        }
        LOD 300

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        ENDHLSL

        Pass
        {
           Name "Crack"
           Tags { "LightMode" = "UniversalForward" }
          
           HLSLPROGRAM
          
           //�}�e���A�� �L�[���[�h
           #pragma shader_feature_local_fragment _ALPHATEST_ON //�A���t�@�e�X�g�L��
           #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON //�v���}���`�v���C�h�A���t�@�L��
           #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF //�X�y�L�����[�n�C���C�g����
           #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF //���}�b�s���O(����)����
           #pragma shader_feature_local_fragment _SPECULAR_SETUP //�X�y�L�����[�V�F�[�_�[�p�X�L��
           #pragma shader_feature_local _RECEIVE_SHADOWS_OFF //���g�ɑ΂��Ẳe�`�斳��
          
           // ���j�o�[�T�� �p�C�v���C�� �L�[���[�h
           #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
           #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
           #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
           #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
           #pragma multi_compile_fragment _ _SHADOWS_SOFT
          
           // GPU�C���X�^���V���O
           #pragma multi_compile_instancing
          
          
           #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
           #include "Packages/com.unity.render-pipelines.universal/Shaders/LitForwardPass.hlsl"
          
           #pragma vertex Vert
           #pragma fragment Frag
          
           //====�ϐ��錾====//
           float _CrackProgress;
           half4 _CrackColor;
           float _CrackDetailedness;
           float _CrackDepth;
           float _CrackWidth;
           float _CrackWallWidth;
           uint _RandomSeed;
          
           //====�\����====//
           struct v2f {//�t���O�����g�f�[�^�\����
               float4 positionCS : SV_POSITION;
               float2 uv : TEXCOORD0;
               float3 normalOS: NORMAL;
               float3 normalWS: TEXCOORD1;
               float3 positionOS: TEXCOORD2;
               float3 positionWS: TEXCOORD3;
               float3 viewDirWS: TEXCOORD4;
           };
          
           struct v2g {//�W�I���g���f�[�^�\����


           };

           //====�����_���n�_�Z�o���\�b�h====//
          
           //Xorshift32��p�����^�������֐�
           uint XOrShift32(uint value) {
               value = value ^ (value << 13);
               value = value ^ (value >> 17);
               value = value ^ (value << 5);
               return value;
           }
          
           //�����̒l��1�����̏����Ƀ}�b�s���O����
           float MapToFloat(uint value) {
               const float precion = 100000000.0;
               return (value % precion) * rcp(precion);//value��precion�̏�]�Z * 1/precion�̋ߎ��l
           }
          
           //3�����̃����_���Ȓl���Z�o����
           float3 Random3(uint3 src, int seed) {
               uint3 random;
               random.x = XOrShift32(mad(src.x, src.y, src.z));
               random.y = XOrShift32(mad(random.x, src.z, src.x) + seed);
               random.z = XOrShift32(mad(random.y, src.x, src.y) + seed);
               random.x = XOrShift32(mad(random.z, src.y, src.z) + seed);
          
               return float3(MapToFloat(random.x), MapToFloat(random.y), MapToFloat(random.z));
           }
           
           //�w�肵�����W�ɑ΂��āA�{���m�C�p�^�[���̍ł��߂������_���_�ƁA2�Ԗڂɋ߂������_���_���擾����
           void CreateVoronoi(float3 pos, out float3 closest, out float3 secondClosest, out float secondDistance) {
               // �Z���ԍ������̒l�ƂȂ�Ȃ��悤�ɃI�t�Z�b�g���Z
               const uint offset = 100;
               uint3 cellIdx;
               float3 reminders = modf(pos + offset, cellIdx);
          
               // �Ώےn�_����������Z���Ɨאڂ���Z���S�Ăɑ΂��ă����_���_�Ƃ̋������`�F�b�N��
               // 1�ԋ߂��_��2�Ԗڂɋ߂��_�����t����
               float2 closestDistances = 8.0;
          
               [unroll]
               for (int i = -1; i <= 1; i++)
                   [unroll]
               for (int j = -1; j <= 1; j++)
                   [unroll]
               for (int k = -1; k <= 1; k++) {
                   int3 neighborIdx = int3(i, j, k);
          
                   // ���̃Z�����ł̃����_���_�̑��Έʒu���擾
                   float3 randomPos = Random3(cellIdx + neighborIdx, _RandomSeed);
                   // �Ώےn�_���烉���_���_�Ɍ������x�N�g��
                   float3 vec = randomPos + float3(neighborIdx)-reminders;
                   // �����͑S�ē��Ŕ�r
                   float distance = dot(vec, vec);
          
                   if (distance < closestDistances.x) {
                       closestDistances.y = closestDistances.x;
                       closestDistances.x = distance;
                       secondClosest = closest;
                       closest = vec;
                   }
                   else if (distance < closestDistances.y) {
                       closestDistances.y = distance;
                       secondClosest = vec;
                   }
               }
          
               secondDistance = closestDistances.y;
           }
          
           //�w�肵�����W���{���m�C�}�̋��E���ƂȂ邩�ǂ�����0�`1�ŕԂ�
           float GetVoronoiBorder(float3 pos, out float secondDistance) {
               float3 a, b;
               CreateVoronoi(pos, a, b, secondDistance);
          
               //�ȉ��̃x�N�g���̓��ς����E���܂ł̋����ƂȂ�
               //�Ώےn�_����A1�ԋ߂������_���_��2�Ԗڂɋ߂��_�̒��_�Ɍ������x�N�g��
               //1�ԋ߂��_��2�Ԗڂɋ߂��_�����Ԑ��̒P�ʃx�N�g��
               float distance = dot(0.5 * (a + b), normalize(b - a));
          
               return 1.0 - smoothstep(_CrackWidth, _CrackWidth + _CrackWallWidth, distance);
           }
          
           //�w�肵�����W�̂Ђѓx������0�`1�ŕԂ�
           float GetCrackLevel(float3 pos) {
               // �{���m�C�}�̋��E���ŋ[���I�ȃN���b�N�͗l��\��
               float secondDistance;
               float level = GetVoronoiBorder(pos * _CrackDetailedness, secondDistance);
          
               // �����I�ɂЂт��������߂Ƀm�C�Y��ǉ�
               // �v�Z�ʂ����Ȃ��čςނ悤�Ƀ{���m�C��F2(2�Ԗڂɋ߂��_�Ƃ̋���)�𗘗p����
               // ���������l�ȉ��̏ꍇ�̓N���b�N�Ώۂ���O��
               float f2Factor = 1.0 - sin(_CrackProgress * PI * 0.5);
               float minTh = (2.9 * f2Factor);
               float maxTh = (3.5 * f2Factor);
               float factor = smoothstep(minTh, maxTh, secondDistance * 2.0);
               level *= factor;
          
               return level;
           }
          
           //�Ђт���������̍��W���v�Z
           float3 CalcCrackedPos(float3 localPos, float3 localNormal, out float crackLevel) {
               // �ЂёΏۂ̏ꍇ�͖@���Ƌt�����ɉ��܂���
               crackLevel = GetCrackLevel(localPos);
               float depth = crackLevel * _CrackDepth;
               localPos -= localNormal * depth;
          
               return localPos;
           }
          
           
           //CrackLevel�ɉ�����Occlusion���Z�o����
           half CalcOcclusion(float crackLevel) {
               // �Ђт̐[���ɉ����ĉe��Z������
               half occlusion = pow(lerp(1.0, 0.9, crackLevel), 2.0);
               // �Ђт��[�������ŁA�אڃs�N�Z���̍��፷���傫���ꍇ�͉e��Z������
               occlusion *= (crackLevel > 0.95 ? lerp(0.9, 1.0, 1.0 - smoothstep(0.0, 0.1, max(abs(ddy(crackLevel)), abs(ddx(crackLevel))))) : 1.0);
          
               return occlusion;
           }
          
          
           //�o�[�e�b�N�X�V�F�[�_�[
           v2f Vert(Attributes input) {
               v2f output;
               output.positionOS = input.positionOS.xyz;
               output.normalOS = input.normalOS;
          
               Varyings varyings = LitPassVertex(input);
               output.positionCS = varyings.positionCS;
               output.uv = varyings.uv;
               output.positionWS = varyings.positionWS;
               output.normalWS = varyings.normalWS;
          
               return output;
           }
          
           //�t���O�����g�V�F�[�_�[
           half4 Frag(v2f input) : SV_Target{
               float crackLevel = 0.0;

                if (_CrackProgress == 0 || dot(input.normalWS, GetViewForwardDir()) > 0.5) {
                    input.positionOS = input.positionOS;
                }
                else {
                    input.positionOS = CalcCrackedPos(input.positionOS, input.normalOS, crackLevel);
                }
          
               Varyings varyings = (Varyings)0;
               varyings.uv = input.uv;
               varyings.positionWS = crackLevel > 0.0 ? TransformObjectToWorld(input.positionOS) : input.positionWS;
               varyings.positionCS = crackLevel > 0.0 ? TransformObjectToHClip(input.positionOS) : input.positionCS;
          
               // �אڂ̃s�N�Z���Ƃ̃��[���h���W�̍������擾��ɊO�ς����߂Ė@���Z�o
               varyings.normalWS = crackLevel > 0.0 ? normalize(cross(ddy(varyings.positionWS), ddx(varyings.positionWS))) : input.normalWS;
          
               SurfaceData surfaceData;
               InitializeStandardLitSurfaceData(varyings.uv, surfaceData);
          
               OUTPUT_SH(varyings.normalWS, varyings.vertexSH);
          
               InputData inputData;
               InitializeInputData(varyings, surfaceData.normalTS, inputData);
               inputData.vertexLighting = VertexLighting(varyings.positionWS, inputData.normalWS);
          
          
               /* �Ђі͗l */
               // �ЂёΏۂ̏ꍇ�̓N���b�N�J���[��ǉ�
               surfaceData.albedo = lerp(surfaceData.albedo, _CrackColor.rgb, crackLevel);
          
               // �Ђѕ�����AO�ݒ�
               surfaceData.occlusion = min(surfaceData.occlusion, CalcOcclusion(crackLevel));
          
               half4 color = UniversalFragmentPBR(inputData, surfaceData);
          
               return color;
           }
           ENDHLSL
        }
     }

     FallBack "Universal Render Pipeline/Lit"
}
