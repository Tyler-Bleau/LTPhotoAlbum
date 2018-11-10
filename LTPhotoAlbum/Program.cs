using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LTPhotoAlbum
{
    public class Photo
    {
        [JsonProperty("albumId")]
        public string AlbumId { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl { get; set; }
    }

    class Program
    {
        static HttpClient client = new HttpClient();

        static async Task<List<Photo>> GetPhotosAsync(string path)
        {
            List<Photo> photos = null;
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                photos = JsonConvert.DeserializeObject<List<Photo>>(json);
            }

            return photos;
        }

        static void Main()
        {
            string albumIdPrompt = "Enter Album ID.  Type 'Exit' to quit.";
            Console.WriteLine(albumIdPrompt);
            string photoId = Console.ReadLine();

            while (photoId.ToLower().Trim() != "exit")
            {
                string path = "https://jsonplaceholder.typicode.com/photos?albumId=" + photoId;

                var photos = GetPhotosAsync(path).Result;

                if (photos.FirstOrDefault() != null)
                {
                    foreach (var photo in photos)
                    {
                        Console.WriteLine("[" + photo.Id + "] " + photo.Title);
                    }
                }
                else
                {
                    Console.WriteLine(System.Environment.NewLine + "Album ID '" + photoId + "' not found.");
                }

                Console.WriteLine(System.Environment.NewLine + albumIdPrompt);
                photoId = Console.ReadLine();
            }
        }
    }
}