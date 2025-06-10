using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Constants
{
    public enum PaymentMethod
    {
        CreditCard = 0,
        DebitCard = 1,
        PayPal = 2,
        BankTransfer = 3,
        DigitalWallet = 4,
        Cryptocurrency = 5,
        GiftCard = 6,
        StoreCredit = 7,
        CashOnDelivery = 8
    }
}
