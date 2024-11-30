Shader "Advanced/Lightmap" {
	Properties {
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_SpecOffset ("Specular Offset from Camera", Vector) = (1,10,2,0)
		_SpecRange ("Specular Range", Float) = 20
		_SpecColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
		_Shininess ("Shininess", Range(0.01,1)) = 0.078125
		_ScrollingSpeed ("Scrolling speed", Vector) = (0,0,0,0)
	}
SubShader { 
 LOD 100
 Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
Program "vp" {
SubProgram "gles " {
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;
#define gl_ModelViewMatrix glstate_matrix_modelview0
uniform mat4 glstate_matrix_modelview0;

varying lowp vec3 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform highp vec4 _ScrollingSpeed;
uniform highp float _Shininess;
uniform highp vec3 _SpecColor;
uniform highp float _SpecRange;
uniform highp vec3 _SpecOffset;
uniform highp vec4 unity_LightmapST;


uniform highp vec4 _Time;
attribute vec4 _glesMultiTexCoord1;
attribute vec4 _glesMultiTexCoord0;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  lowp vec3 tmpvar_1;
  mat3 tmpvar_2;
  tmpvar_2[0] = gl_ModelViewMatrix[0].xyz;
  tmpvar_2[1] = gl_ModelViewMatrix[1].xyz;
  tmpvar_2[2] = gl_ModelViewMatrix[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = ((gl_ModelViewMatrix * _glesVertex).xyz - (_SpecOffset * vec3(1.0, 1.0, -1.0)));
  highp vec3 tmpvar_4;
  tmpvar_4 = (((_SpecColor * pow (clamp (dot ((tmpvar_2 * normalize(_glesNormal)), normalize(((vec3(0.0, 0.0, 1.0) + normalize(-(tmpvar_3))) * 0.5))), 0.0, 1.0), (_Shininess * 128.0))) * 2.0) * (1.0 - clamp ((sqrt(dot (tmpvar_3, tmpvar_3)) / _SpecRange), 0.0, 1.0)));
  tmpvar_1 = tmpvar_4;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying lowp vec3 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz + (xlv_TEXCOORD2 * tmpvar_2.w));
  c_1.xyz = (c_1.xyz * (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz));
  gl_FragData[0] = c_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
"!!GLES"
}
}
 }
}
}