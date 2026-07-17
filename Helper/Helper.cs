namespace menus_project.Helpers
{
	public class Helper
	{
		private readonly IWebHostEnvironment _webHostEnvironment;

		public Helper(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}


		public string SaveImage(IFormFile image)
		{
			var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
			var extension = Path.GetExtension(image.FileName).ToLower();

			if (!allowedExtensions.Contains(extension))
				throw new InvalidOperationException("Validation.InvalidImageType");

			string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

			if(!Directory.Exists(uploadFolder))
				Directory.CreateDirectory(uploadFolder);

			string uniqueFileName = Guid.NewGuid().ToString() + extension;

			string filePath = Path.Combine(uploadFolder, uniqueFileName);

			using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
			{
				image.CopyTo(fileStream);
			}

			return uniqueFileName;
		}

		public void DeleteImage(string imageUrl)
		{
			if (String.IsNullOrEmpty(imageUrl)) return;

			string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
			string filePath = Path.Combine(folderPath, imageUrl.TrimStart('/'));

			if(File.Exists(filePath))
				File.Delete(filePath);
		}
	}
}
