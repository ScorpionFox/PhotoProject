using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoProject.Data.Services;
using PhotoProject.Models;

namespace PhotoProject.Controllers
{
    public class AlbumPhotoController : Controller
    {
        private readonly IAlbumPhotoService _service;

        public AlbumPhotoController(IAlbumPhotoService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Photo(int id)
        {
            var albumPhotos = await _service.GetAlbumPhotoByIdAsync(id);

            return View(albumPhotos);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var albumPhotoDetails = await _service.GetByIdAsync(id);
            if (albumPhotoDetails == null)
                return View("Empty");
            return View(albumPhotoDetails);
        }
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var albumPhotoDetails = await _service.GetByIdAsync(id);
            if (albumPhotoDetails == null) return View("Empty");


            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }

    }
    
}
