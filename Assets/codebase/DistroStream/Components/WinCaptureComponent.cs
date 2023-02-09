using gevo.platform.windows;
using Nucleus.WindowScrape;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Graphics = System.Drawing.Graphics;
using Color = UnityEngine.Color;
using UnityEngine.Rendering.HighDefinition;

namespace DistroStream {
    public class WinCaptureComponent : MonoBehaviour {
        public Renderer rend;
        public Texture2D saveTex;

        private Bitmap bitmap;

        private HwndObject hwnd;

        private Texture2D texture;

        private LockBitmap locker;
        private Size texSize;

        private Material mat;

        private void Awake() {
            Process[] process = Process.GetProcesses();

            Process proc = process.FirstOrDefault(c => c.ProcessName.ToLower().StartsWith("deezer"));

            if (proc == null) {
                Debug.LogError("Deezer process not found");
                return;
            }

            IntPtr ptr = proc.MainWindowHandle;

            hwnd = new HwndObject(ptr);

            texSize = new Size(500, 100);
            texture = new Texture2D(texSize.Width, texSize.Height, TextureFormat.ARGB32, false);

            bitmap = new Bitmap(texSize.Width, texSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            locker = new LockBitmap(bitmap);

            mat = rend.sharedMaterials[2];
        }

        private void Update() {
            Point location = hwnd.Location;
            Size size = hwnd.Size;
            Point upperLeft = new Point(location.X + 385, location.Y + size.Height - texSize.Height - 5);

            using (Graphics g = Graphics.FromImage(bitmap)) {
                //g.Clear(System.Drawing.Color.Yellow);
                g.CopyFromScreen(upperLeft, new Point(0, 0), texSize);
            }

            locker.LockBits();
            byte[] pixels = locker.Pixels;

            for (int i = 0; i < pixels.Length - 3; i += 4) {
                byte r = pixels[i];
                byte g = pixels[i + 1];
                byte b = pixels[i + 2];
                byte a = pixels[i + 3];

                pixels[i + 3] = r;
                pixels[i + 2] = g;
                pixels[i + 1] = b;
                pixels[i] = a;
            }

            texture.SetPixelData(pixels, 0);
            texture.Apply();

            locker.UnlockBits();

            mat.mainTexture = texture;
            float intensity = 10;
            mat.SetColor("_EmissiveColor", Color.white * intensity);

            mat.SetTexture("_EmissiveColorMap", texture);
        }

        private void OnDisable() {
            mat.SetTexture("_EmissiveColorMap", saveTex);
        }
    }
}
