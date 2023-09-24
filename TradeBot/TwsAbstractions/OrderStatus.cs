using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TradeBot.TwsAbstractions
{
    public class OrderStatus
    {
        public OrderStatus(int orderId, string status, double filled, double remaining, double avgFillPrice, 
            int permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice) 
        {
            OrderId = orderId;
            Status = status;
            Filled = filled;                
            Remaining = remaining;
            AvgFillPrice = avgFillPrice;
            PermId = permId;
            ParentId = parentId;
            LastFillPrice = lastFillPrice;
            ClientId = clientId;
            WhyHeld = whyHeld;
            MktCapPrice = mktCapPrice;
        }

        public int OrderId { get; }
        public string Status { get; }
        public double Filled { get; }
        public double Remaining { get; }
        public double AvgFillPrice { get; } 
        public int PermId { get; }
        public int ParentId { get; }
        public double LastFillPrice { get; }
        public int ClientId { get; }
        public string WhyHeld { get; }
        public double MktCapPrice { get; }

        public override string ToString()
        {
            return "OrderStatus: OrderId: " + OrderId + ", Status: " + Status + ", Filled: " + Util.DoubleMaxString(Filled) + ", Remaining: " + Util.DoubleMaxString(Remaining)
                + ", AvgFillPrice: " + Util.DoubleMaxString(AvgFillPrice) + ", PermId: " + Util.IntMaxString(PermId) + ", ParentId: " + Util.IntMaxString(ParentId) +
                ", LastFillPrice: " + Util.DoubleMaxString(LastFillPrice) + ", ClientId: " + Util.IntMaxString(ClientId) + ", WhyHeld: " + WhyHeld + ", MktCapPrice: " + Util.DoubleMaxString(MktCapPrice);
        }


    }
}
