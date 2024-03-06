﻿namespace API.Param.Enums
{
    public enum TransactionEnum
    {
        Received = 1,
        Sent = 2,
    }

    public enum TransactionStatus
    {
        success = 0,
        error = 1,
    }

    public enum TransactionType
    {
        Deposit_Auction_Fee = 1,
        Commistion_Fee = 2,
        Upload_Fee = 3,
        Refund = 4,
    }
}
