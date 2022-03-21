using System.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Exceptions.Users;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Text.RegularExpressions;
using ExplorJobAPI.Infrastructure.Images.Services;

namespace ExplorJobAPI.Domain.Services.Users
{
    public class UserPhotoService : IUserPhotoService
    {
        private readonly ImagesManipulator _imagesManipulator;

        public UserPhotoService(
            ImagesManipulator imagesManipulator
        ) {
            _imagesManipulator = imagesManipulator;
        }

        public async Task<bool> Save(
            string userId,
            IFormFile file
        ) {
            try {
                var type = file.ContentType.Replace(
                    "image/",
                    String.Empty
                );

                if (!Config.UserPhoto.IsTypeAllowed(type)) {
                    throw new UserPhotoTypeNotAllowedException(
                        type
                    );
                }

                var pathToSavePhoto = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    Config.UserPhoto.Folder,
                    $"{ userId }.{ type }"
                );

                using(var stream = new FileStream(
                    pathToSavePhoto,
                    FileMode.Create
                )) {
                    await file.CopyToAsync(
                        stream
                    );

                    _imagesManipulator.ResizeImage(
                        pathToSavePhoto,
                        Config.UserPhoto.Size125x125
                    );

                    _imagesManipulator.ResizeImage(
                        pathToSavePhoto,
                        Config.UserPhoto.Size250x250
                    );

                    _imagesManipulator.ResizeImage(
                        pathToSavePhoto,
                        Config.UserPhoto.Size500x500
                    );
                }

                return true;
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return false;
            }
        }

        public bool Delete(
            string userId
        ) {
            var folderToDeletePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                Config.UserPhoto.Folder
            );

            if (Directory.Exists(folderToDeletePath)) {
                try {
                    Directory.GetFiles(
                        folderToDeletePath
                    ).ToList().RemoveAll(
                        (string directoryFilePath) => Regex.IsMatch(
                            directoryFilePath,
                            userId,
                            RegexOptions.IgnoreCase
                        )
                    );

                    return true;
                }
                catch(Exception e) {
                    Log.Error(e.ToString());
                    return false;
                }
            }
            else {
                return false;
            }
        }
    }
}
