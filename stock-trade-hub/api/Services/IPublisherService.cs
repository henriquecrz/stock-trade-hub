﻿using api.Models;

namespace api.Services
{
    public interface IPublisherService
    {
        public void PublishMessage(TransactionRequest transaction);

        public TransactionTemp? Get(string id);
    }
}
