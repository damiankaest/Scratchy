//// See https://aka.ms/new-console-template for more information


//using MongoDB.Bson;
//using MongoDB.Driver;
//using Newtonsoft.Json.Linq;
//using Scratchy.Application;

//string connectionString = "mongodb://localhost:27017"; // Ändere dies entsprechend deiner Konfiguration

//// Datenbank- und Collection-Namen
//string databaseName = "scratch-up";
//string artistCollectionName = "Artist";
//string mediaDataCollectionName = "MediaData";

//var client = new MongoClient(connectionString);
//var database = client.GetDatabase(databaseName);
//var artistCollection = database.GetCollection<Artist>(artistCollectionName);
//var mediaDataCollection = database.GetCollection<MediaData>(mediaDataCollectionName);


//var spotifyAccessToken = Scratchy.DataGenerator.SpotifyApiHelper.GetSpotifyAccessToken("2783d9fdaf5848ab9a4b7248215f5844", "02d9765ad365414097f1ed7924f71241").Result;



//var artistNames = new List<string> { "Iron Maiden" };

//foreach (var artistName in artistNames)
//{
//    var spotifyClient = new SpotifyApiClient(spotifyAccessToken);
//    var artists = spotifyClient.SearchSpotifyArtistsAsync(artistName).Result;

//    foreach (var item in artists)
//    {
//        //var artist = new Artist();
//        var artistSpotifyId = item["id"]?.ToString();
//        //var filter = Builders<Artist>.Filter.Eq(a => a.Id, artistSpotifyId);
//        //var existingArtist = artistCollection.Find(filter).FirstOrDefault();

//        if (existingArtist == null)
//        {
//            artist = new Artist()
//            {
//                Name = item["name"]?.ToString(),
//                //Genres = item["genres"]?.ToList(), // Alternativ könnten Sie hier auch andere Felder verwenden
//                SpotifyUrl = item["id"]?.ToString(),
//                ImageUrl = item["external_urls"]?["spotify"]?.ToString(),
//                Id = ObjectId.GenerateNewId().ToString() // Generiert eine neue GUID als String für die ID
//            };

//            artistCollection.InsertOne(artist);
//        }

//        var albums = spotifyClient.SearchSpotifyAlbumsByArtistIdAsync(existingArtist.Id).Result;
//        foreach (var album in albums)
//        {
//            var albumId = album["id"]?.ToString();
//            var albumFilter = Builders<MediaData>.Filter.Eq(a => a.spotifyId, albumId);
//            var existingAlbum = mediaDataCollection.Find(albumFilter).FirstOrDefault();
//            if (existingAlbum == null)
//            {
//                var mediaData = new MediaData()
//                {
//                    id = ObjectId.GenerateNewId().ToString(),
//                    spotifyUrl = album["external_urls"]?["spotify"]?.ToString(),
//                    description = album["release_date"]?.ToString() + " - " + album["album_type"]?.ToString(),
//                    title = album["name"]?.ToString(),
//                    artistId = existingArtist.Id,
//                    spotifyId = albumId
//                };
//                mediaDataCollection.InsertOne(mediaData);
//            }
//        }
//    }
//}
//async Task<List<MediaData>> GetAlbumsFromMusicBrainz(string artistId)
//{
//    //2783d9fdaf5848ab9a4b7248215f5844
//    //02d9765ad365414097f1ed7924f71241
//    string url = $"https://musicbrainz.org/ws/2/release-group?artist={Uri.EscapeDataString(artistId)}&type=album&fmt=json";
//    using (var httpClient = new HttpClient())
//    {
//        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MusicBrainzDataImport/1.0 (dami98@web.de)");

//        var response = await httpClient.GetStringAsync(url);
//        var json = JObject.Parse(response);

//        var albums = new List<MediaData>();
//        foreach (var albumJson in json["release-groups"])
//        {
//            var mediaData = new MediaData
//            {
//                id = ObjectId.GenerateNewId().ToString(),
//                title = albumJson["title"]?.ToString(),
//                artistId = artistId,
//                description = albumJson["description"]?.ToString(),
//                imdbRating = 0, 
//                spotifyUrl = "" 
//            };
//            albums.Add(mediaData);
//        }

//        return albums;
//    }
//}

//async Task<Artist> GetArtistDataFromMusicBrainz(string artistName)
//{
//    string url = $"https://musicbrainz.org/ws/2/artist/?query=artist:{Uri.EscapeDataString(artistName)}&fmt=json";
//    using (var httpClient = new HttpClient())
//    { 
//        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MusicBrainzDataImport/1.0 (dami98@web.de)");
//        var response = await httpClient.GetStringAsync(url);
//        var json = JObject.Parse(response);
       


//        var artistJson = json["artists"]?.FirstOrDefault();
//        if (artistJson != null)
//        {
//            return new Artist
//            {
//                Id = ObjectId.GenerateNewId().ToString(),
//                //Id = artistJson["id"]?.ToString(),
//                Name = artistJson["name"]?.ToString(),
//                ImageUrl = artistJson["country"]?.ToString()
//            };
//        }
//    }
//    return null;
//}

//Console.WriteLine("Daten wurden erfolgreich in MongoDB gespeichert.");


//#region Get Artist Albums
//#endregion