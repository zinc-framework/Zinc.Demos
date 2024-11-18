namespace Zinc.Sandbox.Demos;
using static Zinc.Quick;
using static Zinc.Internal.Sokol.App;
using static Zinc.Internal.Sokol.Fontstash;
using Zinc.Internal.Sokol;

[DemoScene("20 Text")]
public class TextDemo : Scene
{
    struct FontState
    {
        public unsafe void* FONSContext;
    }
    float dpiScale;
    int atlasDim;
    int loadedFont;
    readonly int invalid = -1; 
    FontState state;
    public override void Preload()
    {
        dpiScale = dpi_scale();
        atlasDim = round_pow2(512.0f * dpiScale);
        state = default;
        
        int round_pow2(float v) {
            uint vi = ((uint) v) - 1;
            for (uint i = 0; i < 5; i++) {
                vi |= (vi >> (1<<(int)i));
            }
            return (int) (vi + 1);
        }

        unsafe 
        {
            sfons_desc_t font_desc = default;
            font_desc.width = atlasDim;
            font_desc.height = atlasDim;
            state.FONSContext = create(&font_desc);
            var n = System.Text.Encoding.UTF8.GetBytes("inter-regular");
            Console.WriteLine("Loading font...");
            var fontBytes = System.IO.File.ReadAllBytes($"{AppContext.BaseDirectory}/data/fonts/inter/Inter-Regular.ttf");
            fixed (byte* n_p = n, fontBytes_p = fontBytes)
            {
                loadedFont = fonsAddFontMem(state.FONSContext, (sbyte*)n_p, fontBytes_p, fontBytes.Length, 0);
            }
        }
    }
    public override void Update(double dt)
    {
        unsafe
        {
            fonsClearState(state.FONSContext);
        }

        // text rendering via fontstash.h
        float sx, sy, dx, dy, lh = 0.0f;
        uint white = rgba(255, 255, 255, 255);
        uint black = rgba(0, 0, 0, 255);
        uint brown = rgba(192, 128, 0, 128);
        uint blue  = rgba(0, 192, 255, 255);

        sx = 50*dpiScale; sy = 50*dpiScale;
        dx = sx; dy = sy;



        unsafe
        {
            if (loadedFont != invalid) {
                fonsSetFont(state.FONSContext, loadedFont);
                fonsSetSize(state.FONSContext, 124.0f*dpiScale);
                fonsVertMetrics(state.FONSContext, null, null, &lh);
                dx = sx;
                dy += lh;
                fonsSetColor(state.FONSContext, white);

                var n = System.Text.Encoding.UTF8.GetBytes("hello world");
                fixed (byte* n_p = n)
                {
                    dx = fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
                }

            }

            flush(state.FONSContext);
        }
    }

    public override void Cleanup()
    {
        unsafe
        {
            destroy(state.FONSContext);
        }
    }
}