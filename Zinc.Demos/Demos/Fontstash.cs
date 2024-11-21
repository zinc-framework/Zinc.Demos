namespace Zinc.Sandbox.Demos;
using static Zinc.Quick;
using static Zinc.Internal.Sokol.App;
using static Zinc.Internal.Sokol.Fontstash;
using Zinc.Internal.Sokol;

[DemoScene("20 Fontstash")]
public class FontstashDemo : Scene
{
    struct FontState
    {
        public unsafe void* FONSContext;
    }
    float dpiScale;
    int atlasDim;
    
    int normalFont;
    int boldFont;
    int italicFont;
    int japaneseFont;

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
            Console.WriteLine("loading fonts...");

            
            byte[] fontNameBytes = System.Text.Encoding.UTF8.GetBytes("droid-regular");
            byte[] fontDataBytes = System.IO.File.ReadAllBytes($"{AppContext.BaseDirectory}/data/fonts/droidserif/DroidSerif-Regular.ttf");
            fixed (byte* n_p = fontNameBytes, fontBytes_p = fontDataBytes)
            {
                normalFont = fonsAddFontMem(state.FONSContext, (sbyte*)n_p, fontBytes_p, fontDataBytes.Length, 0);
            }

            fontNameBytes = System.Text.Encoding.UTF8.GetBytes("droid-italic");
            fontDataBytes = System.IO.File.ReadAllBytes($"{AppContext.BaseDirectory}/data/fonts/droidserif/DroidSerif-Italic.ttf");
            fixed (byte* n_p = fontNameBytes, fontBytes_p = fontDataBytes)
            {
                italicFont = fonsAddFontMem(state.FONSContext, (sbyte*)n_p, fontBytes_p, fontDataBytes.Length, 0);
            }

            fontNameBytes = System.Text.Encoding.UTF8.GetBytes("droid-bold");
            fontDataBytes = System.IO.File.ReadAllBytes($"{AppContext.BaseDirectory}/data/fonts/droidserif/DroidSerif-Bold.ttf");
            fixed (byte* n_p = fontNameBytes, fontBytes_p = fontDataBytes)
            {
                boldFont = fonsAddFontMem(state.FONSContext, (sbyte*)n_p, fontBytes_p, fontDataBytes.Length, 0);
            }

            fontNameBytes = System.Text.Encoding.UTF8.GetBytes("droid-japanese");
            fontDataBytes = System.IO.File.ReadAllBytes($"{AppContext.BaseDirectory}/data/fonts/droidserif/DroidSansJapanese.ttf");
            fixed (byte* n_p = fontNameBytes, fontBytes_p = fontDataBytes)
            {
                japaneseFont = fonsAddFontMem(state.FONSContext, (sbyte*)n_p, fontBytes_p, fontDataBytes.Length, 0);
            }
        }
    }

    Text testText;
    public override void Create()
    {
        testText = new Text("hello loaded text",$"{AppContext.BaseDirectory}/data/fonts/droidserif/DroidSerif-Regular.ttf");
        // var asd = new Text("hello other",$"{AppContext.BaseDirectory}/data/fonts/droidserif/DroidSerif-Italic.ttf"){
        //     X = 100,
        //     Y = 100,
        // };
    }

    public override void Update(double dt)
    {
        //rotate testText:
        testText.Rotation += .01f;
        // testText.X += 1;
    }

    // string displayText = "Hello World";
    // float sizeScale = 1.0f;
    // public override void Update(double dt)
    // {
    //     unsafe
    //     {
    //         fonsClearState(state.FONSContext);
    //     }

    //     // text rendering via fontstash.h
    //     float sx, sy, dx, dy, lh = 0.0f;
    //     uint white = rgba(255, 255, 255, 255);
    //     uint black = rgba(0, 0, 0, 255);
    //     uint brown = rgba(192, 128, 0, 128);
    //     uint blue  = rgba(0, 192, 255, 255);


    //     Core.ImGUI.Window("display text", () =>
    //     {
    //         Core.ImGUI.TextInput("display text", ref displayText);
    //     });

    //     sx = 50*dpiScale; sy = 50*dpiScale;
    //     dx = sx; dy = sy;
    //     unsafe
    //     {
    //         //input demo
    //         if (normalFont != invalid) {
    //             fonsSetFont(state.FONSContext, normalFont);
    //             fonsSetSize(state.FONSContext, 124.0f*dpiScale);
    //             fonsVertMetrics(state.FONSContext, null, null, &lh);
    //             fonsSetColor(state.FONSContext, white);

    //             if(!string.IsNullOrEmpty(displayText))
    //             {
    //                 var n = System.Text.Encoding.UTF8.GetBytes(displayText);
    //                 fixed (byte* n_p = n)
    //                 {
    //                     // dx = fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //                     dx = fonsDrawText(state.FONSContext, sx, 600, (sbyte*)n_p, null);
    //                 }
    //             }

    //         }

    //         // sokol demos
    //         if (normalFont != invalid) {
    //             fonsSetFont(state.FONSContext, normalFont);
    //             fonsSetSize(state.FONSContext, 124.0f*dpiScale);
    //             fonsVertMetrics(state.FONSContext, null, null, &lh);
    //             dx = sx;
    //             dy += lh;
    //             fonsSetColor(state.FONSContext, white);
    //             var n = System.Text.Encoding.UTF8.GetBytes("The quick ");
    //             fixed (byte* n_p = n)
    //             {
    //                 dx = fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //             }
    //         }
    //         if (italicFont != invalid) {
    //             fonsSetFont(state.FONSContext, italicFont);
    //             fonsSetSize(state.FONSContext, 48.0f*dpiScale);
    //             fonsSetColor(state.FONSContext, brown);
    //             var n = System.Text.Encoding.UTF8.GetBytes("brown ");
    //             fixed (byte* n_p = n)
    //             {
    //                 dx = fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //             }
    //         }
    //         if (normalFont != invalid) {
    //             fonsSetFont(state.FONSContext, normalFont);
    //             fonsSetSize(state.FONSContext, 24.0f*dpiScale);
    //             fonsSetColor(state.FONSContext, white);
    //             var n = System.Text.Encoding.UTF8.GetBytes("fox ");
    //             fixed (byte* n_p = n)
    //             {
    //                 dx = fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //             }
    //         }
    //         if ((normalFont != invalid) && (italicFont != invalid) && (boldFont != invalid)) {
    //             fonsVertMetrics(state.FONSContext, null, null, &lh);
    //             dx = sx;
    //             dy += lh*1.2f;
    //             fonsSetFont(state.FONSContext, italicFont);
    //             var n = System.Text.Encoding.UTF8.GetBytes("jumps over ");
    //             fixed (byte* n_p = n)
    //             {
    //                 dx = fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //             }
    //             fonsSetFont(state.FONSContext, boldFont);
    //             n = System.Text.Encoding.UTF8.GetBytes("the lazy ");
    //             fixed (byte* n_p = n)
    //             {
    //                 dx = fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //             }
    //             fonsSetFont(state.FONSContext, normalFont);
    //             n = System.Text.Encoding.UTF8.GetBytes("dog.");
    //             fixed (byte* n_p = n)
    //             {
    //                 dx = fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //             }
    //         }
    //         if (normalFont != invalid) {
    //             dx = sx;
    //             dy += lh*1.2f;
    //             fonsSetSize(state.FONSContext, 12.0f*dpiScale);
    //             fonsSetFont(state.FONSContext, normalFont);
    //             fonsSetColor(state.FONSContext, blue);
    //             var n = System.Text.Encoding.UTF8.GetBytes("Now is the time for all good men to come to the aid of the party.");
    //             fixed (byte* n_p = n)
    //             {
    //                 fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //             }
    //         }
    //         if (italicFont != invalid) {
    //             fonsVertMetrics(state.FONSContext, null, null, &lh);
    //             dx = sx;
    //             dy += lh*1.2f*2;
    //             fonsSetSize(state.FONSContext, 18.0f*dpiScale);
    //             fonsSetFont(state.FONSContext, italicFont);
    //             fonsSetColor(state.FONSContext, white);
    //             var n = System.Text.Encoding.UTF8.GetBytes("Ég get etið gler án þess að meiða mig.");
    //             fixed (byte* n_p = n)
    //             {
    //                 fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //             }
    //         }
    //         if (japaneseFont != invalid) {
    //             fonsVertMetrics(state.FONSContext, null, null, &lh);
    //             dx = sx;
    //             dy += lh*1.2f;
    //             fonsSetFont(state.FONSContext, japaneseFont);
    //             var n = System.Text.Encoding.UTF8.GetBytes("私はガラスを食べられます。それは私を傷つけません。");
    //             fixed (byte* n_p = n)
    //             {
    //                 fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //             }
    //         }

    //         // Font alignment
    //         if (normalFont != invalid) {
    //             fonsSetSize(state.FONSContext, 18.0f*dpiScale);
    //             fonsSetFont(state.FONSContext, normalFont);
    //             fonsSetColor(state.FONSContext, white);
    //             dx = 50*dpiScale; dy = 350*dpiScale;
    //             line(dx-10*dpiScale, dy, dx+250*dpiScale, dy);
    //             fonsSetAlign(state.FONSContext, (int)(FONSalign.FONS_ALIGN_LEFT | FONSalign.FONS_ALIGN_TOP));
    //             var n = System.Text.Encoding.UTF8.GetBytes("Top");
    //             fixed (byte* n_p = n)
    //             {
    //                 dx = fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //             }
    //             dx += 10*dpiScale;
    //             fonsSetAlign(state.FONSContext, (int)(FONSalign.FONS_ALIGN_LEFT | FONSalign.FONS_ALIGN_MIDDLE));
    //             n = System.Text.Encoding.UTF8.GetBytes("Middle");
    //             fixed (byte* n_p = n)
    //             {
    //                 dx = fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //             }
    //             dx += 10*dpiScale;
    //             fonsSetAlign(state.FONSContext, (int)(FONSalign.FONS_ALIGN_LEFT | FONSalign.FONS_ALIGN_BASELINE));
    //             n = System.Text.Encoding.UTF8.GetBytes("Baseline");
    //             fixed (byte* n_p = n)
    //             {
    //                 dx = fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //             }
    //             dx += 10*dpiScale;
    //             fonsSetAlign(state.FONSContext, (int)(FONSalign.FONS_ALIGN_LEFT | FONSalign.FONS_ALIGN_BOTTOM));
    //             n = System.Text.Encoding.UTF8.GetBytes("Bottom");
    //             fixed (byte* n_p = n)
    //             {
    //                 fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //             }
    //             dx = 150*dpiScale; dy = 400*dpiScale;
    //             line(dx, dy-30*dpiScale, dx, dy+80.0f*dpiScale);
    //             fonsSetAlign(state.FONSContext, (int)(FONSalign.FONS_ALIGN_LEFT | FONSalign.FONS_ALIGN_BASELINE));
    //             n = System.Text.Encoding.UTF8.GetBytes("Left");
    //             fixed (byte* n_p = n)
    //             {
    //                 fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //             }
    //             dy += 30*dpiScale;
    //             fonsSetAlign(state.FONSContext, (int)(FONSalign.FONS_ALIGN_CENTER | FONSalign.FONS_ALIGN_BASELINE));
    //             n = System.Text.Encoding.UTF8.GetBytes("Center");
    //             fixed (byte* n_p = n)
    //             {
    //                 fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //             }
    //             dy += 30*dpiScale;
    //             fonsSetAlign(state.FONSContext, (int)(FONSalign.FONS_ALIGN_RIGHT | FONSalign.FONS_ALIGN_BASELINE));
    //             n = System.Text.Encoding.UTF8.GetBytes("Right");
    //             fixed (byte* n_p = n)
    //             {
    //                 fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //             }
    //         }

    //         // Blur
    //         if (italicFont != invalid) {
    //             dx = 500*dpiScale; dy = 350*dpiScale;
    //             fonsSetAlign(state.FONSContext, (int)(FONSalign.FONS_ALIGN_LEFT | FONSalign.FONS_ALIGN_BASELINE));
    //             fonsSetSize(state.FONSContext, 60.0f*dpiScale);
    //             fonsSetFont(state.FONSContext, italicFont);
    //             fonsSetColor(state.FONSContext, white);
    //             fonsSetSpacing(state.FONSContext, 5.0f*dpiScale);
    //             fonsSetBlur(state.FONSContext, 10.0f);
    //             var n = System.Text.Encoding.UTF8.GetBytes("Blurry...");
    //             fixed (byte* n_p = n)
    //             {
    //                 fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //             }
    //         }

    //         if (boldFont != invalid) {
    //             dy += 50.0f*dpiScale;
    //             fonsSetSize(state.FONSContext, 18.0f*dpiScale);
    //             fonsSetFont(state.FONSContext, boldFont);
    //             fonsSetColor(state.FONSContext, black);
    //             fonsSetSpacing(state.FONSContext, 0.0f);
    //             fonsSetBlur(state.FONSContext, 3.0f);
    //             var n = System.Text.Encoding.UTF8.GetBytes("DROP THAT SHADOW");
    //             fixed (byte* n_p = n)
    //             {
    //                 fonsDrawText(state.FONSContext, dx, dy+2, (sbyte*)n_p, null);
    //             }
    //             fonsSetColor(state.FONSContext, white);
    //             fonsSetBlur(state.FONSContext, 0);
    //             fixed (byte* n_p = n)
    //             {
    //                 fonsDrawText(state.FONSContext, dx, dy, (sbyte*)n_p, null);
    //             }
    //         }

    //         flush(state.FONSContext);

    //         void line(float sx, float sy, float ex, float ey)
    //         {
    //             GL.begin_lines();
    //             GL.c4b(255, 255, 0, 128);
    //             GL.v2f(sx, sy);
    //             GL.v2f(ex, ey);
    //             GL.end();
    //         }
    //     }
    // }
}