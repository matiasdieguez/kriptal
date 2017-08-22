using Realms;
using System;

namespace Kriptal.Models
{
    public class Item : RealmObject
    {
        /// <summary>
        /// Id for item
        /// </summary>
        public long Id { get; set; }


        /// <summary>
        /// Created at time stamp
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        public string BlockNumber { get; set; }

        public string Previous { get; set; }
        public string Nonce { get; set; }
        public string Hash { get; set; }
    }
}
