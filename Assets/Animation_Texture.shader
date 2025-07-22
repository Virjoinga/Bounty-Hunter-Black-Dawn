áShader "Advanced/Animation_Texture" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _NumTexTiles ("Num tex tiles", Vector) = (4,4,0,0)
 _ReplaySpeed ("Replay speed - FPS", Float) = 4
 _Randomize ("Randomize", Float) = 0
 _Color ("Color", Color) = (1,1,1,1)
 _Fade ("Fade", Float) = 1
}
SubShader { 
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

varying lowp vec4 xlv_COLOR;
varying highp vec4 xlv_TEXCOORD0;
uniform highp float _ReplaySpeed;
uniform highp vec4 _NumTexTiles;
uniform highp vec4 _Color;

uniform highp vec4 _Time;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesColor;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tile_1;
  lowp vec4 tmpvar_2;
  highp float tmpvar_3;
  tmpvar_3 = (((_glesColor.w * 60.0) + _Time.y) * _ReplaySpeed);
  highp float tmpvar_4;
  tmpvar_4 = floor(tmpvar_3);
  highp float tmpvar_5;
  tmpvar_5 = (tmpvar_4 + 1.0);
  highp float tmpvar_6;
  tmpvar_6 = (tmpvar_3 - tmpvar_4);
  highp vec2 tmpvar_7;
  tmpvar_7 = (1.0/(_NumTexTiles.xy));
  highp vec2 tmpvar_8;
  tmpvar_8.x = tmpvar_4;
  tmpvar_8.y = floor((tmpvar_4 / _NumTexTiles.x));
  tile_1.xy = tmpvar_8;
  highp vec2 tmpvar_9;
  tmpvar_9.x = tmpvar_5;
  tmpvar_9.y = floor((tmpvar_5 / _NumTexTiles.x));
  tile_1.zw = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (tile_1 / _NumTexTiles.xyxy);
  highp vec4 tmpvar_11;
  tmpvar_11 = (fract(abs(tmpvar_10)) * _NumTexTiles.xyxy);
  highp float tmpvar_12;
  if ((tmpvar_10.x >= 0.0)) {
    tmpvar_12 = tmpvar_11.x;
  } else {
    tmpvar_12 = -(tmpvar_11.x);
  };
  highp float tmpvar_13;
  if ((tmpvar_10.y >= 0.0)) {
    tmpvar_13 = tmpvar_11.y;
  } else {
    tmpvar_13 = -(tmpvar_11.y);
  };
  highp float tmpvar_14;
  if ((tmpvar_10.z >= 0.0)) {
    tmpvar_14 = tmpvar_11.z;
  } else {
    tmpvar_14 = -(tmpvar_11.z);
  };
  highp float tmpvar_15;
  if ((tmpvar_10.w >= 0.0)) {
    tmpvar_15 = tmpvar_11.w;
  } else {
    tmpvar_15 = -(tmpvar_11.w);
  };
  highp vec4 tmpvar_16;
  tmpvar_16.x = tmpvar_12;
  tmpvar_16.y = tmpvar_13;
  tmpvar_16.z = tmpvar_14;
  tmpvar_16.w = tmpvar_15;
  tile_1 = tmpvar_16;
  highp vec4 tmpvar_17;
  tmpvar_17.xyz = (_Color.xyz * _glesColor.xyz);
  tmpvar_17.w = tmpvar_6;
  tmpvar_2 = tmpvar_17;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xyxy + tmpvar_16) * tmpvar_7.xyxy);
  xlv_COLOR = tmpvar_2;
}



#endif
#ifdef FRAGMENT

varying lowp vec4 xlv_COLOR;
varying highp vec4 xlv_TEXCOORD0;
uniform highp float _Fade;
uniform sampler2D _MainTex;
void main ()
{
  lowp vec4 tmpvar_1;
  if ((_Fade > 0.0)) {
    tmpvar_1 = (mix (texture2D (_MainTex, xlv_TEXCOORD0.xy), texture2D (_MainTex, xlv_TEXCOORD0.zw), xlv_COLOR.wwww) * xlv_COLOR);
  } else {
    tmpvar_1 = (texture2D (_MainTex, xlv_TEXCOORD0.xy) * xlv_COLOR);
  };
  gl_FragData[0] = tmpvar_1;
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