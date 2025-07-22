Ó"Shader "Advanced/Blinking_Atmosphere" {
Properties {
 _MainTex ("Base texture", 2D) = "white" {}
 _FadeOutDistNear ("Near fadeout dist", Float) = 10
 _FadeOutDistFar ("Far fadeout dist", Float) = 10000
 _Multiplier ("Color multiplier", Float) = 1
 _Bias ("Bias", Float) = 0
 _TimeOnDuration ("ON duration", Float) = 0.5
 _TimeOffDuration ("OFF duration", Float) = 0.5
 _BlinkingTimeOffsScale ("Blinking time offset scale (seconds)", Float) = 5
 _SizeGrowStartDist ("Size grow start dist", Float) = 5
 _SizeGrowEndDist ("Size grow end dist", Float) = 50
 _MaxGrowSize ("Max grow size", Float) = 2.5
 _NoiseAmount ("Noise amount (when zero, pulse wave is used)", Range(0,0.5)) = 0
 _Color ("Color", Color) = (1,1,1,1)
}
SubShader { 
 LOD 100
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Fog {
   Color (0,0,0,0)
  }
  Blend One One
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

varying lowp vec4 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform highp vec4 _Color;
uniform highp float _NoiseAmount;
uniform highp float _MaxGrowSize;
uniform highp float _SizeGrowEndDist;
uniform highp float _SizeGrowStartDist;
uniform highp float _BlinkingTimeOffsScale;
uniform highp float _TimeOffDuration;
uniform highp float _TimeOnDuration;
uniform highp float _Bias;
uniform highp float _Multiplier;
uniform highp float _FadeOutDistFar;
uniform highp float _FadeOutDistNear;


uniform highp vec4 _Time;
attribute vec4 _glesMultiTexCoord0;
attribute vec3 _glesNormal;
attribute vec4 _glesColor;
attribute vec4 _glesVertex;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec4 mdlPos_2;
  lowp vec4 tmpvar_3;
  highp float tmpvar_4;
  tmpvar_4 = (_Time.y + (_BlinkingTimeOffsScale * _glesColor.z));
  highp vec3 tmpvar_5;
  tmpvar_5 = (gl_ModelViewMatrix * _glesVertex).xyz;
  highp float tmpvar_6;
  tmpvar_6 = sqrt(dot (tmpvar_5, tmpvar_5));
  highp float tmpvar_7;
  tmpvar_7 = clamp ((tmpvar_6 / _FadeOutDistNear), 0.0, 1.0);
  highp float tmpvar_8;
  tmpvar_8 = (1.0 - clamp ((max ((tmpvar_6 - _FadeOutDistFar), 0.0) * 0.2), 0.0, 1.0));
  highp float y_9;
  y_9 = (_TimeOnDuration + _TimeOffDuration);
  highp float tmpvar_10;
  tmpvar_10 = (tmpvar_4 / y_9);
  highp float tmpvar_11;
  tmpvar_11 = (fract(abs(tmpvar_10)) * y_9);
  highp float tmpvar_12;
  if ((tmpvar_10 >= 0.0)) {
    tmpvar_12 = tmpvar_11;
  } else {
    tmpvar_12 = -(tmpvar_11);
  };
  highp float t_13;
  t_13 = max (min ((tmpvar_12 / (_TimeOnDuration * 0.25)), 1.0), 0.0);
  highp float edge0_14;
  edge0_14 = (_TimeOnDuration * 0.75);
  highp float t_15;
  t_15 = max (min (((tmpvar_12 - edge0_14) / (_TimeOnDuration - edge0_14)), 1.0), 0.0);
  highp float tmpvar_16;
  tmpvar_16 = ((t_13 * (t_13 * (3.0 - (2.0 * t_13)))) * (1.0 - (t_15 * (t_15 * (3.0 - (2.0 * t_15))))));
  highp float tmpvar_17;
  tmpvar_17 = (tmpvar_4 * (6.28319 / _TimeOnDuration));
  highp float tmpvar_18;
  tmpvar_18 = ((_NoiseAmount * (sin(tmpvar_17) * ((0.5 * cos(((tmpvar_17 * 0.6366) + 56.7272))) + 0.5))) + (1.0 - _NoiseAmount));
  highp float tmpvar_19;
  tmpvar_19 = min ((max ((tmpvar_6 - _SizeGrowStartDist), 0.0) / _SizeGrowEndDist), 1.0);
  highp float tmpvar_20;
  if ((_NoiseAmount < 0.01)) {
    tmpvar_20 = tmpvar_16;
  } else {
    tmpvar_20 = tmpvar_18;
  };
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_7 * tmpvar_7);
  mdlPos_2.w = _glesVertex.w;
  mdlPos_2.xyz = (_glesVertex.xyz + ((((tmpvar_19 * tmpvar_19) * _MaxGrowSize) * _glesColor.w) * tmpvar_1));
  highp vec4 tmpvar_22;
  tmpvar_22 = (((((tmpvar_21 * tmpvar_21) * (tmpvar_8 * tmpvar_8)) * _Color) * _Multiplier) * (tmpvar_20 + _Bias));
  tmpvar_3 = tmpvar_22;
  gl_Position = (gl_ModelViewProjectionMatrix * mdlPos_2);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_3;
}



#endif
#ifdef FRAGMENT

varying lowp vec4 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform sampler2D _MainTex;
void main ()
{
  gl_FragData[0] = (texture2D (_MainTex, xlv_TEXCOORD0) * xlv_TEXCOORD1);
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