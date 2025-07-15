using MongoDB.Bson.Serialization.Attributes;
using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Repositories
{
    [BsonIgnoreExtraElements]
    public class LibraryEntry : BaseDocument
    {
    }
}