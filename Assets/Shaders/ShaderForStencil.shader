Shader "Custom/ShaderForStencil"
{

Properties{}

SubShader{

Tags { 
 "RenderType" = "Opaque" 
 }
 
 Pass{
 ZWrite Off
 }
 }
}