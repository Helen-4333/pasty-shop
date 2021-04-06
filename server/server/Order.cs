using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
    class Order
    {
        public string buyer;
        public string order;

        public Order(string buyer, string order)
        {
            this.buyer = buyer;
            this.order = order;
        }

        public string collect_order()
        {
            return ($"{buyer}#{order}");
        }
    }
}
