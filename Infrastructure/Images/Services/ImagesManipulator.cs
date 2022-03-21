using System;
using ExplorJobAPI.Infrastructure.Images.Models;
using ExplorJobAPI.Infrastructure.Strings.Services;
using ImageMagick;
using Serilog;

namespace ExplorJobAPI.Infrastructure.Images.Services
{
    public class ImagesManipulator
    {
        private readonly StringsManipulator _stringsManipulator;

        public ImagesManipulator(
            StringsManipulator stringsManipulator
        ) {
            _stringsManipulator = stringsManipulator;
        }

        public bool ResizeImage(
            string imagePath,
            ImageSize imageSize
        ) {
            try {
                using (var image = new MagickImage(imagePath)) {
                    var size = new MagickGeometry(
                        imageSize.Width,
                        imageSize.Height
                    );

                    size.IgnoreAspectRatio = true;

                    image.Resize(size);
                    image.Write(
                        _stringsManipulator.ReplaceLastOccurrence(
                            imagePath,
                            ".",
                            $"_{ imageSize.Label }."
                        )
                    );
                }

                return true;
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return false;
            }
        }
    }
}
