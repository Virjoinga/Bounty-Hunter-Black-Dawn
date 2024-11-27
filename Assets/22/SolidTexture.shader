¯Shader "iPhone/SolidTexture" {
Properties {
 _MainTex ("Texture", 2D) = "" {}
}
SubShader { 
 Pass {
Program "vp" {
SubProgram "gles " {
"!!GLES

#define SHADER_API_GLES 1
#define tex2D texture2D
#line 8

	varying mediump vec2 uv;
	
		
		
	
#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;
#define gl_Vertex _glesVertex
attribute vec4 _glesVertex;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
attribute vec4 _glesMultiTexCoord0;

 void main() {
  gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
  uv = gl_MultiTexCoord0.xy;
 }
 
#endif
#ifdef FRAGMENT

 uniform lowp sampler2D _MainTex;
 void main() {
  gl_FragColor = texture2D(_MainTex, uv);
 }
 
#endif"
}
}
 }
}
SubShader { 
 Pass {
  SetTexture [_MainTex] { combine texture, texture alpha }
 }
}
}