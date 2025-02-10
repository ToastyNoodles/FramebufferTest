#version 420 core
layout (location = 0) out vec4 gPosition;
layout (location = 1) out vec4 gNormal;
layout (location = 2) out vec4 gAlbedo;

in vec3 FragPos;
in vec3 Normal;
in vec2 Texcoord;

layout (binding = 0) uniform sampler2D albedoTexture;

void main()
{
	gPosition = vec4(FragPos, 1.0);
	gNormal = vec4(normalize(Normal), 1.0);
	gAlbedo = vec4(1.0);
}