// using ImageMagick;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using static DrugScanner.Server.Utils.FileHelper;

namespace DrugScanner.Server.Utils;

public static class FileHelperExtensions
{

  #region Write file

  /// <summary>
  /// Write file to the disk.
  /// File paths will be prefixed with `/files`.
  /// </summary>
  /// <param name="file"></param>
  /// <param name="options"></param>
  /// <returns></returns>
  public async static Task<string> WriteFile(this IFormFile? file, FileWriteOptions? options = null)
  {
    bool hasFile = file != null;
    options ??= new();

    string defaultFilePath = string.IsNullOrWhiteSpace(options?.DefaultFilePath) ? string.Empty : Path.Combine(IoCContainer.Configuration!["Host"] ?? "", "files", options.DefaultFilePath).Replace("\\", "/");

    if (file == null)
      return defaultFilePath;

    string modFileName = string.Empty;

    // Check if sent file is empty
    if (file.Length == 0)
      return defaultFilePath;

    if (!IsValidFileType(file, options!.FileTypes))
    {
      if (options.ThrowOnInvalidFileType)
        throw new Exception("Invalid file type");
      else
        return defaultFilePath;
    }

    string fileExtension = Path.GetExtension(file.FileName);

    modFileName = (!string.IsNullOrWhiteSpace(options.ModFileName) ? options.ModFileName : DateTimeOffset.UtcNow.ToFileTime().ToString()) + fileExtension;

    // Prefix `/files` to directory path
    var directoryPaths = new List<string> { Directory.GetCurrentDirectory(), "wwwroot", "files" };
    options.DirectoryPath.ToList().ForEach(directoryPaths.Add);
    string fullDirectory = Path.Combine(directoryPaths.ToArray());
    string fileName = Path.Combine(fullDirectory, modFileName);


    if (!Directory.Exists(fullDirectory))
    {
      Directory.CreateDirectory(fullDirectory);
    }

    if (file.ContentType.StartsWith("image/"))
    {
      await ResizeImage(file, fileName);
    }
    else
    {
      using var stream = new FileStream(fileName, FileMode.Create);
      await file.CopyToAsync(stream);
    }


    // Get file path on server
    var paths = new List<string> { IoCContainer.Configuration!["Host"] ?? "", "files" };
    options.DirectoryPath.ToList().ForEach(paths.Add);

    paths.Add(modFileName);

    string fileUrl = Path.Combine(paths.ToArray()).Replace("\\", "/");

    return fileUrl;
  }

  #endregion

  #region Write files

  /// <summary>
  /// Write file to the disk.
  /// File paths will be prefixed with `/files`.
  /// </summary>
  /// <param name="files"></param>
  /// <param name="options"></param>
  /// <returns></returns>
  public async static Task<List<string>> WriteFiles(this IFormFileCollection files, FileWriteOptions? options = null)
  {
    bool hasFile = files.Count > 0;
    var urls = new List<string>();

    string defaultFilePath = string.IsNullOrWhiteSpace(options?.DefaultFilePath) ? string.Empty : Path.Combine(IoCContainer.Configuration!["Host"] ?? "", "files", options.DefaultFilePath).Replace("\\", "/");

    if (!hasFile)
    {
      urls.Add(defaultFilePath);
      return urls;
    }

    string modFileName = string.Empty;

    for (int i = 0; i < files.Count; i++)
    {
      var file = files[i];

      // Check if sent file is empty
      if (file.Length == 0)
        continue;

      if (!IsValidFileType(file, options!.FileTypes))
      {
        if (options.ThrowOnInvalidFileType)
          throw new Exception("Invalid file type");
        else
          continue;
      }

      string fileExtension = Path.GetExtension(file.FileName);

      modFileName = (!string.IsNullOrWhiteSpace(options.ModFileName) ? options.ModFileName : DateTimeOffset.UtcNow.ToFileTime().ToString()) + $"_{i}" + fileExtension;

      // Prefix `/files` to directory path
      var directoryPaths = new List<string> { Directory.GetCurrentDirectory(), "wwwroot", "files" };
      options.DirectoryPath.ToList().ForEach(path =>
      {
        directoryPaths.Add(path);
      });
      string fullDirectory = Path.Combine(directoryPaths.ToArray());
      string fileName = Path.Combine(fullDirectory, modFileName);

      if (!Directory.Exists(fullDirectory))
      {
        Directory.CreateDirectory(fullDirectory);
      }

      if (file.ContentType.StartsWith("image/"))
      {
        await ResizeImage(file, fileName);
      }
      else
      {
        using var stream = new FileStream(fileName, FileMode.Create);
        await file.CopyToAsync(stream);
      }

      // Get file path on server
      var paths = new List<string> { IoCContainer.Configuration!["Host"] ?? "", "files" };
      options.DirectoryPath.ToList().ForEach(paths.Add);

      paths.Add(modFileName);

      string fileUrl = Path.Combine(paths.ToArray()).Replace("\\", "/");

      urls.Add(fileUrl);
    }

    return urls;
  }

  #endregion

  #region Resize image

  /// <summary>
  /// Compresses an image file
  /// </summary>
  /// <param name="file">The uploaded file</param>
  /// <param name="path">Path to save file</param>
  private static async Task ResizeImage(IFormFile file, string path)
  {
    try
    {
      double scaleFactor = 1;
      using var image = await Image.LoadAsync(file.OpenReadStream()); //MagickImage(file.OpenReadStream());
      // image.Format = image.Format; // Get or Set the format of the image.

      var aspectRatio = (double)image.Width / image.Height;
      if (aspectRatio > 2.5 || aspectRatio < (1 / 2.5))
      {
        throw new Exception("Image upload failed");
      }

      // Check for max image size
      if (image.Width > 800)
      {
        scaleFactor = 800D / image.Width;
      }

      image.Mutate(x =>
        x.Resize((int)(image.Width * scaleFactor), (int)(image.Height * scaleFactor))
        );
      // image.Resize((int)(image.Width * scaleFactor), (int)(image.Height * scaleFactor)); // fit the image into the requested width and height. 
      // image.Quality = 10; // This is the Compression level.
      await image.SaveAsync(path);

      // image.Write(path);

    }
    catch (Exception)
    {
      using var stream = new FileStream(path, FileMode.Create);
      await file.CopyToAsync(stream);
      //throw new Exception(ex.ToString());
    }
  }

  #endregion

  #region Check file type

  /// <summary>
  /// Checks if a file is of a valid type.
  /// If not types are specified, return true
  /// </summary>
  /// <param name="file">The uploaded file</param>
  /// <param name="types">File type options</param>
  /// <returns></returns>
  private static bool IsValidFileType(IFormFile file, AcceptedFileTypes types)
  {
    if ((types & AcceptedFileTypes.Image) == AcceptedFileTypes.Image)
    {
      return file.ContentType.StartsWith("image/");
    }

    if ((types & AcceptedFileTypes.Video) == AcceptedFileTypes.Video)
    {
      return file.ContentType.StartsWith("video/");
    }

    return true;
  }

  #endregion

}

public class FileHelper
{

  #region Helpers

  public class FileWriteOptions
  {
    /// <summary>
    /// File path to return if no files are sent.
    /// </summary>
    public string DefaultFilePath { get; set; } = "";
    /// <summary>
    /// Structure of directory paths where file will be saved.
    /// Eg. <c> new string[] {"foo", "bar"} </c> will be saved as <b>/foo/bar</b>.
    /// </summary>
    public IEnumerable<string> DirectoryPath { get; set; } = [""];
    /// <summary>
    /// The name which the file will be saved as
    /// </summary>
    public string? ModFileName { get; set; } = "";
    /// <summary>
    /// Options for accepted files
    /// </summary>
    public AcceptedFileTypes FileTypes { get; set; }
    /// <summary>
    /// Throws an error if file(s) is of invalid file type
    /// </summary>
    public bool ThrowOnInvalidFileType { get; set; }
  }


  /// <summary>
  /// Accepted file type options
  /// </summary>
  public enum AcceptedFileTypes
  {
    /// <summary>
    /// For image files
    /// </summary>
    Image = 0x1,
    /// <summary>
    /// For video files
    /// </summary>
    Video = 0x2,
  }

  #endregion

}


