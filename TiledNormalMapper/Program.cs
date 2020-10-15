using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

class Program
{
	const int mW = 2; //widh of the mirrior edge;

	/**
	 * convert a greyscale heighmap of a chipset/tileset to a normal map
	 * usefull to not have seams and edges between the tiles on the normal map
	 */
	static void Main(string[] args)
	{
		int tileH, tileW;
		if (args.Length < 3 || !int.TryParse(args[1], out tileH) || !int.TryParse(args[2], out tileW))
		{
			Console.WriteLine("usage: <file> <tileWidth> <tileHeight>");
			return;
		}

		if (!File.Exists(args[0]))
		{
			Console.WriteLine($"no such file: {args[0]}");
		}

		//load stuff and prepare output
		Bitmap input = (Bitmap) Bitmap.FromFile(args[0]);
		
		//the image where tie result came on
		Bitmap output = new Bitmap(input.Height, input.Height);
		Graphics outputCanvasG = Graphics.FromImage(output);
		outputCanvasG.InterpolationMode = InterpolationMode.NearestNeighbor;
		
		for (int tileX = 0; tileX < input.Width; tileX += tileW)
		{
			for (int tileY = 0; tileY < input.Height; tileY += tileH)
			{
				//this image will be used to blit the current worked tile on and mirror the edges
				Bitmap convertCanvas = new Bitmap(tileW + mW * 2, tileH + mW * 2);
				Graphics convertCanvasG = Graphics.FromImage(convertCanvas);
				convertCanvasG.InterpolationMode = InterpolationMode.NearestNeighbor;
				Console.WriteLine($"processing tile that starts at: {tileX},{tileY}");
				convertCanvasG.Clear(Color.FromArgb(255, Color.Black));

				//draw the image area from the input image where the tile is at the 4 sides
				//use source reactange to really only get 1 pixel width (otherwise we have a seam from the next pixel)
				// RectangleF sourceRectangle = new RectangleF(tileX, tileY, 0.99f, 0.99f);
				convertCanvasG.DrawImage(input, new Rectangle(mW, mW, tileW, tileH), tileX, tileY, tileW, tileH,
					GraphicsUnit.Pixel);
				//draw the onto the edges, the 0.1 for the width and height is to prevent seams
				//left and right sides
				convertCanvasG.DrawImage(input, new Rectangle(0, mW, mW, tileH), tileX, tileY, 0.1f, tileH,
					GraphicsUnit.Pixel);
				convertCanvasG.DrawImage(input, new Rectangle(tileW + mW, mW, mW, tileH), tileX + tileW - 1, tileY, 0.1f, tileH,
					GraphicsUnit.Pixel);

				//top & bot
				convertCanvasG.DrawImage(input, new Rectangle(mW, 0, tileW, mW), tileX, tileY, tileW, 0.1f,
					GraphicsUnit.Pixel);
				convertCanvasG.DrawImage(input, new Rectangle(mW, tileH + mW, tileW, mW), tileX, tileY + tileH - 1, tileW, 0.1f,
					GraphicsUnit.Pixel);

				//top left corner
				convertCanvasG.DrawImage(input, new Rectangle(0, 0, mW, mW), tileX, tileY, 0.1f, 0.1f,
					GraphicsUnit.Pixel);
				//top right
				convertCanvasG.DrawImage(input, new Rectangle(tileW + mW, 0, mW, mW), tileX + tileW - 1, tileY, 0.1f, 0.1f,
					GraphicsUnit.Pixel);
				//bot left 
				convertCanvasG.DrawImage(input, new Rectangle(0, tileH + mW, mW, mW), tileX, tileY + tileH - 1, 0.1f, 0.1f,
					GraphicsUnit.Pixel);
				//bot right
				convertCanvasG.DrawImage(input, new Rectangle(tileW + mW, tileH + mW, mW, mW), tileX + tileW - 1,
					tileY + tileH - 1, 0.1f, 0.1f,
					GraphicsUnit.Pixel);

				convertCanvasG.Dispose();
				Bitmap outbmp = CraeteNormalMap(convertCanvas);// swiftBitmap.InternalBitmap;

				outputCanvasG.DrawImage(outbmp, new Rectangle(tileX, tileY, tileW, tileH), new Rectangle(mW, mW, tileW, tileH),GraphicsUnit.Pixel);
			}
		}

		// Bitmap output = normalMap.Create(input);
		string ext = Path.GetExtension(args[0]);
		string outputFile = args[0].Replace(ext, $"_normal{ext}");
		output.Save(outputFile);
		Console.WriteLine($"saved to:{outputFile}");
	}
	
	/**
	 * translated from unity stuff to normal bitmap from:
	 * https://gamedev.stackexchange.com/questions/106703/create-a-normal-map-using-a-script-unity
	 */
	
	private static Bitmap CraeteNormalMap(Bitmap source,float strength = 1f) 
	{
		strength=Math.Clamp(strength,0.0F,1.0F);

		Bitmap normalTexture;
		float xLeft;
		float xRight;
		float yUp;
		float yDown;
		float yDelta;
		float xDelta;

		normalTexture = new Bitmap (source.Width, source.Height);

		for (int y=1; y+1<normalTexture.Height; y++) 
		{
			for (int x=1; x+1<normalTexture.Width; x++) 
			{
				xLeft = (source.GetPixel(x-1,y).R/255f)*strength;
				xRight = (source.GetPixel(x+1,y).R/255f)*strength;
				yUp = (source.GetPixel(x,y-1).R/255f)*strength;
				yDown = (source.GetPixel(x,y+1).R/255f)*strength;
				xDelta = ((xLeft-xRight)+1)*0.5f;
				yDelta = ((yDown-yUp)+1)*0.5f;
				normalTexture.SetPixel(x,y,Color.FromArgb(255,  (int)(xDelta * 255f),(int)(yDelta * 255), 255));
			}
		}
		return normalTexture;
	}
}