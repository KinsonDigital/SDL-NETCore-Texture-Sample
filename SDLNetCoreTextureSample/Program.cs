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
        private static IntPtr _orangeTexturePtr;
        private static IntPtr _blueTexturePtr;
        private static int _textureWidth;
        private static int _textureHeight;
        private static int _textureX = 270;
        private static int _textureY = 190;


        static void Main(string[] args)
        {
            //Initialize SDL.  A true value means initialization was successful.
            var isInitialized = Init();

            _orangeTexturePtr = LoadTexture("OrangeBox");
            _blueTexturePtr = LoadTexture("BlueBox");

            SDL.SDL_QueryTexture(_orangeTexturePtr, out uint format, out int access, out _textureWidth, out _textureHeight);

            SDL.SDL_SetTextureColorMod(_orangeTexturePtr, 255, 255, 255);
            SDL.SDL_SetTextureAlphaMod(_orangeTexturePtr, 32);
            SDL.SDL_SetTextureBlendMode(_orangeTexturePtr, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

            SDL.SDL_SetTextureColorMod(_blueTexturePtr, 255, 255, 255);
            SDL.SDL_SetTextureAlphaMod(_blueTexturePtr, 32);
            SDL.SDL_SetTextureBlendMode(_blueTexturePtr, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

            Console.WriteLine("Press one of the arrow keys!");
            Console.WriteLine();

            //If successfully initialized.
            if (isInitialized)
            {
                //This can represent your game loop
                while (!_quit)
                {
                    ProcessWindowEvents();

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
                    SDL.SDL_RenderCopy(_rendererPtr, _orangeTexturePtr, ref srcRect, ref destRect);

                    destRect.x = _textureX + 60;

                    SDL.SDL_RenderCopy(_rendererPtr, _blueTexturePtr, ref srcRect, ref destRect);

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
                else
                {
                    //Initialize PNG loading
                    var imgFlags = SDL_image.IMG_InitFlags.IMG_INIT_PNG;
                    if ((SDL_image.IMG_Init(imgFlags) > 0 & imgFlags > 0) == false)
                    {
                        //TODO: Convert to exception
                        Console.WriteLine("SDL_image could not initialize! SDL_image Error: {0}", SDL.SDL_GetError());
                        return false;
                    }
                }
            }


            return true;
        }


        private static void ProcessWindowEvents()
        {
            while (SDL.SDL_PollEvent(out var e) != 0)
            {
                if (e.type == SDL.SDL_EventType.SDL_QUIT)
                {
                    _quit = true;
                }
            }
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
