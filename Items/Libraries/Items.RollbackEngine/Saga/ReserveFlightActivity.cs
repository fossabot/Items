﻿using System;
using Items.Common.Utils;

namespace Items.RollbackEngine.Saga
{
    internal class ReserveFlightActivity : Activity
    {
        private static readonly PrefixLogger Logger = PrefixLogger.Create(nameof(ReserveFlightActivity));

        private static readonly Random Rnd = new Random(3);

        public override Uri WorkItemQueueAddress => new Uri("sb://./flightReservations");

        public override Uri CompensationQueueAddress => new Uri("sb://./flightCancellations");


        public ReserveFlightActivity()
        {
        }

        public override WorkLog DoWork(WorkItem workItem)
        {
            Logger.Message("Reserving flight");

            Object car = workItem.Arguments["fatzbatz"]; // this throws
            int reservationId = Rnd.Next(100000);

            Logger.Message($"Reserved flight {reservationId.ToString()}.");

            return new WorkLog(this, new WorkResult { { "reservationId", reservationId } });
        }

        public override bool Compensate(WorkLog item, RoutingSlip routingSlip)
        {
            Object reservationId = item.Result["reservationId"];

            Logger.Message($"Cancelled flight {reservationId}.");

            return true;
        }
    }
}
