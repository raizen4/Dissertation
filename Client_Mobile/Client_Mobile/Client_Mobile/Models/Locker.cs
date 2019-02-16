using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Models
{
    class Locker
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<Profile> AuthorisedProfiles { get; set; }

    }
}
