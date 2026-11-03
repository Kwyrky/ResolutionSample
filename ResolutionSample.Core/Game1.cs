using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ResolutionSample.Core;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch = null!;
    private Texture2D testTexture = null!;

    private KeyboardState previousKeyboardState;

    private const int Width1080p = 1920;
    private const int Height1080p = 1080;

    private const int Width800 = 800;
    private const int Height600 = 600;

    private bool is1080p = true;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = Width1080p,
            PreferredBackBufferHeight = Height1080p,
            IsFullScreen = false,
            SynchronizeWithVerticalRetrace = true
        };

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Window.Title = "MonoGame 3.8.6 DesktopVK Test";
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        testTexture = CreateTestTexture(256, 256);
    }

    private Texture2D CreateTestTexture(int Width, int Height)
    {
        Texture2D texture = new(
            GraphicsDevice,
            Width,
            Height
        );

        Color[] pixels = new Color[Width * Height];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int index = y * Width + x;

                bool checker = ((x / 32) + (y / 32)) % 2 == 0;

                if (x < Width / 2 && y < Height / 2)
                    pixels[index] = checker ? Color.Red : Color.DarkRed;
                else if (x >= Width / 2 && y < Height / 2)
                    pixels[index] = checker ? Color.Lime : Color.DarkGreen;
                else if (x < Width / 2)
                    pixels[index] = checker ? Color.Blue : Color.DarkBlue;
                else
                    pixels[index] = checker ? Color.Yellow : Color.Orange;
            }
        }

        texture.SetData(pixels);

        return texture;
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Escape))
            Exit();

        if (keyboardState.IsKeyDown(Keys.Space) &&
            previousKeyboardState.IsKeyUp(Keys.Space))
        {
            ToggleResolution();
        }

        previousKeyboardState = keyboardState;

        base.Update(gameTime);
    }

    private void ToggleResolution()
    {
        is1080p = !is1080p;

        graphics.PreferredBackBufferWidth =
            is1080p ? Width1080p : Width800;

        graphics.PreferredBackBufferHeight =
            is1080p ? Height1080p : Height600;

        graphics.ApplyChanges();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        Viewport viewport = GraphicsDevice.Viewport;

        Rectangle destination = new(
            (viewport.Width - testTexture.Width) / 2,
            (viewport.Height - testTexture.Height) / 2,
            testTexture.Width,
            testTexture.Height
        );

        spriteBatch.Begin();

        spriteBatch.Draw(
            testTexture,
            destination,
            Color.White
        );

        spriteBatch.End();

        base.Draw(gameTime);
    }

    protected override void UnloadContent()
    {
        testTexture?.Dispose();
        spriteBatch?.Dispose();

        base.UnloadContent();
    }
}