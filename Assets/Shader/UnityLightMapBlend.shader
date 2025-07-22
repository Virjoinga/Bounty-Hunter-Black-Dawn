ÆShader "iPhone/UnityLightMapBlend" {
Properties {
 _MainTex ("Texture1 (RGB)", 2D) = "white" {}
 _Tex2 ("Texture2 (RGB)", 2D) = "white" {}
 _BlendMap ("Blend Mask", 2D) = "white" {}
}
SubShader { 
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "RenderType"="Opaque" }
Program "vp" {
SubProgram "gles " {
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying highp vec2 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_LightmapST;

attribute vec4 _glesMultiTexCoord1;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesColor;
attribute vec4 _glesVertex;
void main ()
{
  lowp vec4 tmpvar_1;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_COLOR = tmpvar_1;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = _glesColor.xy;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

varying highp vec2 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform sampler2D unity_Lightmap;
uniform sampler2D _BlendMap;
uniform sampler2D _Tex2;
uniform sampler2D _MainTex;
void main ()
{
  lowp vec4 col_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_BlendMap, xlv_TEXCOORD1);
  col_1.xyz = ((tmpvar_2.xyz * tmpvar_3.w) + (texture2D (_Tex2, xlv_TEXCOORD0).xyz * (1.0 - tmpvar_3.w)));
  col_1.xyz = (col_1.xyz * (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD2).xyz));
  col_1.w = tmpvar_2.w;
  gl_FragData[0] = col_1;
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