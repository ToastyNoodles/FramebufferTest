#version 460 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexcoord;

out vec3 Position;
out vec2 Texcoord;

void main()
{
	Position = aPosition;
	Texcoord = aTexcoord;
	gl_Position = vec4(aPosition, 1.0);
}