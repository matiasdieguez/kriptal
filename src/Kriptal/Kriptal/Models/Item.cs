using Realms;
using System;

namespace Kriptal.Models
{
    public class Item : RealmObject
    {
        /// <summary>
        /// Id for item
        /// This works as the BlockNumber in the Blockchain
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
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
