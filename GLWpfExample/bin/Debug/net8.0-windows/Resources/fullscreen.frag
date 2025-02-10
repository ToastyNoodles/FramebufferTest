#version 460 core
out vec4 FragColor;

in vec3 Position;
in vec2 Texcoord;

layout (binding = 0) uniform sampler2D gPosition;

void main()
{
	FragColor = texture(gPosition, Texcoord);
}