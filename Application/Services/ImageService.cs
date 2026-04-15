namespace MojiiBackend.Application.Services;

public class ImageService(IWebHostEnvironment _environment)
{
    public async Task<string> SaveImageAsync(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Le fichier est vide ou inexistant.");

        // 1. Définir le chemin du dossier (ex: wwwroot/images/profiles)
        string folderPath = Path.Combine(_environment.WebRootPath, "images");

        string fileName = string.IsNullOrWhiteSpace(Path.GetFileNameWithoutExtension(file.FileName))
            ? "unnamed_image" : Path.GetFileNameWithoutExtension(file.FileName);

        // 3. Générer un nom unique pour éviter les doublons (ex: nom_fichier<GUID>.jpg)
        string fullFileName = $"{fileName}{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        string filePath = Path.Combine(folderPath, fullFileName);

        // 4. Enregistrer le fichier physiquement
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // 5. Retourner l'URL relative (celle qu'on stockera en base)
        // On utilise / pour que ce soit compatible avec le web
        return $"images/{fullFileName}";
    }
}