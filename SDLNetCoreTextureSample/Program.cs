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


        static void Main(string[] args)
        {
            //Initialize SDL.  A true value means initialization was successful.
            var isInitialized = Init();

            Console.WriteLine("Press one of the arrow keys!");
            Console.WriteLine();

            //If successfully initialized.
            if (isInitialized)
            {
                //This can represent your game loop
                while (!_quit)
                {
                    Thread.Sleep(1000);

                    Console.WriteLine("Just Waiting. . . ");
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

                if (_windowPtr == IntPtr.Zero)
                {
                    Console.WriteLine("The window could not be created!!");
                    Console.ReadLine();
                    return false;
                }
            }


            return true;
        }
    }
}
