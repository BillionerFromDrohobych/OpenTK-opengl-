using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Linq;
namespace project
{
	class Program
	{
		public static void Main(string[] args)
		{
			GameWindow window = new GameWindow(1000,1000);
			Game gm = new Game(window);
		}
		
		
	}
	class Game{
		Vector3 position = new Vector3(0.0f, 0.0f,  3.0f);
		double horizontalAnglePosition =90;
		double verticalAnglePosition =90;
		Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
		Vector3 up = new Vector3(0.0f, 1.0f,  0.0f);
		const float speed = 1.5f;
		Matrix4 view;


		GameWindow window;


		private bool _firstMove = true;


		

		private void keyPress(object sender, KeyPressEventArgs e)
		{
			Console.WriteLine(e.KeyChar);
			if(e.KeyChar.ToString()=="+"){
				position += up * speed;
			}if(e.KeyChar.ToString()=="-"){
				position -= up * speed;
			} if(e.KeyChar.ToString()=="a"){
				position -= Vector3.Normalize(Vector3.Cross(front, up)) * speed;
			} if(e.KeyChar.ToString()=="d"){
				position += Vector3.Normalize(Vector3.Cross(front, up)) * speed;
			} if(e.KeyChar.ToString()=="w"){
				position += front * speed;
			} if(e.KeyChar.ToString()=="s"){
				position -= front * speed;
			}if(e.KeyChar.ToString()=="x"){
				verticalAnglePosition+=5;
				front.Y = (float)Math.Cos(MathHelper.DegreesToRadians(verticalAnglePosition));
				front.Z = -(float)Math.Sin(MathHelper.DegreesToRadians(verticalAnglePosition));

			}if(e.KeyChar.ToString()=="c"){
				horizontalAnglePosition+=5;
				front.X = (float)Math.Cos(MathHelper.DegreesToRadians(horizontalAnglePosition));
				front.Z = -(float)Math.Sin(MathHelper.DegreesToRadians(horizontalAnglePosition));
			} if(e.KeyChar.ToString()=="p"){
				GL.MatrixMode(MatrixMode.Projection);
				GL.LoadIdentity();


				GL.Ortho(-100.0f,100.0f,-100.0f,100.0f,-5.0f,100.0f);

				GL.MatrixMode(MatrixMode.Modelview);
			} if(e.KeyChar.ToString()=="o"){
				GL.MatrixMode(MatrixMode.Projection);
				GL.LoadIdentity();
				GL.Frustum(-1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 100.0f);

				GL.MatrixMode(MatrixMode.Modelview);

			}
			glLookAt();
		}
		public Game(GameWindow window){
			this.window = window;
			Start();
		}
		
		private void Start(){

			window.Load += loaded;

			window.Resize+= resize;
			window.KeyPress += keyPress;
			window.RenderFrame +=renderF;
			window.Run(1.0,60.0);
		}
		private void loaded(object o,EventArgs e){
			GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
		}
		private void resize(object o,EventArgs e){
			GL.Viewport(0,0,window.Width,window.Height);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			//GL.Ortho(-160.0,160.0,-160.0,160.0,-1.0,1.0);
			Matrix4 matrix = Matrix4.Perspective(45.0f,window.Width/window.Height,1.0f,100.0f);
			GL.LoadMatrix(ref matrix);
			GL.MatrixMode(MatrixMode.Modelview);
		}
		private Matrix4 glLookAt(){
			return Matrix4.LookAt(position,position+front,up);
		}
		
		
		private void renderF(object o,EventArgs e){
			GL.LoadIdentity();
			GL.Clear(ClearBufferMask.ColorBufferBit|ClearBufferMask.DepthBufferBit);
			view = glLookAt();
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref view);

			GL.Translate(0.0,0.0,-45.0);
			GL.Translate(-15.0,0.0,0.0);
			Tor(8, 25);
			GL.Translate(15.0,0.0,0.0);
			Ploschina();
			GL.Translate(15.0f, 2.0f, 0.0f);
			
			DrawPyramid(2.0f);

			GL.Translate(0.0f, -4.0f, 0.0f);
			DrawPyramid(-2.0f);
			
			
			window.SwapBuffers();
		}
		private void Tor(int numc, int numt){
			int i, j, k;
			double s, t, x, y, z, twopi;
			twopi = 2 * Math.PI;
			for (i = 0; i < numc; i++) {
				GL.Begin(BeginMode.QuadStrip);
				GL.Color3(1.0f,0.0f,0.0f);
				for (j = 0; j <= numt; j++) {
					for (k = 1; k >= 0; k--) {
						s = (i + k) % numc + 0.5;
						t = j % numt;

						x = (1+.1*Math.Cos(s*twopi/numc))*Math.Cos(t*twopi/numt);
						y = (1+.1*Math.Cos(s*twopi/numc))*Math.Sin(t*twopi/numt);
						z = .1 * Math.Sin(s * twopi / numc);
						GL.Vertex3(5*x, 5*y, 5*z);
					}
				}
				GL.End();
			}
		}
		
		private void Ploschina(){
			
			double z;
			for (float x = 0; x <= 1; x+=0.01f) {
				GL.Begin(BeginMode.Points);
				GL.Color3(0.0f,0.0f,1.0f);
				for (float y = 0; y <= 1; y+=0.01f) {
					z =  Math.Sin(x)+Math.Cos(y);
					GL.Vertex3(x*1.0f, y*1.0f, z*1.0f);
				}
				GL.End();
			}
		}

		private void DrawPyramid(float coef)
		{
			GL.Begin(PrimitiveType.Triangles);
			GL.Color3(0.0f,1.0f,0.0f);
			GL.Vertex3(coef*0.0f, coef*1.0f, coef*0.0f);

			GL.Vertex3(coef*-1.0f,coef* -1.0f, coef*1.0f);

			GL.Vertex3(coef*1.0f, coef*-1.0f, coef*1.0f);


			GL.Vertex3(coef*0.0f,coef* 1.0f,coef* 0.0f);

			GL.Vertex3(coef*1.0f, coef*-1.0f,coef* 1.0f);
			GL.Vertex3(coef*1.0f, coef*-1.0f, coef*-1.0f);

			GL.Vertex3(coef*0.0f, coef*1.0f,coef* 0.0f);

			GL.Vertex3(coef*1.0f, coef*-1.0f, coef*-1.0f);

			GL.Vertex3(coef*-1.0f, coef*-1.0f, coef*-1.0f);


			GL.Vertex3(coef*0.0f, coef*1.0f, coef*0.0f);

			GL.Vertex3(coef*-1.0f, coef*-1.0f, coef*-1.0f);

			GL.Vertex3(coef*-1.0f, coef*-1.0f, coef*1.0f);

			GL.Vertex3(coef*0.0f,coef* 1.0f, coef*0.0f);

			GL.Vertex3(coef*-1.0f, coef*-1.0f, coef*1.0f);

			GL.Vertex3(coef*1.0f,coef* -1.0f, coef*1.0f);

			GL.End();
		}

	}
}
