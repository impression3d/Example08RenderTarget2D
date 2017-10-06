using System;
using Impression;

namespace Example08RenderTarget2D
{
    class Program
    {
        static void Main(string[] args)
		{
			using(var game = new Example08RenderTarget2DGame())
            {
                game.Run();
            }
		}
    }
}