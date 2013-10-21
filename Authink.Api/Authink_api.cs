using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

using database = Authink.Data;
using ent      = Authink.Data.Api.App.Entities;
using mappers  = Authink.Api.Mappers;

namespace Authink.Data.Api.Service
{
    public class AuthinkApiAdapter
    {
        public IReadOnlyList<ent::Child.Details> GetChildren(string user_userName)
        {
            using(var db = new database::AuthinkDataModel())
            {
                return
                    db.Users
                        .Single(user => user.Username == user_userName)
                        .Children
                        .ToList()
                        .Select(mappers::Child.FromDatabaseData       )
                        .ToList();
            }                                                         
        }

        public bool Login_User(string username,string password)
        {
            using(var db = new database::AuthinkDataModel())
            {
                var hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
 
                return
                    db.Users.SingleOrDefault(user => user.Username == username && user.Password == hashedPassword) != null;
            }
        }
    }
}