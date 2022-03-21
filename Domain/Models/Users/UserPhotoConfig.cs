using System.Collections.Generic;
using System.IO;
using ExplorJobAPI.Infrastructure.Images.Models;

namespace ExplorJobAPI.Domain.Models.Users
{
    public class UserPhotoConfig
    {
        /**
        * Can be used as a parameter of the 'RequestSizeLimit' attribute
        * 5MB : 5x1024x1024
        */
        public long MaxSize = 5242880;

        public string Folder { get => Path.Combine(
            Config.Folders.Resources,
            Config.Folders.Images,
            Config.Folders.Users,
            Config.Folders.Photos
        ); }

        public List<string> AllowedTypes = new List<string> {
            "jpeg",
            "png"
        };

        public bool IsTypeAllowed(
            string type
        ) {
            return AllowedTypes.Contains(
                type
            );
        }

        public ImageSize Size125x125 = new ImageSize {
            Label = "125x125",
            Width = 125,
            Height = 125
        };

        public ImageSize Size250x250 = new ImageSize {
            Label = "250x250",
            Width = 250,
            Height = 250
        };
        
        public ImageSize Size500x500 = new ImageSize {
            Label = "500x500",
            Width = 500,
            Height = 500
        };
    }
}
