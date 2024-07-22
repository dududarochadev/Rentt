using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rentt.Entities
{
    public class User : IdentityUser
    {
        public User()
        {
            Roles = new List<string>();
        }

        [BsonElement("name")]
        public string? Name { get; set; }

        public IList<string> Roles { get; set; }
    }
}
