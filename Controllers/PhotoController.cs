using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PhotoProject.Data;
using PhotoProject.Data.Services;
using PhotoProject.Data.ViewModels;
using PhotoProject.Models;
using PhotoProject.Data;

namespace PhotoProject.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IPhotoService _service;

        public PhotoController(IPhotoService service)
        {
            _service = service;
        }

        // Details for a photo
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var photoDetails = await _service.GetPhotoByIdAsync(id);
            return View(photoDetails);
        }

        // Show all photos
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        [AllowAnonymous]
        public async Task<IActionResult> SearchPhoto(string searchPhoto)
        {
            var data = await _service.GetAllAsync();


            if (!string.IsNullOrEmpty(searchPhoto))
            {
                var dataFiltered = data.Where(n => n.Name.IndexOf(searchPhoto, StringComparison.OrdinalIgnoreCase) != -1);
                return View("Index", dataFiltered);
            }
            return View("Index", data);

        }


        //Add new photo
        public async Task<IActionResult> Create()
        {
            var dropdown = await _service.GetPhotoDropDownValues();

            ViewBag.AlbumId = new SelectList(dropdown.Albums, "Id", "Name");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync(NewPhotoVM photo)
        {
            if (!ModelState.IsValid)
            {
                return View("Create");
            }
            await _service.AddNewPhotoAsync(photo);
            return RedirectToAction("Index");
        }

        // Update photo
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
        public async Task<IActionResult> UpdateAsync(int id, PhotoModel photo)
        {

            if (!ModelState.IsValid)
            {
                return View("Empty");
            }
            await _service.UpdateAsync(id, photo);
            return RedirectToAction("Index");
        }

        // Delete photo
        public async Task<IActionResult> Delete(int id)
        {
            var photoDetails = await _service.GetByIdAsync(id);
            if (photoDetails == null)
                return View("Empty");
            return View(photoDetails);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photoDetails = await _service.GetByIdAsync(id);
            if (photoDetails == null) return View("Empty");


            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        // download photo
        [AllowAnonymous]
        public async Task<IActionResult> Download(int id)
        {
            var photo = await _service.GetPhotoByIdAsync(id);
            if (photo == null)
                return NotFound();
        
            if (photo.Access == AccessLevel.Private)
            {
                // Zdjęcie ma status prywatny, więc odmów dostępu
                return RedirectToAction("AccessDenied", "Home");
            }

            var fileResult = await _service.DownloadPhotoAsync(id);
            if (fileResult == null)
                return NotFound();

            return fileResult;
        }
    }
}

