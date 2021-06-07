using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Images.Entities;
using Images.Models;
using Images.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;

namespace Images.Controllers
{
    public class ImageController : Controller
    {
        IImageDbService _imageDbService;
        IWebHostEnvironment _hostEnvironment;
        IImageStorageService _imageStorageService;

        public ImageController(
            IImageDbService imageDbService,
            IWebHostEnvironment hostEnvironment,
            IImageStorageService imageStorageService)
        {
            _imageDbService = imageDbService;
            _hostEnvironment = hostEnvironment;
            _imageStorageService = imageStorageService;
        }

        public async Task<ViewResult> Index()
        {
            ImageListViewModel model = new ImageListViewModel();
            model.Images = (await _imageDbService.GetItemsAsync("SELECT * FROM c")).ToList();
            return View(model);
        }
        public ViewResult New()
        {
            ImageViewModel imageViewModel = new ImageViewModel();            
            return View("Edit", imageViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                Image image = null;
                if (!string.IsNullOrWhiteSpace(model.Id))
                    image = (await _imageDbService.GetItemAsync(model.Id));

                if (image == null)
                {
                    image = new Image();
                    image.Id = Guid.NewGuid().ToString();
                    image.Name = model.Name;
                    await _imageDbService.AddItemAsync(image);
                }
                else
                {
                    image.Name = model.Name;
                    await _imageDbService.UpdateItemAsync(image.Id, image);
                }

                return RedirectToAction("Index");
            }
            else
            {                
                return View(model);
            }
        }

        public async Task<ViewResult> Edit(string id)
        {
            Image image = (await _imageDbService.GetItemAsync(id));

            ImageViewModel imageViewModel = new ImageViewModel();
            imageViewModel.Id = image.Id;
            imageViewModel.Name = image.Name;
            return View(imageViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(UploadImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                Image image = null;
                if (!string.IsNullOrWhiteSpace(model.Id))
                    image = (await _imageDbService.GetItemAsync(model.Id));

                if (image == null)
                {
                    return View(model);
                }
                else
                {
                    // delete image from wwwroot/image
                    if (!string.IsNullOrWhiteSpace(image.FileName))
                    {
                        var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", image.FileName);
                        if (System.IO.File.Exists(imagePath))
                            System.IO.File.Delete(imagePath);
                    }

                    string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    string fileExtension = Path.GetExtension(model.ImageFile.FileName);
                    string fileNameWithDate = $"{fileName}-{DateTime.Now.ToString("s").Replace(":","-")}{fileExtension}";
                    image.FileName = fileNameWithDate;                    

                    // upload file
                    await _imageStorageService.UploadImage(model.ImageFile.OpenReadStream(), fileNameWithDate);

                    // update record
                    await _imageDbService.UpdateItemAsync(image.Id, image);
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        public async Task<ViewResult> UploadImage (string id)
        {
            Image image = (await _imageDbService.GetItemAsync(id));

            UploadImageViewModel uploadImageViewModel = new UploadImageViewModel();
            uploadImageViewModel.Id = image.Id;
            uploadImageViewModel.Name = image.Name;
            return View(uploadImageViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(DeleteImageViewModel model)
        {
            Image image = (await _imageDbService.GetItemAsync(model.Id));

            if (image != null)
            {
                // delete the file
                await _imageStorageService.DeleteImage(image.FileName);

                //delete the record                
                await _imageDbService.DeleteItemAsync(model.Id);

                return RedirectToAction(nameof(Index));
            }

            return View(model.Id);
        }

        public async Task<IActionResult> DeleteImage(string id)
        {
            Image image = (await _imageDbService.GetItemAsync(id));

            DeleteImageViewModel viewModel = new DeleteImageViewModel();
            if (image != null)
            {                
                viewModel.Id = image.Id;
                viewModel.Name = image.Name;
                viewModel.ImageFileName = image.FileName;
                viewModel.ImageFileUrl = _imageStorageService.GetImageUrl(image.FileName);
            }
            return View(viewModel);
        }
    }
}