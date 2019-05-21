using SDL2;
using System;
using System.Threading;

namespace SDLNetCoreTextureSample
{
    public class Program
    {
        private const int WINDOW_WIDTH = 640;
        private const int WINDOW_HEIGHT = 480;
        private static bool _quit = false;
        private static IntPtr _windowPtr;
        private static IntPtr _rendererPtr;
        private static IntPtr _texturePtr;
        private static int _textureWidth;
        private static int _textureHeight;
        private static int _textureX = 270;
        private static int _textureY = 190;


        static void Main(string[] args)
        {
            //Initialize SDL.  A true value means initialization was successful.
            var isInitialized = Init();

            _texturePtr = LoadTexture("OrangeBox");

            SDL.SDL_QueryTexture(_texturePtr, out uint format, out int access, out _textureWidth, out _textureHeight);

            Console.WriteLine("Press one of the arrow keys!");
            Console.WriteLine();

            //If successfully initialized.
            if (isInitialized)
            {
                //This can represent your game loop
                while (!_quit)
                {
                    //Clear the screen
                    SDL.SDL_RenderClear(_rendererPtr);

                    var srcRect = new SDL.SDL_Rect()
                    {
                        x = 0,
                        y = 0,
                        w = _textureWidth,
                        h = _textureHeight
                    };

                    var destRect = new SDL.SDL_Rect()
                    {
                        x = _textureX,
                        y = _textureY,
                        w = _textureWidth,
                        h = _textureHeight
                    };

                    //Render texture to screen
                    SDL.SDL_RenderCopy(_rendererPtr, _texturePtr, ref srcRect, ref destRect);

                    //Update screen
                    SDL.SDL_RenderPresent(_rendererPtr);
                }
            }
            else
            {
                Console.WriteLine("Failed to initialize!");
                Console.ReadLine();
            }
        }


        private static bool Init()
        {
            //Init the video card
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Console.WriteLine("SDL could not initialize! SDL_Error: {0}", SDL.SDL_GetError());
                return false;
            }
            else
            {
                //Create an SDL window to render graphics upon.
                _windowPtr = SDL.SDL_CreateWindow("SDL2 Keyboard Sample", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, WINDOW_WIDTH, WINDOW_HEIGHT, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

                _rendererPtr = SDL.SDL_CreateRenderer(_windowPtr, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

                if (_windowPtr == IntPtr.Zero)
                {
                    Console.WriteLine("The window could not be created!!");
                    Console.ReadLine();
                    return false;
                }
            }


            return true;
        }


        private static IntPtr LoadTexture(string textureName)
        {
            var texturePath = $@"Content\{textureName}.png";

            //The final optimized image
            var newTexture = IntPtr.Zero;

            //Load image at specified path
            var loadedSurface = SDL_image.IMG_Load(texturePath);

            if (loadedSurface == IntPtr.Zero)
            {
                Console.WriteLine("Unable to load image {0}! SDL Error: {1}", texturePath, SDL.SDL_GetError());
            }
            else
            {
                //Create texture from surface pixels
                newTexture = SDL.SDL_CreateTextureFromSurface(_rendererPtr, loadedSurface);

                if (newTexture == IntPtr.Zero)
                    Console.WriteLine("Unable to create texture from {0}! SDL Error: {1}", texturePath, SDL.SDL_GetError());

                //Get rid of old loaded surface
                SDL.SDL_FreeSurface(loadedSurface);
            }


            return newTexture;
        }
    }
}
