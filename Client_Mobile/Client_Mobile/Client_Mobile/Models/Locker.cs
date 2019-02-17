using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Models
{
    public class Locker
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<Profile> AuthorisedProfiles { get; set; }

    }
}
