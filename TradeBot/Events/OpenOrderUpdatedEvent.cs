using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TradeBot.TwsAbstractions;

namespace TradeBot.Events
{
    public delegate void OpenOrderUpdatedEventHandler (OpenOrder openOrder);
}
