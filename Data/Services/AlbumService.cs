﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PhotoProject.Data.Base;
using PhotoProject.Data.Services;
using PhotoProject.Data.ViewModels;
using PhotoProject.Models;

namespace PhotoProject.Data.Services
{
    public class AlbumService : EntityBaseRepository<AlbumModel>, IAlbumService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public AlbumService(AppDbContext context, IWebHostEnvironment hostEnvironment) : base(context)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        public async Task AddNewAlbumAsync(NewAlbumVM album)
        {
            var newAlbum = new AlbumModel()
            {
                Name = album.Name,
                UrlName = album.UrlName,
                Access = album.Access,
                ImageFile = album.ImageFile,
                ImageName = album.ImageName
            };
            // save image
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileName(newAlbum.ImageFile.FileName);
            string extension = Path.GetExtension(newAlbum.ImageFile.FileName);
            newAlbum.ImageName = fileName;
            string path = Path.Combine(wwwRootPath + "/album/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await newAlbum.ImageFile.CopyToAsync(fileStream);
            }

            await _context.AddAsync(newAlbum);
            await _context.SaveChangesAsync();

            if (album.PhotosId != null)
            {
                foreach (var photoId in album.PhotosId)
                {
                    var newPhotoAlbum = new AlbumPhotoModel()
                    {
                        PhotoId = photoId,
                        AlbumId = newAlbum.Id
                    };
                    await _context.AlbumPhotos.AddAsync(newPhotoAlbum);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<AlbumModel> GetAlbumByIdAsync(int id)
        {
            var albumDetails = _context.Albums.Include(ap => ap.AlbumsPhotos).ThenInclude(p => p.Photo).FirstOrDefaultAsync(n => n.Id == id);
            return await albumDetails;
        }

        public async Task<PhotoDropDownVM> GetPhotoDropDownValues()
        {
            var dropdown = new PhotoDropDownVM()
            {
                Photos = await _context.Photos.OrderBy(n => n.Name).ToListAsync()
            };
            return dropdown;
        }



    }
}
    








