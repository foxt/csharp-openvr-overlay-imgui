using System;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace DearOVRlay;

public class OpenGLRenderer {

    public static void InitOffscreen(Action<GL> callback, string title = "Dummy window for OpenGL") {
        // it seems like we need to create a window (even though we never interact with it after creating the GL context)
        // so just create a hidden window and 'run' it (we will never let it pump any event loop but still)
        WindowOptions options = WindowOptions.Default;
        options.Title = title;
        options.Size = new Vector2D<int>(1, 1);
        options.IsVisible = false;
        var window = Window.Create(options);
        window.Load += () => callback(window.CreateOpenGL());
        window.Run();
    }
    public static void InitWindowed(Action<IWindow> callback, Vector2D<int> size, string title = "OpenGL Display") {
        // it seems like we need to create a window (even though we never interact with it after creating the GL context)
        // so just create a hidden window and 'run' it (we will never let it pump any event loop but still)
        WindowOptions options = WindowOptions.Default;
        options.Title = title;
        options.Size = size;
        var window = Window.Create(options);
        window.Load += () => callback(window);
        window.Run();
    }

    public GL _gl;
    private uint _fbo;
    private uint _rbo;
    public uint texture;
    private Vector2D<int> _size;
    private bool _renderToWindow;

    public OpenGLRenderer(GL gl, Vector2D<int> size) {
        _gl = gl;
        _size = size;
        _renderToWindow = false;
        InitGL();
    }
    public OpenGLRenderer(IWindow window) {
        _gl = window.CreateOpenGL();
        _size = window.FramebufferSize;
        window.FramebufferResize += size => this.Size = size;
        _renderToWindow = true;
        InitGL();
    }

    private void InitGL() {
                
        // init the texture we'll pass to OVR
        texture = _gl.GenTexture();
        _gl.ActiveTexture(TextureUnit.Texture0);
        _gl.BindTexture(TextureTarget.Texture2D, texture);

        // init framebuffer we'll render imgui to
        _fbo = _gl.GenFramebuffer();
        _gl.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
        _gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, texture, 0);

        // init renderbuffer that the framebuffer will store the actual pixel data
        _rbo = _gl.GenRenderbuffer();
        _gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _rbo);
        InitFrameBuffer();

        // make sure everythings a-ok
        if (_gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != GLEnum.FramebufferComplete)
            throw new Exception("framebuffer incomplete!");

        // tidy up the context
        _gl.BindFramebuffer(GLEnum.Framebuffer, 0);
        _gl.BindTexture(TextureTarget.Texture2D, 0);
        _gl.BindRenderbuffer(GLEnum.Renderbuffer, 0);
    }
    
    public Vector2D<int> Size {
        get => _size;
        set {
            _size = value;
            _gl.BindTexture(TextureTarget.Texture2D, texture);
            _gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _rbo);
            InitFrameBuffer();
            _gl.BindTexture(TextureTarget.Texture2D, 0);
            _gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        }
    }

    public uint Width => (uint)Size.X;
    public uint Height => (uint)Size.Y;

    private unsafe void InitFrameBuffer() {
        _gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);
        _gl.TexParameterI(TextureTarget.Texture2D, GLEnum.TextureMinFilter, (int)TextureMinFilter.Linear);
        _gl.TexParameterI(TextureTarget.Texture2D, GLEnum.TextureMagFilter, (int)TextureMagFilter.Linear);
        
        _gl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, GLEnum.Depth24Stencil8, Width, Height);
        _gl.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, GLEnum.Renderbuffer, _rbo);

        
    }

    public void Render(Action render) {
        _gl.Viewport(0, 0, Width, Height);
        _gl.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
        _gl.Clear(ClearBufferMask.ColorBufferBit);

        render();
        
        _gl.BindFramebuffer(GLEnum.Framebuffer, 0);

        if (_renderToWindow)
            _gl.BlitNamedFramebuffer(
                _fbo, 0,
                0, 0, (int)Width, (int)Height,
                0, 0, (int)Width, (int)Height,
                ClearBufferMask.ColorBufferBit,
                BlitFramebufferFilter.Nearest
            );
        
    }

}