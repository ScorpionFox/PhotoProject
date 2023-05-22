using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhotoProject.Data.Services;
using PhotoProject.Data.ViewModels;
using PhotoProject.Models;

namespace PhotoProject.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IAlbumService _service;

        public AlbumController(IAlbumService service)
        {
            _service = service;
        }

        // Details for a photo
        [AllowAnonymous]
        public async Task<IActionResult> Photos(int id)
        {
            var albumPhotos = await _service.GetAlbumByIdAsync(id);

            return View(albumPhotos);
        }

        // Show all albums
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }


        //Add new album
        public async Task<IActionResult> Create()
        {
            var dropdown = await _service.GetPhotoDropDownValues();

            ViewBag.PhotosId = new SelectList(dropdown.Photos, "Id", "Name");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync(NewAlbumVM album)
        {
            if (!ModelState.IsValid)
            {
                return View("Create");
            }
            await _service.AddNewAlbumAsync(album);
            return RedirectToAction("Index");
        }

        // Update album
        public async Task<IActionResult> Update(int id)
        {
            var dropdown = await _service.GetPhotoDropDownValues();
            
            ViewBag.AuthorId = new SelectList(dropdown.Albums, "Id", "Name");


            var photoDetails = await _service.GetByIdAsync(id);
            if (photoDetails == null)
                return View("Empty");
            return View(photoDetails);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(int id, AlbumModel album)
        {

            if (!ModelState.IsValid)
            {
                return View("Empty");
            }
            await _service.UpdateAsync(id, album);
            return RedirectToAction("Index");
        }

        // Delete album
        public async Task<IActionResult> Delete(int id)
        {
            var albumDetails = await _service.GetByIdAsync(id);
            if (albumDetails == null)
                return View("Empty");
            return View(albumDetails);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var albumDetails = await _service.GetByIdAsync(id);
            if (albumDetails == null) return View("Empty");


            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}

