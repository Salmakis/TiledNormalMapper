using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Utilities.Media;

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

				//now convert this tile
				// convertCanvas = new NormalMap().Create(convertCanvas);
				convertCanvasG.Dispose();
				SwiftBitmap swiftBitmap = new SwiftBitmap(convertCanvas);
				swiftBitmap.Lock();
				swiftBitmap = swiftBitmap.NormalMap();
				Bitmap outbmp = swiftBitmap.InternalBitmap;
				swiftBitmap.Unlock();
				//blit the converted result to the output file
				outputCanvasG.DrawImage(outbmp, new Rectangle(tileX, tileY, tileW, tileH), new Rectangle(mW, mW, tileW, tileH),GraphicsUnit.Pixel);
			}
		}

		// Bitmap output = normalMap.Create(input);
		string ext = Path.GetExtension(args[0]);
		string outputFile = args[0].Replace(ext, $"_normal{ext}");
		output.Save(outputFile);
		Console.WriteLine($"saved to:{outputFile}");
	}
}