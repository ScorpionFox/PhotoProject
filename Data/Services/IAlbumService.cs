using Microsoft.AspNetCore.Mvc;
using PhotoProject.Data.Base;
using PhotoProject.Data.ViewModels;
using PhotoProject.Models;

namespace PhotoProject.Data.Services
{
    public interface IAlbumService : IEntityBaseRepository<AlbumModel>
    {
        Task<AlbumModel> GetAlbumByIdAsync(int id);
        Task AddNewAlbumAsync(NewAlbumVM album);
        Task<PhotoDropDownVM> GetPhotoDropDownValues();
    }
}
