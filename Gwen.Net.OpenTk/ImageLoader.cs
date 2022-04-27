//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using Gwen.Net.OpenTk.Exceptions;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.PixelFormats;

//namespace Gwen.Net.OpenTk
//{
//    public static class ImageLoader
//    {
//        public delegate Image<Rgba32> Loader(string filename);

//        public static readonly Dictionary<string, Loader> loaders = new Dictionary<string, Loader>
//        {
//            { "jpeg", StandardLoader},
//            { "jpe", StandardLoader},
//            { "jfif", StandardLoader},
//            { "jpg", StandardLoader},
//            { "bmp", StandardLoader},
//            { "dib", StandardLoader},
//            { "rle", StandardLoader},
//            { "png", StandardLoader},
//            { "gif", StandardLoader},
//            { "tif", StandardLoader},
//            { "exif", StandardLoader},
//            { "wmf", StandardLoader},
//            { "emf", StandardLoader},
//        };

//        public static Image<Rgba32> StandardLoader(string s)
//        {
//            return new Image.Load<Rgba32>(s);
//        }

//        /*
//        public static Image<Rgba32> Load(string filename)
//        {
//            string resourceType = filename.ToLower().Split('.').Last();
//            if (loaders.TryGetValue(resourceType, out var loader))
//            {
//                return loader.Invoke(filename);
//            }

//            throw new ResourceLoaderNotFoundException(resourceType);
//        }*/
//    }
//}