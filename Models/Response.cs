using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tft_cosmetics_manager.Models
{
    internal class Response
    {
        public string ExpirationDate { get; set; }
        public bool F2p { get; set; }
        public string InventoryType { get; set; }
        public int ItemId { get; set; }
        public bool Loyalty { get; set; }
        public List<object> LoyaltySources { get; set; }
        public bool Owned { get; set; }
        public string OwnershipType { get; set; }
        public object Payload { get; set; }
        public string PurchaseDate { get; set; }
        public int Quantity { get; set; }
        public bool Rental { get; set; }
        public string Uuid { get; set; }
        public int Wins { get; set; }
    }
}
